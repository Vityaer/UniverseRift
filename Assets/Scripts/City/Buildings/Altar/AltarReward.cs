namespace Altar
{
	public struct AltarReward
	{
		public int _minRequireRating;
		public int _factorRating;
		public int _factorRare;
		public Resource _reward;

		public Resource CalculateReward(HeroModel hero)
		{
			Resource result = null;

			if(hero.General.RatingHero >= _minRequireRating)
			{
				//float factor = hero.generalInfo.RatingHero * _factorRating + ((int) hero.generalInfo.Rare) * _factorRare;
				float factor = (hero.General.RatingHero * _factorRating + 1) * _factorRare;
				result = _reward * factor;
			}

			return result;
		}
	}
}