using City.Achievements;
using Cysharp.Threading.Tasks;
using Models.Common;
using Models.Common.BigDigits;
using Network.DataServer;
using Network.DataServer.Messages.Achievments;
using System;
using TMPro;
using UIController;
using UIController.ItemVisual;
using UiExtensions.Misc;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace City.Buildings.Requirement
{
    public class AchievmentView : ScrollableUiView<GameAchievment>, IDisposable
    {
        [Inject] private readonly CommonGameData _commonGameData;

        public ItemSliderController SliderAmount;
        public RewardUIController RewardController;
        public TextMeshProUGUI Name;
        public TextMeshProUGUI Description;
        public GameObject DonePanel;

        public ReactiveCommand ObserverOnChange = new ReactiveCommand();
        public ReactiveCommand ObserverComplete = new ReactiveCommand();

        public bool IsEmpty { get => Data == null; }
        public bool IsComplete { get => !IsEmpty & Data.IsComplete; }

        protected override void Start()
        {
            Button.OnClickAsObservable().Subscribe(_ => GetReward().Forget()).AddTo(Disposable);
        }

        public override void SetData(GameAchievment data, ScrollRect scroll)
        {
            Data = data;
            Scroll = scroll;
            Name.text = Data.ModelId;
            UpdateUI();
            Data.OnChangeData.Subscribe(_ => UpdateUI()).AddTo(Disposable);
        }

        public async UniTaskVoid GetReward()
        {
            var message = new AchievmentGetRewardMessage { PlayerId = _commonGameData.PlayerInfoData.Id, AchievmentId = Data.Id };
            var result = await DataServer.PostData(message);

            if (!string.IsNullOrEmpty(result))
            {
                Data.GiveReward();
                Data.NextStage();
                ObserverOnChange.Execute();
                UpdateUI();
            }
        }

        public void UpdateUI()
        {
            var reward = Data.GetReward();
            RewardController.ShowReward(reward);

            if (Data.CurrentStage < Data.CountStage)
            {
                Button.interactable = Data.CheckCount();
                SliderAmount.SetAmount(Data.Progress, Data.GetRequireCount());
                Button.gameObject.SetActive(true);
                DonePanel.SetActive(false);
            }
            else
            {
                DonePanel.SetActive(true);
                Button.gameObject.SetActive(false);
                SliderAmount.Hide();
            }
        }

        private void SubscribeAction()
        {
            //switch (requirement.type)
            //{
            //    case TypeRequirement.SimpleSpin:
            //        WheelFortuneController.Instance.RegisterOnSimpleRotate(ChangeProgress);
            //        break;
            //    case TypeRequirement.SpecialHireCount:
            //        TavernController.Instance.RegisterOnSpecialHire(ChangeProgress);
            //        break;
            //    case TypeRequirement.GetLevel:
            //        GameController.Instance.RegisterOnLevelUP(ChangeProgress);
            //        break;
            //    case TypeRequirement.SynthesItem:
            //        ForgeController.Instance.RegisterOnCraft(ChangeProgress);
            //        break;
            //    case TypeRequirement.DoneChapter:
            //    case TypeRequirement.DoneMission:
            //    case TypeRequirement.SpecialSpin:
            //    case TypeRequirement.BuyItemCount:
            //    case TypeRequirement.DestroyHeroCount:
            //    case TypeRequirement.CountWin:
            //    case TypeRequirement.CountDefeat:
            //    case TypeRequirement.CountPointsOnSimpleArena:
            //    case TypeRequirement.CountPointsOnTournament:
            //        break;
            //    case TypeRequirement.TryCompleteChallangeTower:
            //        ChallengeTower.Instance.ObserverWinFight.Subscribe(ChangeProgress).AddTo(_disposables);
            //        break;
            //    case TypeRequirement.CompleteChallengeTower:
            //        ChallengeTower.Instance.RegisterOnWinFight(ChangeProgress);
            //        break;
            //    case TypeRequirement.GetHeroesWithRating:
            //        LevelUpRatingHero.Instance.RegisterOnRatingUp(ChangeProgress, requirement.GetIntRecords.GetRecord("RATING").value);
            //        break;
            //    case TypeRequirement.GetHeroesWithRatingAndID:
            //        LevelUpRatingHero.Instance.RegisterOnRatingUp(ChangeProgress, requirement.GetIntRecords.GetRecord("RATING").value, requirement.GetStringRecords.GetRecord("ID").value);
            //        break;
            //    case TypeRequirement.CountWinArenaFight:
            //        ArenaController.Instance.RegisterOnWinFight(ChangeProgress);
            //        break;
            //    case TypeRequirement.CountTryArenaFight:
            //        ArenaController.Instance.RegisterOnTryFight(ChangeProgress);
            //        break;
            //    case TypeRequirement.CountDoneTasks:
            //        TaskGiverController.Instance.RegisterOnDoneTask(ChangeProgress, requirement.GetIntRecords.GetRecord("RATING").value);
            //        break;
            //    case TypeRequirement.CountDoneTravel:
            //        VoyageController.Instance.RegisterOnDoneTravel(ChangeProgress);
            //        break;
            //        // case TypeRequirement.GetHeroes:
            //        //     requirementScript.listRequirement[i].requireInt = EditorGUILayout.IntField("Count:", requirementScript.listRequirement[i].requireInt);
            //        //     break;
            //        // case TypeRequirement.GetHeroesWithLevel:
            //        //     requirementScript.listRequirement[i].requireInt = EditorGUILayout.IntField("Count:", requirementScript.listRequirement[i].requireInt);
            //        //     break;
            //        // case TypeRequirement.GetHeroesCount:
            //        //     requirementScript.listRequirement[i].requireInt = EditorGUILayout.IntField("Count:", requirementScript.listRequirement[i].requireInt);
            //        //     break;
            //        // case TypeRequirement.SynthesCount:
            //        //     requirementScript.listRequirement[i].requireInt = EditorGUILayout.IntField("Count:", requirementScript.listRequirement[i].requireInt);
            //        //     break;
            //        // case TypeRequirement.SynthesItem:
            //        //     requirementScript.listRequirement[i].requireInt = EditorGUILayout.IntField("Count:", requirementScript.listRequirement[i].requireInt);
            //        //     break;  
            //        // case TypeRequirement.BuyItem:
            //        //     requirementScript.listRequirement[i].requireObject = (ScriptableObject) EditorGUILayout.ObjectField ("Object:", requirementScript.listRequirement[i].requireObject, typeof (ScriptableObject), false);
            //        //     break;
            //        // case TypeRequirement.SpendResource:
            //        //     requirementScript.requireRes = (Resource) EditorGUILayout.ObjectField ("Resource:", requirementScript.requireRes, typeof(Resource), true);
            //        //  break;  
            //}
        }
    }
}