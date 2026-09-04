using BepInEx;
using BepInEx.Bootstrap;
using BepInEx.Configuration;
using HarmonyLib;
using MTM101BaldAPI;
using MTM101BaldAPI.AssetTools;
using MTM101BaldAPI.OptionsAPI;
using MTM101BaldAPI.Registers;
using MTM101BaldAPI.SaveSystem;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

namespace LiveStudentReaction
{
    [BepInPlugin("ganaisthere.plus.livestudentreaction", "Live Student Reaction", "0.5.0.0")]
    [BepInDependency("mtm101.rulerp.bbplus.baldidevapi")]
    //Mods Support
    [BepInDependency("com.milk.item", BepInDependency.DependencyFlags.SoftDependency)]
    [BepInDependency("skid.coolmovement", BepInDependency.DependencyFlags.SoftDependency)]
    public class BasePlugin : BaseUnityPlugin
    {
        public TextMeshProUGUI packListText;
        public List<string> packList = new List<string>();
        public int packIndex = 0;
        public ConfigEntry<int> configPackIndex;
        public int packOptionsIndex => configPackIndex.Value;
        //----------------------------------------------------------------------
        public TextMeshProUGUI xPositionText;
        public float xPosition = -90f;
        public ConfigEntry<float> configXPosition;
        public float optionsXPosition => configXPosition.Value;
        //----------------------------------------------------------------------
        public TextMeshProUGUI yPositionText;
        public float yPosition = -120f;
        public ConfigEntry<float> configYPosition;
        public float optionsYPosition => configYPosition.Value;
        //----------------------------------------------------------------------
        public TextMeshProUGUI anchorMaxAndMinXText;
        public float anchorMaxAndMinX = 1f;
        public ConfigEntry<float> configAnchorMaxAndMinX;
        public float optionsAnchorMaxAndMinX => configAnchorMaxAndMinX.Value;
        //----------------------------------------------------------------------
        public TextMeshProUGUI anchorMaxAndMinYText;
        public float anchorMaxAndMinY = 1f;
        public ConfigEntry<float> configAnchorMaxAndMinY;
        public float optionsAnchorMaxAndMinY => configAnchorMaxAndMinY.Value;
        //----------------------------------------------------------------------
        public ConfigEntry<bool> configFlipHorizontallyEnabled;
        public MenuToggle configFlipHorizontallyToggle;
        public bool FlipHorizontallyEnabled => configFlipHorizontallyEnabled.Value;
        //----------------------------------------------------------------------
        public ConfigEntry<bool> configBaldiNearEnabled;
        public MenuToggle configBaldiNearToggle;
        public bool BaldiNearEnabled => configBaldiNearEnabled.Value;

        public bool optionsMenuBuilt = false;
        public bool IsItsBaldiTimeInstalled = false;
        public bool IsBBIMAMTMPInstalled = false;
        public bool IsRandomZoneInstalled = false;
        public bool IsCoolmovementInstalled = false;
        public static BasePlugin Instance { get; private set; }
        public void Awake()
        {
            Instance = this;
            new Harmony("ganaisthere.plus.livestudentreaction").PatchAllConditionals();
            //ModdedSaveGame.AddSaveHandler(base.Info);

            configPackIndex = Config.Bind
            (
                "General",
                "Pack Index",
                0,
                "The index of the selected 'Player TV' pack."
            );
            configXPosition = Config.Bind
            (
                "General",
                "X Position",
                -90f,
                "The X Position of the 'Player TV'."
            );
            configYPosition = Config.Bind
            (
                "General",
                "Y Position",
                -145f,
                "The Y Position of the 'Player TV'."
            );
            configAnchorMaxAndMinX = Config.Bind
            (
                "General",
                "Anchor Max/Min X",
                1f,
                "The Anchor Max/Min X Position of the 'Player TV'."
            );
            configAnchorMaxAndMinY = Config.Bind
            (
                "General",
                "Anchor Max/Min Y",
                1f,
                "The Anchor Max/Min Y Position of the 'Player TV'."
            );
            configFlipHorizontallyEnabled = Config.Bind
            (
                "General",
                "Flip Horizontally",
                false,
                "If true, the image of 'Player TV' will flip left and right."
            );
            configBaldiNearEnabled = Config.Bind
            (
                "General",
                "Baldi Near Reactions",
                false,
                "If true, the reaction of 'Baldi Near' will showen on 'Player TV'.(Reminder: The display condition for 'Baldi Near' Reactions is that Baldi is within 4 blocks of you, which will affect the game balance.)"
            );

            LoadingEvents.RegisterOnAssetsLoaded(base.Info, this.LoadAssets(), LoadingEventOrder.Start);

            packIndex = packOptionsIndex;
            xPosition = optionsXPosition;
            yPosition = optionsYPosition;
            anchorMaxAndMinX = optionsAnchorMaxAndMinX;
            anchorMaxAndMinY = optionsAnchorMaxAndMinY;
            CustomOptionsCore.OnMenuInitialize += OnMen;
        }

        public void Update()
        {
            IsItsBaldiTimeInstalled = Chainloader.PluginInfos.ContainsKey("ganaisthere.plus.itsbalditime");
            IsBBIMAMTMPInstalled = Chainloader.PluginInfos.ContainsKey("com.milk.item");
            IsRandomZoneInstalled = Chainloader.PluginInfos.ContainsKey("wazkitta.plusmod.pluszones");
            IsCoolmovementInstalled = Chainloader.PluginInfos.ContainsKey("skid.coolmovement");

            if (optionsMenuBuilt && configFlipHorizontallyToggle != null && configBaldiNearToggle != null)
            {
                configPackIndex.Value = packIndex;
                configXPosition.Value = xPosition;
                configYPosition.Value = yPosition;
                configAnchorMaxAndMinX.Value = anchorMaxAndMinX;
                configAnchorMaxAndMinY.Value = anchorMaxAndMinY;
                configFlipHorizontallyEnabled.Value = configFlipHorizontallyToggle.Value;
                configBaldiNearEnabled.Value = configBaldiNearToggle.Value;

                if (packIndex >= packList.Count)
                {
                    packIndex = 0;
                }
                if (packList.Count > 0)
                {
                    packListText.text = packList[packIndex];
                }
                xPositionText.text = xPosition.ToString();
                yPositionText.text = yPosition.ToString();
                anchorMaxAndMinXText.text = anchorMaxAndMinX.ToString();
                anchorMaxAndMinYText.text = anchorMaxAndMinY.ToString();
            }
        }

        private void OnMen(OptionsMenu __instance, CustomOptionsHandler handler)
        {
            handler.AddCategory<CustomOption>("Player TV\nOptions");
        }

        public static AssetManager AssetMan = new AssetManager();
        private IEnumerator LoadAssets()
        {
            yield return 12;
            yield return 25;
            yield return "Prepare...";

            string[] allPack = Directory.GetDirectories(AssetLoader.GetModPath(this), "*", SearchOption.TopDirectoryOnly);
            if (allPack.Length > 0)
            {
                foreach (string pack in allPack)
                {
                    string pathName = Path.GetFileName(pack);
                    string[] allFiles = Directory.GetFiles(pack, "*.png");
                    packList.Add(pathName);
                    foreach (string file in allFiles)
                    {
                        string fileName = Path.GetFileNameWithoutExtension(file);
                        yield return fileName.Length;
                        yield return "Loading Assets: " + pathName;
                        AssetMan.Add<Texture2D>(pathName + "-" + fileName, AssetLoader.TextureFromFile(file));
                        Texture2D texture2D = AssetMan.Get<Texture2D>(pathName + "-" + fileName);
                        if (fileName.Contains("_Sheet"))
                        {
                            for (int i = 0; i < 4; i = i + 1)
                            {
                                Sprite sprite = Sprite.Create(texture2D, new Rect(texture2D.width / 4 * i, 0, texture2D.width / 4, texture2D.height), new Vector2((float)0.5, (float)0.5));
                                sprite.name = pathName + "-" + fileName + "_" + i.ToString();
                                AssetMan.Add<Sprite>(pathName + "-" + fileName + "_" + i.ToString(), sprite);
                                //Logger.LogInfo("Loading Assets: " + pathName + "-" + fileName + "_" + i.ToString());
                            }
                        }
                        else
                        {
                            Sprite sprite = Sprite.Create(texture2D, new Rect(0, 0, texture2D.width, texture2D.height), new Vector2((float)0.5, (float)0.5));
                            sprite.name = pathName + "-" + fileName;
                            AssetMan.Add<Sprite>(pathName + "-" + fileName, sprite);
                            //Logger.LogInfo("Loading Assets: " + pathName + "-" + fileName);
                            //cnm为什么加载的时候能用，到HUDMANAGER启动的时候都变成滚木了
                            //why
                        }
                    }
                }
            }
            else
            {
                while (1 == 1)
                {
                    Logger.LogInfo("No Pack Found, Please Check Mod's Folder.\n(The Game Will Be Freeze)");
                    yield return "No Pack Found, Please Check Mod's Folder.\n(The Game Will Be Freeze)";
                }
            }

            yield break;
        }

        private void QuickAddTexture2D(string textureNameWithExtension)
        {
            string getfile = Directory.GetFiles(AssetLoader.GetModPath(this), textureNameWithExtension)[0];
            AssetMan.Add<Texture2D>(Path.GetFileNameWithoutExtension(getfile), AssetLoader.TextureFromFile(getfile));
        }
        private void QuickAddTexture2D(string textureNameWithExtension, string chlidPath)
        {
            string getfile = Directory.GetFiles(AssetLoader.GetModPath(this) + "/" + chlidPath + "/", textureNameWithExtension)[0];
            AssetMan.Add<Texture2D>(Path.GetFileNameWithoutExtension(getfile), AssetLoader.TextureFromFile(getfile));
        }
    }
}
