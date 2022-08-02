using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ObjectSave;
public class MinesScript : Building{
	public List<PlaceForMineScript> minePlaces = new List<PlaceForMineScript>();
	public List<MineControllerScript> buildings = new List<MineControllerScript>();
	[Header("Panels")]
	public PanelInfoMine panelMineInfo;
	public PanelForCreateMine panelNewMineCreate;
	[Header("Data")]
	[SerializeField] private List<DataAboutMines> listDataMines = new List<DataAboutMines>();
	
	public DataAboutMines GetDataMineFromType(TypeMine type){ return listDataMines.Find(x => x.type == type); }
	protected override void Start(){
		PlayerScript.Instance.RegisterOnLoadGame(OnLoadGame);
	}
	List<MineSave> saveMines = new List<MineSave>();
	protected override void OnLoadGame(){
		saveMines = PlayerScript.GetPlayerGame.GetMines;
		PlaceForMineScript place = null;
		MineControllerScript mineController = null;
		DataAboutMines data = null;
		foreach(MineSave mine in saveMines){
			place = minePlaces.Find(x => x.ID == mine.ID);
			if(place != null){
				data = listDataMines.Find(x => x.type == mine.typeMine);
				data.AddMine();
				mineController = Instantiate(data.prefabMine, place.point.position, Quaternion.identity, place.transform).GetComponent<MineControllerScript>();
				place.mineController = mineController;
				mineController.LoadMine(mine);
				buildings.Add(mineController);
			}else{
				Debug.Log(string.Concat("place with ID = ", mine.ID.ToString(), " not found"));
			}		
		}
	}
	public void CreateNewMine(PlaceForMineScript place, DataAboutMines data){
		data.AddMine();
		MineControllerScript mineController = Instantiate(data.prefabMine, place.point.position, Quaternion.identity, place.transform).GetComponent<MineControllerScript>();
		place.mineController = mineController;
		MineSave mine = new MineSave(place.ID, data.type);
		mineController.CreateMine(mine);
	}
	public CostLevelUp GetCostUpdateMine(TypeMine type){ return listDataMines.Find(x => x.type == type).ResourceOnLevelUP; }
	private static MinesScript instance;
	public static MinesScript Instance{get => instance;}
	void Awake(){ if(instance == null) {instance = this;}else{Debug.Log("instance twice");} }

	public static TypeMine GetTypeMineFromTypeResource(TypeResource typeResource){
		TypeMine result = TypeMine.Gold;
		switch(typeResource){
			case TypeResource.Gold:
				result = TypeMine.Gold;
				break;
			case TypeResource.Diamond:
				result = TypeMine.Diamond;
				break;
			case TypeResource.RedDust:
				result = TypeMine.RedDust;
				break;		
		} 
		return result; 
	}
	public static TypeResource GetTypeResourceFromTypeMine(TypeMine typeMine){
		TypeResource result = TypeResource.Gold;
		switch(typeMine){
			case TypeMine.Gold:
				result = TypeResource.Gold;
				break;
			case TypeMine.Diamond:
				result = TypeResource.Diamond;
				break;
			case TypeMine.RedDust:
				result = TypeResource.RedDust;
				break;		
		} 
		return result; 

	}
}
[System.Serializable]
public class DataAboutMines{
	public TypeMine type;
	public int currentCount = 0, maxCount = 2;
	public CostLevelUp ResourceOnLevelProduction, ResourceOnLevelUP;
	public int maxStore = 50;
	public TypeStore typeStore = TypeStore.Percent;
	public ListResource costCreate;
	public GameObject prefabMine;
	public Sprite image{get => prefabMine.GetComponent<SpriteRenderer>().sprite;}
	public void AddMine(){
		currentCount += 1;
	}
} 