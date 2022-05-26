using BlueprintCore.Blueprints.Configurators.Classes;
using BlueprintCore.Blueprints.Configurators.Kingdom;
using BlueprintCore.Utils;
using EasterEggRelics.Utilities;
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

namespace EasterEggRelics.NewContent.KingdomProjects
{
    class HadesSword_ReforgeProject
    {
        public static readonly string ProjectGuid = "bfc04a80c017481eacd4d53faaaf6410";
        private static readonly string ProjectName = "HadesBlade_ReforgeProject";
        private static readonly string ProjectDisplayName = "The Fate of The Blade of No Escape";
        private static readonly string ProjectDisplayNameKey = "HadesBlade_ReforgeProjectDisplayNameKey";
        private static readonly string ProjectDescription =
            "A skilled craftsman can do some work on the relic.";
        private static readonly string ProjectDescriptionKey = "HadesBlade_ReforgeProjectDescriptionKey";


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
                KingdomRoot kingdomRoot = ResourcesLibrary.TryGetBlueprint<KingdomRoot>("f6bd33651fb0ad64d8fa659d3df6e7df");
                //string HadesBlade_EasterEggGuid = "372e460865964e4a8e98ebf73f0741ae";
                BlueprintItemWeapon HadesBlade_EasterEgg = ResourcesLibrary.TryGetBlueprint<BlueprintItemWeapon>("372e460865964e4a8e98ebf73f0741ae");

                var actionList = ActionsBuilder.New()
                    .Conditional(
                        ConditionsBuilder.New()
                            .TargetInMeleeRange(),
                        ifTrue: ActionsBuilder.New().Mount()
                        )
                    .Build();


                var triggerCondition = ConditionsBuilder.New().ItemsEnough(
                    itemToCheck: HadesBlade_EasterEgg,
                    money: false,
                    negate: false,
                    quantity: 1);

                var reforgeEvent = CrusadeEventConfigurator.New("", "")
                    .Configure();

                var counselorSuccessActions = ActionsBuilder.New().KingdomActionImproveStat(KingdomStats.Type.Diplomacy).Build();
                EventResult[] counselorEventResults = new EventResult[] {
                    new EventResult {
                        Margin = EventResult.MarginType.GreatFail,
                        LeaderAlignment = AlignmentMaskType.Any,
                        Condition = new ConditionsChecker{ Operation = Operation.And, Conditions = new Condition[]{ } },
                        Actions = new ActionList(){ },
                        StatChanges = new KingdomStats.Changes(){ },
                        SuccessCount = 0
                    },
                    new EventResult {
                        Margin = EventResult.MarginType.Fail,
                        LeaderAlignment = AlignmentMaskType.Any,
                        Condition = new ConditionsChecker{ Operation = Operation.And, Conditions = new Condition[]{ } },
                        Actions = new ActionList(){ },
                        StatChanges = new KingdomStats.Changes(){ },
                        SuccessCount = 0
                    },
                    new EventResult {
                        Margin = EventResult.MarginType.Success,
                        LeaderAlignment = AlignmentMaskType.Any,
                        Condition = new ConditionsChecker{ Operation = Operation.And, Conditions = new Condition[]{ } },
                        Actions = counselorSuccessActions,
                        StatChanges = new KingdomStats.Changes(){ },
                        SuccessCount = 0
                    },
                    new EventResult {
                        Margin = EventResult.MarginType.GreatSuccess,
                        LeaderAlignment = AlignmentMaskType.Any,
                        Condition = new ConditionsChecker{ Operation = Operation.And, Conditions = new Condition[]{ } },
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
                        Condition = new ConditionsChecker{ Operation = Operation.And, Conditions = new Condition[]{ } },
                        Actions = new ActionList(){ },
                        StatChanges = new KingdomStats.Changes(){ },
                        SuccessCount = 0
                    },
                    new EventResult {
                        Margin = EventResult.MarginType.Fail,
                        LeaderAlignment = AlignmentMaskType.Any,
                        Condition = new ConditionsChecker{ Operation = Operation.And, Conditions = new Condition[]{ } },
                        Actions = new ActionList(){ },
                        StatChanges = new KingdomStats.Changes(){ },
                        SuccessCount = 0
                    },
                    new EventResult {
                        Margin = EventResult.MarginType.Success,
                        LeaderAlignment = AlignmentMaskType.Any,
                        Condition = new ConditionsChecker{ Operation = Operation.And, Conditions = new Condition[]{ } },
                        Actions = strategistActionsList,
                        StatChanges = new KingdomStats.Changes(){ },
                        SuccessCount = 0
                    },
                    new EventResult {
                        Margin = EventResult.MarginType.GreatSuccess,
                        LeaderAlignment = AlignmentMaskType.Any,
                        Condition = new ConditionsChecker{ Operation = Operation.And, Conditions = new Condition[]{ } },
                        Actions = new ActionList(){ },
                        StatChanges = new KingdomStats.Changes(){ },
                        SuccessCount = 0
                    }
                };

                EventResult[] emptyEventResults = new EventResult[] {
                    new EventResult {
                        Margin = EventResult.MarginType.GreatFail,
                        LeaderAlignment = AlignmentMaskType.Any,
                        Condition = new ConditionsChecker{ Operation = Operation.And, Conditions = new Condition[]{ } },
                        Actions = new ActionList(){ },
                        StatChanges = new KingdomStats.Changes(){ },
                        SuccessCount = 0
                    },
                    new EventResult {
                        Margin = EventResult.MarginType.Fail,
                        LeaderAlignment = AlignmentMaskType.Any,
                        Condition = new ConditionsChecker{ Operation = Operation.And, Conditions = new Condition[]{ } },
                        Actions = new ActionList(){ },
                        StatChanges = new KingdomStats.Changes(){ },
                        SuccessCount = 0
                    },
                    new EventResult {
                        Margin = EventResult.MarginType.Success,
                        LeaderAlignment = AlignmentMaskType.Any,
                        Condition = new ConditionsChecker{ Operation = Operation.And, Conditions = new Condition[]{ } },
                        Actions = new ActionList(){ },
                        StatChanges = new KingdomStats.Changes(){ },
                        SuccessCount = 0
                    },
                    new EventResult {
                        Margin = EventResult.MarginType.GreatSuccess,
                        LeaderAlignment = AlignmentMaskType.Any,
                        Condition = new ConditionsChecker{ Operation = Operation.And, Conditions = new Condition[]{ } },
                        Actions = new ActionList(){ },
                        StatChanges = new KingdomStats.Changes(){ },
                        SuccessCount = 0
                    }
                };

                BlueprintQuestObjective Obj4_1 = ResourcesLibrary.TryGetBlueprint<BlueprintQuestObjective>("50820c80a44743ab8e29d36aef9c03d2");
                var diplomatActionsList = ActionsBuilder.New().RemoveItemFromPlayer(
                    money: false,
                    removeAll: false,
                    itemToRemove: HadesBlade_EasterEgg,
                    silent: false,
                    quantity: 1,
                    percentage: 0.0f)
                    .KingdomActionStartEvent(
                        eventValue: "",
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
                        Condition = new ConditionsChecker{ Operation = Operation.And, Conditions = new Condition[]{ } },
                        Actions = new ActionList(){ },
                        StatChanges = new KingdomStats.Changes(){ },
                        SuccessCount = 0
                    },
                    new EventResult {
                        Margin = EventResult.MarginType.Fail,
                        LeaderAlignment = AlignmentMaskType.Any,
                        Condition = new ConditionsChecker{ Operation = Operation.And, Conditions = new Condition[]{ } },
                        Actions = new ActionList(){ },
                        StatChanges = new KingdomStats.Changes(){ },
                        SuccessCount = 0
                    },
                    new EventResult {
                        Margin = EventResult.MarginType.Success,
                        LeaderAlignment = AlignmentMaskType.Any,
                        Condition = new ConditionsChecker{ Operation = Operation.And, Conditions = new Condition[]{ } },
                        Actions = diplomatActionsList,
                        StatChanges = new KingdomStats.Changes(){ },
                        SuccessCount = 0
                    },
                    new EventResult {
                        Margin = EventResult.MarginType.GreatSuccess,
                        LeaderAlignment = AlignmentMaskType.Any,
                        Condition = new ConditionsChecker{ Operation = Operation.And, Conditions = new Condition[]{ } },
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
                    .SetLocalizedName(LocalizationTool.CreateString(ProjectDisplayNameKey, ProjectDisplayName, false))
                    .SetLocalizedDescription(LocalizationTool.CreateString(ProjectDescriptionKey, ProjectDescription))
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
                    .SetTriggerCondition(triggerCondition)
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
                Tools.LogMessage("Built: Hades Sword Reforge Project -> " + reforgeProject.AssetGuidThreadSafe);

                if (Main.Settings.useEasterEggRelics == false) { return; }
                kingdomRoot.m_KingdomProjectEvents.Add(reforgeProject.ToReference<BlueprintKingdomProjectReference>());
            }
        }
    }
}
