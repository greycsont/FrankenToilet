using UnityEngine;
using FrankenToilet.Core;
using HarmonyLib;

namespace FrankenToilet.earthling;

[PatchOnEntry]
[HarmonyPatch(typeof(FistControl))]
public static class FistControlPatches
{
    [HarmonyPrefix]
    [HarmonyPatch("UpdateFistIcon")]
    public static void SwapArmsBeforeUpdatingIcon(FistControl __instance)
    {
        __instance.currentVarNum = (__instance.currentVarNum + 1) % 2;
    }

    [HarmonyPostfix]
    [HarmonyPatch("UpdateFistIcon")]
    public static void SwapArmsAfterUpdatingIcon(FistControl __instance)
    {
        __instance.currentVarNum = (__instance.currentVarNum + 1) % 2;
    }

    [HarmonyPrefix]
    [HarmonyPatch("Update")]
	public static bool UseWrongArm(FistControl __instance) // im sorry
	{
        InputManager inman = InputManager.Instance;
		if (__instance.fistCooldown > -1f)
		{
			__instance.fistCooldown = Mathf.MoveTowards(__instance.fistCooldown, 0f, Time.deltaTime * 2f);
		}
		float punchStamina = WeaponCharges.Instance.punchStamina;
		if (!OptionsManager.Instance.paused && __instance.fistCooldown <= 0f && punchStamina >= 1f)
		{
			if (inman.InputSource.Actions.Fist.PunchFeedbacker.WasPerformedThisFrame() && (__instance.currentVarNum == 0 || __instance.ForceArm(0)))
			{
                __instance.ForceArm(1);
				__instance.currentPunch.heldAction = InputManager.Instance.InputSource.Actions.Fist.PunchFeedbacker;
				__instance.currentPunch.PunchStart();
			}
			if (inman.InputSource.Actions.Fist.PunchKnuckleblaster.WasPerformedThisFrame() && (__instance.currentVarNum == 1 || __instance.ForceArm(1)))
			{
                __instance.ForceArm(0);
				__instance.currentPunch.heldAction = InputManager.Instance.InputSource.Actions.Fist.PunchKnuckleblaster;
				__instance.currentPunch.PunchStart();
			}
		}
		if (!OptionsManager.Instance || OptionsManager.Instance.mainMenu || OptionsManager.Instance.inIntro || OptionsManager.Instance.paused || ((bool)ScanningStuff.Instance && ScanningStuff.Instance.IsReading) || GameStateManager.Instance.PlayerInputLocked)
		{
			return false;
		}
		if (InputManager.Instance.InputSource.Fire2.IsPressed && __instance.shopping)
		{
			__instance.zooming = true;
			CameraController.Instance.Zoom(CameraController.Instance.defaultFov / 2f);
		}
		else if (__instance.zooming)
		{
			__instance.zooming = false;
			CameraController.Instance.StopZoom();
		}
		if (__instance.spawnedArms.Count > 1 && !__instance.shopping && (SpawnMenu.Instance == null || !SpawnMenu.Instance.gameObject.activeInHierarchy))
		{
			if (InputManager.Instance.InputSource.ChangeFist.WasPerformedThisFrame)
			{
				__instance.ScrollArm();
			}
		}
		else if (__instance.spawnedArms.Count > 0 && __instance.currentPunch == null)
		{
            __instance.ArmChange(0);
		}
		if (__instance.spawnedArms.Count == 0 && InputManager.Instance.InputSource.Punch.WasPerformedThisFrame && (__instance.forcedLoadout == null || __instance.forcedLoadout.arm.blueVariant != VariantOption.ForceOff || __instance.forcedLoadout.arm.redVariant != VariantOption.ForceOff))
		{
			HudMessageReceiver.Instance.SendHudMessage("<color=red>CAN'T PUNCH IF YOU HAVE NO ARM EQUIPPED, DUMBASS</color>\nArms can be re-equipped at the shop");
		}

        return false;
	}
}