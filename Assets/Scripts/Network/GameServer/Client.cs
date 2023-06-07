using IdleGame.MultiplayerData;
using Models.City.Arena;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Network.GameServer
{
    public class Client : MonoBehaviour
    {
        public static Client Instance { get; private set; }

        public List<ArenaOpponentModel> listOpponents = new List<ArenaOpponentModel>();

        private void Awake()
        {
            Instance = this;
        }

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
        public void GetListOpponentSimpleArena(List<ArenaOpponentModel> opponents)
        {
            for (int i = 0; i < 3; i++)
            {
                opponents.Add(listOpponents[UnityEngine.Random.Range(0, listOpponents.Count)]);
            }
        }

        public DataCycleEvent GetDataCurrentCycleEvent()
        {
            DataCycleEvent result = new DataCycleEvent();
            result.Stage = 0;
            result.StartTime = DateTime.Today.ToString();
            return result;
        }
    }
}