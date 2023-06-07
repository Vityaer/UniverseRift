using System.Collections.Generic;
using UnityEngine;

namespace UIController.ItemVisual
{
    public class SystemSprites : MonoBehaviour
    {
        public List<GeneralSprite> generalListSprites = new List<GeneralSprite>();
        public List<RaceSprite> raceListSprites = new List<RaceSprite>();
        public List<VocationSprite> vocationListSprites = new List<VocationSprite>();

        public void AddSprite()
        {

        }

        public Sprite GetSprite(string nameRace) { return raceListSprites.Find(x => x.race == nameRace)?.image; }
        public Sprite GetVocationImage(string nameVocation) { return vocationListSprites.Find(x => x.vocation == nameVocation)?.image; }
        public Sprite GetSprite(SpriteName name) { return generalListSprites.Find(x => x.name == name)?.image; }
        private static SystemSprites instance;
        public static SystemSprites Instance { get => instance; }
        void Awake() { instance = this; }
    }

    //TODO: переделать список картинок
    [System.Serializable] public abstract class BaseItemSprite { public Sprite image; }
    [System.Serializable] public class RaceSprite : BaseItemSprite { public string race; }
    [System.Serializable] public class VocationSprite : BaseItemSprite { public string vocation; }
    [System.Serializable] public class GeneralSprite : BaseItemSprite { public SpriteName name = SpriteName.OneStarHero; }


    public enum SpriteName
    {
        OneStarHero = 1,
        TwoStarHero = 2,
        ThreeStarHero = 3,
        FourStartHero = 4,
        FiveStarHero = 5,
        BaseSplinterHero = 6,
        BaseSplinterArtifact = 7,
        OneStarPeople = 11,
        OneStarElf = 12
    }
}