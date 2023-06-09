using Fight.HeroControllers.Generals;
using UnityEngine;

namespace Models.Heroes
{
    [System.Serializable]
    public class GeneralInfoHero
    {
        public string Name;
        public string HeroId;
        public string Race;
        public string ClassHero;
        public int RatingHero = 1;
        public string Rarity;
        public string ViewId;
        private Sprite Avatar;
        public Sprite ImageHero
        {
            get
            {
                if (Avatar == null)
                    Avatar = Prefab?.GetComponent<HeroController>().GetSprite;
                return Avatar;
            }

            set
            {
                Avatar = value;
            }
        }
        public int Level;
        public bool IsAlive = true;

        private GameObject prefab;
        public GameObject Prefab
        {
            get
            {
                if (prefab == null)
                    prefab = Resources.Load<GameObject>($"Heroes/{ViewId}");
                return prefab;
            }

            set => prefab = value;
        }
        [HideInInspector] public int IDCreate = 0;

        public object Clone()
        {
            return new GeneralInfoHero
            {
                Name = this.Name,
                Race = this.Race,
                ClassHero = this.ClassHero,
                RatingHero = this.RatingHero,
                Rarity = this.Rarity,
                ViewId = this.ViewId,
                Level = this.Level,
                IsAlive = this.IsAlive,
                IDCreate = this.IDCreate
            };
        }

    }
}
