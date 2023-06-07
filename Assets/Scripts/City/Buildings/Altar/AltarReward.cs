using Assets.Scripts.Models.Heroes;
using Common.Resourses;

namespace Altar
{
    public struct AltarReward
	{
		public int MinRequireRating;
		public int FactorRating;
		public int FactorRare;
		public Resource Reward;

		public Resource CalculateReward(HeroModel hero)
		{
			Resource result = null;

			if(hero.General.RatingHero >= MinRequireRating)
			{
				//float factor = hero.generalInfo.RatingHero * _factorRating + ((int) hero.generalInfo.Rare) * _factorRare;
				float factor = (hero.General.RatingHero * FactorRating + 1) * FactorRare;
				result = Reward * factor;
			}

			return result;
		}
	}
}