using Fight.HeroControllers.Generals;
using Models.Heroes.Actions;
using Newtonsoft.Json;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Models.Heroes.Skills
{
    [System.Serializable]
    public class Skill
    {
        public string ID;
        public bool IsActive = false;

        [HideInInspector] public string IconPath;
        
        [JsonIgnore]
        [ShowInInspector]
        [LabelText("SpritePath")]
        [PreviewField(100, ObjectFieldAlignment.Left)]
        public Sprite SpritePath
        {
            get
            {
                if (_sprite == null && !string.IsNullOrEmpty(IconPath))
                {
                    _sprite = AssetDatabase.LoadAssetAtPath<Sprite>(IconPath);
                }

                return _sprite;
            }
            set
            {
                _sprite = value;
                var path = AssetDatabase.GetAssetPath(_sprite);
                IconPath = path;
            }
        }

        public List<SkillLevel> Levels = new();

        private int _level = 0;
        private List<Effect> _effects = new();
        private Sprite _sprite;

        [JsonIgnore] public int Level { get => _level; }
        public Skill()
        {
            IsActive = false;
            _effects = new List<Effect>();
        }

        public void GetSkill(int currentBreakthrough)
        {
            for (int i = Levels.Count - 1; i >= 0; i--)
            {
                if (Levels[i].RequireNumBreakthrough <= currentBreakthrough)
                {
                    _effects = Levels[i].Effects;
                    _level = i;
                    break;
                }
            }
        }

        public void GetSkillInfo(int currentBreakthrough)
        {
            GetSkill(currentBreakthrough);
            if (Level == -1)
            {
                _effects = Levels[0].Effects;
                _level = 0;
            }
        }

        //API
        public void CreateSkill(HeroController master, int currentBreakthrough = 0)
        {
            GetSkill(currentBreakthrough);
            foreach (Effect effect in _effects)
            {
                effect.CreateEffect(master);
            }
            if (IsActive)
            {
                master.RegisterOnGetListForSpell(GetStartListForSpell);
            }
        }

        public void GetStartListForSpell(List<HeroController> listTarget)
        {
            if (_effects.Count > 0)
                if (_effects[0].Actions.Count > 0)
                    _effects[0].Actions[0].GetListForSpell(listTarget);
        }
    }
}