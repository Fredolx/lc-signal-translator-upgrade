using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;

namespace SignalTranslatorUpgrade
{
    [BepInPlugin("Fredolx.SignalTranslatorUpgrade", PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        public static string TransmitMessage { get; set; }
        public static string ClientMessage { get; set; }
        public static new ManualLogSource Logger;
        public static ConfigEntry<int> MaxCharacters { get; set; }
        private void Awake()
        {
            if(Logger == null)
                Logger = base.Logger;
            Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");
            MaxCharacters = Config.Bind(
                "General",
                "MaxMessageLength",
                30,
                "The maximum of characters that should be displayed on the screen when transmitting messages"
            );
            var harmony = new Harmony(PluginInfo.PLUGIN_GUID);
            harmony.PatchAll();
        }
    }
}
