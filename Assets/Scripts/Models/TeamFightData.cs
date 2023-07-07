using Models.Data;
using Models.Heroes;
using System.Collections.Generic;

namespace Models
{
    [System.Serializable]
    public class TeamFightData : BaseDataModel
    {
        public List<int> listID = new List<int>() { -1, -1, -1, -1, -1, -1 };
        public int petID = -1;
        public TeamFightData(List<int> newListID, int newPetId = -1)
        {
            for (int i = 0; i < newListID.Count; i++)
                this.listID[i] = newListID[i];
            this.petID = newPetId;
        }
        public TeamFightData() { }
        public void ChangeIdHero(HeroModel oldHero, HeroModel newHero)
        {
            //int pos = listID.FindIndex(x => x == oldHero.General.IDCreate);
            //if (pos >= 0) listID[pos] = newHero.General.IDCreate;
        }
        //public void AddHero(int pos, HeroModel hero) { listID[pos] = hero.General.IDCreate; }
        public void RemoveHero(HeroModel hero)
        {
            //int pos = listID.FindIndex(x => x == hero.General.IDCreate);
            //if (pos >= 0) listID[pos] = hero.General.IDCreate;
        }
        public void SetNewPetId(int newId) { petID = newId; }
        public TeamFightData Clone() { return new TeamFightData(listID, petID); }
    }
}
