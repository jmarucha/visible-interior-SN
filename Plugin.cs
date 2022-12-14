using BepInEx;
using HarmonyLib;
using System.Reflection;


namespace VisibleLockerInterior {
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    public class Plugin: BaseUnityPlugin {

        public Harmony Harmony { get; } = new Harmony(PluginInfo.PLUGIN_GUID);

        private void OnEnable() {
            Harmony.PatchAll(Assembly.GetExecutingAssembly());
        }

        private void OnDisable() {
            Harmony.UnpatchSelf();
        }
    }
}
