using System;
using System.Collections.Generic;
using System.Linq;
using MessagePipe;
using UniRx;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using VContainerUi.Interfaces;
using VContainerUi.Messages;
using VContainerUi.Model;
using VContainerUi.Services;
using Object = UnityEngine.Object;

namespace VContainerUi
{
    public class WindowsController : IWindowsController, IInitializable, IDisposable
    {
        private readonly IObjectResolver _container;
        private readonly IReadOnlyList<IWindow> _windows;

        private readonly WindowState _windowState;
        private readonly IUiMessagesReceiverService _uiMessagesReceiver;
        private readonly IUiMessagesPublisherService _uiMessagesPublisher;
        private readonly Stack<IBaseUiController> _windowsStack = new Stack<IBaseUiController>();
        private readonly CompositeDisposable _disposables = new CompositeDisposable();
        private readonly UiScope _scope;

        private IBaseUiController _window;

        public WindowsController(
            IObjectResolver container,
            IReadOnlyList<IWindow> windows,
            WindowState windowState,
            IUiMessagesReceiverService uiMessagesReceiver,
            IUiMessagesPublisherService uiMessagesPublisher,
            UiScope scope
        )
        {
            _container = container;
            _windows = windows;
            _windowState = windowState;
            _uiMessagesReceiver = uiMessagesReceiver;
            _uiMessagesPublisher = uiMessagesPublisher;
            _scope = scope;
        }

        public void Initialize()
        {
            _uiMessagesReceiver.OpenWindowSubscriber.Subscribe(OnOpen, new UiMessageFilter<MessageOpenWindow>(_scope))
                .AddTo(_disposables);

            _uiMessagesReceiver.CloseWindowSubscriber
                .Subscribe(OnClose, new UiMessageFilter<MessageCloseWindow>(_scope)).AddTo(_disposables);

            _uiMessagesReceiver.BackWindowSubscriber
                .Subscribe(_ => OnBack(), new UiMessageFilter<MessageBackWindow>(_scope)).AddTo(_disposables);
            _uiMessagesReceiver.OpenRootWindowSubscriber.Subscribe(OnOpenRootWindow).AddTo(_disposables);
        }

        private void OnClose(MessageCloseWindow message)
        {
            if (_windowsStack.Count == 0)
                return;

            var windowList = _windowsStack.ToList();
            var removeIndex = windowList.FindLastIndex(window => window.GetType().Equals(message.Type));

            if (removeIndex > 0)
            {
                windowList[removeIndex].Back();
                windowList.RemoveAt(removeIndex);
                _windowsStack.Clear();
                foreach (var window in windowList)
                    _windowsStack.Push(window);
            }
            //CloseWindow();
        }

        void IDisposable.Dispose()
        {
            _disposables.Dispose();
        }

        private void OnOpen(MessageOpenWindow message)
        {
            IBaseUiController window;
            if (message.Type != null)
                window = _container.Resolve(message.Type) as IBaseUiController;
            else
                window = _windows.First(f => f.Name == message.Name);
            Open(window, message.OpenType);
        }

        private void Open(IBaseUiController window, OpenType openType)
        {
            var isNextWindowPopUp = window is IPopUp;
            if ((_windowsStack.Count > 0) && (openType == OpenType.Exclusive))
            {
                var previousWindows = GetPreviouslyOpenedWindows();
                foreach (var openedWindow in previousWindows)
                {
                    openedWindow.SetState(UiControllerState.NotActiveNotFocus, openType);
                }
            }

            window.OpenedType = openType;
            _windowsStack.Push(window);
            _windowState.CurrentWindow = window;
            window.SetState(UiControllerState.IsActiveAndFocus, openType);
            _uiMessagesPublisher.MessageShowWindowPublisher.Publish(new MessageShowWindow(window));
            ActiveAndFocus(window, isNextWindowPopUp);
        }

        private void OnBack()
        {
            if (_windowsStack.Count == 0)
                return;

            var exclusive = _windowState.CurrentWindow.OpenedType == OpenType.Exclusive;
            CloseWindow();
            if (exclusive)
                OpenPreviousWindows();
        }

        private void CloseWindow()
        {
            var currentWindow = _windowsStack.Pop();
            currentWindow.Back();

            if (_windowsStack.Count > 0)
            {
                _windowState.CurrentWindow = _windowsStack.Peek();
            }
            else
            {
                _windowState.CurrentWindow = null;
            }
        }

        private void OpenPreviousWindows()
        {
            if (_windowsStack.Count == 0)
                return;
            var previousWindows = GetPreviouslyOpenedWindows();

            foreach (var openedWindow in previousWindows)
            {
                openedWindow.SetState(UiControllerState.IsActiveAndFocus, openedWindow.OpenedType);
            }

            _windowState.CurrentWindow = previousWindows[^1];

            var firstWindow = GetFirstWindow();
            var isFirstPopUp = false;
            ActiveAndFocus(firstWindow, isFirstPopUp);
        }

        private void ActiveAndFocus(IBaseUiController window, bool isPopUp)
        {
            if (!isPopUp)
                _window = window;

            _uiMessagesPublisher.MessageActiveWindowPublisher.Publish(new MessageActiveWindow(_window));
            _uiMessagesPublisher.MessageFocusWindowPublisher.Publish(new MessageFocusWindow(_window));
        }

        private List<IBaseUiController> GetPreviouslyOpenedWindows()
        {
            var windows = new List<IBaseUiController>();

            foreach (var window in _windowsStack)
            {
                windows.Add(window);

                if (window.OpenedType == OpenType.Exclusive)
                    break;
            }
            return windows;
        }

        private IBaseUiController GetFirstWindow()
        {
            foreach (var element in _windowsStack)
            {
                if (element is IWindow == false)
                    continue;
                return element;
            }

            return null;
        }

        private void OnOpenRootWindow(MessageOpenRootWindow obj)
        {
            while (_windowsStack.Count > 1)
            {
                OnBack();
            }
        }

        public void Reset()
        {
            while (_windowsStack.Count > 0)
            {
                OnBack();
            }
            _window = null;
        }
    }
}