using Kingmaker.Blueprints;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace RelicsOfTheRighteous.Utilities
{
    public class Tools
    {
        internal static readonly JsonSerializerSettings _options = new()
        {
            CheckAdditionalContent = false,
            ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
            DefaultValueHandling = DefaultValueHandling.Include,
            FloatParseHandling = FloatParseHandling.Double,
            Formatting = Formatting.Indented,
            MetadataPropertyHandling = MetadataPropertyHandling.ReadAhead,
            MissingMemberHandling = MissingMemberHandling.Ignore,
            NullValueHandling = NullValueHandling.Include,
            ObjectCreationHandling = ObjectCreationHandling.Replace,
            StringEscapeHandling = StringEscapeHandling.Default,
        };

        public static void LogMessage(string msg)
        {
            Main.logger.Log(msg);
        }

        public static void AddGUIOption(string name, string description, ref bool setting)
        {
            GUILayout.BeginVertical();
            GUILayout.EndVertical();
            GUILayout.BeginHorizontal();
            GUILayout.Space(15);
            name = " " + name;
            int len = name.Length;
            do
            {
                name += "\t";
                if (name.Length >= 50) { break; }
                len += 10;
            } while (len < 49);
            name += description;
            setting = GUILayout.Toggle(setting, name, GUILayout.ExpandWidth(false));
            GUILayout.EndHorizontal();
        }

        public static Dictionary<string, string> LoadBlueprints()
        {
            Dictionary<string, string> values;

            if (!File.Exists(Main.BlueprintsFile)) { values = new(); }
            else
            {
                using (StreamReader r = new(Main.BlueprintsFile))
                {
                    values = JsonConvert.DeserializeObject<Dictionary<string, string>>(r.ReadToEnd());
                }
            }
            return values;
        }

        public static void SaveBlueprints()
        {
            File.WriteAllText(Main.BlueprintsFile, JsonConvert.SerializeObject(Main.Blueprints, _options));
            //LogMessage("DEBUG: Saved Blueprints");
        }

        public static string CreateGuid(string key)
        {
            string newGuid;
            if (Main.Blueprints.Count == 0)
            {
                newGuid = BlueprintGuid.NewGuid().ToString();
                Main.Blueprints.Add(key, newGuid);
                LogMessage("New GUID created for key = > " + key + " ==> " + newGuid);
                //SaveBlueprints(Main.Blueprints, Main.BlueprintsFile);
            }
            else if (Main.Blueprints.ContainsKey(key))
            {
                newGuid = Main.Blueprints[key];
                LogMessage("GUID found for key => " + key + " ==> " + newGuid);
            }
            else
            {
                newGuid = BlueprintGuid.NewGuid().ToString();
                Main.Blueprints.Add(key, newGuid);
                LogMessage("New GUID created for key = > " + key + " ==> " + newGuid);
                //SaveBlueprints(Main.Blueprints, Main.BlueprintsFile);
            }
            return newGuid;
        }
    }
}
