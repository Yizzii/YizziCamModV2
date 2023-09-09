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
            new GameObject().AddComponent<CameraController>();
            CameraController.Instance.YizziStart();
        }
    }
}