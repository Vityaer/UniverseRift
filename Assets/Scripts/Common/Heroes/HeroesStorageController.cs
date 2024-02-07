using Db.CommonDictionaries;
using Hero;
using Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using UIController.Inventory;
using UniRx;
using VContainer;
using VContainer.Unity;

namespace Common.Heroes
{
    public class HeroesStorageController : IInitializable
    {
        [Inject] private readonly CommonDictionaries _commonDictionaries;
        [Inject] private readonly GameController _gameController;
        [Inject] private readonly CommonGameData _commonGameData;

        private List<GameHero> _listHeroes = new List<GameHero>();
        private ReactiveCommand<int> _onChangeMaxCountHeroes = new ReactiveCommand<int>();
        private ReactiveCommand<int> _onChangeCountHeroes = new ReactiveCommand<int>();
        private ReactiveCommand<GameHero> _onChangeHeroes = new ReactiveCommand<GameHero>();
        private CompositeDisposable _disposables = new CompositeDisposable();

        public int GetCurrentCountHeroes => _listHeroes.Count;
        public List<GameHero> ListHeroes => _listHeroes;
        public IObservable<int> OnChangeMaxCountHeroes => _onChangeMaxCountHeroes;
        public IObservable<int> OnChangeCountHeroes => _onChangeCountHeroes;
        public IObservable<GameHero> OnChangeHeroes => _onChangeHeroes;

        public void Initialize()
        {
            _gameController.OnLoadedGameData.Subscribe(_ => OnLoadGame()).AddTo(_disposables);
            _gameController.OnGameSave.Subscribe(_ => OnSave()).AddTo(_disposables);
        }

        private void OnLoadGame()
        {
            var heroesStorage = _commonGameData.HeroesStorage;

            _listHeroes = new List<GameHero>(heroesStorage.ListHeroes.Count);
            foreach (var data in heroesStorage.ListHeroes)
            {
                var heroTemplate = _commonDictionaries.Heroes[data.HeroId];
                var hero = new GameHero(heroTemplate, data);
                foreach (var itemData in data.Costume.Items.Values)
                {
                    hero.Costume.TakeOn(new GameItem(_commonDictionaries.Items[itemData]));
                }

                _listHeroes.Add(hero);
            }
        }

        private void OnSave()
        {
            var data = _listHeroes.Select(hero => hero.GetSaveData()).ToList();
            _commonGameData.HeroesStorage.ListHeroes = data;
        }

        public void AddMaxCountHeroes(int amount)
        {
            //player.PlayerGame.maxCountHeroes += amount;
            //_onChangeMaxCountHeroes.Execute(player.PlayerGame.maxCountHeroes);
        }

        public void AddHero(GameHero newGameHero)
        {
            _listHeroes.Add(newGameHero);
            OnChangeListHeroes(newGameHero);
            OnSave();
        }

        public void RemoveHero(GameHero hero)
        {
            _listHeroes.Remove(hero);
            OnChangeListHeroes(hero);
            OnSave();
        }

        public void RemoveHeroes(List<GameHero> heroes)
        {
            foreach (var hero in heroes)
            {
                _listHeroes.Remove(hero);
                OnChangeListHeroes(hero);
            }
            OnSave();
        }

        private void OnChangeListHeroes(GameHero hero)
        {
            _onChangeHeroes.Execute(hero);
            _onChangeCountHeroes.Execute(_listHeroes.Count);
        }
    }
}
