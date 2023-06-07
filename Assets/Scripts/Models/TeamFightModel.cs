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
        public void ChangeIdHero(HeroModel oldHero, HeroModel newHero)
        {
            int pos = listID.FindIndex(x => x == oldHero.General.IDCreate);
            if (pos >= 0) listID[pos] = newHero.General.IDCreate;
        }
        public void AddHero(int pos, HeroModel hero) { listID[pos] = hero.General.IDCreate; }
        public void RemoveHero(HeroModel hero)
        {
            int pos = listID.FindIndex(x => x == hero.General.IDCreate);
            if (pos >= 0) listID[pos] = hero.General.IDCreate;
        }
        public void SetNewPetId(int newId) { petID = newId; }
        public TeamFightModel Clone() { return new TeamFightModel(listID, petID); }
    }
}
