using System;
using System.Collections.Generic;
using System.Linq;
using City.Panels.SubjectPanels.Items;
using Common.Db.CommonDictionaries;
using Models.Heroes;
using Models.Heroes.HeroCharacteristics;
using UnityEngine;

namespace Hero
{
    public static class GameHeroUtils
    {
        public static int CalculateStrength(this GameHero hero, CommonDictionaries dictionaries)
        {
            float healthValue = hero.GetCharacteristic(dictionaries, TypeCharacteristic.HP) / 6;
            float damageValue = hero.GetCharacteristic(dictionaries, TypeCharacteristic.Damage);
            int result = Mathf.RoundToInt(healthValue + damageValue);
            return result;
        }

        public static float GetCharacteristic(this GameHero hero, CommonDictionaries dictionaries,
            TypeCharacteristic typeBonus)
        {
            float result = 0;
            float baseBonus = hero.GetBaseCharacteristic(typeBonus);
            float itemsBonus = hero.GetItemsBonusCharacteristic(typeBonus);
            float setBonus = hero.GetSetCharacteristic(dictionaries, typeBonus);

            float bonusPercent = 1f;
 
            string bonusPercentName = $"{typeBonus}Percent";
            if (Enum.TryParse(bonusPercentName, out TypeCharacteristic typePercentCharacteristic))
            {
                bonusPercent = hero.GetPercentBonusCharacteristic(dictionaries, typePercentCharacteristic);
            }

            result = baseBonus * bonusPercent + itemsBonus + setBonus;
            return result;
        }

        private static float GetBaseCharacteristic(this GameHero hero, TypeCharacteristic typeBonus)
        {
            float result = 0f;
            switch (typeBonus)
            {
                case TypeCharacteristic.HP:
                    result = hero.Model.Characteristics.HP;
                    break;
                case TypeCharacteristic.Damage:
                    result = hero.Model.Characteristics.Damage;
                    break;
                case TypeCharacteristic.Initiative:
                    result = hero.Model.Characteristics.Initiative;
                    break;
                case TypeCharacteristic.Defense:
                    result = hero.Model.Characteristics.Main.Defense;
                    break;
            }
            
            return result;
        }
        
        private static float GetItemsBonusCharacteristic(this GameHero hero, TypeCharacteristic typeCharacteristic)
        {
            return hero.Costume.GetBonus(typeCharacteristic);
        }
        
        private static float GetSetCharacteristic(this GameHero hero,
            CommonDictionaries dictionaries,
            TypeCharacteristic typeBonus)
        {
            GameCostumeHero costume = hero.Costume;

            var result = 0f;
            HashSet<string> sets = new();
            foreach (var itemPair in costume.Items)
            {
                sets.Add(itemPair.Value.Model.SetName);
            }
            
            foreach (var itemPair in costume.Items.Where(itemPair => sets.Contains(itemPair.Value.Model.SetName)))
            {
                sets.Remove(itemPair.Value.Model.SetName);

                ItemCostumeExtraBonusesUtils.GetExtraBonusesForItemOnHero(hero,
                    itemPair.Value,
                    dictionaries,
                    out var extraBonuses,
                    out var availableExtraBonusCount
                );

                for (var i = 0; i < availableExtraBonusCount; i++)
                {
                    if (extraBonuses[i].Name == typeBonus)
                    {
                        result += extraBonuses[i].Value;
                    }
                }
            }
            return result;
        }
        
        private static float GetPercentBonusCharacteristic(this GameHero hero,
            CommonDictionaries dictionaries,
            TypeCharacteristic typeBonus)
        {
            float result = 1f;
            
            if (!typeBonus.ToString().Contains("Percent"))
            {
                return result;
            }

            float itemsBonus = hero.Costume.GetBonus(typeBonus);
            
            HashSet<string> sets = new();
            foreach (var itemPair in hero.Costume.Items)
            {
                sets.Add(itemPair.Value.Model.SetName);
            }
            
            foreach (var itemPair in hero.Costume.Items.Where(itemPair => sets.Contains(itemPair.Value.Model.SetName)))
            {
                sets.Remove(itemPair.Value.Model.SetName);

                ItemCostumeExtraBonusesUtils.GetExtraBonusesForItemOnHero(hero,
                    itemPair.Value,
                    dictionaries,
                    out var extraBonuses,
                    out var availableExtraBonusCount
                );

                for (var i = 0; i < availableExtraBonusCount; i++)
                {
                    if (extraBonuses[i].Name == typeBonus)
                    {
                        result += extraBonuses[i].Value;
                    }
                }
            }
            
            return result;
        }
    }
}