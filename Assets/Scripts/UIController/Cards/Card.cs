using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Card : MonoBehaviour
{
	public InfoHero hero;
	[SerializeField] private Image _imageUI;
	[SerializeField] private TextMeshProUGUI _levelUI;
	[SerializeField] private Image _panelSelect;
    [SerializeField] private VocationView _vocationUI;
	[SerializeField] private RaceView _raceUI;
	public RatingHero _ratingController;
	private ListCardOnWarTable listCardController;
	public bool Selected = false;

    public void SetData(RequirementHero requirementHero)
    {
        gameObject.SetActive(true);
        _levelUI.text = string.Empty;
        _ratingController.ShowRating(requirementHero.rating);
        // vocationUI.SetData(requirementHero.);
        // raceUI.SetData(requirementHero.);
        SetImage(requirementHero.GetData);
    }

    public void ChangeInfo(InfoHero hero)
    {
        this.hero = hero;
        UpdateUI();
    }

    public void ChangeInfo(InfoHero hero, ListCardOnWarTable listCardController)
    {
        this.hero = hero;
        this.listCardController = listCardController;
        UpdateUI();
    }

    private void UpdateUI()
	{
		_imageUI.sprite       = hero.generalInfo.ImageHero;
		_levelUI.text         = hero.generalInfo.Level.ToString();
		_ratingController.ShowRating(hero.generalInfo.RatingHero); 

	}

	private void SetImage(InfoHero data)
	{
		_imageUI.sprite = data.generalInfo.ImageHero;
	}

//API
	public void ClickOnCard()
	{
		if(Selected == false)
		{
			listCardController.SelectCard(this);
		}
		else
		{
			listCardController.UnselectCard(this);
		}
	}

	public void Select()
	{
		Selected = true;
		_panelSelect.enabled = true;
	}

	public void Unselect()
	{
		Selected = false;
		_panelSelect.enabled = false;
	} 

	public void Clear()
	{
		_imageUI.sprite = null;
		_levelUI.text = string.Empty;
		_ratingController.Hide();
		gameObject.SetActive(false);
	}

	public void DestroyCard()
	{
		listCardController.RemoveCardFromList(this);
		Destroy(gameObject);
	}
}
