using System;
using System.IO;
using System.Reflection;
using BepInEx;
using Cinemachine;
using GorillaLocomotion;
using UnityEngine;
using UnityEngine.UI;
using Utilla;
using YizziCamModV2.Comps;

namespace YizziCamModV2
{
    [BepInDependency("org.legoandmars.gorillatag.utilla", "1.5.0")]
    [BepInPlugin(PluginInfo.GUID, PluginInfo.Name, PluginInfo.Version)]
    public class Main : BaseUnityPlugin
    {
        CameraController controller;
        void Start()
        {
            Utilla.Events.GameInitialized += OnGameInitialized;
        }

        void OnGameInitialized(object sender, EventArgs e)
        {
            controller = new GameObject().AddComponent<CameraController>();
        }
    }
}
