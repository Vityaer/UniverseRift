using Fight.Grid;
using System.Collections;
using Cysharp.Threading.Tasks;

namespace Fight.HeroControllers.Generals.Movements
{
    public interface IMovementable
    {
        UniTask Move(HexagonCell targetCell);
    }
}
