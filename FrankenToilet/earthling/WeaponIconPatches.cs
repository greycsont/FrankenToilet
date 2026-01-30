using FrankenToilet.Core;
using HarmonyLib;

namespace FrankenToilet.earthling;

[PatchOnEntry]
[HarmonyPatch(typeof(WeaponIcon))]
public static class WeaponIconPatches
{
    [HarmonyPrefix]
    [HarmonyPatch("Start")]
    public static void ReplaceIcon(WeaponIcon __instance)
    {
        WeaponDescriptor? awesomeGun = AssetBundleHelper.LoadAsset<WeaponDescriptor>("Assets/Bundles/toiletonfire/awesomegun.asset");

        if (awesomeGun == null)
        {
            LogHelper.LogError("Could not load awesome weapon icons");
            return;
        }

        __instance.weaponDescriptor.icon = awesomeGun.icon;
        __instance.weaponDescriptor.glowIcon = awesomeGun.glowIcon;
    }
}