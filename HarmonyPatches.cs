using System;
using System.Reflection;
using HarmonyLib;

namespace YizziCamModV2
{
    public static class HarmonyPatches
    {
        private static Harmony instance;

        public static bool IsPatched { get; private set; }
        public const string InstanceId = PluginInfo.GUID;

        internal static void ApplyHarmonyPatches()
        {
            if (IsPatched) return;

            if (instance == null)
            {
                instance = new Harmony(InstanceId);
            }

            try
            {
                instance.PatchAll(Assembly.GetExecutingAssembly());
                IsPatched = true;
                UnityEngine.Debug.Log("[YizziCamModV2] Harmony patches applied successfully.");
            }
            catch (Exception ex)
            {
                UnityEngine.Debug.LogError($"[YizziCamModV2] Failed to apply Harmony patches: {ex}");
            }
        }

        internal static void RemoveHarmonyPatches()
        {
            if (instance != null && IsPatched)
            {
                try
                {
                    instance.UnpatchSelf();
                    IsPatched = false;
                    UnityEngine.Debug.Log("[YizziCamModV2] Harmony patches removed successfully.");
                }
                catch (Exception ex)
                {
                    UnityEngine.Debug.LogError($"[YizziCamModV2] Failed to remove Harmony patches: {ex}");
                }
            }
        }
    }
}
