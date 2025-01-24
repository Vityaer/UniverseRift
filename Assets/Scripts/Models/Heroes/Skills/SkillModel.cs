using Models.Heroes.Actions;
using Newtonsoft.Json;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Models.Heroes.Skills
{
    [System.Serializable]
    public class SkillModel
    {
        public string ID;
        public bool IsActive = false;

        [HideInInspector] public string IconPath;

        private Sprite _sprite;

        [JsonIgnore]
        [ShowInInspector]
        [LabelText("SpritePath")]
        [PreviewField(100, ObjectFieldAlignment.Left)]
        public Sprite SpritePath
        {
            get
            {
#if UNITY_EDITOR
                if (_sprite == null && !string.IsNullOrEmpty(IconPath))
                {
                    _sprite = AssetDatabase.LoadAssetAtPath<Sprite>(IconPath);
                }
#endif
                return _sprite;
            }
            set
            {
                _sprite = value;
#if UNITY_EDITOR
                var path = AssetDatabase.GetAssetPath(_sprite);
                IconPath = path;
#endif
            }
        }

        public List<SkillLevel> Levels = new();

        public void GetSkill(int currentBreakthrough, out List<Effect> effects, out int level)
        {
            effects = new List<Effect>();
            level = 0;
            for (int i = Levels.Count - 1; i >= 0; i--)
            {
                if (Levels[i].RequireNumBreakthrough <= currentBreakthrough)
                {
                    effects = Levels[i].Effects;
                    level = i;
                    break;
                }
            }
        }
    }
}