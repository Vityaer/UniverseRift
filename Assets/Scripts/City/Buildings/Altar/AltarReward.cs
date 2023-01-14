namespace Altar
{
	public struct AltarReward
	{
		public int _minRequireRating;
		public int _factorRating;
		public int _factorRare;
		public Resource _reward;

		public Resource CalculateReward(InfoHero hero)
		{
			Resource result = null;

			if(hero.generalInfo.ratingHero >= _minRequireRating)
			{
				float factor = hero.generalInfo.ratingHero * _factorRating + ((int) hero.generalInfo.rare) * _factorRare;
				result = _reward * factor;
			}

			return result;
		}
	}
}