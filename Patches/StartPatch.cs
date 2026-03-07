using HarmonyLib;
using UnityEngine;
using YizziCamModV2;

namespace YizziCamModV2.Patches
{
    [HarmonyPatch(typeof(GorillaTagger))]
    [HarmonyPatch("Start", MethodType.Normal)]
    internal class StartPatch
    {
        private static void Postfix()
        {
            if (CameraController.Instance == null)
            {
                GameObject cameraGO = new GameObject("CameraController");
                cameraGO.AddComponent<CameraController>();
                DontDestroyOnLoad(cameraGO);
            }

            CameraController.Instance?.YizziStart();
        }
    }
}
