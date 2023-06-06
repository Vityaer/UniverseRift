using Cysharp.Threading.Tasks;
using Network.DataServer.Messages;
using Network.DataServer;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Network.DataServer.Models;
using Misc.Json;
using Misc.Json.Impl;
using System.Linq;

public class WheelFortune : Building
{
    [Header("Controller")]
    [SerializeField] private ButtonCostController buttonOneRotate, buttonTenRotate;
    [Header("Images reward")]
    public List<SubjectCellController> places = new List<SubjectCellController>();

    [Header("List reward")]
    private List<FortuneReward> rewards = new List<FortuneReward>();

    public List<FortuneReward<Resource>> resources = new List<FortuneReward<Resource>>();
    public List<FortuneReward<Item>> items = new List<FortuneReward<Item>>();
    public List<FortuneReward<Splinter>> splinters = new List<FortuneReward<Splinter>>();

    [Header("Arrow")]
    public RectTransform arrowRect;
    public float arrowSpeed;
    private float previousTilt = 0f;

    private float generalProbability = 0f;
    [Header("Test")]
    public float testRandom = 0;
    private List<int> numbersReward = new List<int>();
    private float deltaMiss = 15f;
    private Quaternion startRotation;
    private Coroutine coroutineRotate;
    private IJsonConverter _jsonConverter;

    protected override void OpenPage()
    {
        FillWheelFortune();
        CalculateProbability();
    }

    public void PlaySimpleRoulette(int coin = 1)
    {
        coroutineRotate = StartCoroutine(IRotateArrow(GetRandom(coin)));
        OnSimpleRotate(coin);
    }

    private async UniTaskVoid RefreshRewards()
    {
        var message = new FortuneWheelRewards { PlayerId = GameController.GetPlayerInfo.PlayerId };
        var result = await DataServer.PostData(message);
        rewards = _jsonConverter.FromJson<FortuneReward[]>(result).ToList();
    }

    private float GetRandom(int countSpin)
    {
        float result = 0f, rand = 0f;
        numbersReward.Clear();
        int k = 0;
        for (int i = 0; i < countSpin; i++)
        {
            rand = UnityEngine.Random.Range(0f, generalProbability);
            k = 0;
            for (int j = 0; j < rewards.Count; j++)
            {
                result += rewards[j].probability;
                if (result < rand) { k++; } else { break; }
            }
            numbersReward.Add(k);
        }
        return (k * 45f + UnityEngine.Random.Range(-deltaMiss, deltaMiss));
    }
    private void CalculateProbability()
    {
        generalProbability = 0f;
        foreach (FortuneReward reward in rewards)
            generalProbability += reward.probability;
    }
    private void FillWheelFortune()
    {
        for (int i = 0; i < rewards.Count; i++)
        {
            switch (rewards[i])
            {
                case FortuneReward<Resource> product:
                    places[i].SetItem((product as FortuneReward<Resource>).subject);
                    break;
                case FortuneReward<Item> product:
                    places[i].SetItem(product.subject);
                    break;
                case FortuneReward<Splinter> product:
                    places[i].SetItem(product.subject);
                    break;

            }
        }
    }
    IEnumerator IRotateArrow(float targetTilt)
    {
        float startTilt = (((int)(previousTilt / 360)) + 360f);
        targetTilt = startTilt + targetTilt;
        float t = 0;
        Debug.Log("rotate stage 0");
        while (previousTilt > startTilt) previousTilt -= 360f;
        float delta = (startTilt - previousTilt) / 360;
        while (t <= 1)
        {
            arrowRect.rotation = Quaternion.Euler(0, 0, -Mathf.Lerp(previousTilt, startTilt, t));
            if (t < 0.36)
            {
                t += Time.deltaTime * arrowSpeed * (1f / delta) * Mathf.Max(t, 0.1f);
            }
            else
            {
                t += Time.deltaTime * arrowSpeed * (1f / delta);
            }
            yield return null;
        }
        t = 0;
        while (t <= 1)
        {
            arrowRect.rotation = Quaternion.Euler(0, 0, -Mathf.Lerp(startTilt, 360 + startTilt, t));
            t += Time.deltaTime * arrowSpeed;
            yield return null;
        }
        t = 0;
        while (startTilt > targetTilt) startTilt -= 360f;
        delta = (targetTilt - startTilt) / 360;
        while (t <= 1)
        {
            arrowRect.rotation = Quaternion.Euler(0, 0, -Mathf.Lerp(startTilt, targetTilt, t));
            if (t < 0.64)
            {
                t += Time.deltaTime * arrowSpeed * (1f / delta);
            }
            else
            {
                t += Time.deltaTime * arrowSpeed * (1f / delta) * Mathf.Max(1 - t, 0.01f);
            }
            yield return null;
        }
        previousTilt = targetTilt;
        GetReward();
    }
    private void GetReward()
    {
        Reward reward = new Reward();
        for (int i = 0; i < numbersReward.Count; i++)
        {
            switch (rewards[numbersReward[i]])
            {
                case FortuneReward<Resource> res:
                    Resource rewardRes = (rewards[numbersReward[i]] as FortuneReward<Resource>).subject as Resource;
                    reward.AddResource((Resource)rewardRes.Clone());
                    break;
                case FortuneReward<Item> item:
                    Item rewardItem = (rewards[numbersReward[i]] as FortuneReward<Item>).subject as Item;
                    reward.AddItem((Item)rewardItem.Clone());
                    break;
                case FortuneReward<Splinter> splinter:
                    break;
            }
        }
        MessageController.Instance.OpenSimpleRewardPanel(reward);
    }
    private void OneRotate(int count)
    {
        PlaySimpleRoulette(1);
    }
    private void TenRotate(int count)
    {
        PlaySimpleRoulette(10);
    }

    protected override void OnStart()
    {
        _jsonConverter = new JsonConverter();

        buttonOneRotate.RegisterOnBuy(OneRotate);
        buttonTenRotate.RegisterOnBuy(TenRotate);

        for (int i = 0; i < resources.Count; i++) rewards.Add(resources[i]);
        for (int i = 0; i < items.Count; i++) rewards.Add(items[i]);
        for (int i = 0; i < splinters.Count; i++) rewards.Add(splinters[i]);
        FortuneReward x = null;
        for (int i = 0; i < rewards.Count - 1; i++)
        {
            for (int j = i + 1; j < rewards.Count; j++)
            {
                if (rewards[i].posID > rewards[j].posID)
                {
                    x = rewards[i];
                    rewards[i] = rewards[j];
                    rewards[j] = x;
                }
            }
        }
        startRotation = arrowRect.rotation;
    }
    [ContextMenu("StartPosition")]
    public void StartPosition()
    {
        arrowRect.rotation = startRotation;
        if (coroutineRotate != null)
        {
            StopCoroutine(coroutineRotate);
            coroutineRotate = null;
        }

    }
    private static WheelFortune instance;
    public static WheelFortune Instance { get => instance; }
    void Awake()
    {
        instance = this;
    }
    //Observers
    private Action<BigDigit> observerSimpleRotate;
    public void RegisterOnSimpleRotate(Action<BigDigit> d) { observerSimpleRotate += d; }
    private void OnSimpleRotate(int amount) { if (observerSimpleRotate != null) observerSimpleRotate(new BigDigit(amount)); }

}

[System.Serializable]
public class FortuneReward
{
    public float probability;
    public int posID;
}

[System.Serializable]
public class FortuneReward<T> : FortuneReward where T : BaseObject
{
    public T subject;
}