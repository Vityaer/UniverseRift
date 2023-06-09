using Models.Heroes.Skills;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace City.Pets
{
    [CreateAssetMenu(fileName = "New Pet", menuName = "Custom ScriptableObject/Pet", order = 55)]
    [System.Serializable]
    public class PetAvatar : ScriptableObject, ICloneable
    {
        public string Name;
        public uint ID;
        public string Description;
        public Sprite Avatar;
        public GameObject Prefab;
        public uint Level;
        public int Rating;
        public List<Skill> Skills = new List<Skill>();

        public object Clone()
        {
            return new PetAvatar
            {
            };
        }
    }
}