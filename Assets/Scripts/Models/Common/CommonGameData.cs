using City.Achievements;
using City.Buildings.Mines;
using Models.City.Markets;
using Models.City.Mines;
using Models.Data;
using Models.Data.Heroes;
using Models.Data.Players;
using Newtonsoft.Json;
using Sirenix.Utilities;
using System.Collections.Generic;
using UnityEngine;
using Utils;

namespace Models.Common
{
    [System.Serializable]
    public class CommonGameData : BaseDataModel
    {
        public int MaxHeroesCount = 100;

        public CityData City = new CityData();
        public PlayerData Player = new PlayerData();
        public CycleEventsData CycleEventsData = new CycleEventsData();
        public HeroesStorage HeroesStorage = new HeroesStorage();
        public bool IsInited { get; private set; } = false;

        public CommonGameData() { }

        public void Init()
        {
            Player = GetData<PlayerData>();
            HeroesStorage = GetData<HeroesStorage>();
            City = GetData<CityData>();
            CycleEventsData = GetData<CycleEventsData>();
            IsInited = true;
            Debug.Log("data loaded");
        }

        private T GetData<T>() where T : new()
        {
            var jsonData = TextUtils.GetDataFromLocalStorage<T>();

            if (jsonData.IsNullOrWhitespace())
                return new T();

            return JsonConvert.DeserializeObject<T>(jsonData, Constants.Common.SerializerSettings);
        }

        public void Save()
        {
            TextUtils.SaveGameData(Player);
            TextUtils.SaveGameData(HeroesStorage);
            TextUtils.SaveGameData(City);
            TextUtils.SaveGameData(CycleEventsData);

        }

        public void CreateGame(CommonGameData game)
        {
            City = game.City;
            Player = game.Player;
        }

        //API mines
        public void SaveMine(MineController mineController) { City.IndustrySave.SaveMine(mineController); }
        public List<MineModel> GetMines { get => City.IndustrySave.listMine; }
        //API market
        public ShopModel mall { get => City.MallSave; }

        public void NewDataAboutSellProduct(MarketType typeMarket, string IDproduct, int countSell)
        {
            MarketModel market = mall.markets.Find(market => market.Type == typeMarket);
            if (market == null)
            {
                market = new MarketModel();
                mall.markets.Add(market);
            }
            ProductModel product = market.Products.Find(x => x.Id == IDproduct);
            if (product == null)
            {
                product = new ProductModel(IDproduct, countSell);
                market.Products.Add(product);
            }
            else
            {
                product.UpdateData(countSell);
            }
        }

        public List<ProductModel> GetProductForMarket(MarketType typeMarket)
        {
            List<ProductModel> result = new List<ProductModel>();
            MarketModel market = mall.markets.Find(market => market.Type == typeMarket);
            if (market != null) result = market.Products;
            return result;
        }
        //API every time tasks and requrements
        public void SaveAchievments(List<GameAchievment> mainRequirements)
        {
            //GeneralSaveAchievments(allRequirement.MainRequirements);
        }

        public void SaveEveryTimeTask(List<GameAchievment> everyTimeTasks)
        {
            //GeneralSaveAchievments(allRequirement.EveryTimeTasks);
        }

        public void GeneralSaveAchievments(List<AchievmentData> listSave)
        {
            //foreach (var task in listSave)
            //{
            //    var currentSave = listSave.Find(x => x.Id == task.Id);
            //    if (currentSave != null)
            //    {
            //        currentSave.ChangeData(task);
            //    }
            //    else
            //    {
            //        listSave.Add(new AchievmentData(task));
            //    }
            //}
            Debug.Log(listSave.Count);
        }



        //public List<AchievmentData> saveMainRequirements { get => allRequirement.MainRequirements; }
        //public List<AchievmentData> saveEveryTimeTasks { get => allRequirement.EveryTimeTasks; }
    }
}