using City.Buildings.Tavern;
using Models.City.TrainCamp;
using Models.Heroes.Characteristics;
using Models.Heroes.Evolutions;
using Models.Heroes.Skills;
using System.Collections.Generic;
using UIController.Localization.Languages;
using UnityEngine;

namespace Models.Heroes
{
    [System.Serializable]
    public class HeroModel : BaseModel, ICloneable
    {
        public GeneralInfoHero General;
        public GameObject PrefabArrow;
        public Characteristics Characts;
        public ResistanceModel Resistances;
        public IncreaseCharacteristicsModel IncCharacts;
        public CostumeHeroController CostumeHero;
        public List<Skill> skills = new List<Skill>();
        public BreakthroughHero Evolutions;
        public HeroLocalization localization = null;

        public Sprite GetMainImage => General.ImageHero;
        public float GetStrength => Mathf.Round(GetCharacteristic(TypeCharacteristic.Damage) + GetCharacteristic(TypeCharacteristic.HP) / 3f);


        public HeroModel()
        {
        }

        public HeroModel(HeroData heroSave)
        {
            var hero = Tavern.Instance.GetListHeroes.Find(x => x.General.HeroId == heroSave.HeroId);
            CopyData(hero);
            General.HeroId = heroSave.HeroId;
            General.ViewId = heroSave.HeroId;
            General.Name = heroSave.Name;
            General.RatingHero = heroSave.Rating;
            LevelUP(heroSave.Level - 1);
            CostumeHero = new CostumeHeroController();
            CostumeHero.SetData(heroSave.Costume.ItemIds);
            Preparation();
        }

        private void CopyData(HeroModel Data)
        {
            General = (GeneralInfoHero)Data.General.Clone();
            IncCharacts = (IncreaseCharacteristicsModel)Data.IncCharacts.Clone();
            Characts = Data.Characts.Clone();
            Resistances = (ResistanceModel)Data.Resistances.Clone();
            General.Prefab = Resources.Load<GameObject>(string.Concat("Heroes/", General.ViewId.ToString()));
            PrefabArrow = Data.PrefabArrow;
            skills = Data.skills;
            Evolutions = Data.Evolutions;
            CostumeHero = new CostumeHeroController();

        }

        public void Preparation()
        {
            GetSkills();
            PrepareLocalization();
        }

        //API
        public void LevelUP(int count = 1)
        {
            for (int i = 0; i < count; i++)
            {
                if (General.Level < Evolutions.LimitLevel)
                {
                    General.Level += 1;
                    Growth.GrowHero(Characts, Resistances, IncCharacts);
                }
            }
        }

        public float GetCharacteristic(TypeCharacteristic typeBonus)
        {
            float result = 0;
            switch (typeBonus)
            {
                case TypeCharacteristic.HP:
                    result += Characts.HP;
                    break;
                case TypeCharacteristic.Damage:
                    result += Characts.Damage;
                    break;
                case TypeCharacteristic.Initiative:
                    result += Characts.Initiative;
                    break;
                case TypeCharacteristic.Defense:
                    result += Characts.baseCharacteristic.Defense;
                    break;
            }
            result += CostumeHero.GetBonus(typeBonus);
            return result;
        }

        public void PrepareHeroWithLevel(int level)
        {
            General.Level = level;
            Growth.GrowHero(Characts, Resistances, IncCharacts, General.Level);
        }

        private void GetSkills()
        {
            foreach (Skill skill in skills)
            {
                skill.GetSkill(Evolutions.currentBreakthrough);
            }
        }

        public void PrepareLocalization()
        {
            localization = LanguageController.Instance.GetLocalizationHero(General.ViewId);
        }

        public void PrepareSkillLocalization()
        {
            if (localization == null)
                PrepareLocalization();

            if (localization != null)
            {
                foreach (Skill skill in skills)
                    skill.GetInfoAboutSkill(localization);
            }
            else
            {
                //Debug.LogError("localization not found");
            }
        }

        public bool CheckСonformity(RequirementHeroModel requirementHero)
        {
            bool result = false;

            if (General.RatingHero == requirementHero.rating && General.Race == requirementHero.race)
            {
                result = true;
            }

            return result;
        }

        public void UpRating()
        {
            General.RatingHero += 1;
            Growth.GrowHero(Characts, Resistances, Evolutions.GetGrowth(General.RatingHero));
            Evolutions.ChangeData(General);
        }

        public object Clone()
        {
            var copyHero = new HeroModel
            {
                General = (GeneralInfoHero)General.Clone(),
                Characts = Characts.Clone(),
                IncCharacts = (IncreaseCharacteristicsModel)IncCharacts.Clone(),
                PrefabArrow = PrefabArrow,
                Resistances = (ResistanceModel)Resistances.Clone(),
                CostumeHero = CostumeHero.Clone(),
                skills = skills,
                Evolutions = Evolutions,
                localization = localization
            };
            copyHero.Preparation();
            return copyHero;
        }
    }
}