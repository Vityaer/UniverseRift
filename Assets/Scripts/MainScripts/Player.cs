using UnityEngine;
using System;
using Models;
using Models.Common;
using Common;

namespace MainScripts
{
    [Serializable]
    public class Player
    {
        [SerializeField] private GameModel playerGame;
        public PlayerInfoModel GetPlayerInfo { get => playerGame.playerInfo; }
        public GameModel PlayerGame { get => playerGame; }
        public void LevelUP()
        {
            GetPlayerInfo.LevelUP();
            OnLevelUP();
        }

        //Observers
        private Action<BigDigit> observerOnLevelUP;
        public void RegisterOnLevelUP(Action<BigDigit> d) { observerOnLevelUP += d; }
        private void OnLevelUP() { if (observerOnLevelUP != null) observerOnLevelUP(new BigDigit(GetPlayerInfo.Level, 0)); }
    }
}