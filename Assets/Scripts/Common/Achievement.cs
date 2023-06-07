using Models;
using System.Collections.Generic;
using UIController.Reward;
using UnityEngine;

namespace Common
{
    [System.Serializable]
    public class Achievement
    {
        [Header("Info")]
        [SerializeField] protected int id;
        public TypeRequirement type = TypeRequirement.GetLevel;
        public string description;
        [SerializeField] protected List<RequirementStage> stages = new List<RequirementStage>();
        public ProgressType progressType;
        [SerializeField] private IntRecordsModel intRecords = new IntRecordsModel();
        [SerializeField] private StringRecordsModel stringRecords = new StringRecordsModel();

        [SerializeField] protected BigDigit progress = new BigDigit(0, 0);
        [SerializeField] protected int currentStage = 0;

        private bool isComplete = false;

        public int ID => id;
        public int CountStage => stages.Count;
        public BigDigit Progress => progress;
        public int CurrentStage => currentStage;
        public IntRecordsModel GetIntRecords => intRecords;
        public StringRecordsModel GetStringRecords => stringRecords;
        public bool IsComplete => isComplete;

        public void AddProgress(BigDigit amount)
        {
            if (isComplete == false)
            {
                switch (progressType)
                {
                    case ProgressType.StorageAmount:
                        progress.Add(amount);
                        break;
                    case ProgressType.MaxAmount:
                        if (progress > amount)
                            progress = amount;
                        break;
                    case ProgressType.CurrentAmount:
                        progress = amount;
                        break;
                }

                if (progress.CheckCount(GetRequireFinishCount()))
                {
                    progress = new BigDigit(GetRequireFinishCount().Count, GetRequireFinishCount().E10);
                    isComplete = true;
                }
            }
        }

        public void SetProgress(int newCurrentStage, BigDigit newProgress)
        {
            currentStage = newCurrentStage;
            progress = newProgress;
            if (progress.CheckCount(GetRequireFinishCount()))
            {
                isComplete = true;
            }
        }

        private BigDigit GetRequireFinishCount() { return stages[stages.Count - 1].RequireCount; }
        public bool CheckCount() { return progress.CheckCount(GetRequireCount()); }
        public BigDigit GetRequireCount() { return stages[currentStage].RequireCount; }
        public void GetReward() { GameController.Instance.AddReward(stages[currentStage].reward); currentStage++; }
        public Reward GetRewardInfo()
        {
            return stages[currentStage < CountStage ? currentStage : CountStage - 1].reward.Clone();
        }

        public Achievement(int ID, int currentStage, BigDigit progress)
        {
            id = ID;
            this.currentStage = currentStage;
            this.progress = progress;
        }
        public Achievement Clone()
        {
            return new Achievement(ID, currentStage, progress);
        }
        public void ClearProgress()
        {
            currentStage = 0;
            progress = new BigDigit(0, 0);
        }
    }
    public enum TypeRequirement
    {
        GetLevel = 0,
        DoneChapter = 1,
        DoneMission = 2,
        GetHeroes = 3,
        GetHeroesWithLevel = 4,
        GetHeroesWithRating = 5,
        GetHeroesCount = 6,
        SimpleSpin = 7,
        SpecialSpin = 8,
        SynthesItem = 9,
        SynthesCount = 10,
        BuyItem = 11,
        SpendResource = 12,
        BuyItemCount = 13,
        DestroyHeroCount = 14,
        CountWin = 15,
        CountDefeat = 16,
        CountPointsOnSimpleArena = 17,
        CountPointsOnTournament = 18,
        RaceHireCount = 19,
        SimpleHireCount = 20,
        SpecialHireCount = 21,
        TryCompleteChallangeTower = 22,
        CompleteChallengeTower = 23,
        SendFriendHeart = 24,
        GetHeroesWithRatingAndID = 25,
        CountWinArenaFight = 26,
        CountDoneTasks = 27,
        CountDoneTravel = 28,
        CountTryArenaFight = 29,
        CountHourAutoFightReward = 30
    }
    public enum TypeReward
    {
        Resource,
        Item,
        Hero,
        Splinter
    }
    public enum ProgressType
    {
        StorageAmount,
        MaxAmount,
        CurrentAmount
    }
    [System.Serializable]
    public class RequirementStage
    {
        [Header("Requirement")]
        [SerializeField] private BigDigit requireCount;
        [Header("Reward")]
        public Reward reward;
        public BigDigit RequireCount { get => requireCount; }
    }
}