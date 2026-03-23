using System;
using System.Collections.Generic;
using UniRx;
using VContainer;
using VContainerUi.Interfaces;
using VContainerUi.Model;

namespace Plugins.VContainerUi.Runtime.Abstraction
{
    public abstract class UiController<T> : IPopUp, IUiController where T : IUiView
    {
        private readonly Stack<UiControllerState> m_states = new Stack<UiControllerState>();
        private readonly UiControllerState m_defaultState = new UiControllerState(false, false, 0);

        private UiControllerState m_currentState;
        protected readonly ReactiveCommand<bool> OnNewsStatusChangeInternal = new();

        [Inject] protected readonly T View;
        public bool IsActive { get; private set; }
        public bool InFocus { get; private set; }
        public OpenType OpenedType { get; set; }

        public IObservable<bool> OnNewsStatusChange => OnNewsStatusChangeInternal;

        public void SetState(UiControllerState state, OpenType openedType = OpenType.Exclusive)
        {
            OpenedType = openedType;
            m_currentState = state;
            m_states.Push(state);
            ProcessState();
        }

        public virtual void ProcessStateOrder()
        {
            if (!m_currentState.IsActive)
                return;
            SetOrder(m_currentState.Order);
        }

        public void ProcessState()
        {
            if (IsActive != m_currentState.IsActive)
            {
                IsActive = m_currentState.IsActive;
                if (m_currentState.IsActive)
                {
                    Show();
                }

                else
                {
                    Hide();
                }
            }

            if (InFocus == m_currentState.InFocus)
                return;

            InFocus = m_currentState.InFocus;
            OnHasFocus(m_currentState.InFocus);
        }

        public void Back()
        {
            if (m_states.Count > 0)
                m_states.Pop();

            if (m_states.Count == 0)
            {
                m_currentState = m_defaultState;
                SetState(m_defaultState);
                return;
            }

            SetState(m_states.Pop());
        }

        protected virtual void Show()
        {
            View.Show();
            OnShow();
        }

        public virtual void OnShow() { }

        protected virtual void Hide()
        {
            View.Hide();
            OnHide();
        }

        public virtual void OnHide() { }

        public virtual void OnHasFocus(bool inFocus) { }

        IUiElement[] IBaseUiController.GetUiElements() => View.GetUiElements();
        private void SetOrder(int index) => View.SetOrder(index);

        protected virtual void OnLoadGame() { }
    }
}