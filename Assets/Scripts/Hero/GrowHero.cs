﻿using Models.Heroes;
using Models.Heroes.HeroCharacteristics;
using System;
using UnityEngine;

namespace Hero
{
    public class Growth
    {
        public static void GrowHero(Characteristics characts, StorageResistances resistance, IncreaseCharacteristicsModel increaseCharacts, int level = 1)
        {
            GrowthCharact(ref characts.HP, increaseCharacts.increaseHP, level);
            GrowthCharact(ref characts.Damage, increaseCharacts.increaseDamage, level);
            GrowthCharact(ref characts.Accuracy, increaseCharacts.increaseAccuracy, level);
            GrowthCharact(ref characts.Initiative, increaseCharacts.increaseInitiative, level);
            GrowthCharact(ref characts.CleanDamage, increaseCharacts.increaseCleanDamage, level);
            GrowthCharact(ref characts.DamageCriticalAttack, increaseCharacts.increaseDamageCriticalAttack, level);
            GrowthCharact(ref characts.ProbabilityCriticalAttack, increaseCharacts.increaseProbabilityCriticalAttack, level);
            GrowthCharact(ref characts.Dodge, increaseCharacts.increaseDodge, level);

            GrowthCharact(ref resistance.CritResistance, increaseCharacts.increaseCritResistance, level);
            GrowthCharact(ref resistance.MagicResistance, increaseCharacts.increaseMagicResistance, level);
        }

        private static void GrowthCharact(ref int charact, float increase, int level)
        {
            for (int i = 0; i < level; i++)
                charact = (int)Mathf.Ceil(charact * (1 + increase / 100f));
        }

        private static void GrowthCharact(ref float charact, float increase, int level)
        {
            for (int i = 0; i < level; i++)
            {
                charact = charact * (1 + increase / 100f);
                charact = (float)Math.Round(charact, 3);
            }
        }
    }
}