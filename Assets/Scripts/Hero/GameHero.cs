using Fight.Common.HeroControllers.Generals;
using Models;
using Models.Heroes;
using Models.Heroes.HeroCharacteristics;
using Sirenix.Utilities;
using UniRx;
using UnityEngine;

namespace Hero
{
    [System.Serializable]
    public class GameHero
    {
        private readonly HeroModel m_model;
        private readonly HeroData m_heroData;
        private readonly HeroController m_prefab;

        public ReactiveCommand OnChangeData => new ReactiveCommand();
        public GameCostumeHero Costume => new GameCostumeHero();

        public Sprite Avatar
        {
            get
            {
                if(m_heroData == null)
                {
                    Debug.LogError($"Hero data is null, model.id: {m_model.Id}");
                    return null;
                }

                if (m_prefab == null)
                {
                    Debug.LogError($"Prefab is null, model.id: {m_model.Id}");
                    return null;
                }

                if (m_prefab.Stages.IsNullOrEmpty())
                {
                    Debug.LogError($"_prefab.Stages is null or empty, model.id: {m_model.Id}");
                    return null;
                }

                return m_prefab.Stages[m_heroData.Stage].Avatar;
            }
        }
        
        public HeroModel Model => m_model;
        public HeroData HeroData => m_heroData;
        public BaseCharacteristicModel GetBaseCharacteristic => m_model.Characteristics.Main;
        public float MaxHP => m_model.Characteristics.HP;
        public HeroController Prefab => m_prefab;

        public GameHero(HeroModel hero, HeroData data)
        {
            m_model = hero.Clone();
            m_heroData = data;

            var stage = (m_heroData.Rating / 5);
            m_heroData.Stage = stage;
            var path = $"{Constants.ResourcesPath.HEROES_PATH}{m_model.General.HeroId}";
            m_prefab = Resources.Load<HeroController>(path);
            if (m_prefab == null)
            {
                Debug.LogError($"Failed to load Hero Controller: {path}");
            }

            PrepareHero();
            PrepareCharacts(hero);
        }

        private void PrepareHero()
        {
            Growth.GrowHero(m_model.Characteristics, m_model.Resistances, m_model.IncCharacts, m_heroData.Level);
        }

        public void LevelUp(int count = 1)
        {
            for (int i = 0; i < count; i++)
            {
                if (m_heroData.Level >= m_model.Evolutions.LimitLevel())
                {
                    continue;
                }
                
                m_heroData.Level += 1;
                Growth.GrowHero(m_model.Characteristics, m_model.Resistances, m_model.IncCharacts);
            }

            OnChangeData.Execute();
        }

        public void UpRating()
        {
            m_heroData.Rating += 1;
            var increaseStatsContainer = m_model.Evolutions.GetGrowth(m_heroData.Rating);
            Growth.GrowHero(m_model.Characteristics, m_model.Resistances, increaseStatsContainer);
            OnChangeData.Execute();
        }

        public HeroData GetSaveData()
        {
            m_heroData.Costume = new CostumeData(Costume);
            return m_heroData;
        }

        private void PrepareCharacts(HeroModel hero)
        {
            //this.characts = new FightCharacteristics(hero.Characts.Clone());
            //characts.Damage = hero.GetCharacteristic(TypeCharacteristic.Damage);
            //this.characts.GeneralArmor = (int)hero.GetCharacteristic(TypeCharacteristic.Defense);
            //this.characts.GeneralAttack = (int)hero.GetCharacteristic(TypeCharacteristic.Attack);
            //this.characts.Initiative = hero.GetCharacteristic(TypeCharacteristic.Initiative);
            //this.characts.HP = Mathf.Round(hero.GetCharacteristic(TypeCharacteristic.HP));
            //this.characts.MaxHP = this.characts.HP;
        }

        public void PrepareSkills(HeroController master)
        {
            //foreach (Skill skill in skills)
            //{
            //    skill.CreateSkill(master, currentBreakthrough);
            //}
        }

        //public int GetCharacteristic(TypeCharacteristic hP)
        //{
        //    throw new NotImplementedException();
        //}


        //public void UpRating()
        //{
        //    General.Rating += 1;
        //    Growth.GrowHero(Characts, Resistances, Evolutions.GetGrowth(General.Rating));
        //    Evolutions.ChangeData(General);
        //}

        //public int Strength() => Mathf.RoundToInt(GetCharacteristic(TypeCharacteristic.Damage) + GetCharacteristic(TypeCharacteristic.HP) / 3f);
        
    }
}