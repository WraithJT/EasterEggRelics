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

namespace RelicsOfTheRighteous.NewContent.KingdomProjects
{
    class HadesSword_ReforgeProject
    {
        private static readonly string BaseName = "HadesBlade";

        public static readonly string ProjectGuid = "bfc04a80c017481eacd4d53faaaf6410";
        private static readonly string ProjectName = BaseName + "_ReforgeProject";
        private static readonly string ProjectDisplayName = "The Fate of The Blade of No Escape";
        private static readonly string ProjectDescription = "A skilled craftsman can do some work on the relic.";



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
                // testing references
                var LongswordPlus1 = ResourcesLibrary.TryGetBlueprint<BlueprintItemWeapon>("03d706655c07d804cb9d5a5583f9aec5");
                var DaggerPlus1 = ResourcesLibrary.TryGetBlueprint<BlueprintItemWeapon>("2a45458f776442e43bba57de65f9b738");
                // end testing references

                // Required references
                KingdomRoot kingdomRoot = ResourcesLibrary.TryGetBlueprint<KingdomRoot>("f6bd33651fb0ad64d8fa659d3df6e7df");
                BlueprintQuestObjective Obj4_1 = ResourcesLibrary.TryGetBlueprint<BlueprintQuestObjective>("50820c80a44743ab8e29d36aef9c03d2");

                // Relic quest initiating item
                BlueprintItemWeapon HadesBlade_EasterEgg = ResourcesLibrary.TryGetBlueprint<BlueprintItemWeapon>("372e460865964e4a8e98ebf73f0741ae");

                // Component variables
                string unlockFlag1Name = "";
                string unlockFlag1Guid = BlueprintGuid.NewGuid().ToString();
                string unlockFlag2Name = "";
                string unlockFlag2Guid = BlueprintGuid.NewGuid().ToString();

                string finalItem1_Enchant1Name = "";
                string finalItem1_Enchant1Guid = BlueprintGuid.NewGuid().ToString();
                string finalItem1_Enchant2Name = "";
                string finalItem1_Enchant2Guid = BlueprintGuid.NewGuid().ToString();
                string finalItem1_Enchant3Name = "";
                string finalItem1_Enchant3Guid = BlueprintGuid.NewGuid().ToString();

                string finalItem2_Enchant1Name = "";
                string finalItem2_Enchant1Guid = BlueprintGuid.NewGuid().ToString();
                string finalItem2_Enchant2Name = "";
                string finalItem2_Enchant2Guid = BlueprintGuid.NewGuid().ToString();
                string finalItem2_Enchant3Name = "";
                string finalItem2_Enchant3Guid = BlueprintGuid.NewGuid().ToString();

                string intermediateItem1Name = "";
                string intermediateItem1Guid = BlueprintGuid.NewGuid().ToString();

                string intermediateItem2Name = "";
                string intermediateItem2Guid = BlueprintGuid.NewGuid().ToString();

                string enchantingEventName = "";
                string enchantingEventGuid = BlueprintGuid.NewGuid().ToString();

                string item1_EnchantingDecreeName = "";
                string item1_EnchantingDecreeGuid = BlueprintGuid.NewGuid().ToString();

                string item2_EnchantingDecreeName = "";
                string item2_EnchantingDecreeGuid = BlueprintGuid.NewGuid().ToString();

                string reforgeEventName = "";
                string reforgeEventGuid = BlueprintGuid.NewGuid().ToString();

                string reforgeDecreeName = "";
                string reforgeDecreeGuid = BlueprintGuid.NewGuid().ToString();

                string etudeName = "";
                string etudeGuid = BlueprintGuid.NewGuid().ToString();


                // Base config items



                /*/ NOTES
                 * figure out Choice Effects preview (TextTemplateEngine, s_TemplatesByTag)
                 * 
                /*/

                // One UnlockableFlag for each item type (sword, ring, usable item, etc)
                var unlockFlag1 = UnlockableFlagConfigurator.New(unlockFlag1Name, unlockFlag1Guid).Configure();
                var unlockFlag2 = UnlockableFlagConfigurator.New(unlockFlag2Name, unlockFlag2Guid).Configure();

                // One final item for each item type and enchantment path. All items share the same enchantment paths
                var finalItem1_Enchant1 = ItemConfigurator.New(finalItem1_Enchant1Name, finalItem1_Enchant1Guid).Configure();
                var finalItem1_Enchant2 = ItemConfigurator.New(finalItem1_Enchant2Name, finalItem1_Enchant2Guid).Configure();
                var finalItem1_Enchant3 = ItemConfigurator.New(finalItem1_Enchant3Name, finalItem1_Enchant3Guid).Configure();

                var finalItem2_Enchant1 = ItemConfigurator.New(finalItem2_Enchant1Name, finalItem2_Enchant1Guid).Configure();
                var finalItem2_Enchant2 = ItemConfigurator.New(finalItem2_Enchant2Name, finalItem2_Enchant2Guid).Configure();
                var finalItem2_Enchant3 = ItemConfigurator.New(finalItem2_Enchant3Name, finalItem2_Enchant3Guid).Configure();

                // One intermediate item for each item type
                var intermediateItem1 = ItemConfigurator.New(intermediateItem1Name, intermediateItem1Guid).Configure();
                var intermediateItem2 = ItemConfigurator.New(intermediateItem2Name, intermediateItem2Guid).Configure();

                // One etude total
                var etude = EtudeConfigurator.New(etudeName, etudeGuid)
                    .AddEtudePlayTrigger(
                        actions: ActionsBuilder.New()
                            .KingdomActionStartEvent(
                                eventValue: enchantingEventGuid,     // need to build event first? or at least have the Guid available
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

                var intermediateSword = ItemWeaponConfigurator.New("HadesIntermediateSword", "7E934391-7E13-419A-AD49-033B7B40764C")
                    .SetDisplayNameText(LocalizationTool.CreateString("IntermediateSwordNameKey", "Hades Intermediate Sword", false))
                    .SetDescriptionText(LocalizationTool.CreateString("IntermediateSwordDescKey", "This is an intermediate step of relic creation."))
                    .SetFlavorText(LocalizationTool.CreateString("IntermediateSwordFTKey", "Flavor town text", false))
                    .SetType("d56c44bc9eb10204c8b386a02c7eed21")
                    .SetIcon(LongswordPlus1.Icon)
                    .SetCost(5000)
                    .SetVisualParameters(new WeaponVisualParameters
                    {
                        m_Projectiles = Array.Empty<BlueprintProjectileReference>(),
                        m_PossibleAttachSlots = Array.Empty<UnitEquipmentVisualSlotType>(),
                    })
                    .SetCR(13)
                    .Configure();



                var projectTriggerCondition = ConditionsBuilder.New().ItemsEnough(
                    itemToCheck: HadesBlade_EasterEgg,
                    money: false,
                    negate: false,
                    quantity: 1);



                BlueprintUnlockableFlag flag_1 = UnlockableFlagConfigurator.New("HadesBladeSwordProject_Enchanting", "2D4A4118-9318-45F6-8D5E-3962D73E0057").Configure();
                BlueprintItem item_1 = ItemConfigurator.New("Hades Sword", "95CBB4DC-E80E-4360-90A5-82A463F6181A")
                    .SetDisplayNameText(LocalizationTool.CreateString("HadesSwordNameKey", "Hades' Sword", false))
                    .SetDescriptionText(LocalizationTool.CreateString("HadesSwordDescNameKey", "This is an intermediate step of relic creation.", false))
                    .SetIcon(LongswordPlus1.Icon)
                    .Configure();
                RelicSolution relicSolution_1 = new()
                {
                    Flag = flag_1,
                    Item = intermediateSword,
                    SolutionText = "{g|HadesBlade_Sword}[Choice effects]{/g} A sword",
                    ResultText = "Sword."
                };
                Tools.LogMessage("DEBUG: Built relicSolution_1");

                BlueprintUnlockableFlag flag_2 = UnlockableFlagConfigurator.New("HadesBladeDaggerProject_Enchanting", "434945E4-389B-4795-A457-CF526672437A").Configure();
                BlueprintItem item_2 = ItemConfigurator.New("Hades Dagger", "AF91C3EC-89E7-4D93-BDAA-BF0B238D52AC")
                    .SetDisplayNameText(LocalizationTool.CreateString("HadesDaggerNameKey", "Hades' Dagger", false))
                    .SetDescriptionText(LocalizationTool.CreateString("HadesDaggerDescNameKey", "This is an intermediate step of relic creation.", false))
                    .SetIcon(DaggerPlus1.Icon)
                    .Configure();
                RelicSolution relicSolution_2 = new()
                {
                    Flag = flag_2,
                    Item = DaggerPlus1,
                    SolutionText = "{g|HadesBlade_Dagger}[Choice effects]{/g} A dagger",
                    ResultText = "Dagger"
                };
                Tools.LogMessage("DEBUG: Built relicSolution_2");

                RelicSolution[] relicSolutions = new RelicSolution[] { relicSolution_1, relicSolution_2 };
                string reforgingEventName = BaseName + "_Reforging_event";
                string reforgingEventGuid = "2aa61a2bdb3c4c9889d325565845e840";
                //string reforgingEventDisplayName = "The Fate of The Blade of No Escape";
                string reforgingEventDescription =
                    "We reforgin bro";
                Tools.LogMessage("DEBUG: set base variables");
                //var reforgingEvent = RelicQuestBuilder.BuildReforgingEvent(
                //    name: reforgingEventName,
                //    guid: reforgingEventGuid,
                //    displayName: ProjectDisplayName,
                //    description: reforgingEventDescription,
                //    solutions: relicSolutions);

                // readonly string[] _myArray 
                //= Enumerable.Range(1, 1000)
                //    .Select(i => i.ToString())
                //    .ToArray();

                //List<EventSolution> reforgingEventSolutions = new();
                //foreach (RelicSolution rs in relicSolutions)
                //{
                //EventSolution[] eSolutions = relicSolutions.Select<EventSolution>(rs => {
                //    Helpers.Create<EventSolution>(c =>
                //    {
                //        var ab = ActionsBuilder.New();
                //        if (rs.Item is BlueprintItemEquipmentHand) { ab.GiveHandSlotItemToPlayer(rs.Item, identify: true, quantity: 1, silent: false); }
                //        else if (rs.Item is BlueprintItemEquipment) { ab.GiveEquipmentToPlayer(rs.Item, identify: true, quantity: 1, silent: false); }
                //        else { ab.GiveItemToPlayer(rs.Item, identify: true, quantity: 1, silent: false); }
                //        c.m_SolutionText = LocalizationTool.CreateString(BlueprintGuid.NewGuid().ToString(), rs.SolutionText, false);
                //        c.m_AvailConditions = Constants.Empty.Conditions;
                //        c.m_UnavailingBehaviour = UnavailingBehaviour.HideSolution;
                //        c.m_SuccessEffects = ab.UnlockFlag(rs.Flag, flagValue: 1).Build();
                //        c.m_ResultText = LocalizationTool.CreateString(BlueprintGuid.NewGuid().ToString(), rs.ResultText, false);
                //    });
                //})

                //    reforgingEventSolutions.AddItem(es);
                //}
                Tools.LogMessage("DEBUG: Built relicSolutions List");


                //EventSolution[] reforgingEventSolutions = solutions.Select(r => {
                //    return Helpers.Create<EventSolution>(c => {
                //        c.m_SuccessEffects.Actions = ActionsBuilder.New().UnlockFlag(r.Flag).Build().Actions;
                //    });
                //}).ToArray();

                // convert RelicSolution to a List<string, BlueprintItem, BlueprintUnlockableFlag, string>

                List<RelicSolution> relics = new();

                // testing
                //UnlockFlag tflag = new()
                //{
                //    name = "$UnlockFlag$" + BlueprintGuid.NewGuid().ToString(),
                //    m_flag = flag_2.ToReference<BlueprintUnlockableFlagReference>(),
                //    flagValue = 1
                //};

                //AddItemToPlayer addItemToPlayer = new()
                //{
                //    name = "$AddItemToPlayer$" + BlueprintGuid.NewGuid().ToString(),
                //    m_ItemToGive = DaggerPlus1.ToReference<BlueprintItemReference>(),
                //    Silent = false,
                //    Quantity = 1,
                //    Identify = true,
                //    Equip = false,
                //    PreferredWeaponSet = 0,
                //    ErrorIfDidNotEquip = true
                //};

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
                Tools.LogMessage("DEBUG: Built emptySolutions");


                //{g|KnightsEmblem_Belt}[Choice effects]{/g} Belt

                //{g|KnightsEmblem_Chainmail_Unholy}[Choice effects]{/g} Shame the fallen knight Linds
                EventSolution[] eventSolutions = new EventSolution[]
                {
                    Helpers.Create<EventSolution>(c =>
                    {
                        c.m_SolutionText = LocalizationTool.CreateString(nameof(DaggerPlus1)+"STKey", @"{g|KnightsEmblem_Belt}[Choice effects]{/g}Dagger", false);
                        c.m_AvailConditions = Constants.Empty.Conditions;
                        c.m_UnavailingBehaviour = UnavailingBehaviour.HideSolution;
                        c.m_UnavailingPlaceholder = Constants.Empty.String;
                        c.m_SuccessEffects = ActionsBuilder.New()
                            .GiveHandSlotItemToPlayer(DaggerPlus1, identify: true, quantity: 1, silent: false)
                            .UnlockFlag(flag_2, flagValue: 1)
                            .Build();
                        c.m_ResultText = LocalizationTool.CreateString(nameof(DaggerPlus1)+"RTKey", "We make dagger", false);
                    }),
                    Helpers.Create<EventSolution>(c =>
                    {
                        c.m_SolutionText = LocalizationTool.CreateString(nameof(LongswordPlus1)+"STKey", @"{g|KnightsEmblem_Chainmail_Unholy}[Choice effects]{/g}"+nameof(LongswordPlus1), false);
                        c.m_AvailConditions = Constants.Empty.Conditions;
                        c.m_UnavailingBehaviour = UnavailingBehaviour.HideSolution;
                        c.m_UnavailingPlaceholder = Constants.Empty.String;
                        c.m_SuccessEffects = ActionsBuilder.New()
                            .GiveHandSlotItemToPlayer(intermediateSword, identify: true, quantity: 1, silent: false)
                            .UnlockFlag(flag_1, flagValue: 1)
                            .Build();
                        c.m_ResultText = LocalizationTool.CreateString(nameof(LongswordPlus1)+"RTKey", "We make Sword", false);
                    })
                };
                //
                //BlueprintCrusadeEvent UnholySymbolOfRovagug_Reforging_event = ResourcesLibrary.TryGetBlueprint<BlueprintCrusadeEvent>("e90dc6bf9f454a2c9879a1cb6b314fc8");
                //eventSolutions.TrimExcess();
                //EventSolution[] eventSolutions1 = eventSolutions.ToArray();

                var reforgingEvent = CrusadeEventConfigurator.New(reforgingEventName, reforgingEventGuid)
                    .SetLocalizedName(LocalizationTool.CreateString(reforgingEventName + "NameKey", ProjectDisplayName, false))
                    .SetLocalizedDescription(LocalizationTool.CreateString(reforgingEventName + "DescriptionKey", reforgingEventDescription, false))
                    .SetResolutionTime(14)
                    .SetResolveAutomatically(false)
                    .SetNeedToVisitTheThroneRoom(false)
                    .SetAICanCheat(false)
                    .SetSkipRoll(false)
                    .SetResolutionDC(0)
                    .SetAutoResolveResult(EventResult.MarginType.Success)
                    .SetDefaultResolutionType(LeaderType.Counselor)
                    .SetAIStopping(false)
                    .SetSolutions(emptySolutions)
                    .SetEventSolutions(eventSolutions)
                    .Configure();
                reforgingEvent.TriggerCondition = Constants.Empty.Conditions;
                reforgingEvent.Comment = Constants.Empty.String;
                reforgingEvent.DefaultResolutionDescription = Constants.Empty.String;
                Tools.LogMessage("DEBUG: Built reforgingEvent");

                #region old method
                var counselorSuccessActions = ActionsBuilder.New().KingdomActionImproveStat(KingdomStats.Type.Diplomacy).Build();
                EventResult[] counselorEventResults = new EventResult[] {
                    new EventResult {
                        Margin = EventResult.MarginType.GreatFail,
                        LeaderAlignment = AlignmentMaskType.Any,
                        Condition = Constants.Empty.Conditions,
                        Actions = new ActionList(){ },
                        StatChanges = new KingdomStats.Changes(){ },
                        SuccessCount = 0
                    },
                    new EventResult {
                        Margin = EventResult.MarginType.Fail,
                        LeaderAlignment = AlignmentMaskType.Any,
                        Condition = Constants.Empty.Conditions,
                        Actions = new ActionList(){ },
                        StatChanges = new KingdomStats.Changes(){ },
                        SuccessCount = 0
                    },
                    new EventResult {
                        Margin = EventResult.MarginType.Success,
                        LeaderAlignment = AlignmentMaskType.Any,
                        Condition = Constants.Empty.Conditions,
                        Actions = counselorSuccessActions,
                        StatChanges = new KingdomStats.Changes(){ },
                        SuccessCount = 0
                    },
                    new EventResult {
                        Margin = EventResult.MarginType.GreatSuccess,
                        LeaderAlignment = AlignmentMaskType.Any,
                        Condition = Constants.Empty.Conditions,
                        Actions = new ActionList(){ },
                        StatChanges = new KingdomStats.Changes(){ },
                        SuccessCount = 0
                    }
                };

                var strategistActionsList = ActionsBuilder.New()
                    .KingdomActionAddBPRandom(
                        includeInEventStats: true,
                        bonus: 1000,
                        change: new DiceFormula(diceType: DiceType.D100, rollsCount: 10),
                        resourceType: KingdomResource.Finances)
                    .KingdomActionAddBPRandom(
                        includeInEventStats: true,
                        bonus: 100,
                        change: new DiceFormula(diceType: DiceType.D100, rollsCount: 1),
                        resourceType: KingdomResource.Materials)
                    .Build();
                EventResult[] strategistEventResults = new EventResult[] {
                    new EventResult {
                        Margin = EventResult.MarginType.GreatFail,
                        LeaderAlignment = AlignmentMaskType.Any,
                        Condition = Constants.Empty.Conditions,
                        Actions = new ActionList(){ },
                        StatChanges = new KingdomStats.Changes(){ },
                        SuccessCount = 0
                    },
                    new EventResult {
                        Margin = EventResult.MarginType.Fail,
                        LeaderAlignment = AlignmentMaskType.Any,
                        Condition = Constants.Empty.Conditions,
                        Actions = new ActionList(){ },
                        StatChanges = new KingdomStats.Changes(){ },
                        SuccessCount = 0
                    },
                    new EventResult {
                        Margin = EventResult.MarginType.Success,
                        LeaderAlignment = AlignmentMaskType.Any,
                        Condition = Constants.Empty.Conditions,
                        Actions = strategistActionsList,
                        StatChanges = new KingdomStats.Changes(){ },
                        SuccessCount = 0
                    },
                    new EventResult {
                        Margin = EventResult.MarginType.GreatSuccess,
                        LeaderAlignment = AlignmentMaskType.Any,
                        Condition = Constants.Empty.Conditions,
                        Actions = new ActionList(){ },
                        StatChanges = new KingdomStats.Changes(){ },
                        SuccessCount = 0
                    }
                };

                //EventResult[] emptyEventResults = new EventResult[] {
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
                //        Actions = new ActionList(){ },
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


                var diplomatActionsList = ActionsBuilder.New().RemoveItemFromPlayer(
                    money: false,
                    removeAll: false,
                    itemToRemove: HadesBlade_EasterEgg,
                    silent: false,
                    quantity: 1,
                    percentage: 0.0f)
                    .KingdomActionStartEvent(
                        eventValue: reforgingEvent,
                        randomRegion: false,
                        delayDays: 0,
                        startNextMonth: false,
                        checkTriggerImmediately: false,
                        checkTriggerOnStart: false)
                    .Conditional(
                        conditions: ConditionsBuilder.New().ObjectiveStatus(negate: false, state: QuestObjectiveState.Started, questObjective: Obj4_1),
                        ifTrue: ActionsBuilder.New())
                    .Build();
                EventResult[] diplomatEventResults = new EventResult[] {
                    new EventResult {
                        Margin = EventResult.MarginType.GreatFail,
                        LeaderAlignment = AlignmentMaskType.Any,
                        Condition = Constants.Empty.Conditions,
                        Actions = new ActionList(){ },
                        StatChanges = new KingdomStats.Changes(){ },
                        SuccessCount = 0
                    },
                    new EventResult {
                        Margin = EventResult.MarginType.Fail,
                        LeaderAlignment = AlignmentMaskType.Any,
                        Condition = Constants.Empty.Conditions,
                        Actions = new ActionList(){ },
                        StatChanges = new KingdomStats.Changes(){ },
                        SuccessCount = 0
                    },
                    new EventResult {
                        Margin = EventResult.MarginType.Success,
                        LeaderAlignment = AlignmentMaskType.Any,
                        Condition = Constants.Empty.Conditions,
                        Actions = diplomatActionsList,
                        StatChanges = new KingdomStats.Changes(){ },
                        SuccessCount = 0
                    },
                    new EventResult {
                        Margin = EventResult.MarginType.GreatSuccess,
                        LeaderAlignment = AlignmentMaskType.Any,
                        Condition = Constants.Empty.Conditions,
                        Actions = new ActionList(){ },
                        StatChanges = new KingdomStats.Changes(){ },
                        SuccessCount = 0
                    }
                };

                var eventSolutionCounselor = new PossibleEventSolution()
                {
                    Leader = LeaderType.Counselor,
                    CanBeSolved = false,
                    DCModifier = 0,
                    SuccessCount = 0,
                    Resolutions = counselorEventResults
                };
                var eventSolutionStrategist = new PossibleEventSolution()
                {
                    Leader = LeaderType.Strategist,
                    CanBeSolved = false,
                    DCModifier = 0,
                    SuccessCount = 0,
                    Resolutions = strategistEventResults
                };
                var eventSolutionDiplomat = new PossibleEventSolution()
                {
                    Leader = LeaderType.Diplomat,
                    CanBeSolved = true,
                    DCModifier = 0,
                    SuccessCount = 0,
                    Resolutions = diplomatEventResults
                };
                var eventSolutionGeneral = new PossibleEventSolution()
                {
                    Leader = LeaderType.General,
                    CanBeSolved = false,
                    DCModifier = 0,
                    SuccessCount = 0,
                    Resolutions = new EventResult[] { }
                };

                var reforgeProject = KingdomProjectConfigurator.New(ProjectName, ProjectGuid)
                    .SetLocalizedName(LocalizationTool.CreateString(ProjectName + "NameKey", ProjectDisplayName, false))
                    .SetLocalizedDescription(LocalizationTool.CreateString(ProjectName + "DescKey", ProjectDescription))
                    .SetDefaultResolutionDescription(LocalizationTool.CreateString("f879ca1671c048a196808408b78a6349", "Everything is ready for the relic's augmentation.", false))
                    .SetMechanicalDescription(LocalizationTool.CreateString("f110a611db4841aeb95644529fb5087d", "The relic will be augmented.", false))
                    .SetInfoType(KingomEventInfoType.None)
                    .SetResolutionTime(5)
                    .SetResolveAutomatically(false)
                    .SetNeedToVisitTheThroneRoom(false)
                    .SetAICanCheat(false)
                    .SetSkipRoll(true)
                    .SetResolutionDC(1)
                    .SetAutoResolveResult(EventResult.MarginType.Success)
                    .SetDefaultResolutionType(LeaderType.Diplomat)
                    .SetAIStopping(false)
                    .SetProjectType(KingdomProjectType.Relics)
                    .SetSpendRulerTimeDays(0)
                    .SetRepeatable(false)
                    .SetCooldown(0)
                    .SetIsRankUpProject(false)
                    .SetRankupProjectFor(KingdomStats.Type.Strategy)
                    .SetAIPriority(0)
                    .SetTriggerCondition(projectTriggerCondition)
                    .SetProjectStartCost(
                        projectStartCost: new KingdomResourcesAmount()
                        {
                            m_Finances = 1000,
                            m_Materials = 100,
                            m_Favors = 0
                        })
                    .AddEventItemCost(amount: 1, items: new() { HadesBlade_EasterEgg.ToReference<BlueprintItemReference>() })
                    .SetSolutions(new PossibleEventSolutions()
                    {
                        Entries = new PossibleEventSolution[] {
                            eventSolutionCounselor,
                            eventSolutionStrategist,
                            eventSolutionDiplomat,
                            eventSolutionGeneral }
                    })
                    .Configure();
                #endregion

                //var reforgeProject = RelicQuestBuilder.BuildReforgeProject(
                //    name: ProjectName,
                //    guid: ProjectGuid,
                //    displayName: ProjectDisplayName,
                //    description: ProjectDescription,
                //    item: HadesBlade_EasterEgg.ToReference<BlueprintItemReference>(),
                //    isAct3: false,
                //    nextEvent: reforgingEvent.ToReference<BlueprintKingdomEventBaseReference>()); ; ;
                //Tools.LogMessage("Built: Hades Sword Reforge Project -> " + reforgeProject.AssetGuidThreadSafe);

                if (Main.Settings.useRelicsOfTheRighteous == false) { return; }
                kingdomRoot.m_KingdomProjectEvents.Add(reforgeProject.ToReference<BlueprintKingdomProjectReference>());
            }
        }
    }
}
