using Common;
using Models.Fights.Campaign;
using Models.Heroes;
using System;
using System.Collections.Generic;
using TMPro;
using UIController;
using UIController.Cards;
using UnityEngine;
using UnityEngine.UI;

namespace Fight.WarTable
{
    public class WarTableController : MonoBehaviour
    {
        public static WarTableController Instance { get; private set; }

        public List<WarriorPlace> leftTeam = new List<WarriorPlace>();
        public List<WarriorPlace> rightTeam = new List<WarriorPlace>();
        public Canvas warTableCanvas;
        public ListCardOnWarTable listCardPanel;

        [Header("UI")]
        public Button btnStartFight;
        public TextMeshProUGUI textStrengthLeftTeam, textStrengthRightTeam;

        private MissionModel mission;
        private Action<bool> observerOpenCloseMission;

        void Awake()
        {
            Instance = this;
        }

        void Start()
        {
            warTableCanvas = GetComponent<Canvas>();
            listCardPanel.RegisterOnSelect(SelectCard);
            listCardPanel.RegisterOnUnSelect(UnSelectCard);
            textStrengthLeftTeam.text = string.Empty;
            textStrengthRightTeam.text = string.Empty;
        }

        public void SelectCard(Card card)
        {
            AddHero(card);
        }

        public void UnSelectCard(Card card)
        {
            RemoveHero(card);
        }

        private bool AddHero(Card card)
        {
            bool success = false;
            foreach (WarriorPlace place in leftTeam)
            {
                if (place.IsEmpty)
                {
                    place.SetHero(card, card.Hero);
                    UpdateStrengthTeam(leftTeam, textStrengthLeftTeam);
                    break;
                }
            }
            CheckTeam(leftTeam);
            return success;
        }

        private void RemoveHero(Card card)
        {
            foreach (WarriorPlace place in leftTeam)
            {
                if (place.IsEmpty == false)
                {
                    if (place.Card == card)
                    {
                        place.ClearPlace();
                    }
                }
            }
            CheckTeam(leftTeam);
            UpdateStrengthTeam(leftTeam, textStrengthLeftTeam);
        }

        private void ClearRightTeam()
        {
            for (int i = 0; i < rightTeam.Count; i++)
            {
                rightTeam[i].ClearPlace();
            }
        }

        private void ClearLeftTeam()
        {
            for (int i = 0; i < leftTeam.Count; i++)
            {
                leftTeam[i].ClearPlace();
            }
        }

        private void UpdateStrengthTeam(List<WarriorPlace> team, TextMeshProUGUI textComponent)
        {
            float strengthTeam = 0f;
            for (int i = 0; i < team.Count; i++)
                if (team[i].Hero != null)
                    strengthTeam += team[i].Hero.GetStrength;
            textComponent.text = strengthTeam.ToString();
        }

        private void CheckTeam(List<WarriorPlace> team)
        {
            team = team.FindAll(x => x.Hero != null);
            //int racePeople = team.FindAll(x => x.Hero.General.Race == Race.People).Count;
            //int raceElf = team.FindAll(x => x.Hero.General.Race == Race.Elf).Count;
            //int raceUndead = team.FindAll(x => x.Hero.General.Race == Race.Undead).Count;
            //int raceDaemon = team.FindAll(x => x.Hero.General.Race == Race.Daemon).Count;
            //int raceGod = team.FindAll(x => x.Hero.General.Race == Race.God).Count;
            //int raceDarkGod = team.FindAll(x => x.Hero.General.Race == Race.Elemental).Count;
            //switch (team.Count)
            //{
            //    case 1:
            //        Debug.Log("one people");
            //        break;
            //    case 6:
            //        if (racePeople == 6)
            //        {
            //            Debug.Log("all people");
            //        }
            //        else if (raceElf == 6)
            //        {
            //            Debug.Log("all elf");
            //        }
            //        else if (raceUndead == 6)
            //        {
            //            Debug.Log("all undead");
            //        }
            //        else if (raceDaemon == 6)
            //        {
            //            Debug.Log("all daemon");
            //        }
            //        else if (raceGod == 6)
            //        {
            //            Debug.Log("all god");
            //        }
            //        else if (raceDarkGod == 6)
            //        {
            //            Debug.Log("all dark god");
            //        }
            //        else if ((raceElf == 3) && (racePeople == 3))
            //        {
            //            Debug.Log("3 elf and 3 people");
            //        }
            //        else if ((raceUndead == 3) && (raceDaemon == 3))
            //        {
            //            Debug.Log("3 undead and 3 Daemon");
            //        }
            //        else if ((raceGod == 3) && (raceDarkGod == 3))
            //        {
            //            Debug.Log("3 god and 3 darkgod");
            //        }
            //        else if ((racePeople == 2) && (raceElf == 2) && (raceGod == 2))
            //        {
            //            Debug.Log("this is Good");
            //        }
            //        else if ((raceUndead == 2) && (raceDaemon == 2) && (raceDarkGod == 2))
            //        {
            //            Debug.Log("this is Evil");
            //        }
            //        else if ((racePeople == 1) && (raceElf == 1) && (raceDaemon == 1) && (raceUndead == 1) && (raceGod == 1) && (raceDarkGod == 1))
            //        {
            //            Debug.Log("all race");
            //        }
            //        break;
            //}
            btnStartFight.interactable = leftTeam.FindAll(place => place.Hero != null).Count > 0;
        }
        //API
        public void OpenMission(MissionModel mission, List<HeroModel> listHeroes)
        {
            ClearLeftTeam();
            ClearRightTeam();
            this.mission = mission;
            for (int i = 0; i < mission.ListEnemy.Count; i++)
            {
                rightTeam[i].SetEnemy(mission.ListEnemy[i]);
            }
            UpdateStrengthTeam(rightTeam, textStrengthRightTeam);
            UpdateStrengthTeam(leftTeam, textStrengthLeftTeam);
            CheckTeam(rightTeam);
            FillListHeroes(listHeroes);
            Open();
        }

        public void OpenMission(MissionModel mission, Action<bool> del)
        {
            RegisterOnOpenCloseMission(del);
            OpenMission(mission, GameController.Instance.GetListHeroes);
        }

        public void OpenMission(MissionModel mission, Action<bool> actionOnCloseWarTable, Action<FightResultType> actionOnResultFight)
        {
            RegisterOnOpenCloseMission(actionOnCloseWarTable);
            FightController.Instance.RegisterOnFightResult(actionOnResultFight);
            OpenMission(mission, GameController.Instance.GetListHeroes);
        }

        public void ReturnBack()
        {
            OnOpenMission(false);
            ClearRightTeam();
            ClearLeftTeam();
            Close();
        }

        private void Close()
        {
            listCardPanel.EventClose();
            warTableCanvas.enabled = false;
        }

        public void Open()
        {
            OnOpenMission(true);
            MenuController.Instance.CloseMainPage();
            warTableCanvas.enabled = true;
            listCardPanel.EventOpen();
        }

        private void FillListHeroes(List<HeroModel> listHeroes)
        {
            listCardPanel.Clear();
            listCardPanel.SetList(listHeroes);
        }

        public void StartFight()
        {
            Close();
            FightController.Instance.SetMission(mission, leftTeam, rightTeam);
        }

        public void FinishMission()
        {
            OnOpenMission(isOpen: false);
        }
        public void RegisterOnOpenCloseMission(Action<bool> d)
        {
            observerOpenCloseMission += d;
        }

        public void UnregisterOnOpenCloseMission(Action<bool> d)
        {
            observerOpenCloseMission -= d;
        }

        private void OnOpenMission(bool isOpen)
        {
            if (observerOpenCloseMission != null)
            {
                observerOpenCloseMission(isOpen);
            }
        }


    }
}