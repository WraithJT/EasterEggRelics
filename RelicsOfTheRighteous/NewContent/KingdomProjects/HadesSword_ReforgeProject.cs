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
using Kingmaker.Designers.EventConditionActionSystem.Conditions;

namespace RelicsOfTheRighteous.NewContent.KingdomProjects
{
    class HadesSword_ReforgeProject
    {
        //private static readonly string BaseName = "HadesBlade";

        //public static readonly string ProjectGuid = "bfc04a80c017481eacd4d53faaaf6410";
        //private static readonly string ProjectName = BaseName + "_ReforgeProject";
        //private static readonly string ProjectDisplayName = "The Fate of The Blade of No Escape";
        //private static readonly string ProjectDescription = "A skilled craftsman can do some work on the relic.";



        [HarmonyPatch(typeof(BlueprintsCache), "Init")]
        static class BlueprintsCache_Init_patch
        {
            static bool Initialized;

            [HarmonyPriority(Priority.First)]
            static void Postfix()
            {
                if (Initialized) return;
                Initialized = true;

                PatchHadesSword_ReforgeProject();
                //try { PatchHadesSword_ReforgeProject(); }
                //catch (Exception ex) { Tools.LogMessage("EXCEPTION: " + ex.ToString()); }
            }

            public static void PatchHadesSword_ReforgeProject()
            {
                /*/ NOTES
                 * figure out Choice Effects preview (TextTemplateEngine, s_TemplatesByTag)
                 * 
                /*/

                // References
                var LongswordPlus1 = ResourcesLibrary.TryGetBlueprint<BlueprintItemWeapon>("03d706655c07d804cb9d5a5583f9aec5");
                var DaggerPlus1 = ResourcesLibrary.TryGetBlueprint<BlueprintItemWeapon>("2a45458f776442e43bba57de65f9b738");

                // Relic quest initiating item
                BlueprintItemWeapon HadesBlade_EasterEgg = ResourcesLibrary.TryGetBlueprint<BlueprintItemWeapon>("372e460865964e4a8e98ebf73f0741ae");

                // Component variables
                #region Component Variables
                string unlockFlag1Name = "FlagHadesBladeSwordProject_Enchanting";
                string unlockFlag1Guid = BlueprintGuid.NewGuid().ToString();
                string unlockFlag2Name = "FlagHadesBladeDaggerProject_Enchanting";
                string unlockFlag2Guid = BlueprintGuid.NewGuid().ToString();

                RelicItem finalItem1_Enchanted1_Config = new()
                {
                    Name = "FinalHadesSword1",
                    Guid = BlueprintGuid.NewGuid().ToString(),
                    RelicItemType = ItemType.Weapon,
                    DisplayName = LocalizationTool.CreateString(BlueprintGuid.NewGuid().ToString(), "Final Hades Sword 1", false),
                    Description = LocalizationTool.CreateString(BlueprintGuid.NewGuid().ToString(), "Final Hades Sword 1 descirption", false),
                    FlavorText = LocalizationTool.CreateString(BlueprintGuid.NewGuid().ToString(), "Final Hades Sword 1 flavor", false)
                };

                RelicItem finalItem1_Enchanted2_Config = new()
                {
                    Name = "FinalHadesSword2",
                    Guid = BlueprintGuid.NewGuid().ToString(),
                    RelicItemType = ItemType.Weapon,
                    DisplayName = LocalizationTool.CreateString(BlueprintGuid.NewGuid().ToString(), "Final Hades Sword 2", false),
                    Description = LocalizationTool.CreateString(BlueprintGuid.NewGuid().ToString(), "Final Hades Sword 2 desc", false),
                    FlavorText = LocalizationTool.CreateString(BlueprintGuid.NewGuid().ToString(), "Final Hades Sword 2 flavor", false)
                };

                RelicItem finalItem1_Enchanted3_Config = new()
                {
                    Name = "FinalHadesSword3",
                    Guid = BlueprintGuid.NewGuid().ToString(),
                    RelicItemType = ItemType.Weapon,
                    DisplayName = LocalizationTool.CreateString(BlueprintGuid.NewGuid().ToString(), "Final Hades Sword 3", false),
                    Description = LocalizationTool.CreateString(BlueprintGuid.NewGuid().ToString(), "Final Hades Sword 3 descr", false),
                    FlavorText = LocalizationTool.CreateString(BlueprintGuid.NewGuid().ToString(), "Final Hades Sword 3 flavy", false)
                };

                RelicItem finalItem2_Enchanted1_Config = new()
                {
                    Name = "FinalHadesDagger1",
                    Guid = BlueprintGuid.NewGuid().ToString(),
                    RelicItemType = ItemType.Weapon,
                    DisplayName = LocalizationTool.CreateString(BlueprintGuid.NewGuid().ToString(), "Final Hades Dagger 1", false),
                    Description = LocalizationTool.CreateString(BlueprintGuid.NewGuid().ToString(), "Final Hades Dagger 1 stuff", false),
                    FlavorText = LocalizationTool.CreateString(BlueprintGuid.NewGuid().ToString(), "Final Hades Dagger 1 falava", false)
                };
                RelicItem finalItem2_Enchanted2_Config = new()
                {
                    Name = "FinalHadesDagger2",
                    Guid = BlueprintGuid.NewGuid().ToString(),
                    RelicItemType = ItemType.Weapon,
                    DisplayName = LocalizationTool.CreateString(BlueprintGuid.NewGuid().ToString(), "Final Hades Dagger 2", false),
                    Description = LocalizationTool.CreateString(BlueprintGuid.NewGuid().ToString(), "Final Hades Dagger 2 words", false),
                    FlavorText = LocalizationTool.CreateString(BlueprintGuid.NewGuid().ToString(), "Final Hades Dagger 2 asdf", false)
                };
                RelicItem finalItem2_Enchanted3_Config = new()
                {
                    Name = "FinalHadesDagger3",
                    Guid = BlueprintGuid.NewGuid().ToString(),
                    RelicItemType = ItemType.Weapon,
                    DisplayName = LocalizationTool.CreateString(BlueprintGuid.NewGuid().ToString(), "Final Hades Dagger 3", false),
                    Description = LocalizationTool.CreateString(BlueprintGuid.NewGuid().ToString(), "Final Hades Dagger 3 hello fren", false),
                    FlavorText = LocalizationTool.CreateString(BlueprintGuid.NewGuid().ToString(), "Final Hades Dagger 3 story stuff", false)
                };

                RelicItem intermediateItem1_Config = new()
                {
                    Name = "HadesSword_IntermediateItem",
                    Guid = BlueprintGuid.NewGuid().ToString(),
                    RelicItemType = ItemType.Weapon,
                    DisplayName = LocalizationTool.CreateString(BlueprintGuid.NewGuid().ToString(), "Hades Sword", false),
                    Description = LocalizationTool.CreateString(BlueprintGuid.NewGuid().ToString(), "This is an intermediate step of relic creation.", false),
                    FlavorText = LocalizationTool.CreateString(BlueprintGuid.NewGuid().ToString(), "Intermediate sword", false)
                };

                RelicItem intermediateItem2_Config = new()
                {
                    Name = "HadesDagger_IntermediateItem",
                    Guid = BlueprintGuid.NewGuid().ToString(),
                    RelicItemType = ItemType.Weapon,
                    DisplayName = LocalizationTool.CreateString(BlueprintGuid.NewGuid().ToString(), "Hades Dagger", false),
                    Description = LocalizationTool.CreateString(BlueprintGuid.NewGuid().ToString(), "This is an intermediate step of relic creation.", false),
                    FlavorText = LocalizationTool.CreateString(BlueprintGuid.NewGuid().ToString(), "Intermediate dagger", false)
                };

                RelicEvent enchantingEvent_Config = new()
                {
                    Name = "HadesBlade_Enchanting_event",
                    Guid = BlueprintGuid.NewGuid().ToString(),
                    DisplayName = LocalizationTool.CreateString(BlueprintGuid.NewGuid().ToString(), "Enchanting Event", false),
                    Description = LocalizationTool.CreateString(BlueprintGuid.NewGuid().ToString(), "", false),
                };

                RelicProject item1_EnchantingProject_Config = new()
                {
                    Name = "HadesBladeSwordProject_Enchanting",
                    Guid = BlueprintGuid.NewGuid().ToString(),
                    NextEventGuid = enchantingEvent_Config.Guid,
                    RequiredItemGuid = intermediateItem1_Config.Guid,
                    UnlockableFlagGuid = unlockFlag1Guid,
                    DisplayName = LocalizationTool.CreateString(BlueprintGuid.NewGuid().ToString(), "Enchanting Project 1", false),
                    Description = LocalizationTool.CreateString(BlueprintGuid.NewGuid().ToString(), "Enchanting Project 1 words", false),
                    DefaultResolutionDescription = LocalizationTool.CreateString(BlueprintGuid.NewGuid().ToString(), "Everything is ready for the relic's augmentation.", false),
                    MechanicalDescription = LocalizationTool.CreateString(BlueprintGuid.NewGuid().ToString(), "The relic will be augmented.", false),
                    IsAct3 = false
                };

                RelicProject item2_EnchantingProject_Config = new()
                {
                    Name = "HadesBladeDaggerProject_Enchanting",
                    Guid = BlueprintGuid.NewGuid().ToString(),
                    NextEventGuid = enchantingEvent_Config.Guid,
                    RequiredItemGuid = intermediateItem2_Config.Guid,
                    UnlockableFlagGuid = unlockFlag2Guid,
                    DisplayName = LocalizationTool.CreateString(BlueprintGuid.NewGuid().ToString(), "Enchanting Project 2", false),
                    Description = LocalizationTool.CreateString(BlueprintGuid.NewGuid().ToString(), "Enchanting Project 2 descri", false),
                    DefaultResolutionDescription = LocalizationTool.CreateString(BlueprintGuid.NewGuid().ToString(), "Everything is ready for the relic's augmentation.", false),
                    MechanicalDescription = LocalizationTool.CreateString(BlueprintGuid.NewGuid().ToString(), "The relic will be augmented.", false),
                    IsAct3 = false
                };

                RelicEvent reforgingEvent_Config = new()
                {
                    Name = "HadesBlade_Reforge_event",
                    Guid = BlueprintGuid.NewGuid().ToString(),
                    DisplayName = LocalizationTool.CreateString(BlueprintGuid.NewGuid().ToString(), "The Mongrel Blade", false),
                    Description = LocalizationTool.CreateString(BlueprintGuid.NewGuid().ToString(), "Makin a mongrel thingy", false)
                };

                RelicProject reforgingProject_Config = new()
                {
                    Name = "HadesBlade_ReforgeProject",
                    Guid = BlueprintGuid.NewGuid().ToString(),
                    NextEventGuid = reforgingEvent_Config.Guid,
                    RequiredItemGuid = HadesBlade_EasterEgg.AssetGuidThreadSafe,
                    DisplayName = LocalizationTool.CreateString(BlueprintGuid.NewGuid().ToString(), "The Fate of The Blade of No Escape", false),
                    Description = LocalizationTool.CreateString(BlueprintGuid.NewGuid().ToString(), "A skilled craftsman can do some work on the relic.", false),
                    DefaultResolutionDescription = LocalizationTool.CreateString(BlueprintGuid.NewGuid().ToString(), "Everything is ready for the relic's augmentation.", false),
                    MechanicalDescription = LocalizationTool.CreateString(BlueprintGuid.NewGuid().ToString(), "The relic will be augmented.", false),
                    IsAct3 = true
                };

                string etudeName = "HadesBladeEtude";
                string etudeGuid = BlueprintGuid.NewGuid().ToString();

                Main.logger.Log("DEBUG: Built base variables");
                #endregion

                // Base config items
                #region Base Config Items
                // One UnlockableFlag for each item type (sword, ring, usable item, etc)
                var unlockFlag1 = UnlockableFlagConfigurator.New(unlockFlag1Name, unlockFlag1Guid).Configure();
                Main.logger.Log("DEBUG: Built Unlock Flag 1 => " + unlockFlag1.AssetGuidThreadSafe);
                var unlockFlag2 = UnlockableFlagConfigurator.New(unlockFlag2Name, unlockFlag2Guid).Configure();
                Main.logger.Log("DEBUG: Built Unlock Flag 2 => " + unlockFlag2.AssetGuidThreadSafe);

                // One final item for each item type and enchantment path. All items share the same enchantment paths
                // Match the BlueprintItem type to the ItemType
                var finalItem1_Enchanted1 = (BlueprintItemWeapon)ItemConstructor(finalItem1_Enchanted1_Config);
                Main.logger.Log("DEBUG: Constructed Enchanted Item 1-1 => " + finalItem1_Enchanted1.AssetGuidThreadSafe);
                var finalItem1_Enchanted2 = (BlueprintItemWeapon)ItemConstructor(finalItem1_Enchanted2_Config);
                Main.logger.Log("DEBUG: Constructed Enchanted Item 1-2 => " + finalItem1_Enchanted2.AssetGuidThreadSafe);
                var finalItem1_Enchanted3 = (BlueprintItemWeapon)ItemConstructor(finalItem1_Enchanted3_Config);
                Main.logger.Log("DEBUG: Constructed Enchanted Item 1-3 => " + finalItem1_Enchanted3.AssetGuidThreadSafe);

                var finalItem2_Enchanted1 = (BlueprintItemWeapon)ItemConstructor(finalItem2_Enchanted1_Config);
                Main.logger.Log("DEBUG: Constructed Enchanted Item 2-1 => " + finalItem2_Enchanted1.AssetGuidThreadSafe);
                var finalItem2_Enchanted2 = (BlueprintItemWeapon)ItemConstructor(finalItem2_Enchanted2_Config);
                Main.logger.Log("DEBUG: Constructed Enchanted Item 2-2 => " + finalItem2_Enchanted2.AssetGuidThreadSafe);
                var finalItem2_Enchanted3 = (BlueprintItemWeapon)ItemConstructor(finalItem2_Enchanted3_Config);
                Main.logger.Log("DEBUG: Constructed Enchanted Item 2-3 => " + finalItem2_Enchanted3.AssetGuidThreadSafe);

                // One intermediate item for each item type
                // Match the BlueprintItem type to the ItemType
                var intermediateItem1 = (BlueprintItemWeapon)ItemConstructor(intermediateItem1_Config);
                Main.logger.Log("DEBUG: Constructed Intermediate Item 1 => " + intermediateItem1.AssetGuidThreadSafe);
                var intermediateItem2 = (BlueprintItemWeapon)ItemConstructor(intermediateItem2_Config);
                Main.logger.Log("DEBUG: Constructed Intermediate Item 2 => " + intermediateItem2.AssetGuidThreadSafe);

                // One etude total
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
                Main.logger.Log("DEBUG: Built Etude => " + etude.AssetGuidThreadSafe);

                PossibleEventSolutions emptySolutions = new()
                {
                    Entries = new PossibleEventSolution[]
                    {
                        Helpers.Create<PossibleEventSolution>(c => { c.Leader = LeaderType.Counselor; c.CanBeSolved = false; c.DCModifier = 0; c.SuccessCount = 0; c.Resolutions = new EventResult[] { }; }),
                        Helpers.Create<PossibleEventSolution>(c => { c.Leader = LeaderType.Strategist; c.CanBeSolved = false; c.DCModifier = 0; c.SuccessCount = 0; c.Resolutions = new EventResult[] { }; }),
                        Helpers.Create<PossibleEventSolution>(c => { c.Leader = LeaderType.Diplomat; c.CanBeSolved = false; c.DCModifier = 0; c.SuccessCount = 0; c.Resolutions = new EventResult[] { }; }),
                        Helpers.Create<PossibleEventSolution>(c => { c.Leader = LeaderType.General; c.CanBeSolved = false; c.DCModifier = 0; c.SuccessCount = 0; c.Resolutions = new EventResult[] { }; })
                    }
                };
                Main.logger.Log("DEBUG: Empty Solutions");
                #endregion

                // Configure features of each item
                #region Item Configuration
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
                Main.logger.Log("DEBUG: Built Enchanted Item 1-1 => " + finalItem1_Enchanted1.AssetGuidThreadSafe);

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
                Main.logger.Log("DEBUG: Built Enchanted Item 1-2 => " + finalItem1_Enchanted2.AssetGuidThreadSafe);

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
                Main.logger.Log("DEBUG: Built Enchanted Item 1-3 => " + finalItem1_Enchanted3.AssetGuidThreadSafe);

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
                Main.logger.Log("DEBUG: Built Enchanted Item 2-1 => " + finalItem2_Enchanted1.AssetGuidThreadSafe);

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
                Main.logger.Log("DEBUG: Built Enchanted Item 2-2 => " + finalItem2_Enchanted2.AssetGuidThreadSafe);

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
                Main.logger.Log("DEBUG: Built Enchanted Item 2-3 => " + finalItem2_Enchanted3.AssetGuidThreadSafe);

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
                Main.logger.Log("DEBUG: Built Intermediate Item 1 => " + intermediateItem1.AssetGuidThreadSafe);

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
                Main.logger.Log("DEBUG: Built Intermediate Item 2 => " + intermediateItem2.AssetGuidThreadSafe);
                #endregion

                #region Enchanting Event
                //RelicEventSolution relicEventSolution = new() { };

                //var enchantingEvent_Item1Conditions = ConditionsBuilder.New()
                //    .FlagUnlocked(
                //        conditionFlag: unlockFlag1,
                //        exceptSpecifiedValues: false,
                //        negate: false).Build();
                //var enchantingEvent_Item2Conditions = ConditionsBuilder.New()
                //    .FlagUnlocked(
                //        conditionFlag: unlockFlag2,
                //        exceptSpecifiedValues: false,
                //        negate: false).Build();

                //RelicEventSolution relicEventSolution1 = new()
                //{
                //    SolutionText = LocalizationTool.CreateString(nameof(intermediateItem1) + "STKey", @"{g|KnightsEmblem_Chainmail_Unholy}[Choice effects]{/g}" + nameof(intermediateItem1), false),
                //    Item = intermediateItem1,
                //    Flag = unlockFlag1,
                //    ResultText = LocalizationTool.CreateString(nameof(intermediateItem1) + "RTKey", "We make Sword", false),
                //    AvailConditions = Constants.Empty.Conditions
                //};

                EventSolution[] enchantingEventSolutions = new EventSolution[]
                {
                    EventSolutionConstructor(
                        solutionText:LocalizationTool.CreateString(BlueprintGuid.NewGuid().ToString(), "First enchantment", false),
                        item: finalItem1_Enchanted1,
                        flag: unlockFlag1,
                        resultText: LocalizationTool.CreateString(nameof(finalItem1_Enchanted1) + "RTKey", "We make Sword", false),
                        isReforgeEvent: false),
                    EventSolutionConstructor(
                        solutionText:LocalizationTool.CreateString(BlueprintGuid.NewGuid().ToString(), "Second enchantment", false),
                        item: finalItem1_Enchanted2,
                        flag: unlockFlag1,
                        resultText: LocalizationTool.CreateString(nameof(finalItem1_Enchanted2) + "RTKey", "We make Sword2", false),
                        isReforgeEvent: false),
                    EventSolutionConstructor(
                        solutionText:LocalizationTool.CreateString(BlueprintGuid.NewGuid().ToString(), "Third enchantment", false),
                        item: finalItem1_Enchanted3,
                        flag: unlockFlag1,
                        resultText: LocalizationTool.CreateString(nameof(finalItem1_Enchanted3) + "RTKey", "We make Sword3", false),
                        isReforgeEvent: false),
                    EventSolutionConstructor(
                        solutionText:LocalizationTool.CreateString(BlueprintGuid.NewGuid().ToString(), "First enchantment", false),
                        item: finalItem2_Enchanted1,
                        flag: unlockFlag2,
                        resultText: LocalizationTool.CreateString(nameof(finalItem2_Enchanted1) + "RTKey", "We make Dagger", false),
                        isReforgeEvent: false),
                    EventSolutionConstructor(
                        solutionText:LocalizationTool.CreateString(BlueprintGuid.NewGuid().ToString(), "2rd enchantment", false),
                        item: finalItem2_Enchanted2,
                        flag: unlockFlag2,
                        resultText: LocalizationTool.CreateString(nameof(finalItem2_Enchanted2) + "RTKey", "We make Dagger2", false),
                        isReforgeEvent: false),
                    EventSolutionConstructor(
                        solutionText:LocalizationTool.CreateString(BlueprintGuid.NewGuid().ToString(), "3rd enchantment", false),
                        item: finalItem2_Enchanted3,
                        flag: unlockFlag2,
                        resultText: LocalizationTool.CreateString(nameof(finalItem2_Enchanted3) + "RTKey", "We make Dagger3", false),
                        isReforgeEvent: false)
                    //Helpers.Create<EventSolution>(c =>
                    //{
                    //    c.m_SolutionText = LocalizationTool.CreateString(BlueprintGuid.NewGuid().ToString(), "First enchantment", false);
                    //    c.m_AvailConditions = enchantingEvent_Item1Conditions;
                    //    c.m_UnavailingBehaviour = UnavailingBehaviour.HideSolution;
                    //    c.m_UnavailingPlaceholder = Constants.Empty.String;
                    //    c.m_SuccessEffects = ActionsBuilder.New()
                    //        .GiveHandSlotItemToPlayer(finalItem1_Enchanted1, identify: true, quantity: 1, silent: false)
                    //        .Build();
                    //    c.m_ResultText = LocalizationTool.CreateString(BlueprintGuid.NewGuid().ToString(), "Did stuff", false);
                    //}),
                    //Helpers.Create<EventSolution>(c =>
                    //{
                    //    c.m_SolutionText = LocalizationTool.CreateString(BlueprintGuid.NewGuid().ToString(), "Second enchantment", false);
                    //    c.m_AvailConditions = enchantingEvent_Item1Conditions;
                    //    c.m_UnavailingBehaviour = UnavailingBehaviour.HideSolution;
                    //    c.m_UnavailingPlaceholder = Constants.Empty.String;
                    //    c.m_SuccessEffects = ActionsBuilder.New()
                    //        .GiveHandSlotItemToPlayer(finalItem1_Enchanted2, identify: true, quantity: 1, silent: false)
                    //        .Build();
                    //    c.m_ResultText = LocalizationTool.CreateString(BlueprintGuid.NewGuid().ToString(), "Did stuff", false);
                    //}),
                    //Helpers.Create<EventSolution>(c =>
                    //{
                    //    c.m_SolutionText = LocalizationTool.CreateString(BlueprintGuid.NewGuid().ToString(), "Third enchantment", false);
                    //    c.m_AvailConditions = enchantingEvent_Item1Conditions;
                    //    c.m_UnavailingBehaviour = UnavailingBehaviour.HideSolution;
                    //    c.m_UnavailingPlaceholder = Constants.Empty.String;
                    //    c.m_SuccessEffects = ActionsBuilder.New()
                    //        .GiveHandSlotItemToPlayer(finalItem1_Enchanted3, identify: true, quantity: 1, silent: false)
                    //        .Build();
                    //    c.m_ResultText = LocalizationTool.CreateString(BlueprintGuid.NewGuid().ToString(), "Did stuff", false);
                    //}),
                    //Helpers.Create<EventSolution>(c =>
                    //{
                    //    c.m_SolutionText = LocalizationTool.CreateString(BlueprintGuid.NewGuid().ToString(), "First enchantment", false);
                    //    c.m_AvailConditions = enchantingEvent_Item2Conditions;
                    //    c.m_UnavailingBehaviour = UnavailingBehaviour.HideSolution;
                    //    c.m_UnavailingPlaceholder = Constants.Empty.String;
                    //    c.m_SuccessEffects = ActionsBuilder.New()
                    //        .GiveHandSlotItemToPlayer(finalItem2_Enchanted1, identify: true, quantity: 1, silent: false)
                    //        .Build();
                    //    c.m_ResultText = LocalizationTool.CreateString(BlueprintGuid.NewGuid().ToString(), "Did stuff", false);
                    //}),
                    //Helpers.Create<EventSolution>(c =>
                    //{
                    //    c.m_SolutionText = LocalizationTool.CreateString(BlueprintGuid.NewGuid().ToString(), "Second enchantment", false);
                    //    c.m_AvailConditions = enchantingEvent_Item2Conditions;
                    //    c.m_UnavailingBehaviour = UnavailingBehaviour.HideSolution;
                    //    c.m_UnavailingPlaceholder = Constants.Empty.String;
                    //    c.m_SuccessEffects = ActionsBuilder.New()
                    //        .GiveHandSlotItemToPlayer(finalItem2_Enchanted2, identify: true, quantity: 1, silent: false)
                    //        .Build();
                    //    c.m_ResultText = LocalizationTool.CreateString(BlueprintGuid.NewGuid().ToString(), "Did stuff", false);
                    //}),
                    //Helpers.Create<EventSolution>(c =>
                    //{
                    //    c.m_SolutionText = LocalizationTool.CreateString(BlueprintGuid.NewGuid().ToString(), "Third enchantment", false);
                    //    c.m_AvailConditions = enchantingEvent_Item2Conditions;
                    //    c.m_UnavailingBehaviour = UnavailingBehaviour.HideSolution;
                    //    c.m_UnavailingPlaceholder = Constants.Empty.String;
                    //    c.m_SuccessEffects = ActionsBuilder.New()
                    //        .GiveHandSlotItemToPlayer(finalItem2_Enchanted3, identify: true, quantity: 1, silent: false)
                    //        .Build();
                    //    c.m_ResultText = LocalizationTool.CreateString(BlueprintGuid.NewGuid().ToString(), "Did stuff", false);
                    //}),
                };
                Main.logger.Log("DEBUG: Built Enchanting Event Solutions");
                enchantingEvent_Config.EventSolutions = enchantingEventSolutions;

                var enchantingEvent = EventConstructor(enchantingEvent_Config);

                //var enchantingEvent = CrusadeEventConfigurator.New(enchantingEventName, enchantingEventGuid)
                //    .SetLocalizedName(enchantingEventDisplayName)
                //    .SetLocalizedDescription(enchantingEventDescription)
                //    .SetResolutionTime(14)
                //    .SetResolveAutomatically(false)
                //    .SetNeedToVisitTheThroneRoom(false)
                //    .SetAICanCheat(false)
                //    .SetSkipRoll(true)
                //    .SetResolutionDC(0)
                //    .SetAutoResolveResult(EventResult.MarginType.Success)
                //    .SetDefaultResolutionType(LeaderType.Counselor)
                //    .SetAIStopping(false)
                //    .SetSolutions(emptySolutions)
                //    .SetEventSolutions(enchantingEventSolutions)
                //    .Configure();
                //enchantingEvent.TriggerCondition = Constants.Empty.Conditions;
                //enchantingEvent.Comment = Constants.Empty.String;
                //enchantingEvent.DefaultResolutionDescription = Constants.Empty.String;
                Main.logger.Log("DEBUG: Built Enchanting Event => " + enchantingEvent.AssetGuidThreadSafe);
                #endregion

                #region Enchanting Decrees
                var enchantingProject1 = ProjectConstructor(
                    relicProject: item1_EnchantingProject_Config,
                    isEnchantingProject: true);

                //EventResult[] enchantingcounselorEventResults = new EventResult[] {
                //    new EventResult {
                //        Margin = EventResult.MarginType.GreatFail,
                //        LeaderAlignment = AlignmentMaskType.Any,
                //        Condition = Constants.Empty.Conditions,
                //        Actions = new ActionList(){ },
                //        StatChanges = new KingdomStats.Changes(){ },
                //        SuccessCount = 0
                //    },
                //    new EventResult {
                //        Margin = EventResult.MarginType.Fail,
                //        LeaderAlignment = AlignmentMaskType.Any,
                //        Condition = Constants.Empty.Conditions,
                //        Actions = new ActionList(){ },
                //        StatChanges = new KingdomStats.Changes(){ },
                //        SuccessCount = 0
                //    },
                //    new EventResult {
                //        Margin = EventResult.MarginType.Success,
                //        LeaderAlignment = AlignmentMaskType.Any,
                //        Condition = Constants.Empty.Conditions,
                //        Actions = ActionsBuilder.New().KingdomActionImproveStat(KingdomStats.Type.Diplomacy).Build(),
                //        StatChanges = new KingdomStats.Changes(){ },
                //        SuccessCount = 0
                //    },
                //    new EventResult {
                //        Margin = EventResult.MarginType.GreatSuccess,
                //        LeaderAlignment = AlignmentMaskType.Any,
                //        Condition = Constants.Empty.Conditions,
                //        Actions = new ActionList(){ },
                //        StatChanges = new KingdomStats.Changes(){ },
                //        SuccessCount = 0
                //    }
                //};
                //Main.logger.Log("DEBUG: Built Enchanting Project Counselor Event Results");

                //EventResult[] enchantingstrategistEventResults = new EventResult[] {
                //    new EventResult {
                //        Margin = EventResult.MarginType.GreatFail,
                //        LeaderAlignment = AlignmentMaskType.Any,
                //        Condition = Constants.Empty.Conditions,
                //        Actions = new ActionList(){ },
                //        StatChanges = new KingdomStats.Changes(){ },
                //        SuccessCount = 0
                //    },
                //    new EventResult {
                //        Margin = EventResult.MarginType.Fail,
                //        LeaderAlignment = AlignmentMaskType.Any,
                //        Condition = Constants.Empty.Conditions,
                //        Actions = new ActionList(){ },
                //        StatChanges = new KingdomStats.Changes(){ },
                //        SuccessCount = 0
                //    },
                //    new EventResult {
                //        Margin = EventResult.MarginType.Success,
                //        LeaderAlignment = AlignmentMaskType.Any,
                //        Condition = Constants.Empty.Conditions,
                //        Actions = ActionsBuilder.New()
                //            .KingdomActionAddBPRandom(
                //                includeInEventStats: true,
                //                bonus: 1000,
                //                change: new DiceFormula(diceType: DiceType.D100, rollsCount: 10),
                //                resourceType: KingdomResource.Finances)
                //            .KingdomActionAddBPRandom(
                //                includeInEventStats: true,
                //                bonus: 100,
                //                change: new DiceFormula(diceType: DiceType.D100, rollsCount: 1),
                //                resourceType: KingdomResource.Materials)
                //            .Build(),
                //        StatChanges = new KingdomStats.Changes(){ },
                //        SuccessCount = 0
                //    },
                //    new EventResult {
                //        Margin = EventResult.MarginType.GreatSuccess,
                //        LeaderAlignment = AlignmentMaskType.Any,
                //        Condition = Constants.Empty.Conditions,
                //        Actions = new ActionList(){ },
                //        StatChanges = new KingdomStats.Changes(){ },
                //        SuccessCount = 0
                //    }
                //};
                //Main.logger.Log("DEBUG: Built Enchanting Project Strategist Event Results");

                //var enchantingeventSolutionCounselor = new PossibleEventSolution()
                //{
                //    Leader = LeaderType.Counselor,
                //    CanBeSolved = false,
                //    DCModifier = 0,
                //    SuccessCount = 0,
                //    Resolutions = enchantingcounselorEventResults
                //};
                //Main.logger.Log("DEBUG: Built Enchanting Project Counselor Event Solution");
                //var enchantingeventSolutionStrategist = new PossibleEventSolution()
                //{
                //    Leader = LeaderType.Strategist,
                //    CanBeSolved = false,
                //    DCModifier = 0,
                //    SuccessCount = 0,
                //    Resolutions = enchantingstrategistEventResults
                //};
                //Main.logger.Log("DEBUG: Built Enchanting Project Strategist Event Solution");
                //var enchantingeventSolutionGeneral = new PossibleEventSolution()
                //{
                //    Leader = LeaderType.General,
                //    CanBeSolved = false,
                //    DCModifier = 0,
                //    SuccessCount = 0,
                //    Resolutions = new EventResult[] { }
                //};
                //Main.logger.Log("DEBUG: Built Enchanting Project General Event Solution");

                //var enchanting1diplomatActionsList = ActionsBuilder.New()
                //    .RemoveItemFromPlayer(
                //        money: false,
                //        removeAll: false,
                //        itemToRemove: intermediateItem1,
                //        silent: false,
                //        quantity: 1,
                //        percentage: 0.0f)
                //    .KingdomActionStartEvent(
                //        eventValue: item1_EnchantingProject_Config.NextEventGuid,
                //        randomRegion: false,
                //        delayDays: 0,
                //        startNextMonth: false,
                //        checkTriggerImmediately: false,
                //        checkTriggerOnStart: false)
                //    .Build();
                //Main.logger.Log("DEBUG: Built Enchanting Project 1 Diplomat Actions");
                //EventResult[] enchanting1diplomatEventResults = new EventResult[] {
                //    new EventResult {
                //        Margin = EventResult.MarginType.GreatFail,
                //        LeaderAlignment = AlignmentMaskType.Any,
                //        Condition = Constants.Empty.Conditions,
                //        Actions = new ActionList(){ },
                //        StatChanges = new KingdomStats.Changes(){ },
                //        SuccessCount = 0
                //    },
                //    new EventResult {
                //        Margin = EventResult.MarginType.Fail,
                //        LeaderAlignment = AlignmentMaskType.Any,
                //        Condition = Constants.Empty.Conditions,
                //        Actions = new ActionList(){ },
                //        StatChanges = new KingdomStats.Changes(){ },
                //        SuccessCount = 0
                //    },
                //    new EventResult {
                //        Margin = EventResult.MarginType.Success,
                //        LeaderAlignment = AlignmentMaskType.Any,
                //        Condition = Constants.Empty.Conditions,
                //        Actions = enchanting1diplomatActionsList,
                //        StatChanges = new KingdomStats.Changes(){ },
                //        SuccessCount = 0
                //    },
                //    new EventResult {
                //        Margin = EventResult.MarginType.GreatSuccess,
                //        LeaderAlignment = AlignmentMaskType.Any,
                //        Condition = Constants.Empty.Conditions,
                //        Actions = new ActionList(){ },
                //        StatChanges = new KingdomStats.Changes(){ },
                //        SuccessCount = 0
                //    }
                //};
                //Main.logger.Log("DEBUG: Built Enchanting Project 1 Diplomat Event Results");

                //var enchantingevent1SolutionDiplomat = new PossibleEventSolution()
                //{
                //    Leader = LeaderType.Diplomat,
                //    CanBeSolved = true,
                //    DCModifier = 0,
                //    SuccessCount = 0,
                //    Resolutions = enchanting1diplomatEventResults
                //};
                //Main.logger.Log("DEBUG: Built Enchanting Project 1 Diplomat Event Solution");

                //var enchantingProject1 = KingdomProjectConfigurator.New(item1_EnchantingProject_Config.Name, item1_EnchantingProject_Config.Guid)
                //    .SetLocalizedName(item1_EnchantingProject_Config.DisplayName)
                //    .SetLocalizedDescription(item1_EnchantingProject_Config.Description)
                //    .SetDefaultResolutionDescription(item1_EnchantingProject_Config.DefaultResolutionDescription)
                //    .SetMechanicalDescription(item1_EnchantingProject_Config.MechanicalDescription)
                //    .SetInfoType(KingomEventInfoType.None)
                //    .SetResolutionTime(5)
                //    .SetResolveAutomatically(false)
                //    .SetNeedToVisitTheThroneRoom(false)
                //    .SetAICanCheat(false)
                //    .SetSkipRoll(true)
                //    .SetResolutionDC(1)
                //    .SetAutoResolveResult(EventResult.MarginType.Success)
                //    .SetDefaultResolutionType(LeaderType.Diplomat)
                //    .SetAIStopping(false)
                //    .SetProjectType(KingdomProjectType.Relics)
                //    .SetSpendRulerTimeDays(0)
                //    .SetRepeatable(false)
                //    .SetCooldown(0)
                //    .SetIsRankUpProject(false)
                //    .SetRankupProjectFor(KingdomStats.Type.Strategy)
                //    .SetAIPriority(0)
                //    .SetTriggerCondition(enchantingEvent_Item1Conditions)
                //    .SetProjectStartCost(
                //        projectStartCost: new KingdomResourcesAmount()
                //        {
                //            m_Finances = 500,
                //            m_Materials = 50,
                //            m_Favors = 50
                //        })
                //    .AddEventItemCost(amount: 1, items: new() { intermediateItem1.ToReference<BlueprintItemReference>() })
                //    .SetSolutions(new PossibleEventSolutions()
                //    {
                //        Entries = new PossibleEventSolution[] {
                //            enchantingeventSolutionCounselor,
                //            enchantingeventSolutionStrategist,
                //            enchantingevent1SolutionDiplomat,
                //            enchantingeventSolutionGeneral
                //            }
                //    })
                //    .Configure();
                Main.logger.Log("DEBUG: Built Enchanting Project 1 => " + enchantingProject1.AssetGuidThreadSafe);

                var enchantingProject2 = ProjectConstructor(
                    relicProject: item2_EnchantingProject_Config,
                    isEnchantingProject: true);

                //var enchanting2diplomatActionsList = ActionsBuilder.New()
                //    .RemoveItemFromPlayer(
                //        money: false,
                //        removeAll: false,
                //        itemToRemove: intermediateItem2,
                //        silent: false,
                //        quantity: 1,
                //        percentage: 0.0f)
                //    .KingdomActionStartEvent(
                //        eventValue: item2_EnchantingProject_Config.NextEventGuid,
                //        randomRegion: false,
                //        delayDays: 0,
                //        startNextMonth: false,
                //        checkTriggerImmediately: false,
                //        checkTriggerOnStart: false)
                //    .Build();
                //EventResult[] enchanting2diplomatEventResults = new EventResult[] {
                //    new EventResult {
                //        Margin = EventResult.MarginType.GreatFail,
                //        LeaderAlignment = AlignmentMaskType.Any,
                //        Condition = Constants.Empty.Conditions,
                //        Actions = new ActionList(){ },
                //        StatChanges = new KingdomStats.Changes(){ },
                //        SuccessCount = 0
                //    },
                //    new EventResult {
                //        Margin = EventResult.MarginType.Fail,
                //        LeaderAlignment = AlignmentMaskType.Any,
                //        Condition = Constants.Empty.Conditions,
                //        Actions = new ActionList(){ },
                //        StatChanges = new KingdomStats.Changes(){ },
                //        SuccessCount = 0
                //    },
                //    new EventResult {
                //        Margin = EventResult.MarginType.Success,
                //        LeaderAlignment = AlignmentMaskType.Any,
                //        Condition = Constants.Empty.Conditions,
                //        Actions = enchanting2diplomatActionsList,
                //        StatChanges = new KingdomStats.Changes(){ },
                //        SuccessCount = 0
                //    },
                //    new EventResult {
                //        Margin = EventResult.MarginType.GreatSuccess,
                //        LeaderAlignment = AlignmentMaskType.Any,
                //        Condition = Constants.Empty.Conditions,
                //        Actions = new ActionList(){ },
                //        StatChanges = new KingdomStats.Changes(){ },
                //        SuccessCount = 0
                //    }
                //};

                //var enchantingevent2SolutionDiplomat = new PossibleEventSolution()
                //{
                //    Leader = LeaderType.Diplomat,
                //    CanBeSolved = true,
                //    DCModifier = 0,
                //    SuccessCount = 0,
                //    Resolutions = enchanting2diplomatEventResults
                //};

                //var enchantingProject2 = KingdomProjectConfigurator.New(item2_EnchantingProject_Config.Name, item2_EnchantingProject_Config.Guid)
                //    .SetLocalizedName(item2_EnchantingProject_Config.DisplayName)
                //    .SetLocalizedDescription(item2_EnchantingProject_Config.Description)
                //    .SetDefaultResolutionDescription(item2_EnchantingProject_Config.DefaultResolutionDescription)
                //    .SetMechanicalDescription(item2_EnchantingProject_Config.MechanicalDescription)
                //    .SetInfoType(KingomEventInfoType.None)
                //    .SetResolutionTime(5)
                //    .SetResolveAutomatically(false)
                //    .SetNeedToVisitTheThroneRoom(false)
                //    .SetAICanCheat(false)
                //    .SetSkipRoll(true)
                //    .SetResolutionDC(1)
                //    .SetAutoResolveResult(EventResult.MarginType.Success)
                //    .SetDefaultResolutionType(LeaderType.Diplomat)
                //    .SetAIStopping(false)
                //    .SetProjectType(KingdomProjectType.Relics)
                //    .SetSpendRulerTimeDays(0)
                //    .SetRepeatable(false)
                //    .SetCooldown(0)
                //    .SetIsRankUpProject(false)
                //    .SetRankupProjectFor(KingdomStats.Type.Strategy)
                //    .SetAIPriority(0)
                //    .SetTriggerCondition(enchantingEvent_Item2Conditions)
                //    .SetProjectStartCost(
                //        projectStartCost: new KingdomResourcesAmount()
                //        {
                //            m_Finances = 500,
                //            m_Materials = 50,
                //            m_Favors = 50
                //        })
                //    .AddEventItemCost(amount: 1, items: new() { intermediateItem2.ToReference<BlueprintItemReference>() })
                //    .SetSolutions(new PossibleEventSolutions()
                //    {
                //        Entries = new PossibleEventSolution[] {
                //            enchantingeventSolutionCounselor,
                //            enchantingeventSolutionStrategist,
                //            enchantingevent2SolutionDiplomat,
                //            enchantingeventSolutionGeneral
                //            }
                //    })
                //    .Configure();
                Main.logger.Log("DEBUG: Built Enchanting Project 2 => " + enchantingProject2.AssetGuidThreadSafe);
                #endregion

                #region Reforging Event
                //RelicEventSolution relicEventSolution1 = new()
                //{
                //    SolutionText = LocalizationTool.CreateString(nameof(intermediateItem1) + "STKey", @"{g|KnightsEmblem_Chainmail_Unholy}[Choice effects]{/g}" + nameof(intermediateItem1), false),
                //    Item = intermediateItem1,
                //    Flag = unlockFlag1,
                //    ResultText = LocalizationTool.CreateString(nameof(intermediateItem1) + "RTKey", "We make Sword", false),
                //    AvailConditions = Constants.Empty.Conditions
                //};
                //RelicEventSolution relicEventSolution2 = new()
                //{
                //    SolutionText = LocalizationTool.CreateString(nameof(intermediateItem2) + "STKey", @"{g|KnightsEmblem_Belt}[Choice effects]{/g}" + nameof(intermediateItem2), false),
                //    Item = intermediateItem2,
                //    Flag = unlockFlag2,
                //    ResultText = LocalizationTool.CreateString(nameof(intermediateItem2) + "RTKey", "We make dagger", false),
                //    AvailConditions = Constants.Empty.Conditions
                //};

                EventSolution[] reforgingEventSolutions = new EventSolution[]
                {
                    EventSolutionConstructor(
                        solutionText:LocalizationTool.CreateString(nameof(intermediateItem1) + "STKey", @"{g|KnightsEmblem_Chainmail_Unholy}[Choice effects]{/g}" + nameof(intermediateItem1), false),
                        item: intermediateItem1,
                        flag: unlockFlag1,
                        resultText: LocalizationTool.CreateString(nameof(intermediateItem1) + "RTKey", "We make Sword", false),
                        isReforgeEvent: true),
                    EventSolutionConstructor(
                        solutionText:LocalizationTool.CreateString(nameof(intermediateItem2) + "STKey", @"{g|KnightsEmblem_Belt}[Choice effects]{/g}" + nameof(intermediateItem2), false),
                        item: intermediateItem2,
                        flag: unlockFlag2,
                        resultText: LocalizationTool.CreateString(nameof(intermediateItem2) + "RTKey", "We make dagger", false),
                        isReforgeEvent: true)
                    //Helpers.Create<EventSolution>(c =>
                    //{
                    //    c.m_SolutionText = LocalizationTool.CreateString(nameof(intermediateItem2)+"STKey", @"{g|KnightsEmblem_Belt}[Choice effects]{/g}"+nameof(intermediateItem2), false);
                    //    c.m_AvailConditions = Constants.Empty.Conditions;
                    //    c.m_UnavailingBehaviour = UnavailingBehaviour.HideSolution;
                    //    c.m_UnavailingPlaceholder = Constants.Empty.String;
                    //    c.m_SuccessEffects = ActionsBuilder.New()
                    //        .GiveHandSlotItemToPlayer(intermediateItem2, identify: true, quantity: 1, silent: false)
                    //        .UnlockFlag(unlockFlag2, flagValue: 1)
                    //        .Build();
                    //    c.m_ResultText = LocalizationTool.CreateString(nameof(DaggerPlus1)+"RTKey", "We make dagger", false);
                    //}),
                    //Helpers.Create<EventSolution>(c =>
                    //{
                    //    c.m_SolutionText = LocalizationTool.CreateString(nameof(intermediateItem1)+"STKey", @"{g|KnightsEmblem_Chainmail_Unholy}[Choice effects]{/g}"+nameof(intermediateItem1), false);
                    //    c.m_AvailConditions = Constants.Empty.Conditions;
                    //    c.m_UnavailingBehaviour = UnavailingBehaviour.HideSolution;
                    //    c.m_UnavailingPlaceholder = Constants.Empty.String;
                    //    c.m_SuccessEffects = ActionsBuilder.New()
                    //        .GiveHandSlotItemToPlayer(intermediateItem1, identify: true, quantity: 1, silent: false)
                    //        .UnlockFlag(unlockFlag1, flagValue: 1)
                    //        .Build();
                    //    c.m_ResultText = LocalizationTool.CreateString(nameof(LongswordPlus1)+"RTKey", "We make Sword", false);
                    //})
                };
                Main.logger.Log("DEBUG: Built Reforging Event Solutions");
                reforgingEvent_Config.EventSolutions = reforgingEventSolutions;

                var reforgingEvent = EventConstructor(reforgingEvent_Config);

                //var reforgingEvent = CrusadeEventConfigurator.New(reforgingEvent_Config.Name, reforgingEvent_Config.Guid)
                //    .SetLocalizedName(reforgingEvent_Config.DisplayName)
                //    .SetLocalizedDescription(reforgingEvent_Config.Description)
                //    .SetResolutionTime(14)
                //    .SetResolveAutomatically(false)
                //    .SetNeedToVisitTheThroneRoom(false)
                //    .SetAICanCheat(false)
                //    .SetSkipRoll(false)
                //    .SetResolutionDC(0)
                //    .SetAutoResolveResult(EventResult.MarginType.Success)
                //    .SetDefaultResolutionType(LeaderType.Counselor)
                //    .SetAIStopping(false)
                //    .SetSolutions(emptySolutions)
                //    .SetEventSolutions(reforgingEventSolutions)
                //    .Configure();
                //reforgingEvent.TriggerCondition = Constants.Empty.Conditions;
                //reforgingEvent.Comment = Constants.Empty.String;
                //reforgingEvent.DefaultResolutionDescription = Constants.Empty.String;

                Main.logger.Log("DEBUG: Built Reforging Event => " + reforgingEvent.AssetGuidThreadSafe);
                #endregion

                #region Reforging Project
                var reforgingProject = ProjectConstructor(
                    relicProject: reforgingProject_Config,
                    isEnchantingProject: false);

                //var reforgingProjectTriggerCondition = ConditionsBuilder.New().ItemsEnough(
                //    itemToCheck: reforgingProject_Config.RequiredItemGuid,
                //    money: false,
                //    negate: false,
                //    quantity: 1);
                //Main.logger.Log("DEBUG: Built Reforge Project Trigger Condition");

                //EventResult[] counselorEventResults = new EventResult[] {
                //    new EventResult {
                //        Margin = EventResult.MarginType.GreatFail,
                //        LeaderAlignment = AlignmentMaskType.Any,
                //        Condition = Constants.Empty.Conditions,
                //        Actions = new ActionList(){ },
                //        StatChanges = new KingdomStats.Changes(){ },
                //        SuccessCount = 0
                //    },
                //    new EventResult {
                //        Margin = EventResult.MarginType.Fail,
                //        LeaderAlignment = AlignmentMaskType.Any,
                //        Condition = Constants.Empty.Conditions,
                //        Actions = new ActionList(){ },
                //        StatChanges = new KingdomStats.Changes(){ },
                //        SuccessCount = 0
                //    },
                //    new EventResult {
                //        Margin = EventResult.MarginType.Success,
                //        LeaderAlignment = AlignmentMaskType.Any,
                //        Condition = Constants.Empty.Conditions,
                //        Actions = ActionsBuilder.New().KingdomActionImproveStat(KingdomStats.Type.Diplomacy).Build(),
                //        StatChanges = new KingdomStats.Changes(){ },
                //        SuccessCount = 0
                //    },
                //    new EventResult {
                //        Margin = EventResult.MarginType.GreatSuccess,
                //        LeaderAlignment = AlignmentMaskType.Any,
                //        Condition = Constants.Empty.Conditions,
                //        Actions = new ActionList(){ },
                //        StatChanges = new KingdomStats.Changes(){ },
                //        SuccessCount = 0
                //    }
                //};

                //EventResult[] strategistEventResults = new EventResult[] {
                //    new EventResult {
                //        Margin = EventResult.MarginType.GreatFail,
                //        LeaderAlignment = AlignmentMaskType.Any,
                //        Condition = Constants.Empty.Conditions,
                //        Actions = new ActionList(){ },
                //        StatChanges = new KingdomStats.Changes(){ },
                //        SuccessCount = 0
                //    },
                //    new EventResult {
                //        Margin = EventResult.MarginType.Fail,
                //        LeaderAlignment = AlignmentMaskType.Any,
                //        Condition = Constants.Empty.Conditions,
                //        Actions = new ActionList(){ },
                //        StatChanges = new KingdomStats.Changes(){ },
                //        SuccessCount = 0
                //    },
                //    new EventResult {
                //        Margin = EventResult.MarginType.Success,
                //        LeaderAlignment = AlignmentMaskType.Any,
                //        Condition = Constants.Empty.Conditions,
                //        Actions = ActionsBuilder.New()
                //            .KingdomActionAddBPRandom(
                //                includeInEventStats: true,
                //                bonus: 1000,
                //                change: new DiceFormula(diceType: DiceType.D100, rollsCount: 10),
                //                resourceType: KingdomResource.Finances)
                //            .KingdomActionAddBPRandom(
                //                includeInEventStats: true,
                //                bonus: 100,
                //                change: new DiceFormula(diceType: DiceType.D100, rollsCount: 1),
                //                resourceType: KingdomResource.Materials)
                //            .Build(),
                //        StatChanges = new KingdomStats.Changes(){ },
                //        SuccessCount = 0
                //    },
                //    new EventResult {
                //        Margin = EventResult.MarginType.GreatSuccess,
                //        LeaderAlignment = AlignmentMaskType.Any,
                //        Condition = Constants.Empty.Conditions,
                //        Actions = new ActionList(){ },
                //        StatChanges = new KingdomStats.Changes(){ },
                //        SuccessCount = 0
                //    }
                //};

                //var diplomatActionsList = ActionsBuilder.New().RemoveItemFromPlayer(
                //    money: false,
                //    removeAll: false,
                //    itemToRemove: HadesBlade_EasterEgg,
                //    silent: false,
                //    quantity: 1,
                //    percentage: 0.0f)
                //    .KingdomActionStartEvent(
                //        eventValue: reforgingProject_Config.NextEventGuid,
                //        randomRegion: false,
                //        delayDays: 0,
                //        startNextMonth: false,
                //        checkTriggerImmediately: false,
                //        checkTriggerOnStart: false)
                //    .Conditional(
                //        conditions: ConditionsBuilder.New().ObjectiveStatus(negate: false, state: QuestObjectiveState.Started, questObjective: Obj4_1),
                //        ifTrue: ActionsBuilder.New())
                //    .Build();
                //EventResult[] diplomatEventResults = new EventResult[] {
                //    new EventResult {
                //        Margin = EventResult.MarginType.GreatFail,
                //        LeaderAlignment = AlignmentMaskType.Any,
                //        Condition = Constants.Empty.Conditions,
                //        Actions = new ActionList(){ },
                //        StatChanges = new KingdomStats.Changes(){ },
                //        SuccessCount = 0
                //    },
                //    new EventResult {
                //        Margin = EventResult.MarginType.Fail,
                //        LeaderAlignment = AlignmentMaskType.Any,
                //        Condition = Constants.Empty.Conditions,
                //        Actions = new ActionList(){ },
                //        StatChanges = new KingdomStats.Changes(){ },
                //        SuccessCount = 0
                //    },
                //    new EventResult {
                //        Margin = EventResult.MarginType.Success,
                //        LeaderAlignment = AlignmentMaskType.Any,
                //        Condition = Constants.Empty.Conditions,
                //        Actions = diplomatActionsList,
                //        StatChanges = new KingdomStats.Changes(){ },
                //        SuccessCount = 0
                //    },
                //    new EventResult {
                //        Margin = EventResult.MarginType.GreatSuccess,
                //        LeaderAlignment = AlignmentMaskType.Any,
                //        Condition = Constants.Empty.Conditions,
                //        Actions = new ActionList(){ },
                //        StatChanges = new KingdomStats.Changes(){ },
                //        SuccessCount = 0
                //    }
                //};
                //Main.logger.Log("DEBUG: Built Reforge Project Event Results");

                //var eventSolutionCounselor = new PossibleEventSolution()
                //{
                //    Leader = LeaderType.Counselor,
                //    CanBeSolved = false,
                //    DCModifier = 0,
                //    SuccessCount = 0,
                //    Resolutions = counselorEventResults
                //};
                //var eventSolutionStrategist = new PossibleEventSolution()
                //{
                //    Leader = LeaderType.Strategist,
                //    CanBeSolved = false,
                //    DCModifier = 0,
                //    SuccessCount = 0,
                //    Resolutions = strategistEventResults
                //};
                //var eventSolutionDiplomat = new PossibleEventSolution()
                //{
                //    Leader = LeaderType.Diplomat,
                //    CanBeSolved = true,
                //    DCModifier = 0,
                //    SuccessCount = 0,
                //    Resolutions = diplomatEventResults
                //};
                //var eventSolutionGeneral = new PossibleEventSolution()
                //{
                //    Leader = LeaderType.General,
                //    CanBeSolved = false,
                //    DCModifier = 0,
                //    SuccessCount = 0,
                //    Resolutions = new EventResult[] { }
                //};
                //Main.logger.Log("DEBUG: Built Reforge Project Event Solutions");


                //var reforgingProject = KingdomProjectConfigurator.New(reforgingProject_Config.Name, reforgingProject_Config.Guid)
                //    .SetLocalizedName(reforgingProject_Config.DisplayName)
                //    .SetLocalizedDescription(reforgingProject_Config.Description)
                //    .SetDefaultResolutionDescription(reforgingProject_Config.DefaultResolutionDescription)
                //    .SetMechanicalDescription(reforgingProject_Config.MechanicalDescription)
                //    .SetInfoType(KingomEventInfoType.None)
                //    .SetResolutionTime(5)
                //    .SetResolveAutomatically(false)
                //    .SetNeedToVisitTheThroneRoom(false)
                //    .SetAICanCheat(false)
                //    .SetSkipRoll(true)
                //    .SetResolutionDC(1)
                //    .SetAutoResolveResult(EventResult.MarginType.Success)
                //    .SetDefaultResolutionType(LeaderType.Diplomat)
                //    .SetAIStopping(false)
                //    .SetProjectType(KingdomProjectType.Relics)
                //    .SetSpendRulerTimeDays(0)
                //    .SetRepeatable(false)
                //    .SetCooldown(0)
                //    .SetIsRankUpProject(false)
                //    .SetRankupProjectFor(KingdomStats.Type.Strategy)
                //    .SetAIPriority(0)
                //    .SetTriggerCondition(reforgingProjectTriggerCondition)
                //    .SetProjectStartCost(
                //        projectStartCost: new KingdomResourcesAmount()
                //        {
                //            m_Finances = 1000,
                //            m_Materials = 100,
                //            m_Favors = 0
                //        })
                //    .AddEventItemCost(amount: 1, items: new() { HadesBlade_EasterEgg.ToReference<BlueprintItemReference>() })
                //    .SetSolutions(new PossibleEventSolutions()
                //    {
                //        Entries = new PossibleEventSolution[] {
                //            eventSolutionCounselor,
                //            eventSolutionStrategist,
                //            eventSolutionDiplomat,
                //            eventSolutionGeneral
                //            }
                //    })
                //    .Configure();
                Main.logger.Log("DEBUG: Built Reforge Project => " + reforgingProject.AssetGuidThreadSafe);
                #endregion

                #region Add Projects
                if (Main.Settings.useRelicsOfTheRighteous == false) { return; }
                AddProjectToKingdomRoot(enchantingProject1);
                Main.logger.Log("DEBUG: Added Enchanting Project => " + enchantingProject1.AssetGuidThreadSafe);
                AddProjectToKingdomRoot(enchantingProject2);
                Main.logger.Log("DEBUG: Added Enchanting Project => " + enchantingProject2.AssetGuidThreadSafe);
                AddProjectToKingdomRoot(reforgingProject);
                Main.logger.Log("DEBUG: Added Reforge Project => " + reforgingProject.AssetGuidThreadSafe);
                #endregion
            }
        }
    }
}
