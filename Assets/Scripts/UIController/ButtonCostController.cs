﻿using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ButtonCostController : MonoBehaviour
{
    public TextMeshProUGUI textCost;
    public Button btn;
    public Image imgRes;
    public Resource res;
    public Action<int> delBuyMatter;
    public int countBuy = 1;
    private bool disable = false;
    public TypeDefaultMessage typeDefaultMessage = TypeDefaultMessage.Word;

    void Start()
    {
        if (res.Count > 0)
        {
            UpdateInfo();
        }
    }

    public void UpdateCost(Resource res, Action<int> d)
    {
        delBuyMatter = d;
        this.res = res;
        UpdateInfo();
    }

    public void RegisterOnBuy(Action<int> d)
    {
        delBuyMatter = d;
        UpdateInfo();
    }

    public void UpdateCostWithoutInfo(Resource res, Action<int> d)
    {
        delBuyMatter = d;
        this.res = res;
        CheckResource(res);
    }

    public void UpdateLabel(Action<int> d, string text)
    {
        delBuyMatter = d;
        this.res.Clear();
        textCost.text = text;
        disable = false;
        btn.interactable = true;
    }

    private void UpdateInfo()
    {
        if (disable == false)
        {
            if (res.Count > 0)
            {
                textCost.text = res.ToString();
                imgRes.enabled = true;
                imgRes.sprite = res.Image;
                GameController.Instance.RegisterOnChangeResource(CheckResource, res.Name);
            }
            else
            {
                textCost.text = DefaultEmpty();
                if (typeDefaultMessage != TypeDefaultMessage.Number)
                    imgRes.enabled = false;
            }
            CheckResource(res);
        }
    }

    public void Buy()
    {
        if (disable == false && GameController.Instance.CheckResource(this.res))
        {
            Debug.Log("button buy");
            if (res.Count > 0f) SubstractResource();
            if (delBuyMatter != null) delBuyMatter(countBuy);
        }
    }

    public void CheckResource(Resource res)
    {
        if (disable == false)
            btn.interactable = GameController.Instance.CheckResource(this.res);
    }

    public void Disable()
    {
        disable = true;
        GameController.Instance.UnregisterOnChangeResource(CheckResource, res.Name);
        btn.interactable = false;
    }

    public void EnableButton()
    {
        disable = false;
        GameController.Instance.RegisterOnChangeResource(CheckResource, res.Name);
    }

    private void SubstractResource()
    {
        GameController.Instance.SubtractResource(res);
    }

    public void Clear()
    {
        delBuyMatter = null;
        btn.interactable = true;
        imgRes.enabled = false;
        disable = false;
    }

    private string DefaultEmpty()
    {
        string result = string.Empty;
        switch (typeDefaultMessage)
        {
            case TypeDefaultMessage.Emtpy:
                result = string.Empty;
                break;
            case TypeDefaultMessage.Number:
                result = "0";
                break;
            case TypeDefaultMessage.Word:
                result = "Бесплатно";
                break;
        }
        return result;
    }
    public enum TypeDefaultMessage
    {
        Emtpy,
        Number,
        Word
    }
}
