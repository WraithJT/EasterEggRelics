using EasterEggRelics.Utilities;
using HarmonyLib;
using UnityEngine;
using UnityModManagerNet;

namespace EasterEggRelics
{
    public class Main
    {
        internal static UnityModManager.ModEntry.ModLogger logger;
        internal static UnityModManager.ModEntry ModContext_EER;
        public static Settings Settings;
        public static bool Enabled;

        static bool Load(UnityModManager.ModEntry modEntry)
        {
            var harmony = new Harmony(modEntry.Info.Id);
            ModContext_EER = modEntry;
            //Harmony.DEBUG = true;
            logger = modEntry.Logger;
            Settings = Settings.Load<Settings>(modEntry);
            modEntry.OnToggle = OnToggle;
            modEntry.OnGUI = OnGUI;
            modEntry.OnSaveGUI = OnSaveGUI;
            harmony.PatchAll();
            return true;
        }

        static bool OnToggle(UnityModManager.ModEntry modEntry, bool value)
        {
            Enabled = value;
            return true;
        }

        static void OnGUI(UnityModManager.ModEntry modEntry)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("GAME MUST BE RESTARTED FOR CHANGES TO TAKE EFFECT");
            GUILayout.EndHorizontal();

            Tools.AddGUIOption("Companion Ascension",
                "Enables the Ascension of Companions. Disabling this setting will disable all Companion Ascension features",
                ref Settings.useEasterEggRelics);
        }

        static void OnSaveGUI(UnityModManager.ModEntry modEntry)
        {
            Settings.Save(modEntry);
        }
    }
}
