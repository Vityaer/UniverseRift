using City.Buildings.Abstractions;
using Common;
using Fight.Common.WarTable;
using Models.Fights.Challenge;
using System.Collections.Generic;
using UnityEngine;

namespace City.Buildings.ChallengeBuild
{
    public class ChallengeController : BuildingWithFight<ChallengeView>
    {
        [Header("UI")]
        private List<ChallengeUI> listChallengeUI = new List<ChallengeUI>();
        [SerializeField] private bool isFillList = false;

        [Header("Data")]
        [SerializeField] private List<ChallengeModel> challenges = new List<ChallengeModel>();
        [SerializeField] private GameObject prefabChallengeUI;
        [SerializeField] private Transform transformList;
    }
}