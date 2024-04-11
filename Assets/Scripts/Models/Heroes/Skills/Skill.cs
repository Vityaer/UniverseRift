using Fight.HeroControllers.Generals;
using Models.Heroes.Actions;
using Newtonsoft.Json;
using System.Collections.Generic;
using UIController.Localization.Languages;
using UnityEngine;

namespace Models.Heroes.Skills
{
    [System.Serializable]
    public class Skill
    {
        public string ID;
        public Sprite Icon;
        private int _level = 0;

        [SerializeField] private string _name = "empty name";
        [SerializeField] private string _description = "empty description";

        public bool IsActive = false;
        public List<SkillLevel> Levels = new List<SkillLevel>();

        private SkillLevelLocalization _skillLocalization = null;

        private List<Effect> _effects = new List<Effect>();

        [JsonIgnore] public string Name => _name;
        [JsonIgnore] public string Description => _description;
        [JsonIgnore] public int Level { get => _level; }

        public Skill()
        {
            Icon = null;
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

        //Info API	
        public void GetInfoAboutSkill(HeroLocalization localization)
        {
            _skillLocalization = localization.GetDescriptionSkill(ID, Level);
        }

        public string GetDescription(string description)
        {
            string strForReplace = "";
            if (_effects.Count == 0) Debug.Log(string.Concat(Name, " not founed effects"));
            if (_effects.Count == 1)
            {
                for (int i = 0; i < _effects[0].Actions.Count; i++)
                {
                    strForReplace = string.Concat("{Action", (i + 1).ToString());
                    description = description.Replace(string.Concat(strForReplace, ".Count}"), _effects[0].Actions[i].CountTarget.ToString());
                    description = description.Replace(string.Concat(strForReplace, ".Amount}"), _effects[0].Actions[i].Amount.ToString());
                    description = description.Replace(string.Concat(strForReplace, ".RoundCount}"), _effects[0].Actions[i].Rounds.Count.ToString());
                }
            }
            this._description = description;
            return description;
        }
    }
}