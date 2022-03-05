using HarmonyLib;
using System;
using UnityEngine;
using Logger = QModManager.Utility.Logger;
using System.Linq;

namespace lockerMod_SN
{


    [HarmonyPatch(typeof(StorageContainer))]
    [HarmonyPatch(nameof(StorageContainer.Awake))]
    internal class PatchStorageContainerConstructor
    {
        [HarmonyPostfix]
        public static void Postfix(StorageContainer __instance)
        {
            VisibleInterior.RenderInteriorOfStorageContainer(__instance);
        }
    }
    [HarmonyPatch(typeof(StorageContainer))]
    [HarmonyPatch(nameof(StorageContainer.OnClose))]
    internal class PatchCloseAction
    {
        [HarmonyPostfix]
        public static void Postfix(StorageContainer __instance)
        {
            VisibleInterior.RenderInteriorOfStorageContainer(__instance);
        }
    }
    internal class VisibleInterior
    {
        public const string childName = "LockerInterior";
        static readonly int w = 6;
        static readonly int h = 6;
        static readonly float[] xDispl = {0.44f, 0.264f, 0.088f, -0.088f, -0.264f, -0.44f};
        static readonly float[] yDispl = { 1.59f, 1.35f, .97f, 0.59f, 0.28f, 0.05f};
        GameObject[,] slots;
        GameObject root;
        public VisibleInterior(Transform locker)
        {
            root = new GameObject(childName);
            slots = new GameObject[w, h];

            root.transform.SetParent(locker);
            root.transform.localPosition = Vector3.zero;
            root.transform.localRotation = Quaternion.identity;

            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    slots[x, y] = new GameObject($"DisplayItem({x},{y})");
                    slots[x, y].transform.parent = root.transform;
                    slots[x, y].transform.localPosition = new Vector3(xDispl[x],yDispl[y], 0f);
                    slots[x, y].transform.localRotation = Quaternion.identity;
                    slots[x, y].transform.localScale = Vector3.one;

                    GameObject replacement = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    replacement.transform.SetParent(slots[x, y].transform, false);
                    replacement.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
                    replacement.SetActive(false);
                }
            }
        }
        public void UseStorageRoot(Transform storageRoot)
        {
            int numberToDisplay = System.Math.Min(storageRoot.childCount, w*h);

            for (int y = 0; y < h; y++)
            {
                if (y * w >= numberToDisplay) break;
                for (int x = 0; x < w; x++)
                {
                    if (y * w + x >= numberToDisplay) break;
                    Transform child = storageRoot.GetChild(y * w + x);
                    CopyMeshes(child.gameObject, slots[x,y]);
                }
            }
        }
        static void CopyMeshes(GameObject source, GameObject target)
        {
            GameObject meshContainer = new GameObject("MeshContainer");
            meshContainer.transform.SetParent(target.transform, false);
            meshContainer.transform.localPosition = Vector3.zero;
            meshContainer.transform.localRotation = Quaternion.identity;

            Renderer[] componentList = source.GetComponentsInChildren<MeshRenderer>(true);

            if (componentList.Length == 0)
            {
                //fallback for fish
                CopyModelByName(source.transform, meshContainer);
                RescaleContainer(meshContainer);
                return;
            }

            foreach (Renderer component in componentList)
            {
                if (component.gameObject.name.Contains("_fp"))
                {
                    continue;
                }
                if (Array.Exists(blacklist, element => element == component.gameObject.name))
                {
                    continue;
                }

                GameObject meshObject = UnityEngine.Object.Instantiate(component.gameObject);
                meshObject.transform.SetParent(meshContainer.transform, false);
            }
            RescaleContainer(meshContainer);
        }
        static bool CopyModelByName(Transform source, GameObject meshContainer)
        {
            Transform meshObjectOriginal = source.Find("model");
            if (meshObjectOriginal == null) return false;
            GameObject meshObject = UnityEngine.Object.Instantiate(meshObjectOriginal.gameObject);
            meshObject.transform.SetParent(meshContainer.transform, false);
            return true;
        }
        static void RescaleContainer(GameObject meshContainer)
        {

            Bounds bounds = GetMaxBounds(meshContainer);

            float yOffset = meshContainer.transform.InverseTransformPoint(bounds.min).y;
            float extension = System.Math.Max(
                bounds.size.x,
                System.Math.Max(
                    bounds.size.y,
                    bounds.size.z
                )
             );
            float scale = 0.2f / extension;
            meshContainer.transform.localScale = new Vector3(scale, scale, scale);
            meshContainer.transform.Translate(new Vector3(0, -scale * yOffset, 0));
        }
        static Bounds GetMaxBounds(GameObject g)
        {
            var renderers = g.GetComponentsInChildren<Renderer>();
            if (renderers.Length == 0) return new Bounds(g.transform.position, Vector3.zero);
            var b = renderers[0].bounds;
            foreach (Renderer r in renderers)
            {
                if (r is MeshRenderer || r is SkinnedMeshRenderer)
                    b.Encapsulate(r.bounds);
            }
            return b;
        }
        static readonly string[] blacklist = { "fire_extinguisher_handle_01_tp", "x_flashlightC0one" };
        public static void EnsureDestructionOfPreviousInterior(Transform locker)
        {
            Transform interior = locker.Find(childName);
            if (interior == null)
            {
                return; }

            UnityEngine.Object.Destroy(interior.gameObject);
        }
        public static void RenderInteriorOfStorageContainer(StorageContainer storageContainer)
        {
            Transform lockerObject = storageContainer.storageRoot.transform.parent;
            if ("Locker(Clone)" != lockerObject.name) return;
            EnsureDestructionOfPreviousInterior(lockerObject);
            VisibleInterior vs = new VisibleInterior(lockerObject);
            vs.UseStorageRoot(storageContainer.storageRoot.transform);
        }
    }
}