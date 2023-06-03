using System;

namespace Models
{
    [Serializable]
    public class MineBuildModel : BaseModel
    {
        public TypeMine typeMine;
        public int level;
        public MineBuildModel(MineController mineController)
        {
            this.Id = mineController.ID;
            this.typeMine = mineController.GetMine.type;
            this.level = mineController.GetMine.level;
        }
        public MineBuildModel(string ID, TypeMine typeMine)
        {
            this.Id = ID;
            this.typeMine = typeMine;
            this.level = 1;
        }
    }
}
