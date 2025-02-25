﻿using Fight.HeroControllers.Generals;
using Models;
using Models.Heroes;
using Models.Heroes.HeroCharacteristics;
using UniRx;
using UnityEngine;

namespace Hero
{
    [System.Serializable]
    public class GameHero
    {
        private HeroModel _model;
        private HeroData _heroData;
        private HeroController _prefab;

        public ReactiveCommand OnChangeData => new ReactiveCommand();
        public GameCostumeHero Costume = new GameCostumeHero();

        public Sprite Avatar => _prefab.Stages[_heroData.Stage].Avatar;
        public HeroModel Model => _model;
        public HeroData HeroData => _heroData;
        public BaseCharacteristicModel GetBaseCharacteristic => _model.Characteristics.Main;
        public float MaxHP => _model.Characteristics.HP;
        public int Strength => Mathf.RoundToInt(GetCharacteristic(TypeCharacteristic.HP) / 6 + GetCharacteristic(TypeCharacteristic.Damage));
        public HeroController Prefab => _prefab;

        public GameHero(HeroModel hero, HeroData data)
        {
            _model = hero.Clone();
            _heroData = data;

            var stage = (_heroData.Rating / 5);
            _heroData.Stage = stage;
            var path = $"{Constants.ResourcesPath.HEROES_PATH}{_model.General.HeroId}";
            _prefab = Resources.Load<HeroController>(path);

            PrepareHero();
            PrepareCharacts(hero);
        }

        private void PrepareHero()
        {
            Growth.GrowHero(_model.Characteristics, _model.Resistances, _model.IncCharacts, _heroData.Level);
        }

        public void LevelUp(int count = 1)
        {
            for (int i = 0; i < count; i++)
            {
                if (_heroData.Level < _model.Evolutions.LimitLevel())
                {
                    _heroData.Level += 1;
                    Growth.GrowHero(_model.Characteristics, _model.Resistances, _model.IncCharacts);
                }
            }

            OnChangeData.Execute();
        }

        public void UpRating()
        {
            _heroData.Rating += 1;
            var increaseStatsContainer = _model.Evolutions.GetGrowth(_heroData.Rating);
            Growth.GrowHero(_model.Characteristics, _model.Resistances, increaseStatsContainer);
            OnChangeData.Execute();
        }

        public HeroData GetSaveData()
        {
            _heroData.Costume = new CostumeData(Costume);
            return _heroData;
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

        public float GetCharacteristic(TypeCharacteristic typeBonus)
        {
            float result = 0;
            switch (typeBonus)
            {
                case TypeCharacteristic.HP:
                    result += _model.Characteristics.HP;
                    break;
                case TypeCharacteristic.Damage:
                    result += _model.Characteristics.Damage;
                    break;
                case TypeCharacteristic.Initiative:
                    result += _model.Characteristics.Initiative;
                    break;
                case TypeCharacteristic.Defense:
                    result += _model.Characteristics.Main.Defense;
                    break;
            }
            result += Costume.GetBonus(typeBonus);
            return result;
        }
    }
}