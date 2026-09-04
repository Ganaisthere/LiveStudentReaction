using HarmonyLib;
using MilkItem;
using MTM101BaldAPI;

namespace LiveStudentReaction
{
    [ConditionalPatchMod("com.milk.item")]
    [HarmonyPatch(typeof(MilkComponent))]
    public class MilkComponentPatch
    {
        [HarmonyPatch("Use")]
        [HarmonyPostfix]
        public static void Postfix(MilkComponent __instance, bool __result)
        {
            MainUpdatePatch.ChangeState("Milk");
        }
    }
}
