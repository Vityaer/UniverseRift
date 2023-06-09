using City.Acievements;
using City.Buildings.Mines;
using Common.Resourses;
using MainScripts;
using Models.City.Markets;
using Models.City.Mines;
using Models.Requiremets;
using System.Collections.Generic;
using UnityEngine;

namespace Models.Common
{
    [System.Serializable]
    public class GameModel
    {
        public int maxCountHeroes = 100;

        public CityModel citySaveObject = new CityModel();
        public PlayerModel playerSaveObject = new PlayerModel();
        public PlayerInfoModel playerInfo = new PlayerInfoModel();
        public GameModel() { }
        public void CreateGame(GameModel game)
        {
            citySaveObject = game.citySaveObject;
            playerSaveObject = game.playerSaveObject;
            playerInfo = game.playerInfo;
        }
        //API mines
        public void SaveMine(MineController mineController) { citySaveObject.industry.SaveMine(mineController); }
        public List<MineModel> GetMines { get => citySaveObject.industry.listMine; }
        //API market
        public ShopModel mall { get => citySaveObject.mall; }
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
            SaveLoadController.SaveGame(this);
        }
        public List<ProductModel> GetProductForMarket(MarketType typeMarket)
        {
            List<ProductModel> result = new List<ProductModel>();
            MarketModel market = mall.markets.Find(market => market.Type == typeMarket);
            if (market != null) result = market.Products;
            return result;
        }
        //API every time tasks and requrements
        public RequirementStorageModel allRequirement { get => playerSaveObject.allRequirement; }
        public void SaveMainRequirements(List<Achievement> mainRequirements) { SaveRequirement(allRequirement.saveMainRequirements, mainRequirements); }
        public void SaveEveryTimeTask(List<Achievement> everyTimeTasks) { SaveRequirement(allRequirement.saveEveryTimeTasks, everyTimeTasks); }
        public void SaveRequirement(List<AchievementSave> listSave, List<Achievement> requirements)
        {
            AchievementSave currentSave = null;
            foreach (Achievement task in requirements)
            {
                currentSave = listSave.Find(x => x.ID == task.ID);
                if (currentSave != null)
                {
                    currentSave.ChangeData(task);
                }
                else
                {
                    listSave.Add(new AchievementSave(task));
                }
            }
            Debug.Log(listSave.Count);
        }
        public List<AchievementSave> saveMainRequirements { get => allRequirement.saveMainRequirements; }
        public List<AchievementSave> saveEveryTimeTasks { get => allRequirement.saveEveryTimeTasks; }
        public ListResource StoreResources { get => playerSaveObject.storeResources; }
    }
}