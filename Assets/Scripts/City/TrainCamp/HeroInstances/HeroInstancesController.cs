using Db.CommonDictionaries;
using Fight.HeroControllers.Generals;
using Hero;
using UnityEngine;
using VContainer;

namespace City.TrainCamp.HeroInstances
{
    public class HeroInstancesController : MonoBehaviour
    {
        [Inject] private readonly CommonDictionaries _commonDictionaries;

        private HeroController _currentHero;

        [SerializeField] private Transform _root;
        [SerializeField] private Camera _camera;
        [SerializeField] private GameObject _light;

        private bool _work;

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

        private void CreateHero(GameHero gameHero)
        {
            var stage = (gameHero.HeroData.Rating / 5);
            var path = $"{Constants.ResourcesPath.HEROES_PATH}{gameHero.Model.General.HeroId}";
            var heroPrefab = Resources.Load<HeroController>(path);
            heroPrefab.SetStage(stage);
            _currentHero = UnityEngine.Object.Instantiate(heroPrefab, _root.position, Quaternion.identity, _root);
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
    }
}
