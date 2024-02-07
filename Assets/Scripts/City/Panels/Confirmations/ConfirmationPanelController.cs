using System;
using UiExtensions.Scroll.Interfaces;
using UniRx;
using VContainerUi.Messages;
using VContainerUi.Model;

namespace City.Panels.Confirmations
{
    public class ConfirmationPanelController : UiPanelController<ConfirmationPanelView>
    {
        private Action _onConfirmAction;
        private Action _onDenyAction;

        public override void Start()
        {
            View.ConfirmActionButton.OnClickAsObservable().Subscribe(_ => ConfirmAction()).AddTo(Disposables);
            View.DenyActionButton.OnClickAsObservable().Subscribe(_ => DenyAction()).AddTo(Disposables);
            base.Start();
        }

        public void Show(string questionText, Action onConfirmAction, Action onDenyAction)
        {
            View.QuestionText.text = questionText;
            _onConfirmAction = onConfirmAction;
            _onDenyAction = onDenyAction;
            MessagesPublisher.OpenWindowPublisher.OpenWindow<ConfirmationPanelController>(openType: OpenType.Exclusive);
        }

        private void DenyAction()
        {
            _onDenyAction?.Invoke();
        }

        private void ConfirmAction()
        {
            _onConfirmAction?.Invoke();
        }

        public override void OnHide()
        {
            _onConfirmAction = null;
            _onDenyAction = null;
            base.OnHide();
        }
    }
}
