using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace SignalTranslatorUpgrade.Patches
{
    [HarmonyPatch(typeof(Terminal), "ParsePlayerSentence")]
    public class ParsePlayerSentence
    {
        private static bool Prefix(Terminal __instance)
        {
            string text = __instance.screenText.text.Substring(__instance.screenText.text.Length - __instance.textAdded);
            text = Traverse.Create(__instance).Method("RemovePunctuation", text).GetValue<string>();
            var command = text.Split(" ");
            if (command[0] != "transmit")
                return true;
            Plugin.TransmitMessage = String.Join(" ", command.Skip(1));
            return true;
        }
    }
}
