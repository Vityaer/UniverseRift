using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using ObjectSave;
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
	public void SaveMine(MineControllerScript mineController){ citySaveObject.industry.SaveMine(mineController);}
	public List<MineSave> GetMines{ get => citySaveObject.industry.listMine; }
//API timeManagement
	public TimeManagement timeManagement{get => citySaveObject.timeManagement;}
	public void NewTimeReward(TypeDateRecord type, DateTime date){
		DateRecord record = timeManagement.records.Find(x => x.type == type);
		if(record != null){
			record.Date = date;
		}else{
			record = new DateRecord(type, date);
			timeManagement.records.Add(record);
		}
		SaveLoadControllerScript.SaveGame(this);
	}
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
		SaveLoadControllerScript.SaveGame(this);
	}	
	public List<MarketProductSave> GetProductForMarket(TypeMarket typeMarket){
		List<MarketProductSave> result = new List<MarketProductSave>();
		MarketSave market = mall.markets.Find(x => x.typeMarket == typeMarket);
		if(market != null) result = market.products;
		return result;
	}
//API every time tasks and requrements
	public AllRequirement allRequirement{get => playerSaveObject.allRequirement;}
	public void SaveMainRequirements(List<Requirement> mainRequirements){SaveRequirement(allRequirement.saveMainRequirements, mainRequirements);}
	public void SaveEveryTimeTask(List<Requirement> everyTimeTasks){ SaveRequirement(allRequirement.saveEveryTimeTasks, everyTimeTasks); }
	private void SaveRequirement(List<RequirementSave> listSave, List<Requirement> requirements){
		RequirementSave currentSave = null; 
		foreach(Requirement task in requirements){
			currentSave = listSave.Find(x => x.ID == task.ID);
			if(currentSave != null){
				currentSave.ChangeData(task);
			}else{
				listSave.Add(new RequirementSave(task));
			}
		}
		Debug.Log(listSave.Count);
	}
	public List<RequirementSave> saveMainRequirements{get => allRequirement.saveMainRequirements;}
	public List<RequirementSave> saveEveryTimeTasks{get => allRequirement.saveEveryTimeTasks;}
	public ListResource StoreResources{get => playerSaveObject.storeResources;}
}
