using Fight;
using Fight.WarTable;
using System;
using UniRx;
using VContainer;
using VContainer.Unity;
using VContainerUi.Abstraction;

namespace UIController.FadeInOutPanels
{
    public class FadeInOutPanelController : UiController<FadeInOutPanelView>, IInitializable, IDisposable
    {
        [Inject] private readonly WarTableController _warTableController;
        [Inject] private readonly FightController _fightController;

        private readonly CompositeDisposable _disposables = new();
        private IDisposable _temporallyDisposable;
        private bool _fightCreated;

        public void Initialize()
        {
            _warTableController.OnStartMission.Subscribe(_ => OpenFight()).AddTo(_disposables);
            _fightController.OnFinishFight.Subscribe(_ => CloseFight()).AddTo(_disposables);
            View.OnShowAction.Subscribe(_ => OnOpenFade()).AddTo(_disposables);
            View.OnHideAction.Subscribe(_ => OnCloseFade()).AddTo(_disposables);
        }

        private void OnCloseFade()
        {
        }

        private void OnOpenFade()
        {
            if(_fightCreated)
                Hide();
        }

        public void OpenFight()
        {
            _fightCreated = false;
            _temporallyDisposable = _fightController.AfterCreateFight.Subscribe(_ => OnAfterCreateFight());
            Show();
        }

        private void OnAfterCreateFight()
        {
            _fightCreated = true;
            Hide();
        }

        public void CloseFight()
        {
            Hide();
        }

        public void Dispose()
        {
            _disposables.Dispose();
        }
    }
}
