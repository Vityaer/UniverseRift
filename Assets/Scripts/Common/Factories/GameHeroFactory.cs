using Common.Db.CommonDictionaries;
using Hero;
using Models;
using VContainer;

namespace Common.Factories
{
    public class GameHeroFactory
    {
        [Inject] private readonly CommonDictionaries _commonDictionaries;

        public GameHero Create(string Id)
        {
            var model = _commonDictionaries.Heroes[Id];
            var data = new HeroData { Rating = model.General.Rating };
            return new GameHero(model, data);
        }

        public GameHero Create(HeroData heroData)
        {
            var model = _commonDictionaries.Heroes[heroData.HeroId];
            return new GameHero(model, heroData);
        }
    }
}
