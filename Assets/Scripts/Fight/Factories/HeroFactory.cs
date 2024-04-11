using Common.Factories;
using Fight.Grid;
using Fight.HeroControllers.Generals;
using Fight.Misc;
using Hero;
using UnityEngine;
using VContainer;

namespace Fight.Factories
{
    public class HeroFactory : BaseFactory<HeroController>
    {
        public HeroFactory(IObjectResolver objectResolver) : base(objectResolver)
        {
        }

        public HeroController Create(GameHero gameHero, HexagonCell hexagonCell, Side side, Transform parent)
        {
            var stage = (gameHero.HeroData.Rating / 5);
            var path = $"{Constants.ResourcesPath.HEROES_PATH}{gameHero.Model.General.HeroId}";
            var heroPrefab = Resources.Load<HeroController>(path);
            var hero = Object.Instantiate(heroPrefab, hexagonCell.Position, Quaternion.identity, parent);
            hero.SetStage(stage);
            ObjectResolver.Inject(hero);
            return hero;
        }


    }


}
