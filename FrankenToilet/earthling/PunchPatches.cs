using UnityEngine;
using FrankenToilet.Core;
using HarmonyLib;

namespace FrankenToilet.earthling;

[PatchOnEntry]
[HarmonyPatch(typeof(Punch))]
public static class PunchPatches
{
    [HarmonyPostfix]
    [HarmonyPatch("Start")]
    public static void SwapTexture(Punch __instance)
    {
        switch (__instance.type)
        {
            case FistType.Standard:
                Texture2D? redArm = AssetBundleHelper.LoadAsset<Texture2D>("Assets/Bundles/toiletonfire/redarm.png");
                __instance.smr.material.mainTexture = redArm;
                break;
            case FistType.Heavy:
                Texture2D? blueArm = AssetBundleHelper.LoadAsset<Texture2D>("Assets/Bundles/toiletonfire/bluearm.png");
                __instance.smr.material.mainTexture = blueArm;
                break;
        }
    }
}
