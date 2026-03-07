using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

using BepInEx;
using BepInEx.Logging;

using UnityEngine;
using UnityEngine.UI;

using GorillaLocomotion;
using Cinemachine;

using YizziCamModV2.Comps;

#pragma warning disable CS0618

namespace YizziCamModV2
{
    [BepInPlugin(PluginInfo.GUID, PluginInfo.Name, PluginInfo.Version)]
    public class Main : BaseUnityPlugin
    {
        internal static Main Instance;
        internal static ManualLogSource Log;

        void Awake()
        {
            Instance = this;
            Log = Logger;

            HarmonyPatches.ApplyHarmonyPatches();

            DontDestroyOnLoad(gameObject);

            Log.LogInfo("YizziCamModV2 loaded successfully.");
        }
    }
}
