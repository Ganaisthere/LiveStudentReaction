using CoolMovement;
using HarmonyLib;
using MTM101BaldAPI;
using MTM101BaldAPI.AssetTools;

namespace LiveStudentReaction
{
    [ConditionalPatchMod("skid.coolmovement")]
    [HarmonyPatch(typeof(PlayerMovementAddon))]
    public class PMAddonPatches
    {
        [HarmonyPatch("AddBounceCombo")]
        [HarmonyPostfix]
        public static void AddBounceComboPostfix()
        {
            MainUpdatePatch.ChangeState("BounceCombo", 1.5f);
        }

        [HarmonyPatch("UpdateDash")]
        [HarmonyPostfix]
        public static void UpdateDashPostfix(PlayerMovementAddon __instance)
        {
            if (__instance.DashCooldown > 2.5f)
            {
                if (__instance.Crouching)
                {
                    MainUpdatePatch.ChangeState("Dash", 0.5f);
                }
                else
                {
                    MainUpdatePatch.ChangeState("Dash", 0.5f);
                }
            }
        }
    }
}
