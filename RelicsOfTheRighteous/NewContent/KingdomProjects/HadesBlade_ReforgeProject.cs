using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.AreaEx;
using BlueprintCore.Actions.Builder.AVEx;
using BlueprintCore.Actions.Builder.BasicEx;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Actions.Builder.KingdomEx;
using BlueprintCore.Actions.Builder.MiscEx;
using BlueprintCore.Actions.Builder.StoryEx;
using BlueprintCore.Actions.Builder.UpgraderEx;
using BlueprintCore.Blueprints.Configurators;
using BlueprintCore.Blueprints.Configurators.AreaLogic.Etudes;
using BlueprintCore.Blueprints.Configurators.Classes;
using BlueprintCore.Blueprints.Configurators.Items;
using BlueprintCore.Blueprints.Configurators.Items.Armors;
using BlueprintCore.Blueprints.Configurators.Items.Equipment;
using BlueprintCore.Blueprints.Configurators.Items.Shields;
using BlueprintCore.Blueprints.Configurators.Items.Weapons;
using BlueprintCore.Blueprints.Configurators.Kingdom;
using BlueprintCore.Conditions.Builder;
using BlueprintCore.Conditions.Builder.AreaEx;
using BlueprintCore.Conditions.Builder.BasicEx;
using BlueprintCore.Conditions.Builder.ContextEx;
using BlueprintCore.Conditions.Builder.KingdomEx;
using BlueprintCore.Conditions.Builder.MiscEx;
using BlueprintCore.Conditions.Builder.NewEx;
using BlueprintCore.Conditions.Builder.StoryEx;
using BlueprintCore.Utils;
using HarmonyLib;
using Kingmaker.AreaLogic.QuestSystem;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Items;
using Kingmaker.Blueprints.Items.Armors;
using Kingmaker.Blueprints.Items.Components;
using Kingmaker.Blueprints.Items.Ecnchantments;
using Kingmaker.Blueprints.Items.Equipment;
using Kingmaker.Blueprints.Items.Shields;
using Kingmaker.Blueprints.Items.Weapons;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Blueprints.Quests;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.Designers.EventConditionActionSystem.Conditions;
using Kingmaker.ElementsSystem;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.Kingdom;
using Kingmaker.Kingdom.Blueprints;
using Kingmaker.Localization;
using Kingmaker.RuleSystem;
using Kingmaker.TextTools;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Alignments;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.UnitLogic.Mechanics.Properties;
using Kingmaker.View.Equipment;
using RelicsOfTheRighteous.Utilities;
using RelicsOfTheRighteous.Utilities.TTTCore;
using System;
using System.Collections.Generic;
using System.Linq;
using static Kingmaker.UI.Common.ItemsFilter;
using static RelicsOfTheRighteous.Utilities.RelicQuestBuilder;

namespace RelicsOfTheRighteous.NewContent.KingdomProjects
{
    class HadesBlade_ReforgeProject
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

                PatchHadesBlade_ReforgeProject();
                //try { PatchHadesSword_ReforgeProject(); }
                //catch (Exception ex) { Tools.LogMessage("EXCEPTION: " + ex.ToString()); }
            }

            public static void PatchHadesBlade_ReforgeProject()
            {
                #region Variable Declaration (don't touch)
                string unlockFlag1Name;
                string unlockFlag1Guid;
                string unlockFlag2Name;
                string unlockFlag2Guid;

                string itemName;
                string itemDisplayName;
                string itemDescription;
                string itemFlavorText;

                string eventName;
                string eventDisplayName;
                string eventDescription;

                string etudeName;
                string etudeGuid;

                string projectName;
                string projectDisplayName;
                string projectDescription;
                string projectDefaultResolutionDescription;
                string projectMechanicalDescription;
                bool projectIsAct3;
                #endregion

                /*/ NOTES
                 * figure out Choice Effects preview (TextTemplateEngine, s_TemplatesByTag)
                 * 
                /*/

                // References
                var LongswordPlus1 = ResourcesLibrary.TryGetBlueprint<BlueprintItemWeapon>("03d706655c07d804cb9d5a5583f9aec5");
                var DaggerPlus1 = ResourcesLibrary.TryGetBlueprint<BlueprintItemWeapon>("2a45458f776442e43bba57de65f9b738");

                // Relic quest initiating item
                BlueprintItemWeapon requiredItem = ResourcesLibrary.TryGetBlueprint<BlueprintItemWeapon>("372e460865964e4a8e98ebf73f0741ae");

                #region Component Variables
                // One UnlockableFlag for each item type (sword, ring, usable item, etc)
                unlockFlag1Name = "FlagHadesBladeSwordProject_Enchanting";
                unlockFlag1Guid = "fed37ea46a3d4d6c9397765b58ccd9c8";
                var unlockFlag1 = UnlockableFlagConfigurator.New(unlockFlag1Name, unlockFlag1Guid).Configure();

                unlockFlag2Name = "FlagHadesBladeDaggerProject_Enchanting";
                unlockFlag2Guid = "f9ad581191f3408e8647e56a38b47106";
                var unlockFlag2 = UnlockableFlagConfigurator.New(unlockFlag2Name, unlockFlag2Guid).Configure();

                // One final item for each item type and enchantment path. All items share the same enchantment paths
                // Match the BlueprintItem type to the ItemType
                itemName = "FinalHadesSword1";
                itemDisplayName = "Final Hades Sword 1";
                itemDescription = "Final Hades Sword 1 descirption";
                itemFlavorText = "Final Hades Sword 1 flavor";
                ItemType itemType = ItemType.Weapon;
                RelicItem finalItem1_Enchanted1_Config = new()
                {
                    Name = itemName,
                    Guid = Tools.CreateGuid(itemName),
                    RelicItemType = itemType,
                    DisplayName = LocalizationTool.CreateString(Tools.CreateGuid(itemName + "DNKey"), itemDisplayName, false),
                    Description = LocalizationTool.CreateString(Tools.CreateGuid(itemName + "DescKey"), itemDescription),
                    FlavorText = LocalizationTool.CreateString(Tools.CreateGuid(itemName + "FTKey"), itemFlavorText, false)
                };
                var finalItem1_Enchanted1 = (BlueprintItemWeapon)ItemConstructor(finalItem1_Enchanted1_Config);
                finalItem1_Enchanted1 = ItemWeaponConfigurator.For(finalItem1_Enchanted1)
                    .SetType(WeaponTypes.Longsword)
                    .SetIcon(LongswordPlus1.Icon)
                    .SetCost(5000)
                    .SetVisualParameters(new WeaponVisualParameters
                    {
                        m_Projectiles = Array.Empty<BlueprintProjectileReference>(),
                        m_PossibleAttachSlots = Array.Empty<UnitEquipmentVisualSlotType>(),
                    })
                    .AddToEnchantments("d42fc23b92c640846ac137dc26e000d4")
                    .SetCR(13)
                    .Configure();

                itemName = "FinalHadesSword2";
                itemDisplayName = "Final Hades Sword 2";
                itemDescription = "Final Hades Sword 2 desc";
                itemFlavorText = "Final Hades Sword 2 flavor";
                itemType = ItemType.Weapon;
                RelicItem finalItem1_Enchanted2_Config = new()
                {
                    Name = itemName,
                    Guid = Tools.CreateGuid(itemName),
                    RelicItemType = itemType,
                    DisplayName = LocalizationTool.CreateString(Tools.CreateGuid(itemName + "DNKey"), itemDisplayName, false),
                    Description = LocalizationTool.CreateString(Tools.CreateGuid(itemName + "DescKey"), itemDescription),
                    FlavorText = LocalizationTool.CreateString(Tools.CreateGuid(itemName + "FTKey"), itemFlavorText, false)
                };
                var finalItem1_Enchanted2 = (BlueprintItemWeapon)ItemConstructor(finalItem1_Enchanted2_Config);
                finalItem1_Enchanted2 = ItemWeaponConfigurator.For(finalItem1_Enchanted2)
                    .SetType(WeaponTypes.Longsword)
                    .SetIcon(LongswordPlus1.Icon)
                    .SetCost(5000)
                    .SetVisualParameters(new WeaponVisualParameters
                    {
                        m_Projectiles = Array.Empty<BlueprintProjectileReference>(),
                        m_PossibleAttachSlots = Array.Empty<UnitEquipmentVisualSlotType>(),
                    })
                    .AddToEnchantments("d42fc23b92c640846ac137dc26e000d4")
                    .SetCR(13)
                    .Configure();

                itemName = "FinalHadesSword3";
                itemDisplayName = "Final Hades Sword 3";
                itemDescription = "Final Hades Sword 3 descr";
                itemFlavorText = "Final Hades Sword 3 flavy";
                itemType = ItemType.Weapon;
                RelicItem finalItem1_Enchanted3_Config = new()
                {
                    Name = itemName,
                    Guid = Tools.CreateGuid(itemName),
                    RelicItemType = itemType,
                    DisplayName = LocalizationTool.CreateString(Tools.CreateGuid(itemName + "DNKey"), itemDisplayName, false),
                    Description = LocalizationTool.CreateString(Tools.CreateGuid(itemName + "DescKey"), itemDescription),
                    FlavorText = LocalizationTool.CreateString(Tools.CreateGuid(itemName + "FTKey"), itemFlavorText, false)
                };
                var finalItem1_Enchanted3 = (BlueprintItemWeapon)ItemConstructor(finalItem1_Enchanted3_Config);
                finalItem1_Enchanted3 = ItemWeaponConfigurator.For(finalItem1_Enchanted3)
                    .SetType(WeaponTypes.Longsword)
                    .SetIcon(LongswordPlus1.Icon)
                    .SetCost(5000)
                    .SetVisualParameters(new WeaponVisualParameters
                    {
                        m_Projectiles = Array.Empty<BlueprintProjectileReference>(),
                        m_PossibleAttachSlots = Array.Empty<UnitEquipmentVisualSlotType>(),
                    })
                    .AddToEnchantments("d42fc23b92c640846ac137dc26e000d4")
                    .SetCR(13)
                    .Configure();

                itemName = "FinalHadesDagger1";
                itemDisplayName = "Final Hades Dagger 1";
                itemDescription = "Final Hades Dagger 1 stuff";
                itemFlavorText = "Final Hades Dagger 1 falava";
                itemType = ItemType.Weapon;
                RelicItem finalItem2_Enchanted1_Config = new()
                {
                    Name = itemName,
                    Guid = Tools.CreateGuid(itemName),
                    RelicItemType = itemType,
                    DisplayName = LocalizationTool.CreateString(Tools.CreateGuid(itemName + "DNKey"), itemDisplayName, false),
                    Description = LocalizationTool.CreateString(Tools.CreateGuid(itemName + "DescKey"), itemDescription),
                    FlavorText = LocalizationTool.CreateString(Tools.CreateGuid(itemName + "FTKey"), itemFlavorText, false)
                };
                var finalItem2_Enchanted1 = (BlueprintItemWeapon)ItemConstructor(finalItem2_Enchanted1_Config);
                finalItem2_Enchanted1 = ItemWeaponConfigurator.For(finalItem2_Enchanted1)
                    .SetType(WeaponTypes.Dagger)
                    .SetIcon(DaggerPlus1.Icon)
                    .SetCost(5000)
                    .SetVisualParameters(new WeaponVisualParameters
                    {
                        m_Projectiles = Array.Empty<BlueprintProjectileReference>(),
                        m_PossibleAttachSlots = Array.Empty<UnitEquipmentVisualSlotType>(),
                    })
                    .AddToEnchantments("d42fc23b92c640846ac137dc26e000d4")
                    .SetCR(13)
                    .Configure();

                itemName = "FinalHadesDagger2";
                itemDisplayName = "Final Hades Dagger 2";
                itemDescription = "Final Hades Dagger 2 words";
                itemFlavorText = "Final Hades Dagger 2 asdf";
                itemType = ItemType.Weapon;
                RelicItem finalItem2_Enchanted2_Config = new()
                {
                    Name = itemName,
                    Guid = Tools.CreateGuid(itemName),
                    RelicItemType = itemType,
                    DisplayName = LocalizationTool.CreateString(Tools.CreateGuid(itemName + "DNKey"), itemDisplayName, false),
                    Description = LocalizationTool.CreateString(Tools.CreateGuid(itemName + "DescKey"), itemDescription),
                    FlavorText = LocalizationTool.CreateString(Tools.CreateGuid(itemName + "FTKey"), itemFlavorText, false)
                };
                var finalItem2_Enchanted2 = (BlueprintItemWeapon)ItemConstructor(finalItem2_Enchanted2_Config);
                finalItem2_Enchanted2 = ItemWeaponConfigurator.For(finalItem2_Enchanted2)
                    .SetType(WeaponTypes.Dagger)
                    .SetIcon(DaggerPlus1.Icon)
                    .SetCost(5000)
                    .SetVisualParameters(new WeaponVisualParameters
                    {
                        m_Projectiles = Array.Empty<BlueprintProjectileReference>(),
                        m_PossibleAttachSlots = Array.Empty<UnitEquipmentVisualSlotType>(),
                    })
                    .AddToEnchantments("d42fc23b92c640846ac137dc26e000d4")
                    .SetCR(13)
                    .Configure();

                itemName = "FinalHadesDagger3";
                itemDisplayName = "Final Hades Dagger 3";
                itemDescription = "Final Hades Dagger 3 hello fren";
                itemFlavorText = "Final Hades Dagger 3 story stuff";
                itemType = ItemType.Weapon;
                RelicItem finalItem2_Enchanted3_Config = new()
                {
                    Name = itemName,
                    Guid = Tools.CreateGuid(itemName),
                    RelicItemType = itemType,
                    DisplayName = LocalizationTool.CreateString(Tools.CreateGuid(itemName + "DNKey"), itemDisplayName, false),
                    Description = LocalizationTool.CreateString(Tools.CreateGuid(itemName + "DescKey"), itemDescription),
                    FlavorText = LocalizationTool.CreateString(Tools.CreateGuid(itemName + "FTKey"), itemFlavorText, false)
                };
                var finalItem2_Enchanted3 = (BlueprintItemWeapon)ItemConstructor(finalItem2_Enchanted3_Config);
                finalItem2_Enchanted3 = ItemWeaponConfigurator.For(finalItem2_Enchanted3)
                    .SetType(WeaponTypes.Dagger)
                    .SetIcon(DaggerPlus1.Icon)
                    .SetCost(5000)
                    .SetVisualParameters(new WeaponVisualParameters
                    {
                        m_Projectiles = Array.Empty<BlueprintProjectileReference>(),
                        m_PossibleAttachSlots = Array.Empty<UnitEquipmentVisualSlotType>(),
                    })
                    .AddToEnchantments("d42fc23b92c640846ac137dc26e000d4")
                    .SetCR(13)
                    .Configure();

                // One intermediate item for each item type
                // Match the BlueprintItem type to the ItemType
                itemName = "HadesSword_IntermediateItem";
                itemDisplayName = "Hades Sword";
                itemDescription = "This is an intermediate step of relic creation.";
                itemFlavorText = "Intermediate sword";
                itemType = ItemType.Weapon;
                RelicItem intermediateItem1_Config = new()
                {
                    Name = itemName,
                    Guid = Tools.CreateGuid(itemName),
                    RelicItemType = itemType,
                    DisplayName = LocalizationTool.CreateString(Tools.CreateGuid(itemName + "DNKey"), itemDisplayName, false),
                    Description = LocalizationTool.CreateString(Tools.CreateGuid(itemName + "DescKey"), itemDescription),
                    FlavorText = LocalizationTool.CreateString(Tools.CreateGuid(itemName + "FTKey"), itemFlavorText, false)
                };
                var intermediateItem1 = (BlueprintItemWeapon)ItemConstructor(intermediateItem1_Config);
                intermediateItem1 = ItemWeaponConfigurator.For(intermediateItem1)
                    .SetType(WeaponTypes.Longsword)
                    .SetIcon(LongswordPlus1.Icon)
                    .SetCost(5000)
                    .SetVisualParameters(new WeaponVisualParameters
                    {
                        m_Projectiles = Array.Empty<BlueprintProjectileReference>(),
                        m_PossibleAttachSlots = Array.Empty<UnitEquipmentVisualSlotType>(),
                    })
                    .AddToEnchantments("d42fc23b92c640846ac137dc26e000d4")
                    .SetCR(13)
                    .Configure();

                itemName = "HadesDagger_IntermediateItem";
                itemDisplayName = "Hades Dagger";
                itemDescription = "This is an intermediate step of relic creation.";
                itemFlavorText = "Intermediate dagger";
                itemType = ItemType.Weapon;
                RelicItem intermediateItem2_Config = new()
                {
                    Name = itemName,
                    Guid = Tools.CreateGuid(itemName),
                    RelicItemType = itemType,
                    DisplayName = LocalizationTool.CreateString(Tools.CreateGuid(itemName + "DNKey"), itemDisplayName, false),
                    Description = LocalizationTool.CreateString(Tools.CreateGuid(itemName + "DescKey"), itemDescription),
                    FlavorText = LocalizationTool.CreateString(Tools.CreateGuid(itemName + "FTKey"), itemFlavorText, false)
                };
                var intermediateItem2 = (BlueprintItemWeapon)ItemConstructor(intermediateItem2_Config);
                intermediateItem2 = ItemWeaponConfigurator.For(intermediateItem2)
                    .SetType(WeaponTypes.Dagger)
                    .SetIcon(DaggerPlus1.Icon)
                    .SetCost(5000)
                    .SetVisualParameters(new WeaponVisualParameters
                    {
                        m_Projectiles = Array.Empty<BlueprintProjectileReference>(),
                        m_PossibleAttachSlots = Array.Empty<UnitEquipmentVisualSlotType>(),
                    })
                    .AddToEnchantments()
                    .SetCR(13)
                    .Configure();

                // Final enchanting event (where you choose the enchantments)
                // EventName tends to follow this format
                eventName = "HadesBlade_Enchanting_event";
                eventDisplayName = "Enchanting Event";
                eventDescription = "Enchanting Event and stuff";
                // Only modify the string values in the solutionText and resultText sections below
                EventSolution[] enchantingEventSolutions = new EventSolution[]
                {
                    EventSolutionConstructor(
                        solutionText:LocalizationTool.CreateString(Tools.CreateGuid(finalItem1_Enchanted1_Config.Name + "STKey"),
                            value: "First enchantment",
                            false),
                        item: finalItem1_Enchanted1,
                        flag: unlockFlag1,
                        resultText: LocalizationTool.CreateString(Tools.CreateGuid(finalItem1_Enchanted1_Config.Name + "RTKey"),
                            value: "We make Sword",
                            false),
                        isReforgeEvent: false),
                    EventSolutionConstructor(
                        solutionText:LocalizationTool.CreateString(Tools.CreateGuid(finalItem1_Enchanted2_Config.Name + "STKey"),
                            value: "Second enchantment",
                            false),
                        item: finalItem1_Enchanted2,
                        flag: unlockFlag1,
                        resultText: LocalizationTool.CreateString(Tools.CreateGuid(finalItem1_Enchanted2_Config.Name + "RTKey"),
                            value: "We make Sword2",
                            false),
                        isReforgeEvent: false),
                    EventSolutionConstructor(
                        solutionText:LocalizationTool.CreateString(Tools.CreateGuid(finalItem1_Enchanted3_Config.Name + "STKey"),
                            value: "Third enchantment",
                            false),
                        item: finalItem1_Enchanted3,
                        flag: unlockFlag1,
                        resultText: LocalizationTool.CreateString(Tools.CreateGuid(finalItem1_Enchanted3_Config.Name + "RTKey"),
                            value: "We make Sword3",
                            false),
                        isReforgeEvent: false),
                    EventSolutionConstructor(
                        solutionText:LocalizationTool.CreateString(Tools.CreateGuid(finalItem2_Enchanted1_Config.Name + "STKey"),
                            value: "First enchantment",
                            false),
                        item: finalItem2_Enchanted1,
                        flag: unlockFlag2,
                        resultText: LocalizationTool.CreateString(Tools.CreateGuid(finalItem2_Enchanted1_Config.Name + "RTKey"),
                            value: "We make Dagger",
                            false),
                        isReforgeEvent: false),
                    EventSolutionConstructor(
                        solutionText:LocalizationTool.CreateString(Tools.CreateGuid(finalItem2_Enchanted2_Config.Name + "STKey"),
                            value: "2rd enchantment",
                            false),
                        item: finalItem2_Enchanted2,
                        flag: unlockFlag2,
                        resultText: LocalizationTool.CreateString(Tools.CreateGuid(finalItem2_Enchanted2_Config.Name + "RTKey"),
                            value: "We make Dagger2",
                            false),
                        isReforgeEvent: false),
                    EventSolutionConstructor(
                        solutionText:LocalizationTool.CreateString(Tools.CreateGuid(finalItem2_Enchanted3_Config.Name + "STKey"),
                            value: "3rd enchantment",
                            false),
                        item: finalItem2_Enchanted3,
                        flag: unlockFlag2,
                        resultText: LocalizationTool.CreateString(Tools.CreateGuid(finalItem2_Enchanted3_Config.Name + "RTKey"),
                            value: "We make Dagger3",
                            false),
                        isReforgeEvent: false)
                };
                RelicEvent enchantingEvent_Config = new()
                {
                    Name = eventName,
                    Guid = Tools.CreateGuid(eventName),
                    DisplayName = LocalizationTool.CreateString(Tools.CreateGuid(eventName + "DNKey"), eventDisplayName, false),
                    Description = LocalizationTool.CreateString(Tools.CreateGuid(eventName + "DescKey"), eventDescription, false),
                    EventSolutions = enchantingEventSolutions
                };
                var enchantingEvent = EventConstructor(enchantingEvent_Config);

                // One project for each unique item type
                // Name, DefaultResolutionDescription, and MechanicalDescription tend to follow these formats
                projectName = "HadesBladeSwordProject_Enchanting";
                projectDisplayName = "Enchanting Project 1";
                projectDescription = "Enchanting Project 1 words";
                projectDefaultResolutionDescription = "Everything is ready for the relic's augmentation.";
                projectMechanicalDescription = "The relic will be augmented.";
                projectIsAct3 = false;
                RelicProject item1_EnchantingProject_Config = new()
                {
                    Name = projectName,
                    Guid = Tools.CreateGuid(projectName),
                    NextEventGuid = enchantingEvent_Config.Guid,
                    RequiredItemGuid = intermediateItem1_Config.Guid,
                    UnlockableFlagGuid = unlockFlag1Guid,
                    DisplayName = LocalizationTool.CreateString(Tools.CreateGuid(projectName + "DNKey"), projectDisplayName, false),
                    Description = LocalizationTool.CreateString(Tools.CreateGuid(projectName + "DescKey"), projectDescription),
                    DefaultResolutionDescription = LocalizationTool.CreateString(Tools.CreateGuid(projectName + "DefDescKey"), projectDefaultResolutionDescription, false),
                    MechanicalDescription = LocalizationTool.CreateString(Tools.CreateGuid(projectName + "MechDescKey"), projectMechanicalDescription, false),
                    IsAct3 = projectIsAct3
                };
                var enchantingProject1 = ProjectConstructor(item1_EnchantingProject_Config, true);

                projectName = "HadesBladeDaggerProject_Enchanting";
                projectDisplayName = "Enchanting Project 2";
                projectDescription = "Enchanting Project 2 descri";
                projectDefaultResolutionDescription = "Everything is ready for the relic's augmentation.";
                projectMechanicalDescription = "The relic will be augmented.";
                projectIsAct3 = false;
                RelicProject item2_EnchantingProject_Config = new()
                {
                    Name = projectName,
                    Guid = Tools.CreateGuid(projectName),
                    NextEventGuid = enchantingEvent_Config.Guid,
                    RequiredItemGuid = intermediateItem1_Config.Guid,
                    UnlockableFlagGuid = unlockFlag1Guid,
                    DisplayName = LocalizationTool.CreateString(Tools.CreateGuid(projectName + "DNKey"), projectDisplayName, false),
                    Description = LocalizationTool.CreateString(Tools.CreateGuid(projectName + "DescKey"), projectDescription),
                    DefaultResolutionDescription = LocalizationTool.CreateString(Tools.CreateGuid(projectName + "DefDescKey"), projectDefaultResolutionDescription, false),
                    MechanicalDescription = LocalizationTool.CreateString(Tools.CreateGuid(projectName + "MechDescKey"), projectMechanicalDescription, false),
                    IsAct3 = projectIsAct3
                };
                var enchantingProject2 = ProjectConstructor(item2_EnchantingProject_Config, true);

                // Event that occurs after the initial project (where you choose the item type)
                // Naming tends to follow this format
                eventName = "HadesBlade_Reforge_event";
                eventDisplayName = "The Mongrel Blade";
                eventDescription = "Makin a mongrel thingy";
                // Only modify the string values in the solutionText and resultText sections below
                EventSolution[] reforgingEventSolutions = new EventSolution[]
                {
                    EventSolutionConstructor(
                        solutionText:LocalizationTool.CreateString(Tools.CreateGuid(intermediateItem1_Config.Name + "STKey"),
                            value: @"{g|HadesSword_IntermediateItem}[Choice effects]{/g} Longsword",
                            false),
                        item: intermediateItem1,
                        flag: unlockFlag1,
                        resultText: LocalizationTool.CreateString(Tools.CreateGuid(intermediateItem1_Config.Name + "RTKey"),
                            value: "We make Sword",
                            false),
                        isReforgeEvent: true),
                    EventSolutionConstructor(
                        solutionText:LocalizationTool.CreateString(Tools.CreateGuid(intermediateItem2_Config.Name + "STKey"),
                            value: @"{g|HadesDagger_IntermediateItem}[Choice effects]{/g} Dagger",
                            false),
                        item: intermediateItem2,
                        flag: unlockFlag2,
                        resultText: LocalizationTool.CreateString(Tools.CreateGuid(intermediateItem2_Config.Name + "RTKey"),
                            value: "We make Dagger",
                            false),
                        isReforgeEvent: true)
                };
                RelicEvent reforgingEvent_Config = new()
                {
                    Name = eventName,
                    Guid = Tools.CreateGuid(eventName),
                    DisplayName = LocalizationTool.CreateString(Tools.CreateGuid(eventName + "DNKey"), eventDisplayName, false),
                    Description = LocalizationTool.CreateString(Tools.CreateGuid(eventName + "DescKey"), eventDescription, false),
                    EventSolutions = reforgingEventSolutions
                };
                var reforgingEvent = EventConstructor(reforgingEvent_Config);

                // The base etude that accompanies the initial project
                // Should only need to edit the etudeName variable
                // Naming tends to follow this format
                etudeName = "HadesBladeEtude";
                etudeGuid = Tools.CreateGuid(etudeName);
                var etude = EtudeConfigurator.New(etudeName, etudeGuid)
                    .AddEtudePlayTrigger(
                        actions: ActionsBuilder.New()
                            .KingdomActionStartEvent(
                                eventValue: enchantingEvent_Config.Guid,
                                randomRegion: false,
                                delayDays: 0,
                                startNextMonth: false,
                                checkTriggerImmediately: false,
                                checkTriggerOnStart: false)
                            .CompleteEtude(etude: etudeGuid),
                        conditions: ConditionsBuilder.New(),
                        once: false)
                    .SetActivationCondition(ConditionsBuilder.New())
                    .Configure();

                // The initial Reforge project (decree) that occurs when you gain the initial item
                // The names and descriptions tend to follow the patterns below
                // Set projectIsAct3 to True if the project should occur in Act 3
                projectName = "HadesBlade_ReforgeProject";
                projectDisplayName = "The Fate of The Blade of No Escape";
                projectDescription = "A skilled craftsman can do some work on the relic.";
                projectDefaultResolutionDescription = "Everything is ready for the relic's augmentation.";
                projectMechanicalDescription = "The relic will be augmented.";
                projectIsAct3 = true;
                RelicProject reforgingProject_Config = new()
                {
                    Name = projectName,
                    Guid = Tools.CreateGuid(projectName),
                    NextEventGuid = reforgingEvent_Config.Guid,
                    RequiredItemGuid = requiredItem.AssetGuidThreadSafe,
                    DisplayName = LocalizationTool.CreateString(Tools.CreateGuid(projectName + "DNKey"), projectDisplayName, false),
                    Description = LocalizationTool.CreateString(Tools.CreateGuid(projectName + "DescKey"), projectDescription),
                    DefaultResolutionDescription = LocalizationTool.CreateString(Tools.CreateGuid(projectName + "DefDescKey"), projectDefaultResolutionDescription, false),
                    MechanicalDescription = LocalizationTool.CreateString(Tools.CreateGuid(projectName + "MechDescKey"), projectMechanicalDescription, false),
                    IsAct3 = projectIsAct3
                };
                var reforgingProject = ProjectConstructor(relicProject: reforgingProject_Config, isEnchantingProject: false);

                #endregion

                // Save the blueprints file to commit any newly generated GUIDs, then reload to refresh cached GUIDs
                Tools.SaveBlueprints();
                Tools.LoadBlueprints();

                // If mod is enabled, add the projects to the KingdomRoot blueprint
                if (Main.Settings.useRelicsOfTheRighteous == false) { return; }
                AddProjectToKingdomRoot(enchantingProject1);
                AddProjectToKingdomRoot(enchantingProject2);
                AddProjectToKingdomRoot(reforgingProject);
                Main.logger.Log("Built: Hades Blade Reforge Project => " + reforgingProject.AssetGuidThreadSafe);
            }
        }
    }
}