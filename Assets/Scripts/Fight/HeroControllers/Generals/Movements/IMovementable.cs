using Fight.Grid;
using System.Collections;

namespace Fight.HeroControllers.Generals.Movements
{
    public interface IMovementable
    {
        IEnumerator Move(HexagonCell targetCell);
    }
}
