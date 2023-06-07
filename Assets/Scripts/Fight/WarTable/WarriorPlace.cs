using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;
public class WarriorPlace : MonoBehaviour
{
    public int ID;
    public Card card;
    private HeroModel hero;
    public WarTableController WarTable;
    [SerializeField] private Image ImageHero;
    [SerializeField] private TextMeshProUGUI textLevel;
    public HeroModel Hero => hero;
    void Start()
    {
        WarTable = WarTableController.Instance;
    }
    public void SetHero(Card card, HeroModel hero)
    {
        if (card != null) this.card = card;
        this.hero = hero;
        card.Select();
        UpdateUI();
    }
    public void OnClickPlace() { if (card != null) WarTable.UnSelectCard(card); }
    public void ClearPlace()
    {
        if (card != null)
        {
            card.Unselect();
            card = null;
        }
        hero = null;
        ClearUI();
    }
    public void UpdateUI()
    {
        ImageHero.sprite = hero?.General.ImageHero;
        ImageHero.enabled = true;
        textLevel.text = hero.General.Level.ToString();
    }
    private void ClearUI()
    {
        ImageHero.enabled = false;
        ImageHero.sprite = null;
        textLevel.text = string.Empty;
    }
    public void SetEnemy(MissionEnemy enemy)
    {
        hero = enemy.enemyPrefab;
        hero.PrepareHeroWithLevel(enemy.level);
        UpdateUI();
    }
    public bool IsEmpty() => (card == null);
}
