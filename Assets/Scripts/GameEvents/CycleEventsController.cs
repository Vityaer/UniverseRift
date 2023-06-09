using Common;
using HelpFuction;
using IdleGame.MultiplayerData;
using Models;
using Network.GameServer;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameEvents
{
    public class CycleEventsController : MonoBehaviour
    {
        private const string CURRENT_CYCLE_STAGE = "CycleStage";
        private const string START_DATETIME_CYCLE = "DateTimeCycle";
        public List<MainEventController> listEvents = new List<MainEventController>();
        public int cycleTimeDay = 7;
        private DateTime dateMonthlyEvent, dateLastChangeCycle;
        private CycleEventsModel сycleEventsSave = null;

        private StageCycleEvent currentStageCycle;
        private MainEventController currentEventController = null;
        private DateTime startTime;
        private TimeSpan requireTime;

        void Start()
        {
            GameController.Instance.RegisterOnLoadGame(OnLoadGame);
            // TimeControllerSystem.Instance.RegisterOnNewMonth(UpdateMonthlyEvent);
            // TimeControllerSystem.Instance.RegisterOnNewWeek(UpdateMonthlyEvent);
        }

        GameTimer timerForNextStageCycle = null;

        private void OnLoadGame()
        {
            сycleEventsSave = GameController.GetCitySave.cycleEvents;
            if (сycleEventsSave.CheckRecordDate(START_DATETIME_CYCLE))
            {
                currentStageCycle = (StageCycleEvent)сycleEventsSave.GetRecordInt(CURRENT_CYCLE_STAGE);
                startTime = сycleEventsSave.GetRecordDate(START_DATETIME_CYCLE);
            }
            else
            {
                DataCycleEvent data = Client.Instance.GetDataCurrentCycleEvent();
                currentStageCycle = (StageCycleEvent)data.Stage;
                startTime = FunctionHelp.StringToDateTime(data.StartTime);
            }

            OpenCurrentCycleStage();
            timerForNextStageCycle =
                TimerScript.Timer.StartTimer(FunctionHelp.GetLeftSecondsToEnd(startTime, requireTime), NextCycleStage);
        }

        [ContextMenu("NextCycleStage")]
        private void NextCycleStage()
        {
            int current = (int)currentStageCycle;
            current = current < listEvents.Count - 1 ? current + 1 : 0;
            currentStageCycle = (StageCycleEvent)current;
            сycleEventsSave.SetRecordDate(START_DATETIME_CYCLE, DateTime.Today);
            сycleEventsSave.SetRecordInt(CURRENT_CYCLE_STAGE, (int)currentStageCycle);
            OpenCurrentCycleStage();
            timerForNextStageCycle =
                TimerScript.Timer.StartTimer(FunctionHelp.GetLeftSecondsToEnd(startTime, requireTime), NextCycleStage);
        }

        private void OpenCurrentCycleStage()
        {
            requireTime = new TimeSpan(cycleTimeDay, 0, 0, 0);
            currentEventController = listEvents.Find(x => x.StageCycleEvent == currentStageCycle);
            currentEventController.Open(startTime, requireTime);
        }
    }
}