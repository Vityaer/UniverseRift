using City.Buildings.General;
using Common;
using Common.Resourses;
using Cysharp.Threading.Tasks;
using IdleGame.AdvancedObservers;
using MainScripts;
using Misc.Json;
using Misc.Json.Impl;
using Models;
using Models.Heroes;
using Network.DataServer;
using Network.DataServer.Messages;
using Network.DataServer.Models;
using System;
using System.Collections.Generic;
using UIController.Buttons;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace City.Buildings.Tavern
{
    public class TavernController : Building
    {
        [Header("All rating heroes")]
        [SerializeField] private List<HeroModel> _listHeroes = new List<HeroModel>();
        [SerializeField] private Button _specialHire;
        [SerializeField] private Button _simpleHire;
        [SerializeField] private Button _friendHire;
        [SerializeField] private ButtonWithObserverResource _btnCostOneHire;
        [SerializeField] private ButtonWithObserverResource _btnCostManyHire;

        private Resource _simpleHireCost = new Resource(TypeResource.SimpleHireCard, 1, 0);
        private Resource _specialHireCost = new Resource(TypeResource.SpecialHireCard, 1, 0);
        private Resource _friendHireCost = new Resource(TypeResource.FriendHeart, 10, 0);

        private ObserverActionWithHero observersHireRace = new ObserverActionWithHero();
        private IJsonConverter _jsonConverter;
        public List<HeroModel> GetListHeroes => _listHeroes;
        public static TavernController Instance { get; private set; }

        void Awake()
        {
            Instance = this;
            _jsonConverter = new JsonConverter();
        }

        protected override void OnStart()
        {
            CheckLoadedHeroes();
            SelectHire<SpecialHire>(_simpleHireCost, OnSpecialHire);

            _simpleHire.OnClickAsObservable().Subscribe(_ => SelectHire<SimpleHire>(_simpleHireCost, OnSimpleHire));
            _specialHire.OnClickAsObservable().Subscribe(_ => SelectHire<SpecialHire>(_specialHireCost, OnSpecialHire));
            _friendHire.OnClickAsObservable().Subscribe(_ => SelectHire<FriendHire>(_friendHireCost, OnFriendHire));
        }
        
        public void SelectHire<T>(Resource costOneHire, Action<int> onResultHire) where T : AbstractHireMessage, new()
        {
            _btnCostOneHire.ChangeCost(costOneHire, _ => HireHero<T>(1, onResultHire).Forget());
            _btnCostManyHire.ChangeCost(costOneHire * 10f, _ => HireHero<T>(10, onResultHire).Forget());
        }

        private async UniTaskVoid HireHero<T>(int count, Action<int> onResultHire) where T : AbstractHireMessage, new()
        {
            var message = new T { PlayerId = GameController.GetPlayerInfo.PlayerId, Count = count };
            var result = await DataServer.PostData(message);
            var newHeroes = _jsonConverter.FromJson<DataHero[]>(result);

            for (int i = 0; i < newHeroes.Length; i++)
            {
                var heroSave = new HeroData(newHeroes[i]);
                var hero = new HeroModel(heroSave);
                OnHireHeroes(hero);
                if (hero != null)
                {
                    hero.General.Name = $"{hero.General.HeroId} #{UnityEngine.Random.Range(0, 1000)}";
                    AddNewHero(hero);
                }
            }

            onResultHire(count);
        }

        public void AddNewHero(HeroModel hero)
        {
            MessageController.Instance.AddMessage($"Новый герой! Это - {hero.General.Name}");
            GameController.Instance.AddHero(hero);
        }


        private void CheckLoadedHeroes()
        {
            if (_listHeroes.Count == 0)
            {
                //listHeroes = new List<InfoHero>(Resources.LoadAll("ScriptableObjects/HeroesData", typeof(InfoHero)) as InfoHero[]);
            }
        }
        //API

        public HeroModel GetInfoHero(string ID)
        {
            HeroModel hero = (HeroModel)_listHeroes.Find(x => x.General.HeroId == ID)?.Clone();
            if (hero == null)
                Debug.Log(string.Concat("not exist hero with ID= ", ID.ToString()));
            return hero;
        }

        //Observers
        private Action<BigDigit> observerSimpleHire, observerSpecialHire, observerFriendHire;
        public void RegisterOnSimpleHire(Action<BigDigit> d) { observerSimpleHire += d; }
        public void RegisterOnSpecialHire(Action<BigDigit> d) { observerSpecialHire += d; }
        public void RegisterOnFriendHire(Action<BigDigit> d) { observerFriendHire += d; }
        private void OnSimpleHire(int amount)
        {
            if (observerSimpleHire != null)
                observerSimpleHire(new BigDigit(amount));
        }

        private void OnSpecialHire(int amount)
        {
            if (observerSpecialHire != null)
                observerSpecialHire(new BigDigit(amount));
        }

        private void OnFriendHire(int amount)
        {
            if (observerFriendHire != null)
                observerFriendHire(new BigDigit(amount));
        }

        public void RegisterOnHireHeroes(Action<BigDigit> d, string ID = "")
        {
            observersHireRace.Add(d, ID, 1);
        }

        private void OnHireHeroes(HeroModel hero)
        {
            //and Rarity
            observersHireRace.OnAction(string.Empty, 1);
            observersHireRace.OnAction(hero.General.ViewId, 1);
        }

    }
}