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
