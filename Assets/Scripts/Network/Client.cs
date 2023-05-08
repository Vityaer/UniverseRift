using IdleGame.MultiplayerData;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Client : MonoBehaviour
{
    public DateTime GetServerTime()
    {
        return DateTime.Now;
    }
    // public DateTime GetCurrentDay(){
    // 	return currentDay;
    // }
    // public DateTime GetCurrentWeek(){

    // }
    // public DateTime GetCurrentMonth(){

    // }
    public void GetListOpponentSimpleArena(List<ArenaOpponent> opponents)
    {
        for (int i = 0; i < 3; i++)
        {
            opponents.Add(listOpponents[UnityEngine.Random.Range(0, listOpponents.Count)]);
        }
    }
    public DataCycleEvent GetDataCurrentCycleEvent()
    {
        DataCycleEvent result = new DataCycleEvent();
        result.stage = 0;
        result.startTime = (DateTime.Today).ToString();
        return result;
    }
    void Awake() { instance = this; }
    private static Client instance;
    public static Client Instance { get => instance; }
    [Header("Data")]
    public List<ArenaOpponent> listOpponents = new List<ArenaOpponent>();
}
