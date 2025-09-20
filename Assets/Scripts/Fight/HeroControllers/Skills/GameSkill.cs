using Models.Heroes.Actions;
using Models.Heroes.Skills;
using System.Collections.Generic;
using Fight.Common.HeroControllers.Generals;
using UniRx;

namespace Fight.Common.HeroControllers.Skills
{
    public class GameSkill
    {
        private readonly SkillModel _model;
        private readonly HeroController _master;

        private List<Effect> _effects = new();


        public GameSkill(SkillModel model, HeroController heroController, int breakthrough)
        {
            _model = model;
            _master = heroController;

            _model.GetSkill(breakthrough, out _effects, out _);
        }

        public void CreateSkill(HeroController master, CompositeDisposable disposables, int currentBreakthrough = 0)
        {
            foreach (var effect in _effects)
            {
                effect.CreateEffect(master, disposables);
            }

            if (_model.IsActive)
            {
                master.OnChangeListSpell.Subscribe(GetStartListForSpell).AddTo(disposables);
            }
        }

        public void GetStartListForSpell(List<HeroController> listTarget)
        {
            if (_effects.Count == 0)
                return;

            if (_effects[0].Actions.Count == 0)
                return;

            _effects[0].Actions[0].GetListForSpell(listTarget);
        }
    }
}
