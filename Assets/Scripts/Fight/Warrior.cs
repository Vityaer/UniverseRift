using Fight.Common.Grid;
using Fight.Common.HeroControllers.Generals;

namespace Fight.Common
{

    [System.Serializable]
    public class Warrior
    {
        public HeroController heroController = null;
        public HexagonCell Cell { get => heroController.Cell; }
        public Warrior(HeroController heroController)
        {
            this.heroController = heroController;
        }

    }
}
