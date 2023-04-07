using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using ObjectSave;
using TMPro;
using UnityEngine.UI;
using System;

public class HeroPanel : Building
{
    [Header("Controller")]
	public Button btnToLeft;
	public Button btnToRight;
	public Button btnLevelUP;

    [Header("Information")]
	public Image imageHero;
	public TextMeshProUGUI textLevel;
	public TextMeshProUGUI textNameHero;
	public TextMeshProUGUI textHP;
	public TextMeshProUGUI textAttack;
	public TextMeshProUGUI textArmor;
	public TextMeshProUGUI textInitiative;
	public TextMeshProUGUI textStrengthHero;

	[Header("Items")]
	public List<CellItemHeroScript> CellsForItem = new List<CellItemHeroScript>(); 
	[Header("Skills")]
	public SkillUIControllerScript skillController;
	[Header("Costs")]
	public CostUIListScript costController;
	public CostLevelUp costLevelObject;

	[Header("Details")]
	[SerializeField] private HeroDetailsPanel _heroDetailsPanel;
	public Button btnOpenHeroDetails;
	public Button btnCloseHeroDetails;
	
    public Action LeftButtonClick;
    public Action RightButtonClick;

    private InfoHero _hero;

    protected override void OnStart()
    {
        btnToLeft.onClick.AddListener(() => LeftButtonClick());
        btnToRight.onClick.AddListener(() => RightButtonClick());
        btnLevelUP.onClick.AddListener(() => LevelUp());
        btnOpenHeroDetails.onClick.AddListener(() => _heroDetailsPanel.Open());
        btnCloseHeroDetails.onClick.AddListener(() => _heroDetailsPanel.Close());
    }

    public void ShowHero(InfoHero hero)
    {
        _hero = hero;
		UpdateInfoAbountHero();
    }

    public void UpdateInfoAbountHero()
	{
		imageHero.sprite    = _hero.generalInfo.ImageHero;
		textNameHero.text   = _hero.generalInfo.Name;
		UpdateTextAboutHero();
		foreach(CellItemHeroScript cell in CellsForItem)
        {
			cell.Clear();
			cell.SetItem(_hero.CostumeHero.GetItem(cell.typeCell));
		}
		CheckResourceForLevelUP();
	} 

    public void UpdateTextAboutHero()
    {
		textLevel.text      =  _hero.generalInfo.Level.ToString();
		textHP.text         = ((int) _hero.GetCharacteristic(TypeCharacteristic.HP)        ).ToString();
		textAttack.text     = ((int) _hero.GetCharacteristic(TypeCharacteristic.Damage)    ).ToString();
		textArmor.text      = ((int) _hero.GetCharacteristic(TypeCharacteristic.Defense)   ).ToString();
		textInitiative.text = ((int) _hero.GetCharacteristic(TypeCharacteristic.Initiative)).ToString();
		textStrengthHero.text  = _hero.GetStrength.ToString(); 
		_hero.PrepareSkillLocalization();
		skillController.ShowSkills(_hero.skills);
		costController.ShowCosts( costLevelObject.GetCostForLevelUp(_hero.generalInfo.Level) );
		// _heroDetailsPanel.ShowDetails(_hero);
	}

	private void CheckResourceForLevelUP()
    {
		btnLevelUP.interactable = PlayerScript.Instance.CheckResource( costLevelObject.GetCostForLevelUp(_hero.generalInfo.Level) );
	}

    public void TakeOff(Item item)
    {
        _hero.CostumeHero.TakeOff(item);
		UpdateTextAboutHero();
    }

    public void LevelUp()
    {
		PlayerScript.Instance.SubtractResource( costLevelObject.GetCostForLevelUp(_hero.generalInfo.Level) );
        _hero.LevelUP();
		UpdateInfoAbountHero();
    }
}