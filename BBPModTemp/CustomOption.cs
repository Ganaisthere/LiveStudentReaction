using LiveStudentReaction;
using MTM101BaldAPI.OptionsAPI;
using MTM101BaldAPI.UI;
using System;
using System.Collections.Generic;
using System.Text;
using TMPro;
using Unity.Mathematics;
using UnityEngine;

namespace LiveStudentReaction
{
    public class CustomOption : CustomOptionsCategory
    {
        private TextMeshProUGUI packText;
        private TextMeshProUGUI xPositionText;
        private TextMeshProUGUI yPositionText;
        private TextMeshProUGUI anchorMaxAndMinXText;
        private TextMeshProUGUI anchorMaxAndMinYText;
        public override void Build()
        {
            CreateText("PackTitle", "Pack", new Vector3(0f, 60f, 0f), BaldiFonts.BoldComicSans12, TextAlignmentOptions.Center, new Vector2(200f, 70f), Color.black);
            CreateButton(PackSelectionLeft, base.menuArrowLeft, base.menuArrowLeftHighlight, "PackSelectionLeftBtn", new Vector3(-163f, 34f, 0f));
            CreateButton(PackSelectionRight, base.menuArrowRight, base.menuArrowRightHighlight, "PackSelectionRightBtn", new Vector3(170f, 34f, 0f));
            packText = CreateText("PackText", "PackText", new Vector3(0f, 37f, 0f), BaldiFonts.ComicSans24, TextAlignmentOptions.Center, new Vector2(400f, 32f), Color.black);
            BasePlugin.Instance.packListText = packText;

            CreateText("XPositionTitle", "X Position", new Vector3(-100f, 5f, 0f), BaldiFonts.BoldComicSans12, TextAlignmentOptions.Center, new Vector2(200f, 70f), Color.black);
            CreateButton(XPositionLeft, base.menuArrowLeft, base.menuArrowLeftHighlight, "XPositionLeftBtn", new Vector3(-163f, -24f, 0f));
            CreateButton(XPositionRight, base.menuArrowRight, base.menuArrowRightHighlight, "XPositionRightBtn", new Vector3(-40f, -24f, 0f));
            xPositionText = CreateText("XPositionText", "XPositionText", new Vector3(-100f, -21f, 0f), BaldiFonts.ComicSans24, TextAlignmentOptions.Center, new Vector2(400f, 32f), Color.black);
            BasePlugin.Instance.xPositionText = xPositionText;

            CreateText("YPositionTitle", "Y Position", new Vector3(100f, 5f, 0f), BaldiFonts.BoldComicSans12, TextAlignmentOptions.Center, new Vector2(200f, 70f), Color.black);
            CreateButton(YPositionLeft, base.menuArrowLeft, base.menuArrowLeftHighlight, "YPositionLeftBtn", new Vector3(40f, -24f, 0f));
            CreateButton(YPositionRight, base.menuArrowRight, base.menuArrowRightHighlight, "YPositionRightBtn", new Vector3(170f, -24f, 0f));
            yPositionText = CreateText("YPositionText", "YPositionText", new Vector3(100f, -21f, 0f), BaldiFonts.ComicSans24, TextAlignmentOptions.Center, new Vector2(400f, 32f), Color.black);
            BasePlugin.Instance.yPositionText = yPositionText;

            CreateText("AnchorMaxAndMinXTitle", "Anchor Max/Min X", new Vector3(-100f, -50f, 0f), BaldiFonts.BoldComicSans12, TextAlignmentOptions.Center, new Vector2(200f, 70f), Color.black);
            CreateButton(AnchorMaxAndMinXLeft, base.menuArrowLeft, base.menuArrowLeftHighlight, "AnchorMaxAndMinXLeftBtn", new Vector3(-163f, -79f, 0f));
            CreateButton(AnchorMaxAndMinXRight, base.menuArrowRight, base.menuArrowRightHighlight, "AnchorMaxAndMinXRightBtn", new Vector3(-40f, -79f, 0f));
            anchorMaxAndMinXText = CreateText("AnchorMaxAndMinXText", "AnchorMaxAndMinXText", new Vector3(-100f, -76f, 0f), BaldiFonts.ComicSans24, TextAlignmentOptions.Center, new Vector2(400f, 32f), Color.black);
            BasePlugin.Instance.anchorMaxAndMinXText = anchorMaxAndMinXText;

            CreateText("AnchorMaxAndMinYTitle", "Anchor Max/Min Y", new Vector3(100f, -50f, 0f), BaldiFonts.BoldComicSans12, TextAlignmentOptions.Center, new Vector2(200f, 70f), Color.black);
            CreateButton(AnchorMaxAndMinYLeft, base.menuArrowLeft, base.menuArrowLeftHighlight, "AnchorMaxAndMinYLeftBtn", new Vector3(40f, -79f, 0f));
            CreateButton(AnchorMaxAndMinYRight, base.menuArrowRight, base.menuArrowRightHighlight, "AnchorMaxAndMinYRightBtn", new Vector3(170f, -79f, 0f));
            anchorMaxAndMinYText = CreateText("AnchorMaxAndMinYText", "AnchorMaxAndMinYText", new Vector3(100f, -76f, 0f), BaldiFonts.ComicSans24, TextAlignmentOptions.Center, new Vector2(400f, 32f), Color.black);
            BasePlugin.Instance.anchorMaxAndMinYText = anchorMaxAndMinYText;

            MenuToggle menuToggle0 = CreateToggle("FlipHorizontallyToggle", "Flip Horizontally", BasePlugin.Instance.FlipHorizontallyEnabled, new Vector3(140f, -120f, 0f), 330f);
            AddTooltip(menuToggle0, "If enabled, the image of 'Player Tv' will flip left and right.");
            BasePlugin.Instance.configFlipHorizontallyToggle = menuToggle0;

            MenuToggle menuToggle1 = CreateToggle("BaldiNearToggle", "'Baldi Near' Reactions", BasePlugin.Instance.BaldiNearEnabled, new Vector3(140f, -155f, 0f), 420f);
            AddTooltip(menuToggle1, "If enabled, the reaction of 'Baldi Near' will showen on 'Player TV'.\n(Reminder: The display condition for 'Baldi Near' Reactions is that\nBaldi is within 4 blocks of you, which will affect the game balance.)");
            BasePlugin.Instance.configBaldiNearToggle = menuToggle1;

            CreateTextButton(Reset, "Reset", "Reset", new Vector3(104f, 80f, 0f), BaldiFonts.ComicSans24, TextAlignmentOptions.BottomRight, new Vector2(100f, 32f), Color.black);

            BasePlugin.Instance.optionsMenuBuilt = true;
        }

        private void PackSelectionLeft()
        {
            BasePlugin.Instance.packIndex--;
            if (BasePlugin.Instance.packIndex < 0)
            {
                BasePlugin.Instance.packIndex = Mathf.Max(0, BasePlugin.Instance.packList.Count - 1);
            }
        }

        private void PackSelectionRight()
        {
            BasePlugin.Instance.packIndex++;
            if (BasePlugin.Instance.packIndex > BasePlugin.Instance.packList.Count - 1)
            {
                BasePlugin.Instance.packIndex = 0;
            }
        }

        private void XPositionLeft()
        {
            BasePlugin.Instance.xPosition -= 5f;
        }

        private void XPositionRight()
        {
            BasePlugin.Instance.xPosition += 5f;
        }

        private void YPositionLeft()
        {
            BasePlugin.Instance.yPosition -= 5f;
        }

        private void YPositionRight()
        {
            BasePlugin.Instance.yPosition += 5f;
        }

        private void AnchorMaxAndMinXLeft()
        {
            float edit1 = BasePlugin.Instance.anchorMaxAndMinX * 100f;
            edit1 -= 5f;
            float edit2 = math.round(edit1);
            edit2 = edit2 / 100f;
            BasePlugin.Instance.anchorMaxAndMinX = edit2;
        }

        private void AnchorMaxAndMinXRight()
        {
            float edit1 = BasePlugin.Instance.anchorMaxAndMinX * 100f;
            edit1 += 5f;
            float edit2 = math.round(edit1);
            edit2 = edit2 / 100f;
            BasePlugin.Instance.anchorMaxAndMinX = edit2;
        }

        private void AnchorMaxAndMinYLeft()
        {
            float edit1 = BasePlugin.Instance.anchorMaxAndMinY * 100f;
            edit1 -= 5f;
            float edit2 = math.round(edit1);
            edit2 /= 100f;
            BasePlugin.Instance.anchorMaxAndMinY = edit2;
        }

        private void AnchorMaxAndMinYRight()
        {
            float edit1 = BasePlugin.Instance.anchorMaxAndMinY * 100f;
            edit1 += 5f;
            float edit2 = math.round(edit1);
            edit2 /= 100f;
            BasePlugin.Instance.anchorMaxAndMinY = edit2;
        }

        private void Reset()
        {
            BasePlugin.Instance.packIndex = 0;
            BasePlugin.Instance.xPosition = -90f;
            BasePlugin.Instance.yPosition = -145f;
            BasePlugin.Instance.anchorMaxAndMinX = 1f;
            BasePlugin.Instance.anchorMaxAndMinY = 1f;
            BasePlugin.Instance.configFlipHorizontallyToggle.Set(false);
            BasePlugin.Instance.configBaldiNearToggle.Set(false);
        }
    }
}
