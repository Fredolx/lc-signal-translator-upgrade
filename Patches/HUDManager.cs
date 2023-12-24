using BepInEx;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Text;
using Unity.Netcode;
using UnityEngine;

namespace SignalTranslatorUpgrade.Patches
{

    [HarmonyPatch(typeof(HUDManager), "UseSignalTranslatorServerRpc")]
    public class UseSignalTranslatorServerRpc
    {
        private static void Postfix(HUDManager __instance, string signalMessage)
        {
            if (signalMessage.Length <= 12)
                return;
            SignalTranslator signalTranslator = UnityEngine.Object.FindObjectOfType<SignalTranslator>();
            if (Time.realtimeSinceStartup - signalTranslator.timeLastUsingSignalTranslator < 8f)
                return;
            signalTranslator.timeLastUsingSignalTranslator = Time.realtimeSinceStartup;
            signalTranslator.timesSendingMessage++;
            __instance.UseSignalTranslatorClientRpc(signalMessage, signalTranslator.timesSendingMessage);
            Plugin.Logger.LogInfo($"ServerRPC Postfix: {signalMessage}");
            return;
        }
        private static void Prefix(ref string signalMessage, HUDManager __instance)
        {
            if(Plugin.TransmitMessage != null)
            {
                signalMessage = Plugin.TransmitMessage;
                Plugin.Logger.LogInfo($"ServerRPC Prefix: {signalMessage}");
                Plugin.TransmitMessage = null;
            }
        }
    }

    [HarmonyPatch(typeof(HUDManager), "UseSignalTranslatorClientRpc")]
    public class UseSignalTranslatorClientRpc
    {
        private static void Prefix(string signalMessage)
        {
            Plugin.Logger.LogInfo($"Client RPC Prefix: {signalMessage}");
            Plugin.ClientMessage = signalMessage;
        }
    }

    [HarmonyPatch(typeof(HUDManager), "DisplaySignalTranslatorMessage")]
    public class DisplaySignalTranslatorMessage
    {
        private static void Prefix(ref string signalMessage)
        {
            signalMessage = Plugin.ClientMessage.Length < Plugin.MaxCharacters.Value ? Plugin.ClientMessage : Plugin.ClientMessage.Substring(0, Plugin.MaxCharacters.Value);
            Plugin.Logger.LogInfo($"Display: {signalMessage}");
            Plugin.ClientMessage = null;
        }
    }
}
