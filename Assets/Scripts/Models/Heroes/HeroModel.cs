using City.Buildings.Tavern;
using Fight;
using Hero;
using Models.City.TrainCamp;
using Models.Heroes.Evolutions;
using Models.Heroes.HeroCharacteristics;
using Models.Heroes.Skills;
using System;
using System.Collections.Generic;
using UIController.Localization.Languages;
using UnityEngine;

namespace Models.Heroes
{
    [System.Serializable]
    public class HeroModel : BaseModel
    {
        public GeneralInfoHero General;
        public string ArrowPrefabPath;
        public Evolution Evolutions;
        public Characteristics Characteristics;
        public IncreaseCharacteristicsModel IncCharacts;
        public StorageResistances Resistances;
        public List<Skill> Skills = new List<Skill>();

        private void GetSkills()
        {
            foreach (Skill skill in Skills)
            {
                skill.GetSkill(Evolutions.CurrentBreakthrough);
            }
        }

        public HeroModel Clone()
        {
            return new HeroModel()
            {
                General = this.General.Clone(),
                ArrowPrefabPath = this.ArrowPrefabPath,
                Evolutions = this.Evolutions.Clone(),
                Characteristics = this.Characteristics.Clone(),
                IncCharacts = this.IncCharacts.Clone(),
                Resistances = this.Resistances.Clone(),
                Skills = this.Skills
            };
        }

    }
}