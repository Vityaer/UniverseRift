using City.TrainCamp;
using Newtonsoft.Json;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Models.City.TrainCamp
{
    [System.Serializable]
    public class RequirementHeroModel
    {
        public int Rating;
        public int Count;
        public EvolutionRequireType RequireRace;
        [HideInInspector] public string IconPath;

        private Sprite _sprite;

        [JsonIgnore]
        [ShowInInspector]
        [HorizontalGroup("2")]
        [LabelText("SpritePath")]
        [PreviewField(100, ObjectFieldAlignment.Left)]
        public Sprite SpritePath
        {
            get
            {
#if UNITY_EDITOR
                if (_sprite == null)
                {
                    _sprite = UnityEditor.AssetDatabase.LoadAssetAtPath<Sprite>(IconPath);
                }
#endif
                return _sprite;
            }
            set
            {
                _sprite = value;
#if UNITY_EDITOR
                var path = UnityEditor.AssetDatabase.GetAssetPath(_sprite);
                IconPath = path;
#endif
            }
        }
    }
}
