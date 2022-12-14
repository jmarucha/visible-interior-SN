using HarmonyLib;
using System;
using UnityEngine;
using Logger = QModManager.Utility.Logger;
using System.Linq;
using System.Collections.Generic;


namespace lockerMod_SN {

    [HarmonyPatch(typeof(StorageContainer))]
    [HarmonyPatch(nameof(StorageContainer.Awake))]
    internal class PatchStorageContainerConstructor {
        [HarmonyPostfix]
        public static void Postfix(StorageContainer __instance) {
            VisibleInterior.UpdateInterior(__instance);
        }
    }

    [HarmonyPatch(typeof(StorageContainer))]
    [HarmonyPatch(nameof(StorageContainer.OnClose))]
    internal class PatchCloseAction {
        [HarmonyPostfix]
        public static void Postfix(StorageContainer __instance) {
            VisibleInterior.UpdateInterior(__instance);
        }
    }

    internal class LockerModData : MonoBehaviour {
        public TechType type;
        public string classId;
    }

    internal class VisibleInterior {
        private const string interiorName = "LockerInterior";
        private const int itemPerRow = 8;
        private static readonly float[] xDispl =
            { 0.469f, 0.335f, 0.201f, 0.067f, -0.067f, -0.201f, -0.335f, -0.469f };
        private static readonly float[] yDispl =
            { 1.58803f, 1.32938f, 0.966707f, 0.57335f, 0.288304f, 0.046258f };
        private const float zDispl = -0.03f;
        
        public static void UpdateInterior(StorageContainer storageContainer) {
            if ("Locker(Clone)" != storageContainer.prefabRoot.name) return;
            var interior = GetInteriorInstance(storageContainer);
            var items = GetSortedItems(storageContainer.storageRoot.gameObject);
            var dummies = GetSortedDummies(interior);
            for (int i = 0, j = 0; i < items.Count || j < dummies.Count;) {
                var targetPosition = new Vector3(xDispl[i % itemPerRow], yDispl[i / itemPerRow], zDispl);
                int cmp = 
                    i == items.Count ? 1 :
                    j == dummies.Count ? -1 :
                    CompareTechType(
                        items[i].GetComponent<Pickupable>().GetTechType(),
                        dummies[j].GetComponent<LockerModData>().type
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

        private static GameObject CreateDummy(GameObject interior, GameObject source) {
            var dummy = GameObject.Instantiate(source, interior.transform);
            var pickupable = source.GetComponent<Pickupable>();
            var techType = pickupable?.GetTechType() ?? TechType.None;
            var prefabIdentifier = source.GetComponent<PrefabIdentifier>();
            var classId = prefabIdentifier?.ClassId ?? "";
            dummy.SetActive(true);
            SanitizeObject(dummy, techType);
            var dummyComp = dummy.AddComponent<LockerModData>();
            dummyComp.type = techType;
            dummyComp.classId = classId;
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

            foreach (Transform childTransform in obj.transform)
                SanitizeObject(childTransform.gameObject, techType);
        }

        private static Quaternion GetIdealRotation(GameObject dummy) {
            var comp = dummy.GetComponent<LockerModData>();
            return Quirk.overrideRotation.ContainsKey(comp.classId) ?
                Quirk.overrideRotation[comp.classId] :
                Quirk.GetIdealRotationByTechType(comp.type);
        }

        private static Bounds GetIdealBounds(GameObject dummy) {
            var comp = dummy.GetComponent<LockerModData>();
            if (Quirk.overrideBounds.ContainsKey(comp.classId)) return Quirk.overrideBounds[comp.classId];
            var currentRot = dummy.transform.rotation;
            var currentScale = dummy.transform.localScale;
            var currentPos = dummy.transform.localPosition;
            dummy.transform.rotation = GetIdealRotation(dummy);
            dummy.transform.localScale = Vector3.one;
            dummy.transform.position = Vector3.zero;
            var renderers = dummy.GetComponentsInChildren<Renderer>();
            if (renderers.Length == 0) return new Bounds(dummy.transform.position, Vector3.zero);
            var b = renderers[0].bounds;
            foreach (Renderer r in renderers)
                if (r is MeshRenderer || r is SkinnedMeshRenderer) b.Encapsulate(r.bounds);
            dummy.transform.rotation = currentRot;
            dummy.transform.localScale = currentScale;
            dummy.transform.position = currentPos;
            return Quirk.overrideBounds[comp.classId] = b;
        }

        private static float GetIdealDeltaScale(GameObject dummy) {
            var comp = dummy.GetComponent<LockerModData>();
            return Quirk.overrideDeltaScale.ContainsKey(comp.classId) ?
                Quirk.overrideDeltaScale[comp.classId] :
                1;
        }

        private static GameObject GetInteriorInstance(StorageContainer storageContainer) {
            var locker = storageContainer.prefabRoot;
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
            foreach (Transform dummyTransform in lockerInterior.transform) result.Add(dummyTransform.gameObject);
            result.Sort(
                (g1, g2) => 
                CompareTechType(
                    g1.GetComponent<LockerModData>().type,
                    g2.GetComponent<LockerModData>().type
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
}
