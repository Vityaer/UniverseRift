using System;
using System.Collections.Generic;

namespace Models
{
    [Serializable]
    public class IndustryModel : BaseModel
    {
        public List<MineBuildModel> listAdminMine = new List<MineBuildModel>();
        public List<MineModel> listMine = new List<MineModel>();
        public void SaveMine(MineController mineController)
        {
            MineModel mineSave = listMine.Find(x => x.Id == mineController.ID);
            if (mineSave != null)
            {
                mineSave.ChangeInfo(mineController);
            }
            else
            {
                listMine.Add(new MineModel(mineController));
            }
        }
    }
}
