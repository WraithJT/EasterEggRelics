using BlueprintCore.Blueprints.Configurators.Classes;
using BlueprintCore.Blueprints.Configurators.Kingdom;
using BlueprintCore.Utils;
using RelicsOfTheRighteous.Utilities;
using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Items.Weapons;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.UnitLogic.Mechanics.Properties;
using System;
using BlueprintCore.Conditions.Builder;
using BlueprintCore.Conditions.Builder.KingdomEx;
using BlueprintCore.Conditions.Builder.BasicEx;
using BlueprintCore.Conditions.Builder.AreaEx;
using BlueprintCore.Conditions.Builder.ContextEx;
using BlueprintCore.Conditions.Builder.MiscEx;
using BlueprintCore.Conditions.Builder.NewEx;
using BlueprintCore.Conditions.Builder.StoryEx;
using Kingmaker.Kingdom;
using Kingmaker.Kingdom.Blueprints;
using Kingmaker.UnitLogic.Alignments;
using Kingmaker.ElementsSystem;
using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.AreaEx;
using BlueprintCore.Actions.Builder.AVEx;
using BlueprintCore.Actions.Builder.BasicEx;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Actions.Builder.KingdomEx;
using BlueprintCore.Actions.Builder.MiscEx;
using BlueprintCore.Actions.Builder.StoryEx;
using BlueprintCore.Actions.Builder.UpgraderEx;
using Kingmaker.RuleSystem;
using Kingmaker.AreaLogic.QuestSystem;
using Kingmaker.Blueprints.Quests;
using RelicsOfTheRighteous.Utilities.TTTCore;
using BlueprintCore.Blueprints.Configurators;
using Kingmaker.Blueprints.Items;
using BlueprintCore.Blueprints.Configurators.Items;
using System.Collections.Generic;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.Blueprints.Items.Equipment;
using Kingmaker.Localization;
using System.Linq;
using BlueprintCore.Blueprints.Configurators.Items.Weapons;
using Kingmaker.View.Equipment;
using BlueprintCore.Blueprints.Configurators.AreaLogic.Etudes;
using static Kingmaker.UI.Common.ItemsFilter;
using Kingmaker.Blueprints.Items.Armors;
using BlueprintCore.Blueprints.Configurators.Items.Armors;
using Kingmaker.Blueprints.Items.Shields;
using Kingmaker.Blueprints.Items.Ecnchantments;
using Kingmaker.Blueprints.Items.Components;
using static RelicsOfTheRighteous.Utilities.RelicQuestBuilder;
using BlueprintCore.Blueprints.Configurators.Items.Shields;
using BlueprintCore.Blueprints.Configurators.Items.Equipment;

namespace RelicsOfTheRighteous.NewContent.KingdomProjects
{
    class TestingArea
    {
        [HarmonyPatch(typeof(BlueprintsCache), "Init")]
        static class BlueprintsCache_Init_patch
        {
            static bool Initialized;

            [HarmonyPriority(Priority.First)]
            static void Postfix()
            {
                if (Initialized) return;
                Initialized = true;

                //PatchTestingArea();
                //try { PatchHadesSword_ReforgeProject(); }
                //catch (Exception ex) { Tools.LogMessage("EXCEPTION: " + ex.ToString()); }
            }

            static void PatchTestingArea()
            {
                var LongswordPlus1 = ResourcesLibrary.TryGetBlueprint<BlueprintItemWeapon>("03d706655c07d804cb9d5a5583f9aec5");
                var DaggerPlus1 = ResourcesLibrary.TryGetBlueprint<BlueprintItemWeapon>("2a45458f776442e43bba57de65f9b738");

                RelicItem finalItem1_Enchanted1_Config = new()
                {
                    Name = "FinalHadesSword1",
                    Guid = BlueprintGuid.NewGuid().ToString(),
                    RelicItemType = ItemType.Weapon,
                    DisplayName = LocalizationTool.CreateString(BlueprintGuid.NewGuid().ToString(), "Final Hades Sword 1", false),
                    Description = LocalizationTool.CreateString(BlueprintGuid.NewGuid().ToString(), "Final Hades Sword 1 descirption", false),
                    FlavorText = LocalizationTool.CreateString(BlueprintGuid.NewGuid().ToString(), "Final Hades Sword 1 flavor", false)
                };

                //var finalItem1_Enchanted1 = (BlueprintItemWeapon)ItemConstructor(finalItem1_Enchanted1_Config);
                //Main.logger.Log("DEBUG: Constructed Enchanted Item 1-1 => " + finalItem1_Enchanted1.AssetGuidThreadSafe);

                var finalItem1_Enchanted1 = ItemWeaponConfigurator.New("FinalHadesSword1", "DD33CFD8-256F-4F58-84EA-579BE143043F")
                    .SetDisplayNameText(LocalizationTool.CreateString("9D3A4B8E-9AEA-46CE-B22C-04AE21353096", "Final Hades Sword 1", false))
                    .SetDescriptionText(LocalizationTool.CreateString("A6D33B9B-04BD-455C-8E6C-3C70BCEFF1A8", "Final Hades Sword 1 descirption", false))
                    .SetFlavorText(LocalizationTool.CreateString("9F79AB87-785C-44B4-A6B4-72AE274D093A", "Final Hades Sword 1 flavor", false))
                    .SetType(WeaponTypes.Longsword)
                    .SetIcon(LongswordPlus1.Icon)
                    .SetCost(5000)
                    .AddToEnchantments("d42fc23b92c640846ac137dc26e000d4")
                    .SetVisualParameters(new WeaponVisualParameters
                    {
                        m_Projectiles = Array.Empty<BlueprintProjectileReference>(),
                        m_PossibleAttachSlots = Array.Empty<UnitEquipmentVisualSlotType>(),
                    })
                    
                    .SetCR(13)
                    .Configure();
                Main.logger.Log("DEBUG: Built Enchanted Item 1-1 => " + finalItem1_Enchanted1.AssetGuidThreadSafe);
            }
        }
    }
}
