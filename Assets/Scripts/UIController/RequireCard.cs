using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RequireCard : MonoBehaviour
{
    [SerializeField] private Card card;
    [SerializeField] private TextMeshProUGUI textCountRequirement;

    [Header("Panel select heroes")]
    public OpenClosePanel panelListHeroes;
    public ListCardOnWarTable listCard;
    public RequireCard requireCardInfo;

    private int requireSelectCount = 0;
    private RequirementHero requirementHero;
    private List<InfoHero> selectedHeroes = new List<InfoHero>();

    public void SetData(RequirementHero requirementHero)
    {
        ClearData();
        this.requirementHero = requirementHero;
        requireSelectCount = requirementHero.count;
        card.SetData(requirementHero);
        UpdateUI();
    }

    public void AddHero(Card card)
    {
        if (selectedHeroes.Count < requireSelectCount)
        {
            card.Select();
            selectedHeroes.Add(card.hero);
            UpdateUI();
            requireCardInfo.UpdateUI();
        }
    }

    public void RemoveHero(Card card)
    {
        if (selectedHeroes.Count > 0)
        {
            card.Unselect();
            selectedHeroes.Remove(card.hero);
            UpdateUI();
            requireCardInfo.UpdateUI();
        }
    }

    public void UpdateUI()
    {
        textCountRequirement.text = $"{selectedHeroes.Count}/{requireSelectCount}";
    }

    public bool CheckHeroes()
    {
        bool result = false;
        return result;
    }

    public void OpenListCard()
    {
        listCard.RegisterOnSelect(AddHero);
        listCard.RegisterOnUnSelect(RemoveHero);
        List<InfoHero> currentHeroes = GameController.Instance.GetListHeroes;
        currentHeroes = currentHeroes.FindAll(x => x.CheckСonformity(requirementHero));
        currentHeroes.Remove(TrainCamp.Instance.ReturnSelectHero());
        listCard.SetList(currentHeroes);
        listCard.SelectCards(selectedHeroes);
        panelListHeroes.Open();
        requireCardInfo.ShowData(this.requirementHero, selectedHeroes);
        panelListHeroes.RegisterOnClose(OnClosePanelHeroes);
    }

    void OnClosePanelHeroes()
    {
        panelListHeroes.UnregisterOnClose(OnClosePanelHeroes);
        listCard.UnRegisterOnSelect(AddHero);
        listCard.UnRegisterOnUnSelect(RemoveHero);
    }

    public void ClearData()
    {
        selectedHeroes.Clear();
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void DeleteSelectedHeroes()
    {
        for (int i = 0; i < selectedHeroes.Count; i++)
        {
            GameController.Instance.RemoveHero(selectedHeroes[i]);
        }
        ClearData();
    }

    public void ShowData(RequirementHero requirementHero, List<InfoHero> selectedHeroes)
    {
        this.selectedHeroes = selectedHeroes;
        this.requirementHero = requirementHero;
        requireSelectCount = requirementHero.count;
        card.SetData(requirementHero);
        UpdateUI();
    }

}