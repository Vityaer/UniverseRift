using Common;
using Common.Resourses;
using Models.City.Mines;
using System;
using System.Collections.Generic;

namespace City.Buildings.Mines
{
    [Serializable]
    public class Mine
    {
        public GameResource income;
        public int level;
        public DateTime previousDateTime;
        private GameResource reward, store;
        public TypeMine type;

        private MineData data = null;
        private GameResource maxStoreResource = null;
        private Action<int> observerLevel;

        public GameResource GetMaxStore
        {
            get
            {
                if (maxStoreResource == null) maxStoreResource = CalculateMaxStoreAmount();
                return maxStoreResource;
            }
        }
        public GameResource GetStore => store;

        private void CalculateReward()
        {
            if (store < maxStoreResource)
            {
                //int tact = CalculateCountTact(previousDateTime, MaxCount: 86400, lenthTact: 1);
                int tact = 10;
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
            //GameController.Instance.SubtractResource(GetCostLevelUp());
            level += 1;
            OnLevelChange();
        }

        public void GetResources()
        {
            if (level > 0)
            {
                CalculateReward();
                //GameController.Instance.AddResource(store);
                store.Clear();
            }
        }

        public List<GameResource> GetCostLevelUp()
        {
            return data.ResourceOnLevelUP.GetCostForLevelUp(level);
        }

        public GameResource CalculateMaxStoreAmount()
        {
            GameResource result = null;
            switch (data.typeStore)
            {
                case TypeStore.Percent:
                    result = income * (data.maxStore / 100f);
                    break;
                case TypeStore.Num:
                    result = new GameResource(income.Type, data.maxStore);
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
            store = new GameResource(mineSave.Store.Type, mineSave.Store.Amount);
            TypeMine typeMine = MinesController.GetTypeMineFromTypeResource(mineSave.Store.Type);
            data = MinesController.Instance.GetDataMineFromType(typeMine);
            //income = data.ResourceOnLevelProduction.GetCostForLevelUp(level).List[0];
            maxStoreResource = CalculateMaxStoreAmount();
            if (income != null) CalculateReward();
        }

        public void RegisterOnObserverLevel(Action<int> d) { observerLevel += d; }
        public void UnregisterOnObserverLevel(Action<int> d) { observerLevel -= d; }
        private void OnLevelChange() { if (observerLevel != null) observerLevel(level); }
    }
}