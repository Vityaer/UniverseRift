using Models.Heroes;
using Common.Resourses;

namespace Altar
{
    public struct AltarReward
	{
		public int MinRequireRating;
		public int FactorRating;
		public int FactorRare;
		public GameResource Reward;

		public GameResource CalculateReward(HeroModel hero)
		{
			GameResource result = null;

			if(hero.General.Rating >= MinRequireRating)
			{
				//float factor = hero.generalInfo.RatingHero * _factorRating + ((int) hero.generalInfo.Rare) * _factorRare;
				float factor = (hero.General.Rating * FactorRating + 1) * FactorRare;
				result = Reward * factor;
			}

			return result;
		}
	}
}