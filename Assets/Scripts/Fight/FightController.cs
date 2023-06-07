using Assets.Scripts.Fight;
using Assets.Scripts.Fight.AI;
using Assets.Scripts.Fight.Grid;
using Assets.Scripts.Fight.Misc;
using Assets.Scripts.Fight.WarTable;
using Assets.Scripts.Models.Fights.Campaign;
using Fight.AI;
using Fight.Grid;
using Fight.HeroControllers.Generals;
using Fight.Misc;
using Fight.WarTable;
using Models.Fights.Campaign;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Fight
{
    public partial class FightController : MonoBehaviour
    {

        [Header("UI")]
        public Canvas canvasFightUI;
        public TextMeshProUGUI textNumRound;
        public int MaxCountRound = 3;

        [Header("Place heroes")]
        public GridController Grid;
        [SerializeField] private List<Warrior> leftTeam = new List<Warrior>();
        [SerializeField] private List<Warrior> rightTeam = new List<Warrior>();
        [SerializeField] private List<HeroController> listInitiative = new List<HeroController>();
        [SerializeField] private List<HeroController> listHeroesWithAction = new List<HeroController>();

        [Header("Location")]
        public LocationController locationController;

        private bool isFightFinish = false;
        private int currentHero = -1;
        private int round = 1;
        private MissionModel _mission;
        private Action<FightResultType> _delsFightResult;

        public List<Warrior> GetLeftTeam => leftTeam;
        public List<Warrior> GetRightTeam => rightTeam;

        public static FightController Instance { get; private set; }

        void Awake()
        {
            Instance = this;
        }

        //Create Teams
        public void SetMission(MissionModel mission, List<WarriorPlace> leftWarriorPlace, List<WarriorPlace> rightWarriorPlace)
        {
            this._mission = mission;
            locationController.OpenLocation(mission.Location);
            Grid.OpenGrid();
            AIController.Instance.StartAIFight();
            CreateTeams(leftWarriorPlace, rightWarriorPlace);
        }

        private void CreateTeams(List<WarriorPlace> leftWarriorPlace, List<WarriorPlace> rightWarriorPlace)
        {
            canvasFightUI.enabled = true;
            CreateTeam(GridController.Instance.GetLeftTeamPos, leftWarriorPlace, Side.Left);
            CreateTeam(GridController.Instance.GetRightTeamPos, rightWarriorPlace, Side.Right);
            listInitiative.Sort(new HeroInitiativeComparer());
            StartCoroutine(StartFightCountdown());
        }

        IEnumerator StartFightCountdown()
        {
            for (int i = 3; i > 0; i--)
            {
                textNumRound.text = i.ToString();
                yield return new WaitForSeconds(0.75f);
            }
            textNumRound.text = "Fight!";
            yield return new WaitForSeconds(0.5f);
            textNumRound.text = string.Concat("Round 1");
            isFightFinish = false;
            PlayDelegateOnStartFight();
            StartFight();
        }

        private void CreateTeam(List<HexagonCell> teamPos, List<WarriorPlace> team, Side side)
        {
            HeroController heroScript;
            GameObject currentHeroPrefab;
            for (int i = 0; i < team.Count; i++)
            {
                currentHeroPrefab = null;
                heroScript = null;

                if ((side == Side.Left && (team[i].card != null)) || (team[i].Hero != null))
                    currentHeroPrefab = Instantiate(team[i].Hero.General.Prefab, teamPos[i].Position, Quaternion.identity, GridController.Instance.ParentTemplateObjects);

                if (currentHeroPrefab != null)
                {
                    heroScript = currentHeroPrefab.GetComponent<HeroController>();
                    List<Warrior> workTeam = (side == Side.Left) ? leftTeam : rightTeam;
                    workTeam.Add(new Warrior(heroScript));
                    heroScript.SetHero(team[i].Hero, teamPos[i], side);
                    listInitiative.Add(heroScript);
                }
            }
        }

        //Fight loop    	
        public void StartFight()
        {
            NextHero();
        }

        public void AddHeroWithAction(HeroController newHero)
        {
            listHeroesWithAction.Add(newHero);
        }

        public void RemoveHeroWithAction(HeroController removeHero)
        {
            listHeroesWithAction.Remove(removeHero);
            if (listHeroesWithAction.Count == 0) NextHero();
        }

        public void RemoveHeroWithActionAll(HeroController removeHero)
        {
            listHeroesWithAction.RemoveAll(x => x == removeHero);
            if (listHeroesWithAction.Count == 0) NextHero();
        }

        private void NextHero()
        {
            if ((currentHero + 1) < listInitiative.Count)
            {
                currentHero++;
            }
            else
            {
                NewRound();
            }
            if (isFightFinish == false)
            {
                listInitiative[currentHero].DoTurn();
            }
        }

        private void NewRound()
        {
            UpdateListInitiative();
            currentHero = 0;
            round++;
            textNumRound.text = string.Concat("Round ", round.ToString());
            PlayDelegateEndRound();
            if (round == MaxCountRound)
            {
                Win(Side.Right);
            }
        }

        private void UpdateListInitiative()
        {
            listInitiative.Clear();
            for (int i = 0; i < leftTeam.Count; i++)
            {
                if (leftTeam[i].heroController != null)
                {
                    listInitiative.Add(leftTeam[i].heroController);
                }
                else
                {
                    Debug.Log("left team hero null");
                }
            }
            for (int i = 0; i < rightTeam.Count; i++)
            {
                if (rightTeam[i].heroController != null)
                {
                    listInitiative.Add(rightTeam[i].heroController);
                }
                else
                {
                    Debug.Log("Right team hero null");
                }
            }
            listInitiative.Sort(new HeroInitiativeComparer());
        }

        public void WaitTurn()
        {
            HeroController hero = GetCurrentHero();
            listInitiative.Remove(hero);
            currentHero -= 1;
            listInitiative.Add(hero);
            hero.StartWait();
        }

        //Result fight
        private void CheckFinishFight()
        {
            if ((leftTeam.Count == 0) || (rightTeam.Count == 0))
                Win(leftTeam.Count > 0 ? Side.Left : Side.Right);
        }

        void Win(Side side)
        {
            isFightFinish = true;
            PlayDelegateOnFinishFight();
            if (side == Side.Left)
            {
                MessageController.Instance.AddMessage("Ты выиграл!");
            }
            else
            {
                MessageController.Instance.AddMessage("Ты проиграл!");

            }
            StartCoroutine(FinishFightCountdown(side));

        }

        IEnumerator FinishFightCountdown(Side side)
        {
            textNumRound.text = "Конец боя!";

            if (side == Side.Right) CheckSaveResult();
            yield return new WaitForSeconds(2.5f);

            var fightResult = (side == Side.Left) ? FightResultType.Win : FightResultType.Defeat;
            _mission.OnFinishFight(fightResult);
            OnFightResult(result: fightResult);
            textNumRound.text = string.Empty;
            CloseFigthUI();
            ClearAll();
        }

        private void CloseFigthUI()
        {
            canvasFightUI.enabled = false;
        }

        void ClearAll()
        {
            DeleteTeam(rightTeam);
            DeleteTeam(leftTeam);
            listInitiative.Clear();
            listHeroesWithAction.Clear();
            currentHero = -1;
            round = 1;
        }

        void DeleteTeam(List<Warrior> team)
        {
            for (int i = 0; i < team.Count; i++)
            {
                team[i].heroController.DeleteHero();
            }
            team.Clear();
        }

        void CheckSaveResult()
        {
            //if ((_mission is BossMission))
            //    ((BossMission)_mission).SaveResult();
        }

        //API
        public void MessageAboutDamageForAttacker(float damage)
        {
            listInitiative[currentHero].MessageDamageAfterStrike(damage);
        }
        public void DeleteHero(HeroController heroForDelete)
        {
            RemoveHeroWithActionAll(heroForDelete);
            Warrior warrior = leftTeam.Find(x => x.heroController == heroForDelete);
            if (warrior != null)
            {
                leftTeam.Remove(warrior);
            }
            else
            {
                warrior = rightTeam.Find(x => x.heroController == heroForDelete);
                rightTeam.Remove(warrior);
            }
            listInitiative.Remove(heroForDelete);
            CheckFinishFight();
        }

        public HeroController GetCurrentHero() { return listInitiative[currentHero]; }

        //Listeners
        private Action delsOnEndRound, delsOnStartFight, delsOnFinishFight;
        public void RegisterOnStartFight(Action d) { delsOnStartFight += d; }
        public void UnregisterOnStartFight(Action d) { delsOnStartFight -= d; }
        private void PlayDelegateOnStartFight() { if (delsOnStartFight != null) delsOnStartFight(); }

        public void RegisterOnEndRound(Action d) { delsOnEndRound += d; }
        public void UnregisterOnEndRound(Action d) { delsOnEndRound -= d; }
        private void PlayDelegateEndRound() { if (delsOnEndRound != null) delsOnEndRound(); }

        public void RegisterOnFinishFight(Action d) { delsOnFinishFight += d; }
        public void UnregisterOnFinishFight(Action d) { delsOnFinishFight -= d; }
        private void PlayDelegateOnFinishFight() { if (delsOnFinishFight != null) delsOnFinishFight(); }

        public void RegisterOnFightResult(Action<FightResultType> d) { Debug.Log("register on finish fight result"); _delsFightResult += d; }
        public void UnregisterOnFightResult(Action<FightResultType> d) { _delsFightResult -= d; }

        public void OnFightResult(FightResultType result)
        {
            if (_delsFightResult != null)
            {
                _delsFightResult(result);
                _delsFightResult = null;
            }
        }
    }
}

