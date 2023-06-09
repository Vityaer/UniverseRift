using City.TrainCamp;
using Models.Heroes;
using UIController.ItemVisual;
using UnityEngine;

namespace Models.City.TrainCamp
{
    [System.Serializable]
    public class RequirementHeroModel
    {
        public string ID;
        public int rating, count;
        public RequireRaceType requireRace;
        public string race;
        private HeroModel dataHero;

        public HeroModel GetData => dataHero;

        public void UpdateData(HeroModel hero)
        {
            var IDHero = hero.General.ViewId;
            race = hero.General.Race;
            dataHero = new HeroModel();
            if (ID.Equals(string.Empty))
            {
                ID = IDHero;
                dataHero.General = (GeneralInfoHero)hero.General.Clone();
                dataHero.General.Level = 1;
            }
            else
            {
                dataHero.General = new GeneralInfoHero();
                dataHero.General.RatingHero = rating;
                dataHero.General.Race = race;
                SpriteName spriteName = SpriteName.OneStarHero;
                switch (rating)
                {
                    case 1:
                        spriteName = SpriteName.OneStarHero;
                        break;
                    case 2:
                        spriteName = SpriteName.TwoStarHero;
                        break;
                    case 3:
                        spriteName = SpriteName.ThreeStarHero;
                        break;
                    case 4:
                        spriteName = SpriteName.FourStartHero;
                        break;
                    case 5:
                        spriteName = SpriteName.FiveStarHero;
                        break;
                }
                if (SystemSprites.Instance == null) Debug.Log("SystemSprites.Instance null");
                if (dataHero == null) Debug.Log("dataHero null");
                if (dataHero.General == null) Debug.Log("dataHero.generalInfo null");
                dataHero.General.ImageHero = SystemSprites.Instance.GetSprite(spriteName);
            }
        }
    }
}
