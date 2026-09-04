using HarmonyLib;
using MTM101BaldAPI.AssetTools;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using static UnityEngine.Random;

namespace LiveStudentReaction
{
    [HarmonyPatch(typeof(HudManager), "Awake")]
    public class HudManagerAwakePatch
    {
        public static UnityEngine.UI.Image Live_Reaction_Inner;
        public static UnityEngine.UI.Image Live_Reaction_Overlay;
        public static UnityEngine.UI.Image Live_Reaction_Static;
        public static UnityEngine.UI.Image Student;
        public static List<UnityEngine.UI.Image> customImagesToDarken = new List<UnityEngine.UI.Image>();

        //---------------------------------------------------------------------------------

        public static UnityEngine.Sprite[] Live_Reaction_Inner_Sprite = new UnityEngine.Sprite[4];// = Sprite.Create(Live_Reaction_Inner_Texture2D, new Rect(0, 0, Live_Reaction_Inner_Texture2D.width, Live_Reaction_Inner_Texture2D.height), new Vector2((float)0.5, (float)0.5));
        public static UnityEngine.Sprite[] Live_Reaction_Overlay_Sprite = new UnityEngine.Sprite[4];// = Sprite.Create(Live_Reaction_Overlay_Texture2D, new Rect(0, 0, Live_Reaction_Overlay_Texture2D.width, Live_Reaction_Overlay_Texture2D.height), new Vector2((float)0.5, (float)0.5));
        public static UnityEngine.Sprite[] Live_Reaction_Static_Sprite = new UnityEngine.Sprite[4];
        public static UnityEngine.Sprite[] Student_Idle_Sprite = new UnityEngine.Sprite[4];
        public static UnityEngine.Sprite[] Student_Back_Sprite = new UnityEngine.Sprite[4];
        public static UnityEngine.Sprite[] Student_Run_Sprite = new UnityEngine.Sprite[4];
        public static UnityEngine.Sprite[] Student_Notebook_Sprite = new UnityEngine.Sprite[4];
        public static UnityEngine.Sprite[] Student_Sweat_Sprite = new UnityEngine.Sprite[4];
        public static UnityEngine.Sprite[] Student_Back_Sweat_Sprite = new UnityEngine.Sprite[4];
        public static UnityEngine.Sprite[] Student_Sticker_Sprite = new UnityEngine.Sprite[4];
        public static UnityEngine.Sprite[] Student_Nametag_Sprite = new UnityEngine.Sprite[4];
        public static UnityEngine.Sprite[] Student_Detention_Sprite = new UnityEngine.Sprite[4];
        public static UnityEngine.Sprite[] Student_LostItem_Sprite = new UnityEngine.Sprite[4];
        public static UnityEngine.Sprite[] Student_Quarter_Sprite = new UnityEngine.Sprite[4];
        public static UnityEngine.Sprite[] Student_DetentionKey_Sprite = new UnityEngine.Sprite[4];
        public static UnityEngine.Sprite[] Student_DoorLock_Sprite = new UnityEngine.Sprite[4];
        public static UnityEngine.Sprite[] Student_Scissors_Sprite = new UnityEngine.Sprite[4];
        public static UnityEngine.Sprite[] Student_Tape_Sprite = new UnityEngine.Sprite[4];
        public static UnityEngine.Sprite[] Student_Key_Sprite = new UnityEngine.Sprite[4];
        public static UnityEngine.Sprite[] Student_AlarmClock_Sprite = new UnityEngine.Sprite[4];
        public static UnityEngine.Sprite[] Student_Boots_Sprite = new UnityEngine.Sprite[4];
        public static UnityEngine.Sprite[] Student_Soda_Sprite = new UnityEngine.Sprite[4];
        public static UnityEngine.Sprite[] Student_Invisibility_Sprite = new UnityEngine.Sprite[4];
        public static UnityEngine.Sprite[] Student_NanaPeel_Sprite = new UnityEngine.Sprite[4];
        public static UnityEngine.Sprite[] Student_Slipping_Sprite = new UnityEngine.Sprite[4];
        public static UnityEngine.Sprite[] Student_NoSquee_Sprite = new UnityEngine.Sprite[4];
        public static UnityEngine.Sprite[] Student_PortalPoster_Sprite = new UnityEngine.Sprite[4];
        public static UnityEngine.Sprite[] Student_Whistle_Sprite = new UnityEngine.Sprite[4];
        public static UnityEngine.Sprite[] Student_ReachExtender_Sprite = new UnityEngine.Sprite[4];
        public static UnityEngine.Sprite[] Student_ZestyBar_Sprite = new UnityEngine.Sprite[4];
        public static UnityEngine.Sprite[] Student_YTPs_Sprite = new UnityEngine.Sprite[4];
        public static UnityEngine.Sprite[] Student_ChalkEraser_Sprite = new UnityEngine.Sprite[4];
        public static UnityEngine.Sprite[] Student_BaldiNear_Sprite = new UnityEngine.Sprite[4];
        public static UnityEngine.Sprite[] Student_BaldiApple_Sprite = new UnityEngine.Sprite[4];
        public static UnityEngine.Sprite[] Student_EndGame_Sprite = new UnityEngine.Sprite[4];
        public static UnityEngine.Sprite[] Student_Squish_Sprite = new UnityEngine.Sprite[4];
        public static UnityEngine.Sprite[] Student_Run_Sweat_Sprite = new UnityEngine.Sprite[4];
        public static UnityEngine.Sprite[] Student_GoodGrade_Sprite = new UnityEngine.Sprite[4];
        public static UnityEngine.Sprite[] Student_BadGrade_Sprite = new UnityEngine.Sprite[4];
        public static UnityEngine.Sprite[] Student_GetApple_Sprite = new UnityEngine.Sprite[4];
        public static UnityEngine.Sprite[] Student_GetMap_Sprite = new UnityEngine.Sprite[4];
        public static UnityEngine.Sprite[] Student_HideLocker_Sprite = new UnityEngine.Sprite[4];
        public static UnityEngine.Sprite[] Student_GetGum_Sprite = new UnityEngine.Sprite[4];
        public static UnityEngine.Sprite[] Student_Teleporter_Sprite = new UnityEngine.Sprite[4];
        public static UnityEngine.Sprite[] Student_Playtime_Sprite = new UnityEngine.Sprite[4];

        //Mods Support: BBIMAMTMP
        public static UnityEngine.Sprite[] Compacts_Milk_Sprite = new UnityEngine.Sprite[4];

        //Mods Support: In-Between Time-Zones
        public static UnityEngine.Sprite[] Compacts_Drawing_Happy_Sprite = new UnityEngine.Sprite[4];
        public static UnityEngine.Sprite[] Compacts_Drawing_Mid_Sprite = new UnityEngine.Sprite[4];
        public static UnityEngine.Sprite[] Compacts_Drawing_Scared_Sprite = new UnityEngine.Sprite[4];
        public static Dictionary<string, Sprite[]> customStates = new Dictionary<string, Sprite[]>();

        //Mods Support: Coolmovement
        public static UnityEngine.Sprite[] Compacts_BounceCombo_Sprite = new UnityEngine.Sprite[4];
        public static UnityEngine.Sprite[] Compacts_Dash_Sprite = new UnityEngine.Sprite[4];


        [HarmonyPostfix]
        public static void Postfix(HudManager __instance)
        {
            var itemTitle = AccessTools.Field(typeof(HudManager), "itemTitle").GetValue(__instance) as TMP_Text;
            itemTitle.rectTransform.sizeDelta = new Vector2(420f, 50f);

            if (BasePlugin.Instance.IsRandomZoneInstalled)
            {
                customStates.Clear();
                customStates.Add("Happy", Compacts_Drawing_Happy_Sprite);
                customStates.Add("Mid", Compacts_Drawing_Mid_Sprite);
                customStates.Add("Scary", Compacts_Drawing_Scared_Sprite);
            }

            customImagesToDarken.Clear();

            GameObject Live_Reaction_Inner_Obj = new GameObject("Live_Reaction");
            Live_Reaction_Inner_Obj.transform.SetParent(__instance.transform, false);
            Live_Reaction_Inner = Live_Reaction_Inner_Obj.AddComponent<UnityEngine.UI.Image>();
            Live_Reaction_Inner.color = new UnityEngine.Color(1f, 1f, 1f, 1f);
            Live_Reaction_Inner.rectTransform.sizeDelta = new Vector2(166f, 110f);
            Live_Reaction_Inner.rectTransform.anchoredPosition = new Vector2(BasePlugin.Instance.configXPosition.Value, BasePlugin.Instance.configYPosition.Value);
            Live_Reaction_Inner.rectTransform.anchorMax = new Vector2(BasePlugin.Instance.configAnchorMaxAndMinX.Value, BasePlugin.Instance.configAnchorMaxAndMinY.Value);
            Live_Reaction_Inner.rectTransform.anchorMin = new Vector2(BasePlugin.Instance.configAnchorMaxAndMinX.Value, BasePlugin.Instance.configAnchorMaxAndMinY.Value);
            float flip = 1f;
            if (BasePlugin.Instance.configFlipHorizontallyEnabled.Value)
            {
                flip = -1f;
            }
            Live_Reaction_Inner.rectTransform.localScale = new Vector3(flip, 1f, 1f);
            Live_Reaction_Inner.sprite = AssetFinder.FindOfTypeWithName<Sprite>("Transparent", false);//BasePlugin.AssetMan.Get<Sprite>("Student_0-Live_Reaction_Inner_Sheet_1");
            customImagesToDarken.Add(Live_Reaction_Inner);

            GameObject Student_Obj = new GameObject("Student");
            Student_Obj.transform.SetParent(Live_Reaction_Inner_Obj.transform, false);
            Student = Student_Obj.AddComponent<UnityEngine.UI.Image>();
            Student.color = new UnityEngine.Color(1f, 1f, 1f, 1f);
            Student.rectTransform.sizeDelta = new Vector2(166f, 110f);
            Student.sprite = Student_Idle_Sprite[1];

            GameObject Live_Reaction_Overlay_Obj = new GameObject("Live_Reaction_Overlay");
            Live_Reaction_Overlay_Obj.transform.SetParent(Live_Reaction_Inner_Obj.transform, false);
            Live_Reaction_Overlay = Live_Reaction_Overlay_Obj.AddComponent<UnityEngine.UI.Image>();
            Live_Reaction_Overlay.color = new UnityEngine.Color(1f, 1f, 1f, 1f);
            Live_Reaction_Overlay.rectTransform.sizeDelta = new Vector2(166f, 110f);
            Live_Reaction_Overlay.sprite = AssetFinder.FindOfTypeWithName<Sprite>("Transparent", false);//BasePlugin.AssetMan.Get<Sprite>("Student_0-Live_Reaction_Overlay_Sheet_1");
            customImagesToDarken.Add(Live_Reaction_Overlay);

            GameObject Live_Reaction_Static_Obj = new GameObject("Live_Reaction_Static");
            Live_Reaction_Static_Obj.transform.SetParent(Live_Reaction_Inner_Obj.transform, false);
            Live_Reaction_Static = Live_Reaction_Static_Obj.AddComponent<UnityEngine.UI.Image>();
            Live_Reaction_Static.color = new UnityEngine.Color(1f, 1f, 1f, 1f);
            //Live_Reaction_Static.color = new UnityEngine.Color(0f, 0f, 0f, 0f);
            Live_Reaction_Static.rectTransform.sizeDelta = new Vector2(166f, 110f);
            Live_Reaction_Static.sprite = Live_Reaction_Static_Sprite[0];
            Live_Reaction_Static.gameObject.SetActive(false);
        }
    }

    [HarmonyPatch(typeof(HudManager), "ForceUpdateColor")]
    public static class ForceUpdateColorPatches
    {
        [HarmonyPostfix]
        public static void Postfix(HudManager __instance)
        {
            if (HudManagerAwakePatch.customImagesToDarken.Count <= 0)
            {
                return;
            }
            float colorValue = (float)AccessTools.Field(typeof(HudManager), "colorValue").GetValue(__instance);
            Color darkColor = (Color)AccessTools.Field(typeof(HudManager), "darkColor").GetValue(__instance);
            float colorTargetValue = (float)AccessTools.Field(typeof(HudManager), "colorTargetValue").GetValue(__instance);
            if (colorValue != float.NaN && darkColor != null && colorTargetValue != float.NaN)
            {
                if (colorValue != colorTargetValue)
                {
                    float num = Time.deltaTime * 2f;
                    colorValue = Mathf.Max(colorValue - num, Mathf.Min(colorValue + num, colorTargetValue));
                    for (int i = 0; i < HudManagerAwakePatch.customImagesToDarken.Count; i++)
                    {
                        HudManagerAwakePatch.customImagesToDarken[i].color = Color.Lerp(darkColor, Color.white, colorValue - Mathf.Repeat(colorValue, 0.2f));
                    }
                }
            }
        }
    }

    [HarmonyPatch(typeof(HudManager), "Update")]
    public class HudManagerUpdatePatch
    {
        public static string StudentState = "Idle";
        public static string StudentStateOld = "Idle";
        public static int StudentImage = 0;
        public static float StudentTimer = 0f;
        public static float StudentStateKeepTimer = 0f;
        public static float StaticTimer = 0f;

        [HarmonyPostfix]
        public static void Postfix()
        {
            if (HudManagerAwakePatch.Live_Reaction_Inner != null)
            {
                HudManagerAwakePatch.Live_Reaction_Inner.rectTransform.anchoredPosition = new Vector2(BasePlugin.Instance.configXPosition.Value, BasePlugin.Instance.configYPosition.Value);
                HudManagerAwakePatch.Live_Reaction_Inner.rectTransform.anchorMax = new Vector2(BasePlugin.Instance.configAnchorMaxAndMinX.Value, BasePlugin.Instance.configAnchorMaxAndMinY.Value);
                HudManagerAwakePatch.Live_Reaction_Inner.rectTransform.anchorMin = new Vector2(BasePlugin.Instance.configAnchorMaxAndMinX.Value, BasePlugin.Instance.configAnchorMaxAndMinY.Value);
                float flip = 1f;
                if (BasePlugin.Instance.configFlipHorizontallyEnabled.Value)
                {
                    flip = -1f;
                }
                HudManagerAwakePatch.Live_Reaction_Inner.rectTransform.localScale = new Vector3(flip, 1f, 1f);
            }
            if (BasePlugin.Instance.packList.Count > 0)
            {

                SetSprites(HudManagerAwakePatch.Live_Reaction_Inner_Sprite, "Live_Reaction_Inner_Sheet");
                SetSprites(HudManagerAwakePatch.Live_Reaction_Overlay_Sprite, "Live_Reaction_Overlay_Sheet");
                SetSprites(HudManagerAwakePatch.Live_Reaction_Static_Sprite, "Live_Reaction_Static_Sheet");

                SetSprites(HudManagerAwakePatch.Student_AlarmClock_Sprite, "Student_AlarmClock_Sheet");
                SetSprites(HudManagerAwakePatch.Student_Back_Sprite, "Student_Back_Sheet");
                SetSprites(HudManagerAwakePatch.Student_Back_Sweat_Sprite, "Student_Back_Sweat_Sheet");
                SetSprites(HudManagerAwakePatch.Student_Boots_Sprite, "Student_Boots_Sheet");
                SetSprites(HudManagerAwakePatch.Student_ChalkEraser_Sprite, "Student_ChalkEraser_Sheet");
                SetSprites(HudManagerAwakePatch.Student_DetentionKey_Sprite, "Student_DetentionKey_Sheet");
                SetSprites(HudManagerAwakePatch.Student_Detention_Sprite, "Student_Detention_Sheet");
                SetSprites(HudManagerAwakePatch.Student_DoorLock_Sprite, "Student_DoorLock_Sheet");
                SetSprites(HudManagerAwakePatch.Student_Idle_Sprite, "Student_Idle_Sheet");
                SetSprites(HudManagerAwakePatch.Student_Invisibility_Sprite, "Student_Invisibility_Sheet");
                SetSprites(HudManagerAwakePatch.Student_Key_Sprite, "Student_Key_Sheet");
                SetSprites(HudManagerAwakePatch.Student_LostItem_Sprite, "Student_LostItem_Sheet");
                SetSprites(HudManagerAwakePatch.Student_Nametag_Sprite, "Student_Nametag_Sheet");
                SetSprites(HudManagerAwakePatch.Student_NanaPeel_Sprite, "Student_NanaPeel_Sheet");
                SetSprites(HudManagerAwakePatch.Student_NoSquee_Sprite, "Student_NoSquee_Sheet");
                SetSprites(HudManagerAwakePatch.Student_Notebook_Sprite, "Student_Notebook_Sheet");
                SetSprites(HudManagerAwakePatch.Student_PortalPoster_Sprite, "Student_PortalPoster_Sheet");
                SetSprites(HudManagerAwakePatch.Student_Quarter_Sprite, "Student_Quarter_Sheet");
                SetSprites(HudManagerAwakePatch.Student_ReachExtender_Sprite, "Student_ReachExtender_Sheet");
                SetSprites(HudManagerAwakePatch.Student_Run_Sprite, "Student_Run_Sheet");
                SetSprites(HudManagerAwakePatch.Student_Scissors_Sprite, "Student_Scissors_Sheet");
                SetSprites(HudManagerAwakePatch.Student_Slipping_Sprite, "Student_Slipping_Sheet");
                SetSprites(HudManagerAwakePatch.Student_Soda_Sprite, "Student_Soda_Sheet");
                SetSprites(HudManagerAwakePatch.Student_Sticker_Sprite, "Student_Sticker_Sheet");
                SetSprites(HudManagerAwakePatch.Student_Sweat_Sprite, "Student_Sweat_Sheet");
                SetSprites(HudManagerAwakePatch.Student_Tape_Sprite, "Student_Tape_Sheet");
                SetSprites(HudManagerAwakePatch.Student_Whistle_Sprite, "Student_Whistle_Sheet");
                SetSprites(HudManagerAwakePatch.Student_YTPs_Sprite, "Student_YTPs_Sheet");
                SetSprites(HudManagerAwakePatch.Student_ZestyBar_Sprite, "Student_ZestyBar_Sheet");
                SetSprites(HudManagerAwakePatch.Student_BaldiNear_Sprite, "Student_BaldiNear_Sheet");
                SetSprites(HudManagerAwakePatch.Student_BaldiApple_Sprite, "Student_BaldiApple_Sheet");
                SetSprites(HudManagerAwakePatch.Student_EndGame_Sprite, "Student_EndGame_Sheet");
                SetSprites(HudManagerAwakePatch.Student_Squish_Sprite, "Student_Squish_Sheet");
                SetSprites(HudManagerAwakePatch.Student_Run_Sweat_Sprite, "Student_Run_Sweat_Sheet");
                SetSprites(HudManagerAwakePatch.Student_GoodGrade_Sprite, "Student_GoodGrade_Sheet");
                SetSprites(HudManagerAwakePatch.Student_BadGrade_Sprite, "Student_BadGrade_Sheet");
                SetSprites(HudManagerAwakePatch.Student_GetApple_Sprite, "Student_GetApple_Sheet");
                SetSprites(HudManagerAwakePatch.Student_GetMap_Sprite, "Student_GetMap_Sheet");
                SetSprites(HudManagerAwakePatch.Student_HideLocker_Sprite, "Student_HideLocker_Sheet");
                SetSprites(HudManagerAwakePatch.Student_GetGum_Sprite, "Student_GetGum_Sheet");
                SetSprites(HudManagerAwakePatch.Student_Teleporter_Sprite, "Student_Teleporter_Sheet");
                SetSprites(HudManagerAwakePatch.Student_Playtime_Sprite, "Student_Playtime_Sheet");
                //Mods Support: BBIMAMTMP
                if (BasePlugin.Instance.IsBBIMAMTMPInstalled)
                {
                    SetSprites(HudManagerAwakePatch.Compacts_Milk_Sprite, "Compacts_Milk_Sheet");
                }
                //Mods Support: In-Between Time-Zones - Need LSRAndRZConnector
                if (BasePlugin.Instance.IsRandomZoneInstalled)
                {
                    SetSprites(HudManagerAwakePatch.Compacts_Drawing_Happy_Sprite, "Compacts_Drawing_Happy_Sheet");
                    SetSprites(HudManagerAwakePatch.Compacts_Drawing_Mid_Sprite, "Compacts_Drawing_Mid_Sheet");
                    SetSprites(HudManagerAwakePatch.Compacts_Drawing_Scared_Sprite, "Compacts_Drawing_Scared_Sheet");
                    HudManagerAwakePatch.customStates["Happy"] = HudManagerAwakePatch.Compacts_Drawing_Happy_Sprite;
                    HudManagerAwakePatch.customStates["Mid"] = HudManagerAwakePatch.Compacts_Drawing_Mid_Sprite;
                    HudManagerAwakePatch.customStates["Scary"] = HudManagerAwakePatch.Compacts_Drawing_Scared_Sprite;
                }
                //Mods Support: Coolmovement
                if (BasePlugin.Instance.IsCoolmovementInstalled)
                {
                    SetSprites(HudManagerAwakePatch.Compacts_BounceCombo_Sprite, "Compacts_BounceCombo_Sheet");
                    SetSprites(HudManagerAwakePatch.Compacts_Dash_Sprite, "Compacts_Dash_Sheet");
                }
            }

            StudentTimer += Time.deltaTime;
            if (StudentTimer > 0.1f)
            {
                ChangeImage();
                StudentTimer = 0f;
            }
            if (StudentStateKeepTimer > 0f)
            {
                StudentStateKeepTimer -= Time.deltaTime;
            }
            else
            {
                StudentStateKeepTimer = 0f;
            }
            if (StaticTimer > 0f)
            {
                //HudManagerAwakePatch.Live_Reaction_Static.color = new UnityEngine.Color(1f, 1f, 1f, 1f);
                HudManagerAwakePatch.Live_Reaction_Static.gameObject.SetActive(true);
                StaticTimer -= Time.deltaTime;
                int sprite = 0;
                if (StaticTimer > 0.15f)
                {
                    sprite = 3;
                }
                else if (StaticTimer > 0.1f)
                {
                    sprite = 2;
                }
                else if (StaticTimer > 0.05f)
                {
                    sprite = 1;
                }
                else
                {
                    sprite = 0;
                }
                HudManagerAwakePatch.Live_Reaction_Static.sprite = HudManagerAwakePatch.Live_Reaction_Static_Sprite[sprite];
            }
            else
            {
                //HudManagerAwakePatch.Live_Reaction_Static.color = new UnityEngine.Color(0f, 0f, 0f, 0f);
                HudManagerAwakePatch.Live_Reaction_Static.gameObject.SetActive(false);
                StaticTimer = 0f;
            }
            CheckStudentState();
        }
        public static void Reset()
        {
            StudentState = "Idle";
            StudentStateOld = "Idle";
            StudentImage = 0;
            StudentTimer = 0f;
            StaticTimer = 0f;
            StudentStateKeepTimer = 0f;
            HudManagerAwakePatch.Student.sprite = HudManagerAwakePatch.Student_Idle_Sprite[0];
        }
        public static void ChangeImage()
        {
            Sprite sprite1 = HudManagerAwakePatch.Student.sprite;
            Sprite sprite2 = HudManagerAwakePatch.Live_Reaction_Inner.sprite;
            Sprite sprite3 = HudManagerAwakePatch.Live_Reaction_Overlay.sprite;
            if (StudentImage == 3)
            {
                StudentImage = 0;
            }
            else
            {
                StudentImage += 1;
            }
            if (StudentState == "BaldiNear")
            {
                sprite1 = HudManagerAwakePatch.Student_BaldiNear_Sprite[StudentImage];
            }
            else if (StudentState == "Idle")
            {
                if ((Singleton<BaseGameManager>.Instance.AllNotebooksFound || Singleton<BaseGameManager>.Instance.Ec.timeOut) && !Singleton<BaseGameManager>.Instance.InPitstop())
                {
                    sprite1 = HudManagerAwakePatch.Student_Sweat_Sprite[StudentImage];
                }
                else
                {
                    sprite1 = HudManagerAwakePatch.Student_Idle_Sprite[StudentImage];
                }
            }
            else if (StudentState == "Back")
            {
                if ((Singleton<BaseGameManager>.Instance.AllNotebooksFound || Singleton<BaseGameManager>.Instance.Ec.timeOut) && !Singleton<BaseGameManager>.Instance.InPitstop())
                {
                    sprite1 = HudManagerAwakePatch.Student_Back_Sweat_Sprite[StudentImage];
                }
                else
                {
                    sprite1 = HudManagerAwakePatch.Student_Back_Sprite[StudentImage];
                }
            }
            else if (StudentState == "Run")
            {
                if ((Singleton<BaseGameManager>.Instance.AllNotebooksFound || Singleton<BaseGameManager>.Instance.Ec.timeOut) && !Singleton<BaseGameManager>.Instance.InPitstop())
                {
                    sprite1 = HudManagerAwakePatch.Student_Run_Sweat_Sprite[StudentImage];
                }
                else
                {
                    sprite1 = HudManagerAwakePatch.Student_Run_Sprite[StudentImage];
                }
            }
            else if (StudentState == "Notebook")
            {
                sprite1 = HudManagerAwakePatch.Student_Notebook_Sprite[StudentImage];
            }
            else if (StudentState == "Sticker")
            {
                sprite1 = HudManagerAwakePatch.Student_Sticker_Sprite[StudentImage];
            }
            else if (StudentState == "Nametag")
            {
                sprite1 = HudManagerAwakePatch.Student_Nametag_Sprite[StudentImage];
            }
            else if (StudentState == "Detention")
            {
                sprite1 = HudManagerAwakePatch.Student_Detention_Sprite[StudentImage];
            }
            else if (StudentState == "LostItem")
            {
                sprite1 = HudManagerAwakePatch.Student_LostItem_Sprite[StudentImage];
            }
            else if (StudentState == "Quarter")
            {
                sprite1 = HudManagerAwakePatch.Student_Quarter_Sprite[StudentImage];
            }
            else if (StudentState == "DetentionKey")
            {
                sprite1 = HudManagerAwakePatch.Student_DetentionKey_Sprite[StudentImage];
            }
            else if (StudentState == "DoorLock")
            {
                sprite1 = HudManagerAwakePatch.Student_DoorLock_Sprite[StudentImage];
            }
            else if (StudentState == "Scissors")
            {
                sprite1 = HudManagerAwakePatch.Student_Scissors_Sprite[StudentImage];
            }
            else if (StudentState == "Tape")
            {
                sprite1 = HudManagerAwakePatch.Student_Tape_Sprite[StudentImage];
            }
            else if (StudentState == "Key")
            {
                sprite1 = HudManagerAwakePatch.Student_Key_Sprite[StudentImage];
            }
            else if (StudentState == "AlarmClock")
            {
                sprite1 = HudManagerAwakePatch.Student_AlarmClock_Sprite[StudentImage];
            }
            else if (StudentState == "Boots")
            {
                sprite1 = HudManagerAwakePatch.Student_Boots_Sprite[StudentImage];
            }
            else if (StudentState == "Soda")
            {
                sprite1 = HudManagerAwakePatch.Student_Soda_Sprite[StudentImage];
            }
            else if (StudentState == "Invisibility")
            {
                sprite1 = HudManagerAwakePatch.Student_Invisibility_Sprite[StudentImage];
            }
            else if (StudentState == "NanaPeel")
            {
                sprite1 = HudManagerAwakePatch.Student_NanaPeel_Sprite[StudentImage];
            }
            else if (StudentState == "Slipping")
            {
                sprite1 = HudManagerAwakePatch.Student_Slipping_Sprite[StudentImage];
            }
            else if (StudentState == "NoSquee")
            {
                sprite1 = HudManagerAwakePatch.Student_NoSquee_Sprite[StudentImage];
            }
            else if (StudentState == "PortalPoster")
            {
                sprite1 = HudManagerAwakePatch.Student_PortalPoster_Sprite[StudentImage];
            }
            else if (StudentState == "Whistle")
            {
                sprite1 = HudManagerAwakePatch.Student_Whistle_Sprite[StudentImage];
            }
            else if (StudentState == "ReachExtender")
            {
                sprite1 = HudManagerAwakePatch.Student_ReachExtender_Sprite[StudentImage];
            }
            else if (StudentState == "ZestyBar")
            {
                sprite1 = HudManagerAwakePatch.Student_ZestyBar_Sprite[StudentImage];
            }
            else if (StudentState == "YTPs")
            {
                sprite1 = HudManagerAwakePatch.Student_YTPs_Sprite[StudentImage];
            }
            else if (StudentState == "ChalkEraser")
            {
                sprite1 = HudManagerAwakePatch.Student_ChalkEraser_Sprite[StudentImage];
            }
            else if (StudentState == "BaldiApple")
            {
                sprite1 = HudManagerAwakePatch.Student_BaldiApple_Sprite[StudentImage];
            }
            else if (StudentState == "Squish")
            {
                sprite1 = HudManagerAwakePatch.Student_Squish_Sprite[StudentImage];
            }
            else if (StudentState == "GoodGrade")
            {
                sprite1 = HudManagerAwakePatch.Student_GoodGrade_Sprite[StudentImage];
            }
            else if (StudentState == "BadGrade")
            {
                sprite1 = HudManagerAwakePatch.Student_BadGrade_Sprite[StudentImage];
            }
            else if (StudentState == "GetApple")
            {
                sprite1 = HudManagerAwakePatch.Student_GetApple_Sprite[StudentImage];
            }
            else if (StudentState == "GetMap")
            {
                sprite1 = HudManagerAwakePatch.Student_GetMap_Sprite[StudentImage];
            }
            else if (StudentState == "HideLocker")
            {
                sprite1 = HudManagerAwakePatch.Student_HideLocker_Sprite[StudentImage];
            }
            else if (StudentState == "GetGum")
            {
                sprite1 = HudManagerAwakePatch.Student_GetGum_Sprite[StudentImage];
            }
            else if (StudentState == "Teleporter")
            {
                sprite1 = HudManagerAwakePatch.Student_Teleporter_Sprite[StudentImage];
            }
            else if (StudentState == "Playtime")
            {
                sprite1 = HudManagerAwakePatch.Student_Playtime_Sprite[StudentImage];
            }
            //Mods Support: BBIMAMTMP
            else if (StudentState == "Milk")
            {
                sprite1 = HudManagerAwakePatch.Compacts_Milk_Sprite[StudentImage];
            }
            //Mods Support: Coolmovement
            else if (StudentState == "BounceCombo")
            {
                sprite1 = HudManagerAwakePatch.Compacts_BounceCombo_Sprite[StudentImage];
            }
            else if (StudentState == "Dash")
            {
                sprite1 = HudManagerAwakePatch.Compacts_Dash_Sprite[StudentImage];
            }

            HudManagerAwakePatch.Student.sprite = sprite1;
            sprite2 = HudManagerAwakePatch.Live_Reaction_Inner_Sprite[StudentImage];
            sprite3 = HudManagerAwakePatch.Live_Reaction_Overlay_Sprite[StudentImage];
            HudManagerAwakePatch.Live_Reaction_Inner.sprite = sprite2;
            HudManagerAwakePatch.Live_Reaction_Overlay.sprite = sprite3;
        }
        public static void CheckStudentState()
        {
            if (StudentStateOld != StudentState)
            {
                StaticTimer = 0.2f;
                StudentStateOld = StudentState;
            }
        }

        public static void SetSprites(Sprite[] sprite, string spriteName)
        {
            for (int i = 0; i < sprite.Length; i++)
            {
                Sprite GetSprite = BasePlugin.AssetMan.Get<Sprite>(BasePlugin.Instance.packList[BasePlugin.Instance.configPackIndex.Value] + "-" + spriteName + "_" + i.ToString());
                Sprite GetIdleSprite = BasePlugin.AssetMan.Get<Sprite>(BasePlugin.Instance.packList[BasePlugin.Instance.configPackIndex.Value] + "-Student_Notebook_Sheet_" + i.ToString());
                Sprite VanillaSprite = BasePlugin.AssetMan.Get<Sprite>(".Vanilla-" + spriteName + "_" + i.ToString());
                Sprite VanillaIdleSprite = BasePlugin.AssetMan.Get<Sprite>(".Vanilla-Student_Notebook_Sheet_" + i.ToString());
                if (GetSprite != null)
                {
                    sprite[i] = GetSprite;
                }
                else if (GetIdleSprite != null)
                {
                    sprite[i] = GetIdleSprite;
                }
                else if (VanillaSprite != null)
                {
                    sprite[i] = VanillaSprite;
                }
                else if (VanillaIdleSprite != null)
                {
                    sprite[i] = VanillaIdleSprite;
                }
            }
        }
    }
}
