using Assets.Scripts.City.Buildings.General;
using Assets.Scripts.Fight.WarTable;
using Common;
using Fight.WarTable;
using Models.Fights.Challenge;
using System.Collections.Generic;
using UnityEngine;

namespace City.Buildings.ChallengeBuild
{
    public class ChallengeBuild : Building, IWorkWithWarTable
    {
        [Header("UI")]
        private List<ChallengeUI> listChallengeUI = new List<ChallengeUI>();
        [SerializeField] private bool isFillList = false;

        [Header("Data")]
        [SerializeField] private List<ChallengeModel> challenges = new List<ChallengeModel>();
        [SerializeField] private GameObject prefabChallengeUI;
        [SerializeField] private Transform transformList;

        protected override void OpenPage()
        {
            UnregisterOnOpenCloseWarTable();
            if (isFillList == false) FillListChallenge();
        }

        public void OpenChallenge(ChallengeModel challenge)
        {
            RegisterOnOpenCloseWarTable();
            WarTableController.Instance.OpenMission(challenge.Mission, GameController.Instance.GetListHeroes);
        }

        public void Change(bool isOpen)
        {
            if (!isOpen) { UpdateAllUI(); Open(); } else { Close(); }
        }

        private void FillListChallenge()
        {
            isFillList = true;
            for (int i = 0; i < challenges.Count; i++)
            {
                ChallengeUI curChallengeUI = Instantiate(prefabChallengeUI, transformList).GetComponent<ChallengeUI>();
                curChallengeUI.SetData(challenges[i], this);
                listChallengeUI.Add(curChallengeUI);
            }
        }
        private void UpdateAllUI()
        {
            for (int i = 0; i < listChallengeUI.Count; i++)
                listChallengeUI[i].UpdateControllersUI();
        }
        public void RegisterOnOpenCloseWarTable() { WarTableController.Instance.RegisterOnOpenCloseMission(Change); }
        public void UnregisterOnOpenCloseWarTable() { WarTableController.Instance.UnregisterOnOpenCloseMission(Change); }
    }
}