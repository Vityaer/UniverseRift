using System;
using System.Collections.Generic;
using System.Linq;
using MessagePipe;
using UniRx;
using VContainer;
using VContainer.Unity;
using VContainerUi;
using VContainerUi.Interfaces;
using VContainerUi.Messages;
using VContainerUi.Model;
using VContainerUi.Services;

namespace Plugins.VContainerUi.Runtime
{
    public class WindowsController : IWindowsController, IInitializable, IDisposable
    {
        private readonly IObjectResolver m_container;
        private readonly IReadOnlyList<IWindow> m_windows;

        private readonly WindowState m_windowState;
        private readonly IUiMessagesReceiverService m_uiMessagesReceiver;
        private readonly IUiMessagesPublisherService m_uiMessagesPublisher;
        private readonly Stack<IBaseUiController> m_windowsStack = new Stack<IBaseUiController>();
        private readonly CompositeDisposable m_disposables = new CompositeDisposable();
        private readonly UiScope m_scope;

        private readonly ReactiveCommand<IUiController> m_onElementClose = new ReactiveCommand<IUiController>();
        private readonly ReactiveCommand<IUiController> m_onElementOpen = new ReactiveCommand<IUiController>();
        private IBaseUiController m_window;

        public IObservable<IUiController> OnElementClose => m_onElementClose;
        public IObservable<IUiController> OnElementOpen => m_onElementOpen;
        
        public WindowsController(
            IObjectResolver container,
            IReadOnlyList<IWindow> windows,
            WindowState windowState,
            IUiMessagesReceiverService uiMessagesReceiver,
            IUiMessagesPublisherService uiMessagesPublisher,
            UiScope scope
        )
        {
            m_container = container;
            m_windows = windows;
            m_windowState = windowState;
            m_uiMessagesReceiver = uiMessagesReceiver;
            m_uiMessagesPublisher = uiMessagesPublisher;
            m_scope = scope;
        }

        public void Initialize()
        {
            m_uiMessagesReceiver.OpenWindowSubscriber.Subscribe(OnOpen,
                    new UiMessageFilter<MessageOpenWindow>(m_scope))
                .AddTo(m_disposables);

            m_uiMessagesReceiver.CloseWindowSubscriber
                .Subscribe(OnClose, new UiMessageFilter<MessageCloseWindow>(m_scope))
                .AddTo(m_disposables);

            m_uiMessagesReceiver.BackWindowSubscriber
                .Subscribe(_ => OnBack(), new UiMessageFilter<MessageBackWindow>(m_scope))
                .AddTo(m_disposables);
            
            m_uiMessagesReceiver.OpenRootWindowSubscriber.Subscribe(OnOpenRootWindow)
                .AddTo(m_disposables);
        }

        private void OnClose(MessageCloseWindow message)
        {
            if (m_windowsStack.Count == 0)
                return;

            List<IBaseUiController> windowList = m_windowsStack.ToList();
            int removeIndex = windowList.FindLastIndex(window
                => window.GetType() == message.Type);

            if (removeIndex <= 0)
            {
                return;
            }

            m_onElementClose.Execute(windowList[removeIndex] as IUiController);
            windowList[removeIndex].Back();
            windowList.RemoveAt(removeIndex);
            m_windowsStack.Clear();
            foreach (IBaseUiController window in windowList)
            {
                m_windowsStack.Push(window);
            }
        }

        void IDisposable.Dispose()
        {
            m_disposables.Dispose();
        }

        private void OnOpen(MessageOpenWindow message)
        {
            IBaseUiController window;
            if (message.Type != null)
                window = m_container.Resolve(message.Type) as IBaseUiController;
            else
                window = m_windows.First(f => f.Name == message.Name);

            Open(window, message.OpenType);
        }

        private void Open(IBaseUiController window, OpenType openType)
        {
            bool isNextWindowPopUp = window is IPopUp;
            if (m_windowsStack.Count > 0 && openType == OpenType.Exclusive)
            {
                List<IBaseUiController> previousWindows = GetPreviouslyOpenedWindows();
                foreach (IBaseUiController openedWindow in previousWindows)
                {
                    openedWindow.SetState(UiControllerState.NotActiveNotFocus, openType);
                }
            }

            m_onElementOpen.Execute(window as IUiController);
            window.OpenedType = openType;
            m_windowsStack.Push(window);
            m_windowState.CurrentWindow = window;
            window.SetState(UiControllerState.IsActiveAndFocus, openType);
            m_uiMessagesPublisher.MessageShowWindowPublisher.Publish(new MessageShowWindow(window));
            ActiveAndFocus(window, isNextWindowPopUp);
        }

        private void OnBack()
        {
            if (m_windowsStack.Count == 0)
                return;

            var exclusive = m_windowState.CurrentWindow.OpenedType == OpenType.Exclusive;
            CloseWindow();
            if (exclusive)
                OpenPreviousWindows();
        }

        private void CloseWindow()
        {
            IBaseUiController currentWindow = m_windowsStack.Pop();
            currentWindow.Back();

            m_windowState.CurrentWindow = m_windowsStack.Count > 0
                ? m_windowsStack.Peek()
                : null;
        }

        private void OpenPreviousWindows()
        {
            if (m_windowsStack.Count == 0)
                return;
            List<IBaseUiController> previousWindows = GetPreviouslyOpenedWindows();

            foreach (IBaseUiController openedWindow in previousWindows)
            {
                openedWindow.SetState(UiControllerState.IsActiveAndFocus, openedWindow.OpenedType);
            }

            m_windowState.CurrentWindow = previousWindows[^1];

            IBaseUiController firstWindow = GetFirstWindow();
            ActiveAndFocus(firstWindow, false);
        }

        private void ActiveAndFocus(IBaseUiController window, bool isPopUp)
        {
            if (!isPopUp)
                m_window = window;

            m_uiMessagesPublisher.MessageActiveWindowPublisher.Publish(new MessageActiveWindow(m_window));
            m_uiMessagesPublisher.MessageFocusWindowPublisher.Publish(new MessageFocusWindow(m_window));
        }

        private List<IBaseUiController> GetPreviouslyOpenedWindows()
        {
            List<IBaseUiController> windows = new List<IBaseUiController>();

            foreach (IBaseUiController window in m_windowsStack)
            {
                windows.Add(window);

                if (window.OpenedType == OpenType.Exclusive)
                    break;
            }
            return windows;
        }

        private IBaseUiController GetFirstWindow()
        {
            foreach (IBaseUiController element in m_windowsStack)
            {
                if (element is IWindow == false)
                    continue;
                return element;
            }

            return null;
        }

        private void OnOpenRootWindow(MessageOpenRootWindow obj)
        {
            while (m_windowsStack.Count > 1)
            {
                OnBack();
            }
        }

        public void Reset()
        {
            while (m_windowsStack.Count > 0)
            {
                OnBack();
            }
            m_window = null;
        }
    }
}