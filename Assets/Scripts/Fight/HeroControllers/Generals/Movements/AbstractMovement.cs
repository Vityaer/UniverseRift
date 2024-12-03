using Cysharp.Threading.Tasks;
using Fight.Grid;
using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace Fight.HeroControllers.Generals.Movements
{
    public abstract class AbstractMovement : MonoBehaviour, IMovementable
    {
        private ReactiveCommand<CellDirectionType> _moveDirection = new ReactiveCommand<CellDirectionType>();

        public IObservable<CellDirectionType> MoveDirection => _moveDirection;

        public abstract IEnumerator Move(HeroController hero, Stack<HexagonCell> cells);
    }
}
