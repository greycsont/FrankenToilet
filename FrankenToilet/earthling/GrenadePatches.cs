using UnityEngine;
using FrankenToilet.Core;
using HarmonyLib;

namespace FrankenToilet.earthling;

[PatchOnEntry]
[HarmonyPatch(typeof(Grenade))]
public static class GrenadePatches
{
    [HarmonyPostfix]
    [HarmonyPatch(nameof(Grenade.PlayerRideStart))]
    public static void NoRocketRidesForYouBuddy(Grenade __instance)
    {
        __instance.totalDamageMultiplier = 100f;
        GameObject superExplosion = __instance.superExplosion;
        __instance.superExplosion = GameObject.Instantiate(superExplosion);
        __instance.superExplosion.transform.GetChild(0).GetComponent<Explosion>().maxSize = 1000;
        __instance.Explode(true, false, true, 3f, false, null);
    }
}
