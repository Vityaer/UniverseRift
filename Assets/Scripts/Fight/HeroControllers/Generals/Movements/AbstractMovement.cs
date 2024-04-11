using Cysharp.Threading.Tasks;
using Fight.Grid;
using System;
using System.Collections.Generic;
using UniRx;

namespace Fight.HeroControllers.Generals.Movements
{
    public abstract class AbstractMovement : IMovementable
    {
        private ReactiveCommand<CellDirectionType> _moveDirection = new ReactiveCommand<CellDirectionType>();

        public IObservable<CellDirectionType> MoveDirection => _moveDirection;

        public abstract UniTask Move(HeroController hero, Stack<HexagonCell> cells);
    }
}
