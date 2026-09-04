using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Text;

namespace LiveStudentReaction
{
    [HarmonyPatch(typeof(ElevatorScreen), "StartGame")]
    public class ElevatorScreenStartGamePatches
    {
        [HarmonyPostfix]
        public static void Postfix()
        {
            HudManagerUpdatePatch.Reset();
        }
    }
}
