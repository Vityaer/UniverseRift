using Cysharp.Threading.Tasks;
using Fight.Grid;
using System.Collections.Generic;
using UnityEngine;

namespace Fight.HeroControllers.Generals.Movements
{
    public interface IMovementable
    {
        UniTask Move(HeroController hero, Stack<HexagonCell> cells);
    }
}
