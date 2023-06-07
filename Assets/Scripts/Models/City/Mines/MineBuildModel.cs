using City.Buildings.Mines;
using System;

namespace Models.City.Mines
{
    [Serializable]
    public class MineBuildModel : BaseModel
    {
        public TypeMine typeMine;
        public int level;

        public MineBuildModel() { }

        public MineBuildModel(MineController mineController)
        {
            Id = mineController.ID;
            typeMine = mineController.GetMine.type;
            level = mineController.GetMine.level;
        }

        public MineBuildModel(string ID, TypeMine typeMine)
        {
            Id = ID;
            this.typeMine = typeMine;
            level = 1;
        }
    }
}
