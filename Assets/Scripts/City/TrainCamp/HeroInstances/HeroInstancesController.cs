using Db.CommonDictionaries;
using Fight.HeroControllers.Generals;
using Hero;
using System;
using System.Collections.Generic;
using UnityEngine;
using VContainer;

namespace City.TrainCamp.HeroInstances
{
    public class HeroInstancesController : MonoBehaviour
    {
        [Inject] private readonly CommonDictionaries _commonDictionaries;

        private HeroController _currentHero;

        [SerializeField] private Transform _root;
        [SerializeField] private Transform _createPoint;
        [SerializeField] private Camera _camera;
        [SerializeField] private GameObject _light;

        private bool _work;
        private Dictionary<string, HeroController> _heroes = new();

        public void ShowHero(GameHero hero)
        {
            if (!_work)
            {
                _camera.enabled = true;
                _work = true;
                _light.SetActive(true);
            }

            if (_currentHero != null)
                Destroy(_currentHero.gameObject);

            CreateHero(hero);
            _currentHero.enabled = false;
        }

        public HeroController GetHero(string heroId)
        {
            if (_heroes.TryGetValue(heroId, out var result))
            {
                return result;
            }
            else
            {
                var path = $"{Constants.ResourcesPath.HEROES_PATH}{heroId}";
                var newHero = Resources.Load<HeroController>(path);
                _heroes.Add(heroId, newHero);
                return newHero;
            }
        }

        private void CreateHero(GameHero gameHero)
        {
            var stage = (gameHero.HeroData.Rating / 5);
            var heroPrefab = GetHero(gameHero.Model.General.HeroId);
            heroPrefab.SetStage(stage);
            _currentHero = UnityEngine.Object.Instantiate(heroPrefab, _createPoint.position, Quaternion.identity, _createPoint);
        }

        public void ShowAnimation()
        {
            _currentHero.HeroAnimator.Play("Attack");
        }

        public void Hide()
        {
            if (_work)
            {
                _camera.enabled = false;
                _work = false;
                _light.SetActive(false);

                if (_currentHero != null)
                    Destroy(_currentHero.gameObject);
            }
        }

        public void OpenLight()
        {
            _light.SetActive(true);
        }

        public void HideLight()
        {
            _light.SetActive(false);
        }
    }
}
