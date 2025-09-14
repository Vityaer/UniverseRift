using System;
using UniRx;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using VContainerUi.Abstraction;
using VContainerUi.Interfaces;
using VContainerUi.Services;

namespace City.Panels.ScreenBlockers
{
    public class ScreenBlockerController : UiController<ScreenBlockerView>, IStartable, IDisposable
    {
        private CompositeDisposable _disposables = new();

        [Inject] protected readonly IUiMessagesPublisherService UiMessagesPublisher;

        private IUiView _view;
        
        public ReactiveCommand OnHasten = new();

        public void Start()
        {
            _view = View;
            View.Button.OnClickAsObservable()
                .Subscribe(_ => OnHasten.Execute())
                .AddTo(_disposables);
            View.transform.SetAsLastSibling();
        }

        public void Dispose()
        {
            _disposables.Dispose();
        }

        public void Open()
        {
            _view.Show();
            View.transform.SetAsLastSibling();
        }
        
        public void Close()
        {
            _view.Hide();
        }
    }
}