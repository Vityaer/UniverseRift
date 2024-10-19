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
        private bool _isFade = false;
        private bool _otherWaitFadeHide = false;

        public void Initialize()
        {
            _warTableController.OnStartMission.Subscribe(_ => OpenFight()).AddTo(_disposables);
            _fightController.OnFinishFight.Subscribe(_ => CloseFight()).AddTo(_disposables);
            View.OnShowAction.Subscribe(_ => OnFadeShow()).AddTo(_disposables);
            View.OnHideAction.Subscribe(_ => OnFadeHide()).AddTo(_disposables);
        }

        private void OnFadeHide()
        {
            _isFade = false;
            TryStartHide();
        }

        private void OnFadeShow()
        {
            _isFade = true;
            TryStartHide();
        }

        private void TryStartHide()
        {
            if (_isFade && _otherWaitFadeHide)
                Hide();
        }

        public void OpenFight()
        {
            _otherWaitFadeHide = false;
            _temporallyDisposable = _fightController.AfterCreateFight.Subscribe(_ => DoneForHide());
            Show();
        }

        private void DoneForHide()
        {
            _otherWaitFadeHide = true;
            _temporallyDisposable?.Dispose();
            TryStartHide();
        }

        public void CloseFight()
        {
            _otherWaitFadeHide = false;
            _temporallyDisposable = _fightController.OnFigthResult.Subscribe(_ => DoneForHide());
            Show();
        }

        public void Dispose()
        {
            _disposables.Dispose();
            _temporallyDisposable?.Dispose();
        }
    }
}
