using DG.Tweening;
using System;
using UIController.GameSystems;
using UniRx;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using VContainerUi.Abstraction;

namespace City.SliderCity
{
    public class CityScroller : UiController<CityView>, IInitializable, IDisposable
    {
        private const float MOVE_ANIMATION_TIME = 0.25f;

        [Inject] private readonly MainSwipeController _mainSwipeController;
        private Vector2 _leftPos;
        private Vector2 _rightPos;
        private readonly CompositeDisposable _disposables = new CompositeDisposable();

        private int _curentSheet;
        private Tween _tween;

        public void Initialize()
        {
            _leftPos = new Vector2(-ScreenSize.X, 0);
            _rightPos = new Vector2(ScreenSize.X, 0);
            _curentSheet = View.ListCitySheet.Count / 2;
            SetStartPosition();
            _mainSwipeController.OnSwipe.Subscribe(OnSwipe).AddTo(_disposables);
        }

        private void OnSwipe(SwipeType typeSwipe)
        {
            switch (typeSwipe)
            {
                case SwipeType.Left:
                    SwipeLeft();
                    break;
                case SwipeType.Right:
                    SwipeRight();
                    break;
            }
        }

        public void SwipeLeft()
        {
            View.ListCitySheet[_curentSheet].DOMove(_rightPos, MOVE_ANIMATION_TIME);
            _curentSheet = Math.Clamp(_curentSheet - 1, 0, View.ListCitySheet.Count);
            View.ListCitySheet[_curentSheet].DOMove(Vector2.zero, MOVE_ANIMATION_TIME);
        }

        public void SwipeRight()
        {
            View.ListCitySheet[_curentSheet].DOMove(_leftPos, MOVE_ANIMATION_TIME);
            _curentSheet = Math.Clamp(_curentSheet + 1, 0, View.ListCitySheet.Count);
            View.ListCitySheet[_curentSheet].DOMove(Vector2.zero, MOVE_ANIMATION_TIME);
        }

        private void SetStartPosition()
        {
            for (int i = 0; i < _curentSheet; i++)
            {
                View.ListCitySheet[i].position = _leftPos;
            }

            for (int i = _curentSheet + 1; i < View.ListCitySheet.Count; i++)
            {
                View.ListCitySheet[i].position = _rightPos;
            }
        }

        public void Dispose()
        {
            _disposables.Dispose();
        }
    }
}