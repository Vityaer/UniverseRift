using Fight.Grid;
using Fight.HeroControllers.Generals;

namespace Fight
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
