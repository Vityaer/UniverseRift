using City.Achievements;
using Models.Common.BigDigits;
using System;
using TMPro;
using UIController;
using UIController.ItemVisual;
using UiExtensions.Misc;
using UniRx;
using UnityEngine.UI;
using VContainer;

namespace City.Buildings.Requirement
{
    public class AchievmentView : ScrollableUiView<GameAchievment>, IDisposable
    {
        [Inject] private IObjectResolver _objectResolver;
        
        public Button GetRewardButton;
        public ItemSliderController SliderAmount;
        public RewardUIController RewardController;
        public TextMeshProUGUI Description;

        public ReactiveCommand ObserverOnChange = new ReactiveCommand();
        public ReactiveCommand ObserverComplete = new ReactiveCommand();

        public bool IsEmpty { get => Data == null; }
        public bool IsComplete { get => !IsEmpty & Data.IsComplete; }

        private void Start()
        {
            //GetRewardButton.OnClickAsObservable().Subscribe(_ => GetReward()).AddTo(Disposable);   
        }

        public override void SetData(GameAchievment data, ScrollRect scroll)
        {
            Data = data;
            Scroll = scroll;
            if (!data.IsComplete)
            {
                SubscribeAction();
            }

            UpdateUI();
        }

        public void ChangeProgress(BigDigit amount)
        {
            Data.AddProgress(amount);
            UpdateUI();
        }

        public void GetReward()
        {
            var reward = Data.GetReward();
            Data.NextStage();
            ObserverOnChange.Execute();
            UpdateUI();
        }

        public void UpdateUI()
        {
            RewardController.ShowReward(Data.GetReward());
            if (Data.CurrentStage < Data.CountStage)
            {
                GetRewardButton.interactable = Data.CheckCount();
                SliderAmount.SetAmount(Data.Progress, Data.GetRequireCount());
                GetRewardButton.gameObject.SetActive(true);
            }
            else
            {
                GetRewardButton.gameObject.SetActive(false);
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