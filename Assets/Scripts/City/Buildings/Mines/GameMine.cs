using Common;
using Common.Resourses;
using Models.City.Mines;
using System;
using System.Collections.Generic;
using Common.Inventories.Resourses;

namespace City.Buildings.Mines
{
    [Serializable]
    public class GameMine
    {
        public GameResource income;
        public int level;
        public DateTime previousDateTime;
        private GameResource reward, store;
        public MineType type;

        private MineModel _model;
        private MineData _data;
        private GameResource maxStoreResource = null;
        private Action<int> observerLevel;

        public GameMine(MineModel model, MineData data)
        {
            _model = model;
            _data = data;
        }

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
            previousDateTime = DateTime.UtcNow;
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

        //public List<GameResource> GetCostLevelUp()
        //{
        //    return _data.ResourceOnLevelUP.GetCostForLevelUp(level);
        //}

        public GameResource CalculateMaxStoreAmount()
        {
            GameResource result = null;
            //switch (_data.typeStore)
            //{
            //    case TypeStore.Percent:
            //        result = income * (_data.maxStore / 100f);
            //        break;
            //    case TypeStore.Num:
            //        result = new GameResource(income.Type, _data.maxStore);
            //        break;
            //}
            return result;
        }

        public void SetData(MineModel mineSave)
        {
            //level = mineSave.Level;
            //OnLevelChange();
            //previousDateTime = mineSave.PreviousDateTime;
            //type = mineSave.Type;
            //store = new GameResource(mineSave.Store.Type, mineSave.Store.Amount);
            //TypeMine typeMine = MinesPageController.GetTypeMineFromTypeResource(mineSave.Store.Type);
            //data = MinesPageController.Instance.GetDataMineFromType(typeMine);
            //income = data.ResourceOnLevelProduction.GetCostForLevelUp(level).List[0];
            //maxStoreResource = CalculateMaxStoreAmount();
            //if (income != null) CalculateReward();
        }

        public void RegisterOnObserverLevel(Action<int> d) { observerLevel += d; }
        public void UnregisterOnObserverLevel(Action<int> d) { observerLevel -= d; }
        private void OnLevelChange() { if (observerLevel != null) observerLevel(level); }
    }
}