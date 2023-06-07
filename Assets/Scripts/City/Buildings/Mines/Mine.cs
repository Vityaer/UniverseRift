using Assets.Scripts.City.Buildings.Mines;
using Common;
using Common.Resourses;
using Models.City.Mines;
using System;

namespace City.Buildings.Mines
{
    [Serializable]
    public class Mine
    {
        public Resource income;
        public int level;
        public DateTime previousDateTime;
        private Resource reward, store;
        public TypeMine type;

        private DataAboutMines data = null;
        private Resource maxStoreResource = null;
        private Action<int> observerLevel;

        public Resource GetMaxStore
        {
            get
            {
                if (maxStoreResource == null) maxStoreResource = CalculateMaxStoreAmount();
                return maxStoreResource;
            }
        }
        public Resource GetStore => store;

        private void CalculateReward()
        {
            if (store < maxStoreResource)
            {
                int tact = FunctionHelp.CalculateCountTact(previousDateTime, MaxCount: 86400, lenthTact: 1);
                store.AddResource(income * (tact / 86400) * 100f);
                if (store > maxStoreResource)
                {
                    store = maxStoreResource;
                }
            }
            previousDateTime = DateTime.Now;
        }

        public void LevelUP()
        {
            GameController.Instance.SubtractResource(GetCostLevelUp());
            level += 1;
            OnLevelChange();
        }

        public void GetResources()
        {
            if (level > 0)
            {
                CalculateReward();
                GameController.Instance.AddResource(store);
                store.Clear();
            }
        }

        public ListResource GetCostLevelUp()
        {
            return data.ResourceOnLevelUP.GetCostForLevelUp(level);
        }

        public Resource CalculateMaxStoreAmount()
        {
            Resource result = null;
            switch (data.typeStore)
            {
                case TypeStore.Percent:
                    result = (Resource)(income * (data.maxStore / 100f)).Clone();
                    break;
                case TypeStore.Num:
                    result = new Resource(income.Name, data.maxStore);
                    break;
            }
            return result;
        }

        public void SetData(MineModel mineSave)
        {
            level = mineSave.level;
            OnLevelChange();
            previousDateTime = mineSave.PreviousDateTime;
            type = mineSave.typeMine;
            store = new Resource(mineSave.store.type, mineSave.store.amount);
            TypeMine typeMine = MinesController.GetTypeMineFromTypeResource(mineSave.store.type);
            data = MinesController.Instance.GetDataMineFromType(typeMine);
            income = data.ResourceOnLevelProduction.GetCostForLevelUp(level).List[0];
            maxStoreResource = CalculateMaxStoreAmount();
            if (income != null) CalculateReward();
        }

        public void RegisterOnObserverLevel(Action<int> d) { observerLevel += d; }
        public void UnregisterOnObserverLevel(Action<int> d) { observerLevel -= d; }
        private void OnLevelChange() { if (observerLevel != null) observerLevel(level); }
    }
}