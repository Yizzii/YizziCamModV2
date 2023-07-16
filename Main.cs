using System;
using BepInEx;
using UnityEngine;
using YizziCamModV2.Comps;

namespace YizziCamModV2
{
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
            controller.gameObject.AddComponent<UI>();
        }
    }
}
