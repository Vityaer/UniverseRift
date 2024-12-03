using Fight.Grid;
using System.Collections.Generic;
using System.Collections;

namespace Fight.HeroControllers.Generals.Movements
{
    public interface IMovementable
    {
        IEnumerator Move(HeroController hero, Stack<HexagonCell> cells);
    }
}
