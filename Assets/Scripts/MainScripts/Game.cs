using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Models;
using Models.Requiremets;
using City.Buildings.Market;

[System.Serializable]
public class Game{

	public int maxCountHeroes = 100;

	public CityModel citySaveObject = new CityModel();
	public PlayerModel playerSaveObject = new PlayerModel();
	public PlayerInfoModel playerInfo = new PlayerInfoModel();
	public Game(){}
	public void CreateGame(Game game){
		citySaveObject = game.citySaveObject;
		playerSaveObject = game.playerSaveObject;
		playerInfo = game.playerInfo;
	}
//API mines
	public void SaveMine(MineController mineController){ citySaveObject.industry.SaveMine(mineController);}
	public List<MineModel> GetMines{ get => citySaveObject.industry.listMine; }
//API market
	public ShopModel mall{get => citySaveObject.mall;}
	public void NewDataAboutSellProduct(TypeMarket typeMarket, string IDproduct, int countSell){
		MarketModel market = mall.markets.Find(x => x.typeMarket == typeMarket);
		if(market == null){
			market = new MarketModel();
			mall.markets.Add(market);
		}
		MarketProductModel product = market.products.Find(x => x.Id == IDproduct);
		if(product == null){
			product = new MarketProductModel(IDproduct, countSell);
			market.products.Add(product);
		}else{
			product.UpdateData(countSell);
		}
		SaveLoadController.SaveGame(this);
	}	
	public List<MarketProductModel> GetProductForMarket(TypeMarket typeMarket){
		List<MarketProductModel> result = new List<MarketProductModel>();
		MarketModel market = mall.markets.Find(x => x.typeMarket == typeMarket);
		if(market != null) result = market.products;
		return result;
	}
//API every time tasks and requrements
	public RequirementStorageModel allRequirement{get => playerSaveObject.allRequirement;}
	public void SaveMainRequirements(List<Achievement> mainRequirements){SaveRequirement(allRequirement.saveMainRequirements, mainRequirements);}
	public void SaveEveryTimeTask(List<Achievement> everyTimeTasks){ SaveRequirement(allRequirement.saveEveryTimeTasks, everyTimeTasks); }
	public void SaveRequirement(List<AchievementSave> listSave, List<Achievement> requirements){
		AchievementSave currentSave = null; 
		foreach(Achievement task in requirements){
			currentSave = listSave.Find(x => x.ID == task.ID);
			if(currentSave != null){
				currentSave.ChangeData(task);
			}else{
				listSave.Add(new AchievementSave(task));
			}
		}
		Debug.Log(listSave.Count);
	}
	public List<AchievementSave> saveMainRequirements{get => allRequirement.saveMainRequirements;}
	public List<AchievementSave> saveEveryTimeTasks{get => allRequirement.saveEveryTimeTasks;}
	public ListResource StoreResources{get => playerSaveObject.storeResources;}
}
