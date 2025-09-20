using System.Collections;
using Cysharp.Threading.Tasks;
using Fight.Common.Grid;

namespace Fight.Common.HeroControllers.Generals.Movements
{
    public interface IMovementable
    {
        UniTask Move(HexagonCell targetCell);
    }
}
