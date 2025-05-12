using DG.Tweening;
using Fight.Grid;
using Fight.Misc;
using System;
using System.Collections;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;

namespace Fight.HeroControllers.Generals.Movements
{
    public abstract class AbstractMovement : MonoBehaviour, IMovementable
    {
        protected Animator Animator;

        protected HeroController Hero;
        protected GridController GridController;
        protected HexagonCell MyPlace;
        protected Tween MoveTween;

        protected ReactiveCommand<HexagonCell> _onChangePlace = new();
        protected ReactiveCommand<CellDirectionType> _moveDirection = new();

        public IObservable<CellDirectionType> MoveDirection => _moveDirection;
        public IObservable<HexagonCell> OnChangePlace => _onChangePlace;

        public void Init(GridController gridController, HeroController hero, HexagonCell place, Animator animator)
        {
            GridController = gridController;
            Hero = hero;
            MyPlace = place;
            Animator = animator;
        }

        public abstract UniTask Move(HexagonCell targetCell);

        protected void SetMyPlaceColor()
        {
            if (Hero.Side == Side.Left)
            {
                MyPlace.SetColor(Constants.Colors.NOT_ACHIEVABLE_FRIEND_CELL_COLOR);
            }
            else
            {
                MyPlace.SetColor(Constants.Colors.NOT_ACHIEVABLE_ENEMY_CELL_COLOR);
            }
        }

        private void OnDestroy()
        {
            MoveTween.Kill();
        }
    }
}
