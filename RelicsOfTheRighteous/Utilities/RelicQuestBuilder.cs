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
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using System.Linq;
using System.Collections.Generic;
using Kingmaker.UI.Common;
using Kingmaker.Blueprints.Items.Equipment;
using System.Collections;
using static Kingmaker.UI.Common.ItemsFilter;
using BlueprintCore.Blueprints.Configurators.Items;
using BlueprintCore.Blueprints.Configurators.Items.Weapons;
using BlueprintCore.Blueprints.Configurators;
using BlueprintCore.Blueprints.Configurators.Items.Shields;
using BlueprintCore.Blueprints.Configurators.Items.Armors;
using BlueprintCore.Blueprints.Configurators.Items.Equipment;
using Kingmaker.Blueprints.Items.Armors;
using Kingmaker.Localization;
using Microsoft.Build.Framework;
using Kingmaker.TextTools;

namespace RelicsOfTheRighteous.Utilities
{

    class RelicQuestBuilder
    {
        //public static BlueprintKingdomProject ProjectConstructor(string name, string guid, LocalizedString displayName, LocalizedString description, string requiredItem, LocalizedString defaultResolutionDescription, LocalizedString mechanicalDescription, string nextEventGuid, bool isAct3 = false)
        //{
        //    return ProjectConstructor(new() { Name = name, Guid = guid, DisplayName = displayName, Description = description, RequiredItemGuid = requiredItem, DefaultResolutionDescription = defaultResolutionDescription, MechanicalDescription = mechanicalDescription, NextEventGuid = nextEventGuid }, isAct3);
        //}

        public static BlueprintKingdomProject ProjectConstructor(RelicProject relicProject, bool isEnchantingProject = false)
        {
            var projectTriggerCondition = ConditionsBuilder.New();
            if (isEnchantingProject)
            {
                projectTriggerCondition.FlagUnlocked(
                    conditionFlag: relicProject.UnlockableFlagGuid,
                    exceptSpecifiedValues: false,
                    negate: false);
            }
            else
            {
                projectTriggerCondition.ItemsEnough(
                  itemToCheck: relicProject.RequiredItemGuid,
                  money: false,
                  negate: false,
                  quantity: 1);
            }

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
                        Actions = ActionsBuilder.New().KingdomActionImproveStat(KingdomStats.Type.Diplomacy).Build(),
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
                        Actions = ActionsBuilder.New()
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
                            .Build(),
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

            BlueprintQuestObjective Obj4_1 = ResourcesLibrary.TryGetBlueprint<BlueprintQuestObjective>("50820c80a44743ab8e29d36aef9c03d2");
            var diplomatActionsListBuilder = ActionsBuilder.New().RemoveItemFromPlayer(
                money: false,
                removeAll: false,
                itemToRemove: relicProject.RequiredItemGuid,
                silent: false,
                quantity: 1,
                percentage: 0.0f)
                .KingdomActionStartEvent(
                    eventValue: relicProject.NextEventGuid,
                    randomRegion: false,
                    delayDays: 0,
                    startNextMonth: false,
                    checkTriggerImmediately: false,
                    checkTriggerOnStart: false);
            if (relicProject.IsAct3)
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
                        Actions = diplomatActionsListBuilder.Build(),
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

            var projectBuilder = KingdomProjectConfigurator.New(relicProject.Name, relicProject.Guid)
                .SetLocalizedName(relicProject.DisplayName)
                .SetLocalizedDescription(relicProject.Description)
                .SetDefaultResolutionDescription(relicProject.DefaultResolutionDescription)
                .SetMechanicalDescription(relicProject.MechanicalDescription)
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
                .AddEventItemCost(amount: 1, items: new() { relicProject.RequiredItemGuid })
                .SetSolutions(new PossibleEventSolutions()
                {
                    Entries = new PossibleEventSolution[] {
                    eventSolutionCounselor,
                    eventSolutionStrategist,
                    eventSolutionDiplomat,
                    eventSolutionGeneral }
                });
            if (isEnchantingProject)
            {
                projectBuilder.SetProjectStartCost(
                    projectStartCost: new KingdomResourcesAmount()
                    {
                        m_Finances = 500,
                        m_Materials = 50,
                        m_Favors = 50
                    });
            }
            else
            {
                projectBuilder.SetProjectStartCost(
                    projectStartCost: new KingdomResourcesAmount()
                    {
                        m_Finances = 1000,
                        m_Materials = 100,
                        m_Favors = 0
                    });
            }
            var project = projectBuilder.Configure();
            Tools.LogMessage("Built: " + relicProject.Name + " Reforge Project -> " + project.AssetGuidThreadSafe);

            return project;
        }

        //public static BlueprintCrusadeEvent ReforgingEventConstructor(string name, string guid, LocalizedString displayName, LocalizedString description, EventSolution eventSolution)
        //{
        //    EventSolution[] eventSolutions = new EventSolution[] { eventSolution };
        //    return ReforgingEventConstructor(name, guid, displayName, description, eventSolutions);
        //}

        public static BlueprintCrusadeEvent EventConstructor(RelicEvent relicEvent)
        {
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

            var _event = CrusadeEventConfigurator.New(relicEvent.Name, relicEvent.Guid)
                .SetLocalizedName(relicEvent.DisplayName)
                .SetLocalizedDescription(relicEvent.Description)
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
                .SetEventSolutions(relicEvent.EventSolutions)
                .Configure();
            _event.TriggerCondition = Constants.Empty.Conditions;
            _event.Comment = Constants.Empty.String;
            _event.DefaultResolutionDescription = Constants.Empty.String;

            return _event;
        }

        //public static EventSolution EventSolutionConstructor(LocalizedString solutionText, BlueprintItem item, BlueprintUnlockableFlag flag, LocalizedString resultText)
        //{
        //    RelicEventSolution relicEventSolution = new()
        //    {
        //        SolutionText = solutionText,
        //        Item = item,
        //        Flag = flag,
        //        ResultText = resultText
        //    };
        //    return EventSolutionConstructor(relicEventSolution);
        //}

        public static EventSolution EventSolutionConstructor(LocalizedString solutionText, BlueprintItem item, BlueprintUnlockableFlag flag, LocalizedString resultText, bool isReforgeEvent = false)
        {
            var ab = ActionsBuilder.New();
            if (item is BlueprintItemEquipmentHand) { ab.GiveHandSlotItemToPlayer(item, identify: true, quantity: 1, silent: false); }
            else if (item is BlueprintItemEquipment) { ab.GiveEquipmentToPlayer(item, identify: true, quantity: 1, silent: false); }
            else { ab.GiveItemToPlayer(item, identify: true, quantity: 1, silent: false); }
            if (isReforgeEvent) { ab.UnlockFlag(flag, flagValue: 1); }
            EventSolution es = Helpers.Create<EventSolution>(c =>
            {
                c.m_SolutionText = solutionText;
                //c.m_AvailConditions = availConditions;
                c.m_UnavailingBehaviour = UnavailingBehaviour.HideSolution;
                c.m_SuccessEffects = ab.Build();
                c.m_ResultText = resultText;
            });
            if (isReforgeEvent) { es.m_AvailConditions = Constants.Empty.Conditions; }
            else
            {
                es.m_AvailConditions = ConditionsBuilder.New()
                    .FlagUnlocked(
                        conditionFlag: flag,
                        exceptSpecifiedValues: false,
                        negate: false).Build();
            }

            return es;
        }

        public static object ItemConstructor(string name, string guid, LocalizedString displayName, LocalizedString description, LocalizedString flavorText, ItemType itemType)
        {
            return ItemConstructor(new() { Name = name, Guid = guid, DisplayName = displayName, Description = description, FlavorText = flavorText, RelicItemType = itemType });
        }

        public static object ItemConstructor(RelicItem relicItem)
        {
            var item = relicItem.RelicItemType switch
            {
                ItemType.Weapon => ItemWeaponConfigurator.New(relicItem.Name, relicItem.Guid).SetDisplayNameText(relicItem.DisplayName).SetDescriptionText(relicItem.Description).SetFlavorText(relicItem.FlavorText).Configure(),
                ItemType.Shield => ItemShieldConfigurator.New(relicItem.Name, relicItem.Guid).SetDisplayNameText(relicItem.DisplayName).SetDescriptionText(relicItem.Description).SetFlavorText(relicItem.FlavorText).Configure(),
                ItemType.Armor => ItemArmorConfigurator.New(relicItem.Name, relicItem.Guid).SetDisplayNameText(relicItem.DisplayName).SetDescriptionText(relicItem.Description).SetFlavorText(relicItem.FlavorText).Configure(),
                ItemType.Ring => ItemEquipmentRingConfigurator.New(relicItem.Name, relicItem.Guid).SetDisplayNameText(relicItem.DisplayName).SetDescriptionText(relicItem.Description).SetFlavorText(relicItem.FlavorText).Configure(),
                ItemType.Belt => ItemEquipmentBeltConfigurator.New(relicItem.Name, relicItem.Guid).SetDisplayNameText(relicItem.DisplayName).SetDescriptionText(relicItem.Description).SetFlavorText(relicItem.FlavorText).Configure(),
                ItemType.Feet => ItemEquipmentFeetConfigurator.New(relicItem.Name, relicItem.Guid).SetDisplayNameText(relicItem.DisplayName).SetDescriptionText(relicItem.Description).SetFlavorText(relicItem.FlavorText).Configure(),
                ItemType.Gloves => ItemEquipmentGlovesConfigurator.New(relicItem.Name, relicItem.Guid).SetDisplayNameText(relicItem.DisplayName).SetDescriptionText(relicItem.Description).SetFlavorText(relicItem.FlavorText).Configure(),
                ItemType.Head => ItemEquipmentHeadConfigurator.New(relicItem.Name, relicItem.Guid).SetDisplayNameText(relicItem.DisplayName).SetDescriptionText(relicItem.Description).SetFlavorText(relicItem.FlavorText).Configure(),
                ItemType.Neck => ItemEquipmentNeckConfigurator.New(relicItem.Name, relicItem.Guid).SetDisplayNameText(relicItem.DisplayName).SetDescriptionText(relicItem.Description).SetFlavorText(relicItem.FlavorText).Configure(),
                ItemType.Shoulders => ItemEquipmentShouldersConfigurator.New(relicItem.Name, relicItem.Guid).SetDisplayNameText(relicItem.DisplayName).SetDescriptionText(relicItem.Description).SetFlavorText(relicItem.FlavorText).Configure(),
                ItemType.Wrist => ItemEquipmentWristConfigurator.New(relicItem.Name, relicItem.Guid).SetDisplayNameText(relicItem.DisplayName).SetDescriptionText(relicItem.Description).SetFlavorText(relicItem.FlavorText).Configure(),
                ItemType.Usable => ItemEquipmentUsableConfigurator.New(relicItem.Name, relicItem.Guid).SetDisplayNameText(relicItem.DisplayName).SetDescriptionText(relicItem.Description).SetFlavorText(relicItem.FlavorText).Configure(),
                ItemType.Glasses => ItemEquipmentGlassesConfigurator.New(relicItem.Name, relicItem.Guid).SetDisplayNameText(relicItem.DisplayName).SetDescriptionText(relicItem.Description).SetFlavorText(relicItem.FlavorText).Configure(),
                ItemType.Shirt => ItemEquipmentShirtConfigurator.New(relicItem.Name, relicItem.Guid).SetDisplayNameText(relicItem.DisplayName).SetDescriptionText(relicItem.Description).SetFlavorText(relicItem.FlavorText).Configure(),
                _ => ItemConfigurator.New(relicItem.Name, relicItem.Guid).SetDisplayNameText(relicItem.DisplayName).SetDescriptionText(relicItem.Description).SetFlavorText(relicItem.FlavorText).Configure(),
            };
            return item;
        }

        public static void AddProjectToKingdomRoot(BlueprintKingdomProject project)
        {
            var kingdomRoot = ResourcesLibrary.TryGetBlueprint<KingdomRoot>("f6bd33651fb0ad64d8fa659d3df6e7df");
            kingdomRoot.m_KingdomProjectEvents.Add(project.ToReference<BlueprintKingdomProjectReference>());
        }

        private static readonly HashSet<string> Added = new();
        private static void Add(string tag, Func<string> simple)
        {
            Added.Add(tag);
            TextTemplateEngine.s_TemplatesByTag[tag] = new FixedTextTemplate(simple);
        }
    }

    public class FixedTextTemplate : TextTemplate
    {
        private readonly Func<string> gen;

        public FixedTextTemplate(Func<string> gen)
        {
            this.gen = gen;
        }

        public override string Generate(bool capitalized, List<string> parameters)
        {
            return gen();
        }
    }

    public class RelicItem
    {
        public string Name;
        public string Guid;
        public ItemType RelicItemType;
        public LocalizedString DisplayName;
        public LocalizedString Description;
        public LocalizedString FlavorText;
    }

    public class RelicEventSolution
    {
        public LocalizedString SolutionText;
        public BlueprintItem Item;
        public BlueprintUnlockableFlag Flag;
        public LocalizedString ResultText;
        public ConditionsChecker AvailConditions;
    }

    public class RelicEvent
    {
        public string Name;
        public string Guid;
        public LocalizedString DisplayName;
        public LocalizedString Description;
        public EventSolution[] EventSolutions;
    }

    public class RelicProject
    {
        public string Name;
        public string Guid;
        public LocalizedString DisplayName;
        public LocalizedString Description;
        public string RequiredItemGuid;
        public string UnlockableFlagGuid;
        public LocalizedString DefaultResolutionDescription;
        public LocalizedString MechanicalDescription;
        public string NextEventGuid;
        public bool IsAct3;
    }

    public static class WeaponTypes
    {
        public static BlueprintWeaponType Bardiche => ResourcesLibrary.TryGetBlueprint<BlueprintWeaponType>("b1cbf457fd471d148b39ae56667f405a");
        public static BlueprintWeaponType BastardSword => ResourcesLibrary.TryGetBlueprint<BlueprintWeaponType>("d2fe2c5516b56f04da1d5ea51ae3ddfe");
        public static BlueprintWeaponType Battleaxe => ResourcesLibrary.TryGetBlueprint<BlueprintWeaponType>("2bc77aa47f97de348aefcf03ec8fa43b");
        public static BlueprintWeaponType Club => ResourcesLibrary.TryGetBlueprint<BlueprintWeaponType>("26aa0672af2c7d84ba93bec37758c712");
        public static BlueprintWeaponType CompositeLongbow => ResourcesLibrary.TryGetBlueprint<BlueprintWeaponType>("1ac79088a7e5dde46966636a3ac71c35");
        public static BlueprintWeaponType CompositeShortbow => ResourcesLibrary.TryGetBlueprint<BlueprintWeaponType>("011f6f86a0b16df4bbf7f40878c3e80b");
        public static BlueprintWeaponType Dagger => ResourcesLibrary.TryGetBlueprint<BlueprintWeaponType>("07cc1a7fceaee5b42b3e43da960fe76d");
        public static BlueprintWeaponType Dart => ResourcesLibrary.TryGetBlueprint<BlueprintWeaponType>("f415ae950523a7843a74d7780dd551af");
        public static BlueprintWeaponType DoubleAxe => ResourcesLibrary.TryGetBlueprint<BlueprintWeaponType>("87d76c7534506a546a065731ee6a38cd");
        public static BlueprintWeaponType DoubleSword => ResourcesLibrary.TryGetBlueprint<BlueprintWeaponType>("e95efb85a0912a7489be69d5f03a5308");
        public static BlueprintWeaponType DuelingSword => ResourcesLibrary.TryGetBlueprint<BlueprintWeaponType>("a6f7e3dc443ff114ba68b4648fd33e9f");
        public static BlueprintWeaponType DwarvenUrgrosh => ResourcesLibrary.TryGetBlueprint<BlueprintWeaponType>("0ec97c08fdf87e44f8f16ba87b511743");
        public static BlueprintWeaponType DwarvenUrgroshSpearHead => ResourcesLibrary.TryGetBlueprint<BlueprintWeaponType>("c4e95e3c489721e4382fd8514a522f9d");
        public static BlueprintWeaponType DwarvenWaraxe => ResourcesLibrary.TryGetBlueprint<BlueprintWeaponType>("a6925f5f897801449a648d865637e5a0");
        public static BlueprintWeaponType EarthBreaker => ResourcesLibrary.TryGetBlueprint<BlueprintWeaponType>("54e960775daa4714fa52e0954d8cf862");
        public static BlueprintWeaponType ElvenCurvedBlade => ResourcesLibrary.TryGetBlueprint<BlueprintWeaponType>("b5e6838ad2a62b146b49619bcf9f42aa");
        public static BlueprintWeaponType Estoc => ResourcesLibrary.TryGetBlueprint<BlueprintWeaponType>("d516765b3c2904e4a939749526a52a9a");
        public static BlueprintWeaponType Falcata => ResourcesLibrary.TryGetBlueprint<BlueprintWeaponType>("1af5621e2ae551e42bd1dd6744d98639");
        public static BlueprintWeaponType Falchion => ResourcesLibrary.TryGetBlueprint<BlueprintWeaponType>("6ddc9acbbb6e40746a6a1671df1f7b47");
        public static BlueprintWeaponType Fauchard => ResourcesLibrary.TryGetBlueprint<BlueprintWeaponType>("7a40899c4defec94bb9c291bde74f1a8");
        public static BlueprintWeaponType Flail => ResourcesLibrary.TryGetBlueprint<BlueprintWeaponType>("bf1e53f7442ed0c43bf52d3abe55e16a");
        public static BlueprintWeaponType Glaive => ResourcesLibrary.TryGetBlueprint<BlueprintWeaponType>("7a14a1b224cd173449cb7ffc77d5f65c");
        public static BlueprintWeaponType GnomeHookedHammerHead => ResourcesLibrary.TryGetBlueprint<BlueprintWeaponType>("91645b645cf121a479c1fabc5678dcb3");
        public static BlueprintWeaponType GnomeHookedHammerHook => ResourcesLibrary.TryGetBlueprint<BlueprintWeaponType>("018ad48ffd3460d47900491656d2ff26");
        public static BlueprintWeaponType Greataxe => ResourcesLibrary.TryGetBlueprint<BlueprintWeaponType>("e8059a8eac62cd74f9171d748a5ae428");
        public static BlueprintWeaponType Greatclub => ResourcesLibrary.TryGetBlueprint<BlueprintWeaponType>("1b8c24cd1f9358c48839bb39266468c3");
        public static BlueprintWeaponType Greatsword => ResourcesLibrary.TryGetBlueprint<BlueprintWeaponType>("5f824fbb0766a3543bbd6ae50248688f");
        public static BlueprintWeaponType Handaxe => ResourcesLibrary.TryGetBlueprint<BlueprintWeaponType>("54e7473ff73910c47812fd39c897dc1a");
        public static BlueprintWeaponType HeavyCrossbow => ResourcesLibrary.TryGetBlueprint<BlueprintWeaponType>("36d0551b8a28587438a47fcbbf53c083");
        public static BlueprintWeaponType HeavyFlail => ResourcesLibrary.TryGetBlueprint<BlueprintWeaponType>("8fefb7e0da38b06408f185e29372c703");
        public static BlueprintWeaponType HeavyMace => ResourcesLibrary.TryGetBlueprint<BlueprintWeaponType>("d5a167f0f0208dd439ec7481e8989e21");
        public static BlueprintWeaponType HeavyPick => ResourcesLibrary.TryGetBlueprint<BlueprintWeaponType>("a492410f3d65f744c892faf09daad84a");
        public static BlueprintWeaponType Javelin => ResourcesLibrary.TryGetBlueprint<BlueprintWeaponType>("a70cea34b275522458654beb3c53fe3f");
        public static BlueprintWeaponType Kama => ResourcesLibrary.TryGetBlueprint<BlueprintWeaponType>("f5872eb0deb3a1b48a36549f8d92c19e");
        public static BlueprintWeaponType Kukri => ResourcesLibrary.TryGetBlueprint<BlueprintWeaponType>("006ecd4715809b343b7001e859e3ddb2");
        public static BlueprintWeaponType LightCrossbow => ResourcesLibrary.TryGetBlueprint<BlueprintWeaponType>("d525e7a6d8d5aa648a976ac41194b8d0");
        public static BlueprintWeaponType LightHammer => ResourcesLibrary.TryGetBlueprint<BlueprintWeaponType>("bbf7d0ea57e4c9349b65c82abc6787b4");
        public static BlueprintWeaponType LightMace => ResourcesLibrary.TryGetBlueprint<BlueprintWeaponType>("cf3e703db4b81904e982a66d7b8474ea");
        public static BlueprintWeaponType LightPick => ResourcesLibrary.TryGetBlueprint<BlueprintWeaponType>("da922f4cdb60ef548a2ec0168620568a");
        public static BlueprintWeaponType Longbow => ResourcesLibrary.TryGetBlueprint<BlueprintWeaponType>("7a1211c05ec2c46428f41e3c0db9423f");
        public static BlueprintWeaponType Longspear => ResourcesLibrary.TryGetBlueprint<BlueprintWeaponType>("fa2dd17cbde7d3f4aa918d467c30516e");
        public static BlueprintWeaponType Longsword => ResourcesLibrary.TryGetBlueprint<BlueprintWeaponType>("d56c44bc9eb10204c8b386a02c7eed21");
        public static BlueprintWeaponType Nunchaku => ResourcesLibrary.TryGetBlueprint<BlueprintWeaponType>("4703b4c0922142f4cbe8895c10a47a9f");
        public static BlueprintWeaponType PunchingDagger => ResourcesLibrary.TryGetBlueprint<BlueprintWeaponType>("89dc4dc5f386e2049b5e0a5e209a3407");
        public static BlueprintWeaponType Quarterstaff => ResourcesLibrary.TryGetBlueprint<BlueprintWeaponType>("629736dabac7f9f4a819dc854eaed2d6");
        public static BlueprintWeaponType Rapier => ResourcesLibrary.TryGetBlueprint<BlueprintWeaponType>("2ece38f30500f454b8569136221e55b0");
        public static BlueprintWeaponType Sai => ResourcesLibrary.TryGetBlueprint<BlueprintWeaponType>("0944f411666c7594aa1398a7476ecf7d");
        public static BlueprintWeaponType Scimitar => ResourcesLibrary.TryGetBlueprint<BlueprintWeaponType>("d9fbec4637d71bd4ebc977628de3daf3");
        public static BlueprintWeaponType Scythe => ResourcesLibrary.TryGetBlueprint<BlueprintWeaponType>("4eacfc7e152930a45a1a16217c35011c");
        public static BlueprintWeaponType Shortbow => ResourcesLibrary.TryGetBlueprint<BlueprintWeaponType>("99ce02fb54639b5439d07c99c55b8542");
        public static BlueprintWeaponType Shortspear => ResourcesLibrary.TryGetBlueprint<BlueprintWeaponType>("cf72040b79c99504785976b28d54b2b7");
        public static BlueprintWeaponType Shortsword => ResourcesLibrary.TryGetBlueprint<BlueprintWeaponType>("a7da36e0e7bb60e42b9f23462ce2f4fc");
        public static BlueprintWeaponType Sickle => ResourcesLibrary.TryGetBlueprint<BlueprintWeaponType>("ec2da496c7936e14c9a28ce616a6b4cd");
        public static BlueprintWeaponType SlingStaff => ResourcesLibrary.TryGetBlueprint<BlueprintWeaponType>("25da2dc95ed4a6b419608c678f2a9cc3");
        public static BlueprintWeaponType Spear => ResourcesLibrary.TryGetBlueprint<BlueprintWeaponType>("4b289eccefe6d704093201e52eb6d123");
        public static BlueprintWeaponType SpikedHeavyShield => ResourcesLibrary.TryGetBlueprint<BlueprintWeaponType>("a1b85d048fb5003438f34356df938a9f");
        public static BlueprintWeaponType SpikedLightShield => ResourcesLibrary.TryGetBlueprint<BlueprintWeaponType>("98a0dc03586a6d04791901c41700e516");
        public static BlueprintWeaponType Starknife => ResourcesLibrary.TryGetBlueprint<BlueprintWeaponType>("5a939137fc039084580725b2b0845c3f");
        public static BlueprintWeaponType ThrowingAxe => ResourcesLibrary.TryGetBlueprint<BlueprintWeaponType>("ca131c71f4fefcb48b30b5991520e01d");
        public static BlueprintWeaponType Tongi => ResourcesLibrary.TryGetBlueprint<BlueprintWeaponType>("13fa38737d46c9e4abc7f4d74aaa59c3");
        public static BlueprintWeaponType Torch => ResourcesLibrary.TryGetBlueprint<BlueprintWeaponType>("cf47423b22cbc2641ab5599adc1d1ce7");
        public static BlueprintWeaponType Trident => ResourcesLibrary.TryGetBlueprint<BlueprintWeaponType>("6ff66364e0a2c89469c2e52ebb46365e");
        public static BlueprintWeaponType Warhammer => ResourcesLibrary.TryGetBlueprint<BlueprintWeaponType>("fac41e149f49cba4a8e06ce39b41a6fa");
        public static BlueprintWeaponType WeaponHeavyShield => ResourcesLibrary.TryGetBlueprint<BlueprintWeaponType>("be9b6408e6101cb4997a8996484baf19");
        public static BlueprintWeaponType WeaponLightShield => ResourcesLibrary.TryGetBlueprint<BlueprintWeaponType>("1fd965e522502fe479fdd423cca07684");
    }

    public static class ArmorTypes
    {
        public static BlueprintArmorType Banded => ResourcesLibrary.TryGetBlueprint<BlueprintArmorType>("da1b160cd13f16a429499b96636f6ed9");
        public static BlueprintArmorType Breastplate => ResourcesLibrary.TryGetBlueprint<BlueprintArmorType>("d326c3c61a84c6f40977c84fab41503d");
        public static BlueprintArmorType Chainmail => ResourcesLibrary.TryGetBlueprint<BlueprintArmorType>("cd4a47c5bacbff3498e960eec3a83485");
        public static BlueprintArmorType ChainmailBarding => ResourcesLibrary.TryGetBlueprint<BlueprintArmorType>("bdcce0ac4c930b84a849f935a4bdd93e");
        public static BlueprintArmorType Chainshirt => ResourcesLibrary.TryGetBlueprint<BlueprintArmorType>("7467b0ab8641d8f43af7fc46f2108a1a");
        public static BlueprintArmorType ChainshirtBarding => ResourcesLibrary.TryGetBlueprint<BlueprintArmorType>("7b9bb0bc92bb7414d8ba44bcdd55ece6");
        public static BlueprintArmorType Cloth => ResourcesLibrary.TryGetBlueprint<BlueprintArmorType>("56d86616f18533e4d87f9a56a85f015d");
        public static BlueprintArmorType FullBarding => ResourcesLibrary.TryGetBlueprint<BlueprintArmorType>("55a6ac3d3ed31fc4d8a7f26f509999a8");
        public static BlueprintArmorType Fullplate => ResourcesLibrary.TryGetBlueprint<BlueprintArmorType>("afd9065539b3ac5499eddca923654c4b");
        public static BlueprintArmorType Halfplate => ResourcesLibrary.TryGetBlueprint<BlueprintArmorType>("a59bf41f441456a4e8b04b09cb889af8");
        public static BlueprintArmorType Haramaki => ResourcesLibrary.TryGetBlueprint<BlueprintArmorType>("9511d62bcfc57c245bf64350a5933470");
        public static BlueprintArmorType Hide => ResourcesLibrary.TryGetBlueprint<BlueprintArmorType>("7a01292cef39bf2408f7fae7a9f47594");
        public static BlueprintArmorType HideBarding => ResourcesLibrary.TryGetBlueprint<BlueprintArmorType>("bcb6205a498355c47a972558565c7783");
        public static BlueprintArmorType Leather => ResourcesLibrary.TryGetBlueprint<BlueprintArmorType>("c850ba40ed3a61b489b438a5ffa71de9");
        public static BlueprintArmorType LeatherBarding => ResourcesLibrary.TryGetBlueprint<BlueprintArmorType>("503056b4c5a01f44db0ed10ce4c9ef01");
        public static BlueprintArmorType PaddedLeather => ResourcesLibrary.TryGetBlueprint<BlueprintArmorType>("b7e26eed57d524b478c0c90df0f0b8f1");
        public static BlueprintArmorType ScaleBarding => ResourcesLibrary.TryGetBlueprint<BlueprintArmorType>("abefac7d8f98aeb40a167e0f5978d9c7");
        public static BlueprintArmorType Scalemail => ResourcesLibrary.TryGetBlueprint<BlueprintArmorType>("f95c21c70a5677346b75e447c7225ba6");
        public static BlueprintArmorType StuddedLeather => ResourcesLibrary.TryGetBlueprint<BlueprintArmorType>("aae2cb63162d6334b9a9150398124d46");
    }
}
