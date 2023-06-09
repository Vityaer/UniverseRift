using System;
using UnityEngine;

namespace UIController.GameSystems
{
    public class MainTouchControllerScript : MonoBehaviour
    {
        public float WidthPercent = 0.2f;

        private Vector3 _startPosition;
        private Vector3 _endPosition;
        private Vector2 _swipeDistance;
        private Action<TypeSwipe> _observerSwipe;

        public static MainTouchControllerScript Instance { get; private set; }

        void Awake()
        {
            Instance = this;
        }

        void Start()
        {
            _swipeDistance.x = Screen.width * WidthPercent;
        }

        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                _startPosition = Input.mousePosition;
            }
            if (Input.GetMouseButtonUp(0))
            {
                _endPosition = Input.mousePosition;
                if (Mathf.Abs(_endPosition.y - _startPosition.y) < _swipeDistance.x)
                {
                    if (_endPosition.x - _startPosition.x > _swipeDistance.x)
                    {
                        OnSwipe(TypeSwipe.Left);
                    }
                    else if (_startPosition.x - _endPosition.x > _swipeDistance.x)
                    {
                        OnSwipe(TypeSwipe.Right);
                    }
                }
            }
        }

        public void RegisterOnObserverSwipe(Action<TypeSwipe> d) { _observerSwipe += d; }
        public void UnregisterOnObserverSwipe(Action<TypeSwipe> d) { _observerSwipe -= d; }
        private void OnSwipe(TypeSwipe type) { if (_observerSwipe != null) _observerSwipe(type); }

    }
}