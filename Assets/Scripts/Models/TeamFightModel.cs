using System.Collections.Generic;

namespace Models
{
    [System.Serializable]
    public class TeamFightModel : BaseModel
    {
        public List<int> listID = new List<int>() { -1, -1, -1, -1, -1, -1 };
        public int petID = -1;
        public TeamFightModel(List<int> newListID, int newPetId = -1)
        {
            for (int i = 0; i < newListID.Count; i++)
                this.listID[i] = newListID[i];
            this.petID = newPetId;
        }
        public TeamFightModel() { }
        public void ChangeIdHero(InfoHero oldHero, InfoHero newHero)
        {
            int pos = listID.FindIndex(x => x == oldHero.generalInfo.IDCreate);
            if (pos >= 0) listID[pos] = newHero.generalInfo.IDCreate;
        }
        public void AddHero(int pos, InfoHero hero) { listID[pos] = hero.generalInfo.IDCreate; }
        public void RemoveHero(InfoHero hero)
        {
            int pos = listID.FindIndex(x => x == hero.generalInfo.IDCreate);
            if (pos >= 0) listID[pos] = hero.generalInfo.IDCreate;
        }
        public void SetNewPetId(int newId) { petID = newId; }
        public TeamFightModel Clone() { return new TeamFightModel(listID, petID); }
    }
}
