using HarmonyLib;
using YizziCamModV2;
namespace GorillaChat.Patches
{
    [HarmonyPatch(typeof(GorillaTagger))]
    [HarmonyPatch("Start", MethodType.Normal)]
    internal class StartPatch
    {
        private static void Postfix()
        {
            Main.Instance.YizziStart();
        }
    }
}