using Common.Factories;
using Fight.Grid;
using Fight.HeroControllers.Generals;
using Fight.Misc;
using Hero;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Fight.Factories
{
    public class HeroFactory : BaseFactory<HeroController>, IStartable
    {
        private HeroController _heroTemplate;

        public HeroFactory(IObjectResolver objectResolver) : base(objectResolver)
        {
        }

        public void Start()
        {
            _heroTemplate = Resources.Load<HeroController>(Constants.ResourcesPath.HERO_TEMPLATE_PATH);
        }

        public HeroController Create(GameHero gameHero, HexagonCell hexagonCell, Side side, Transform parent)
        {
            var hero = Object.Instantiate(gameHero.Prefab, hexagonCell.Position, Quaternion.identity, parent);
            ObjectResolver.Inject(hero);
            return hero;
        }


    }


}
