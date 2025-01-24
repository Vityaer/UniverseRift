using Fight.HeroControllers.Generals;
using System;
using UniRx;
using UnityEngine.Localization;
using VContainer.Unity;
using VContainerUi.Abstraction;

namespace Fight.UI
{
    public class FightPanelController : UiController<FightPanelView>, IInitializable, IDisposable
    {
        private readonly FightController _fightController;
        private readonly CompositeDisposable _compositeDisposable = new();

        public FightPanelController(FightController fightController)
        {
            _fightController = fightController;
        }

        public void Initialize()
        {
            View.WaitButton.OnClickAsObservable().Subscribe(_ => _fightController.WaitTurn()).AddTo(_compositeDisposable);
            View.DeffendStateButton.OnClickAsObservable().Subscribe(_ => _fightController.DefenseTurn()).AddTo(_compositeDisposable);
            View.SpellButton.OnClickAsObservable().Subscribe(_ => _fightController.SpellTurn()).AddTo(_compositeDisposable);

            _fightController.AfterCreateFight.Subscribe(_ => Show()).AddTo(_compositeDisposable);
            _fightController.OnFinishFight.Subscribe(_ => Hide()).AddTo(_compositeDisposable);

            _fightController.OnChangeFightUiText.Subscribe(OnChangeFightUiText).AddTo(_compositeDisposable);

            _fightController.SetControllerUi(this);
        }

        public void SetHeroStatus(HeroController heroController)
        {
            View.WaitButton.interactable = heroController.CanWait;
            var spell = heroController.Hero.Model.Skills.Find(skill => skill.IsActive);
            View.SpellButton.gameObject.SetActive(spell != null);
            View.SpellButton.interactable = (heroController.Stamina >= 100f);
        }

        private void OnChangeFightUiText(LocalizedString label)
        {
            View.FightStatusText.StringReference = label;
            View.FightStatusText.RefreshString();
        }

        public void Dispose()
        {
            _compositeDisposable?.Dispose();
        }
    }
}
