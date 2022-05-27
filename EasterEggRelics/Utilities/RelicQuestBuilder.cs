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
using Kingmaker.Blueprints.Items;
using Kingmaker.Designers.Quests.Common;

namespace RelicsOfTheRighteous.Utilities
{
    class RelicQuestBuilder
    {
        public static BlueprintKingdomProject BuildReforgeProject(string name, string guid, string displayName, string description, BlueprintItemReference item, bool isAct3, BlueprintKingdomEventBaseReference nextEvent)
        {

            var projectTriggerCondition = ConditionsBuilder.New().ItemsEnough(
                itemToCheck: item,
                money: false,
                negate: false,
                quantity: 1);

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

            BlueprintQuestObjective Obj4_1 = ResourcesLibrary.TryGetBlueprint<BlueprintQuestObjective>("50820c80a44743ab8e29d36aef9c03d2");
            var diplomatActionsListBuilder = ActionsBuilder.New().RemoveItemFromPlayer(
                money: false,
                removeAll: false,
                itemToRemove: item,
                silent: false,
                quantity: 1,
                percentage: 0.0f)
                .KingdomActionStartEvent(
                    eventValue: nextEvent,
                    randomRegion: false,
                    delayDays: 0,
                    startNextMonth: false,
                    checkTriggerImmediately: false,
                    checkTriggerOnStart: false);
            if (isAct3)
            {
                diplomatActionsListBuilder.Conditional(
                  conditions: ConditionsBuilder.New().ObjectiveStatus(
                      negate: false,
                      state: QuestObjectiveState.Started,
                      questObjective: Obj4_1),
                  ifTrue: ActionsBuilder.New().SetObjectiveStatus(
                      status: SummonPoolCountTrigger.ObjectiveStatus.Complete,
                      objective: Obj4_1,
                      startObjectiveIfNone: false));
            }
            var diplomatActionsList = diplomatActionsListBuilder.Build();
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

            var reforgeProject = KingdomProjectConfigurator.New(name, guid)
                        .SetLocalizedName(LocalizationTool.CreateString(name + "NameKey", displayName, false))
                        .SetLocalizedDescription(LocalizationTool.CreateString(name + "DescriptionKey", description))
                        .SetDefaultResolutionDescription(LocalizationTool.CreateString(name + "DefDescriptionKey", "Everything is ready for the relic's augmentation.", false))
                        .SetMechanicalDescription(LocalizationTool.CreateString(name + "MechDescriptionKey", "The relic will be augmented.", false))
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
                        .AddEventItemCost(amount: 1, items: new() { item })
                        .SetSolutions(new PossibleEventSolutions()
                        {
                            Entries = new PossibleEventSolution[] {
                            eventSolutionCounselor,
                            eventSolutionStrategist,
                            eventSolutionDiplomat,
                            eventSolutionGeneral }
                        })
                        .Configure();
            Tools.LogMessage("Built: " + name + " Reforge Project -> " + reforgeProject.AssetGuidThreadSafe);

            return reforgeProject;
        }

        public static BlueprintCrusadeEvent BuildReforgingEvent(string name, string guid, string displayName, string Description)
        {
            PossibleEventSolutions emptySolutions = new();
            emptySolutions.Entries.AddItem(Helpers.Create<PossibleEventSolution>(c => { c.Leader = LeaderType.Counselor; c.CanBeSolved = false; c.DCModifier = 0; c.SuccessCount = 0; }));
            emptySolutions.Entries.AddItem(Helpers.Create<PossibleEventSolution>(c => { c.Leader = LeaderType.Strategist; c.CanBeSolved = false; c.DCModifier = 0; c.SuccessCount = 0; }));
            emptySolutions.Entries.AddItem(Helpers.Create<PossibleEventSolution>(c => { c.Leader = LeaderType.Diplomat; c.CanBeSolved = false; c.DCModifier = 0; c.SuccessCount = 0; }));
            emptySolutions.Entries.AddItem(Helpers.Create<PossibleEventSolution>(c => { c.Leader = LeaderType.General; c.CanBeSolved = false; c.DCModifier = 0; c.SuccessCount = 0; }));

            EventSolution[] reforgingEventSolutions = new EventSolution[]
            {
                    Helpers.Create<EventSolution>(c=>{  })
            };

            var reforgingEvent = CrusadeEventConfigurator.New(name, guid)
                .SetLocalizedName(LocalizationTool.CreateString(name + "NameKey", displayName, false))
                .SetLocalizedDescription(LocalizationTool.CreateString(name + "DescriptionKey", Description, false))
                .SetTriggerCondition(ConditionsBuilder.New())
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
                .SetEventSolutions(reforgingEventSolutions)
                .Configure();

            return reforgingEvent;
        }
    }
}
