using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using ObjectSave;
using Models.Requiremets;

[System.Serializable]
public class Game{

	public int maxCountHeroes = 100;

	public CitySaveObject citySaveObject = new CitySaveObject();
	public PlayerSaveObject playerSaveObject = new PlayerSaveObject();
	public PlayerInfo playerInfo = new PlayerInfo();
	public Game(){}
	public void CreateGame(Game game){
		citySaveObject = game.citySaveObject;
		playerSaveObject = game.playerSaveObject;
		playerInfo = game.playerInfo;
	}
//API mines
	public void SaveMine(MineController mineController){ citySaveObject.industry.SaveMine(mineController);}
	public List<MineSave> GetMines{ get => citySaveObject.industry.listMine; }
//API market
	public MallSave mall{get => citySaveObject.mall;}
	public void NewDataAboutSellProduct(TypeMarket typeMarket, int IDproduct, int countSell){
		MarketSave market = mall.markets.Find(x => x.typeMarket == typeMarket);
		if(market == null){
			market = new MarketSave();
			mall.markets.Add(market);
		}
		MarketProductSave product = market.products.Find(x => x.ID == IDproduct);
		if(product == null){
			product = new MarketProductSave(IDproduct, countSell);
			market.products.Add(product);
		}else{
			product.UpdateData(countSell);
		}
		SaveLoadController.SaveGame(this);
	}	
	public List<MarketProductSave> GetProductForMarket(TypeMarket typeMarket){
		List<MarketProductSave> result = new List<MarketProductSave>();
		MarketSave market = mall.markets.Find(x => x.typeMarket == typeMarket);
		if(market != null) result = market.products;
		return result;
	}
//API every time tasks and requrements
	public AllRequirement allRequirement{get => playerSaveObject.allRequirement;}
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
