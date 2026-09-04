using HarmonyLib;
using Rewired;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using tripolygon.UModeler;
using UnityEngine;
using UnityEngine.UI;

namespace LiveStudentReaction
{
    [HarmonyPatch(typeof(PlayerMovement), "StaminaUpdate")]
    public class MainUpdatePatch
    {
        public static bool baldiNear = false;
        [HarmonyPostfix]
        public static void Postfix(PlayerMovement __instance)
        {
            var GetEntityField = AccessTools.Field(typeof(PlayerMovement), "entity");
            var GetEntity = GetEntityField.GetValue(__instance) as Entity;
            //BaldiNear
            if (Singleton<BaseGameManager>.Instance != null)
            {
                bool should = BasePlugin.Instance.configBaldiNearEnabled.Value;
                Baldi baldi = Singleton<BaseGameManager>.Instance.Ec.GetBaldi();
                if (baldi == null)
                {
                    baldiNear = false;
                }
                else
                {
                    float dist = Vector3.Distance(baldi.transform.position, Singleton<CoreGameManager>.Instance.GetPlayer(0).transform.position);
                    if (dist < 40f)
                    {
                        baldiNear = should;
                    }
                    else
                    {
                        if (BasePlugin.Instance.IsItsBaldiTimeInstalled && Singleton<BaseGameManager>.Instance.Ec.timeOut)
                        {
                            baldiNear = true;
                        }
                        else
                        {
                            baldiNear = false;
                        }
                    }
                }
            }
            //InputAction
            if (HudManagerUpdatePatch.StudentStateKeepTimer <= 0f)
            {
                if (Singleton<InputManager>.Instance.GetDigitalInput("LookBack", onDown: false))
                {
                    HudManagerUpdatePatch.StudentState = "Back";
                }
                else if (GetEntity.InternalMovement.magnitude > 0f)
                {
                    if (Singleton<InputManager>.Instance.GetDigitalInput("Run", onDown: false))
                    {
                        if (Singleton<CoreGameManager>.Instance.GetPlayer(0).plm.stamina > 0)
                        {
                            HudManagerUpdatePatch.StudentState = "Run";
                        }
                        else
                        {
                            if (baldiNear)
                            {
                                HudManagerUpdatePatch.StudentState = "BaldiNear";
                            }
                            else
                            {
                                HudManagerUpdatePatch.StudentState = "Idle";
                            }
                        }
                    }
                    else
                    {
                        if (baldiNear)
                        {
                            HudManagerUpdatePatch.StudentState = "BaldiNear";
                        }
                        else
                        {
                            HudManagerUpdatePatch.StudentState = "Idle";
                        }
                    }
                }
                else
                {
                    if (baldiNear)
                    {
                        HudManagerUpdatePatch.StudentState = "BaldiNear";
                    }
                    else
                    {
                        HudManagerUpdatePatch.StudentState = "Idle";
                    }
                }
            }
        }

        public static void ChangeState(string i, float h)
        {
            if (HudManagerUpdatePatch.StudentState != i)
            {
                HudManagerUpdatePatch.StaticTimer = 0.2f;
                HudManagerUpdatePatch.StudentState = i;
            }
            HudManagerUpdatePatch.StudentStateKeepTimer = h;
        }

        public static void ChangeState(string i)
        {
            if (HudManagerUpdatePatch.StudentState != i)
            {
                HudManagerUpdatePatch.StaticTimer = 0.2f;
                HudManagerUpdatePatch.StudentState = i;
            }
            HudManagerUpdatePatch.StudentStateKeepTimer = 3f;
        }
    }

    [HarmonyPatch(typeof(Baldi))]
    public class BaldiPatch
    {
        [HarmonyPatch("TakeApple")]
        [HarmonyPostfix]
        public static void TakeApplePostfix()
        {
            MainUpdatePatch.ChangeState("BaldiApple");
        }
    }

    [HarmonyPatch(typeof(PlayerEntity))]
    public class PlayerEntityPatch
    {
        [HarmonyPatch("Squish")]
        [HarmonyPostfix]
        public static void Postfix()
        {
            MainUpdatePatch.ChangeState("Squish");
        }
    }

    [HarmonyPatch(typeof(CoreGameManager))]
    public class EndGamePatch
    {
        [HarmonyPatch("EndGame")]
        [HarmonyPostfix]
        public static void Postfix()
        {
            HudManagerAwakePatch.Student.sprite = HudManagerAwakePatch.Student_EndGame_Sprite[HudManagerUpdatePatch.StudentImage];
            HudManagerAwakePatch.Live_Reaction_Static.gameObject.SetActive(false);
        }
    }

    [HarmonyPatch(typeof(Notebook), "Clicked")]
    public class NotebookClickedPatch
    {
        [HarmonyPostfix]
        public static void Postfix()
        {
            MainUpdatePatch.ChangeState("Notebook");
        }
    }

    [HarmonyPatch(typeof(ITM_StickerPack), "Use")]
    public class ITM_StickerPackPatch
    {
        [HarmonyPostfix]
        public static void Postfix(bool __result)
        {
            if (!__result)
            {
                return;
            }
            MainUpdatePatch.ChangeState("Sticker");
        }
    }

    [HarmonyPatch(typeof(ITM_Nametag), "Use")]
    public class ITM_NametagPatch
    {
        [HarmonyPostfix]
        public static void Postfix(bool __result)
        {
            if (!__result)
            {
                return;
            }
            MainUpdatePatch.ChangeState("Nametag");
        }
    }

    [HarmonyPatch(typeof(DetentionRoomFunction), "Activate")]
    public class DetentionRoomFunctionActivatePatch
    {
        [HarmonyPostfix]
        public static void Postfix()
        {
            MainUpdatePatch.ChangeState("Detention");
        }
    }

    [HarmonyPatch(typeof(ITM_Acceptable), "Use")]
    public class ITM_AcceptablePatch
    {
        [HarmonyPostfix]
        public static void Postfix(ITM_Acceptable __instance, bool __result)
        {
            if (!__result)
            {
                return;
            }
            var GetItemsField = AccessTools.Field(typeof(ITM_Acceptable), "item");
            Items GetItems = (Items)GetItemsField.GetValue(__instance);
            if (GetItems.ToString().Contains("lost") || GetItems.ToString().Contains("Lost"))
            {
                MainUpdatePatch.ChangeState("lost");
            }
            else if (GetItems.ToString().Contains("Quarter") || GetItems.ToString().Contains("quarter"))
            {
                MainUpdatePatch.ChangeState("Quarter");
            }
            else if (GetItems.ToString().Contains("Key") || GetItems.ToString().Contains("key"))
            {
                if (GetItems == Items.DetentionKey)
                {
                    MainUpdatePatch.ChangeState("DetentionKey");
                }
                else
                {
                    MainUpdatePatch.ChangeState("Key");
                }
            }
            else if (GetItems.ToString().Contains("Lock") || GetItems.ToString().Contains("lock"))
            {
                MainUpdatePatch.ChangeState("DoorLock");
            }
            else if (GetItems.ToString().Contains("Tape") || GetItems.ToString().Contains("tape"))
            {
                MainUpdatePatch.ChangeState("Tape");
            }
        }
    }

    [HarmonyPatch(typeof(ITM_Scissors), "Use")]
    public class ITM_ScissorsPatch
    {
        [HarmonyPostfix]
        public static void Postfix(bool __result)
        {
            if (!__result)
            {
                return;
            }
            MainUpdatePatch.ChangeState("Scissors");
        }
    }

    [HarmonyPatch(typeof(ITM_AlarmClock), "Use")]
    public class ITM_AlarmClockPatch
    {
        [HarmonyPostfix]
        public static void Postfix(bool __result)
        {
            if (!__result)
            {
                return;
            }
            MainUpdatePatch.ChangeState("AlarmClock");
        }
    }

    [HarmonyPatch(typeof(ITM_Boots), "Use")]
    public class ITM_BootsPatch
    {
        [HarmonyPostfix]
        public static void Postfix(bool __result)
        {
            if (!__result)
            {
                return;
            }
            MainUpdatePatch.ChangeState("Boots");
        }
    }

    [HarmonyPatch(typeof(ITM_BSODA), "Use")]
    public class ITM_BSODAPatch
    {
        [HarmonyPostfix]
        public static void Postfix(bool __result)
        {
            if (!__result)
            {
                return;
            }
            MainUpdatePatch.ChangeState("Soda");
        }
    }

    [HarmonyPatch(typeof(ITM_InvisibilityElixir), "Use")]
    public class ITM_InvisibilityElixirPatch
    {
        [HarmonyPostfix]
        public static void Postfix(bool __result)
        {
            if (!__result)
            {
                return;
            }
            MainUpdatePatch.ChangeState("Invisibility");
        }
    }

    [HarmonyPatch(typeof(ITM_NanaPeel), "Use")]
    public class ITM_NanaPeelUsePatch
    {
        [HarmonyPostfix]
        public static void Postfix(bool __result)
        {
            if (!__result)
            {
                return;
            }
            MainUpdatePatch.ChangeState("NanaPeel");
        }
    }

    [HarmonyPatch(typeof(ITM_NanaPeel), "Update")]
    public class ITM_NanaPeelStayPatch
    {
        private static bool slipping = false;

        [HarmonyPostfix]
        public static void Postfix(ITM_NanaPeel __instance)
        {
            if (__instance != null)
            {
                if (AccessTools.Field(typeof(ITM_NanaPeel), "slipping").GetValue(__instance) != null)
                {
                    slipping = (bool)AccessTools.Field(typeof(ITM_NanaPeel), "slipping").GetValue(__instance);
                    if (Singleton<CoreGameManager>.Instance.GetPlayer(0) != null && slipping)
                    {
                        FieldInfo entityField = AccessTools.Field(typeof(ITM_NanaPeel), "entity");
                        var entity = entityField.GetValue(__instance) as Entity;
                        float dist = Vector3.Distance(entity.transform.position, Singleton<CoreGameManager>.Instance.GetPlayer(0).transform.position);
                        float intensity = 1f - Mathf.Clamp01((dist - 40f) / 210f);
                        if (dist <= 10f)
                        {
                            MainUpdatePatch.ChangeState("Slipping", 0.1f);
                        }
                    }
                }
            }
        }
    }

    [HarmonyPatch(typeof(ITM_NoSquee), "Use")]
    public class ITM_NoSqueePatch
    {
        [HarmonyPostfix]
        public static void Postfix(bool __result)
        {
            if (!__result)
            {
                return;
            }
            MainUpdatePatch.ChangeState("NoSquee");
        }
    }

    [HarmonyPatch(typeof(ITM_PortalPoster), "Use")]
    public class ITM_PortalPosterPatch
    {
        //private static bool __resultd = false;

        [HarmonyPostfix]
        public static void Postfix(bool __result)
        {
            if (!__result)
            {
                return;
            }
            MainUpdatePatch.ChangeState("PortalPoster");
            /*
            if (__instance != null)
            {
                FieldInfo UseField = AccessTools.Field(typeof(ITM_PortalPoster), "Use");
                Used = (bool)UseField.GetValue(__instance);
                if (Used)
                {
                    MainUpdatePatch.ChangeState("PortalPoster");
                }
            }it's broken game
            */
        }
    }

    [HarmonyPatch(typeof(ITM_PrincipalWhistle), "Use")]
    public class ITM_PrincipalWhistlePatch
    {
        [HarmonyPostfix]
        public static void Postfix(bool __result)
        {
            if (!__result)
            {
                return;
            }
            MainUpdatePatch.ChangeState("Whistle");
        }
    }

    [HarmonyPatch(typeof(ITM_ReachExtender), "Use")]
    public class ITM_ReachExtenderPatch
    {
        [HarmonyPostfix]
        public static void Postfix(bool __result)
        {
            if (!__result)
            {
                return;
            }
            MainUpdatePatch.ChangeState("ReachExtender");
        }
    }

    [HarmonyPatch(typeof(ITM_ZestyBar), "Use")]
    public class ITM_ZestyBarPatch
    {
        [HarmonyPostfix]
        public static void Postfix(bool __result)
        {
            if (!__result)
            {
                return;
            }
            MainUpdatePatch.ChangeState("ZestyBar");
        }
    }

    [HarmonyPatch(typeof(ITM_YTPs), "Use")]
    public class ITM_YTPsPatch
    {
        private static int value = 25;

        [HarmonyPostfix]
        public static void Postfix(ITM_YTPs __instance, bool __result)
        {
            if (!__result)
            {
                return;
            }
            if (__instance != null)
            {
                FieldInfo valueField = AccessTools.Field(typeof(ITM_YTPs), "value");
                value = (int)valueField.GetValue(__instance);
                if (value >= 100)
                {
                    MainUpdatePatch.ChangeState("YTPs");
                }
            }
        }
    }

    [HarmonyPatch(typeof(ChalkEraser), "Use")]
    public class ChalkEraserPatch
    {
        [HarmonyPostfix]
        public static void Postfix(bool __result)
        {
            if (!__result)
            {
                return;
            }
            MainUpdatePatch.ChangeState("ChalkEraser");
        }
    }

    [HarmonyPatch(typeof(BaseGameManager))]
    public class BaseGameManagerPatch
    {
        [HarmonyPatch("ActivityCompleted")]
        [HarmonyPostfix]
        public static void Postfix(bool correct)
        {
            if (correct)
            {
                MainUpdatePatch.ChangeState("GoodGrade");
            }
            else
            {
                MainUpdatePatch.ChangeState("BadGrade");
            }
        }
    }

    [HarmonyPatch(typeof(Pickup))]
    public class PickupPatch
    {
        [HarmonyPatch("Collect")]
        [HarmonyPrefix]
        public static void Prefix(Pickup __instance, int player)
        {
            if (__instance.item.itemType == Items.Apple)
            {
                MainUpdatePatch.ChangeState("GetApple");
            }
            else if (__instance.item.itemType == Items.Map)
            {
                MainUpdatePatch.ChangeState("GetMap");
            }
        }
    }

    [HarmonyPatch(typeof(HideableLocker))]
    public class HideableLockerPatch
    {
        [HarmonyPatch("Clicked")]
        [HarmonyPostfix]
        public static void Postfix()
        {
            MainUpdatePatch.ChangeState("HideLocker", 1f);
        }
    }

    [HarmonyPatch(typeof(Gum))]
    public class GumPatch
    {
        [HarmonyPatch("EntityTriggerEnter")]
        [HarmonyPostfix]
        public static void Postfix(Collider other)
        {
            if (other.isTrigger && other.CompareTag("Player"))
            {
                MainUpdatePatch.ChangeState("GetGum");
            }
        }
    }

    [HarmonyPatch(typeof(ITM_Teleporter))]
    public class ITM_TeleporterPatch
    {
        [HarmonyPatch("Use")]
        [HarmonyPostfix]
        public static void Postfix(bool __result)
        {
            if (!__result)
            {
                return;
            }
            MainUpdatePatch.ChangeState("Teleporter");
        }
    }

    [HarmonyPatch(typeof(Playtime))]
    public class PlaytimePatch
    {
        [HarmonyPatch("StartJumprope")]
        [HarmonyPostfix]
        public static void Postfix()
        {
            MainUpdatePatch.ChangeState("Playtime");
        }
    }
}
