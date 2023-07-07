using City.SliderCity;
using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UniRx;
using UnityEngine;
using VContainer.Unity;

namespace UIController.GameSystems
{
    public class MainSwipeController : IInitializable
    {
        public const float WidthPercent = 0.2f;

        private Vector3 _startPosition;
        private Vector3 _endPosition;
        private Vector2 _swipeDistance;
        private ReactiveCommand<SwipeType> _onSwipe = new ReactiveCommand<SwipeType>();
        private readonly CancellationTokenSource _tokenSource = new CancellationTokenSource();

        public IObservable<SwipeType> OnSwipe => _onSwipe;

        public void Initialize()
        {
            _swipeDistance.x = Screen.width * WidthPercent;
            SwipeChecker(_tokenSource).Forget();
        }

        private async UniTaskVoid SwipeChecker(CancellationTokenSource cancellationTokenSource)
        {
            while (true)
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
                            _onSwipe.Execute(SwipeType.Left);
                        }
                        else if (_startPosition.x - _endPosition.x > _swipeDistance.x)
                        {
                            _onSwipe.Execute(SwipeType.Right);
                        }
                    }
                }

                await UniTask.Yield(cancellationTokenSource.Token);
            }
        }
    }
}