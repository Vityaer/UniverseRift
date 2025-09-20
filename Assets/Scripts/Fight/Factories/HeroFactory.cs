using Common.Factories;
using Fight.Common.Grid;
using Fight.Common.HeroControllers.Generals;
using Fight.Common.Misc;
using Hero;
using UnityEngine;
using VContainer;

namespace Fight.Common.Factories
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

            if (heroPrefab == null)
            {
                return null;
            }
            
            var hero = Object.Instantiate(heroPrefab, hexagonCell.Position, Quaternion.identity, parent);
            ObjectResolver.Inject(hero);
            hero.SetStage(stage);
            return hero;
        }


    }


}
