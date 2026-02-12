namespace FrankenToilet.Bryan.Patches;

using FrankenToilet.Core;
using HarmonyLib;
using UnityEngine;
using UnityEngine.Video;

/// <summary> Replaces the video that every video player plays with amercia. </summary>
[PatchOnEntry]
[HarmonyPatch(typeof(VideoPlayer))]
public class VideoPatch
{
    /// <summary> Replace video with amercia. </summary>
    [HarmonyPrefix]
    [HarmonyPatch("Prepare")] [HarmonyPatch("Play")] [HarmonyPatch("Pause")] [HarmonyPatch("Stop")]
    public static void ReplaceVideo(VideoPlayer __instance)
    {
        if (!__instance.GetComponent<NonReplaceableVideo>())
        {
            __instance.isLooping = true;
            __instance.clip = BundleLoader.Amercia;
            __instance.url = "";
        }
    }
}

public class NonReplaceableVideo : MonoBehaviour
{

}