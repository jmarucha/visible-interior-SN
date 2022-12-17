using System;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;

namespace VisibleLockerInterior {
    internal class Controller {
        private const string interiorName = "mod_LockerInterior";
        private const int itemPerRow = 8;
        private static readonly float[] xPos =
            { 0.469f, 0.335f, 0.201f, 0.067f, -0.067f, -0.201f, -0.335f, -0.469f };
        private static readonly float[] yPos =
            { 1.58803f, 1.32938f, 0.966707f, 0.57335f, 0.288304f, 0.046258f };
        private const float zPos = -0.03f;

        public static void UpdateInterior(StorageContainer sc) {
            if ("Locker(Clone)" != sc.prefabRoot.name) return;
            var interior = GetInteriorInstance(sc);
            var items = GetSortedItems(sc.storageRoot.gameObject);
            var dummies = GetSortedDummies(interior);
            for (int i = 0, j = 0; i < items.Count || j < dummies.Count;) {
                var targetPosition =
                    new Vector3(xPos[i % itemPerRow], yPos[i / itemPerRow], zPos);
                int cmp = 
                    i == items.Count ? 1 :
                    j == dummies.Count ? -1 :
                    CompareTechType(
                        items[i].GetComponent<Pickupable>().GetTechType(),
                        dummies[j].GetComponent<VisibleLockerInteriorDummyData>().techType
                        );
                if (cmp == -1) {
                    var dummy = CreateDummy(interior, items[i]);
                    RepositionDummy(dummy.gameObject, targetPosition);
                    i++;
                } else if (cmp == 0) {
                    RepositionDummy(dummies[j].gameObject, targetPosition);
                    i++; j++;
                } else {
                    GameObject.Destroy(dummies[j].gameObject);
                    j++;
                }
            }
        }

        private static GameObject CreateDummy(GameObject interior, GameObject src) {
            var dummy = GameObject.Instantiate(src, interior.transform);
            var techType = src.GetComponent<Pickupable>()?.GetTechType() ?? TechType.None;
            var classId = src.GetComponent<PrefabIdentifier>()?.ClassId ?? "";
            dummy.SetActive(true);
            SanitizeObject(dummy, techType);
            var dummyComp = dummy.AddComponent<VisibleLockerInteriorDummyData>();
            dummyComp.techType = techType;
            dummyComp.prefabId = classId;
            return dummy;
        }

        private static void RepositionDummy(GameObject dummy, Vector3 targetPosition) {
            var bounds = GetIdealBounds(dummy);
            dummy.transform.localRotation = GetIdealRotation(dummy);
            var scale = new[] {
                0.13f / bounds.size.x,
                0.14f / bounds.size.y,
                0.27f / bounds.size.z
            }.Min() * GetIdealDeltaScale(dummy);
            var offset = -bounds.center + bounds.extents.y * Vector3.up;
            dummy.transform.localScale = scale * Vector3.one;
            dummy.transform.localPosition = targetPosition + offset * scale;
        }

        private static void SanitizeObject(GameObject obj, TechType techType) {
            if (!obj.activeSelf || obj.name.StartsWith("x")) {
                GameObject.Destroy(obj);
                return;
            }

            var destroyList = new List<Component>();
            do {
                destroyList.Clear();
                destroyList.AddRange(obj.GetComponents<Collider>());
                destroyList.AddRange(obj.GetComponents<Rigidbody>());
                destroyList.AddRange(obj.GetComponents<ParticleSystem>());
                foreach (var r in obj.GetComponents<Renderer>()) {
                    if (r is MeshRenderer || r is SkinnedMeshRenderer) continue;
                    destroyList.Add(r);
                }
                foreach (var b in obj.GetComponents<Behaviour>()) {
                    if (b is SkyApplier) {
                        b.enabled = true;
                    } else if (b is Animator && Quirk.MustHaveAnimator(techType)) {
                        b.enabled = false;
                    } else {
                        destroyList.Add(b);
                    }
                }
                destroyList.Reverse();
                foreach (var comp in destroyList) GameObject.DestroyImmediate(comp);
            } while (destroyList.Count > 0);

            if (Quirk.IsKelp(techType))
                foreach (var r in obj.GetComponents<Renderer>())
                    foreach (var m in r.materials)
                        m.DisableKeyword("FX_KELP");

            foreach (Transform childTransform in obj.transform)
                SanitizeObject(childTransform.gameObject, techType);
        }

        private static Quaternion GetIdealRotation(GameObject dummy) {
            var comp = dummy.GetComponent<VisibleLockerInteriorDummyData>();
            return Quirk.overrideRotation.ContainsKey(comp.prefabId) ?
                Quirk.overrideRotation[comp.prefabId] :
                Quirk.GetIdealRotationByTechType(comp.techType);
        }

        private static Bounds GetIdealBounds(GameObject dummy) {
            var comp = dummy.GetComponent<VisibleLockerInteriorDummyData>();
            if (Quirk.overrideBounds.ContainsKey(comp.prefabId)) 
                return Quirk.overrideBounds[comp.prefabId];
            var currentRot = dummy.transform.rotation;
            var currentScale = dummy.transform.localScale;
            var currentPos = dummy.transform.localPosition;
            dummy.transform.rotation = GetIdealRotation(dummy);
            dummy.transform.localScale = Vector3.one;
            dummy.transform.position = Vector3.zero;
            var renderers = dummy.GetComponentsInChildren<Renderer>();
            if (renderers.Length == 0)
                return new Bounds(dummy.transform.position, Vector3.zero);
            var b = renderers[0].bounds;
            foreach (Renderer r in renderers)
                if (r is MeshRenderer || r is SkinnedMeshRenderer)
                    b.Encapsulate(r.bounds);
            dummy.transform.rotation = currentRot;
            dummy.transform.localScale = currentScale;
            dummy.transform.position = currentPos;
            return Quirk.overrideBounds[comp.prefabId] = b;
        }

        private static float GetIdealDeltaScale(GameObject dummy) {
            var comp = dummy.GetComponent<VisibleLockerInteriorDummyData>();
            return Quirk.overrideDeltaScale.ContainsKey(comp.prefabId) ?
                Quirk.overrideDeltaScale[comp.prefabId] :
                1;
        }

        private static GameObject GetInteriorInstance(StorageContainer sc) {
            var locker = sc.prefabRoot;
            var interiorTransform = locker.transform.Find(interiorName);
            if (interiorTransform) return interiorTransform.gameObject;
            var interiorObject = new GameObject(interiorName);
            interiorObject.transform.SetParent(locker.transform);
            interiorObject.transform.localRotation = Quaternion.identity;
            interiorObject.transform.localPosition = Vector3.zero;
            return interiorObject;
        }

        private static List<GameObject> GetSortedItems(GameObject storageRoot) {
            var result = new List<GameObject>(storageRoot.transform.childCount);
            foreach (Transform item in storageRoot.transform) {
                if (!item.gameObject.GetComponent<Pickupable>()) continue;
                result.Add(item.gameObject);
            }
            result.Sort(
                (g1, g2) =>
                CompareTechType(
                    g1.GetComponent<Pickupable>().GetTechType(),
                    g2.GetComponent<Pickupable>().GetTechType()
                )
            );
            return result;
        }

        private static List<GameObject> GetSortedDummies(GameObject lockerInterior) {
            var result = new List<GameObject>(lockerInterior.transform.childCount);
            foreach (Transform dummyTransform in lockerInterior.transform)
                result.Add(dummyTransform.gameObject);
            result.Sort(
                (g1, g2) => 
                CompareTechType(
                    g1.GetComponent<VisibleLockerInteriorDummyData>().techType,
                    g2.GetComponent<VisibleLockerInteriorDummyData>().techType
                )
            );
            return result;
        }

        private static int CompareTechType(TechType t1, TechType t2) {
            var size1 = CraftData.GetItemSize(t1);
            var size2 = CraftData.GetItemSize(t2);
            if (size1.Equals(size2)) return t1.CompareTo(t2);
            var l1 = Math.Max(size1.x, size1.y);
            var l2 = Math.Max(size2.x, size2.y);
            if (l1 != l2) return l2.CompareTo(l1);
            var a1 = size1.x * size1.y;
            var a2 = size2.x * size2.y;
            return a1 == a2 ? size2.y.CompareTo(size1.y) : a2.CompareTo(a1);
        }
    }

    internal class VisibleLockerInteriorDummyData : MonoBehaviour {
        public TechType techType;
        public string prefabId;
    }
}
