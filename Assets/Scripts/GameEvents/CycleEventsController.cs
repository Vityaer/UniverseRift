using Common;
using Db.CommonDictionaries;
using HelpFuction;
using IdleGame.MultiplayerData;
using Models;
using Models.Common;
using Network.GameServer;
using System;
using System.Collections.Generic;
using UnityEngine;
using VContainer;
using VContainerUi.Abstraction;

namespace GameEvents
{
    public class CycleEventsController : UiController<CycleEventsView>
    {
        private const string CURRENT_CYCLE_STAGE = "CycleStage";
        private const string START_DATETIME_CYCLE = "DateTimeCycle";

        [Inject] private readonly CommonGameData _gameData;
        [Inject] private readonly CommonDictionaries _commonDictionaries;

        private int cycleTimeDay = 7;
        private DateTime dateMonthlyEvent, dateLastChangeCycle;
        private CycleEventsData сycleEventsSave = null;

        private StageCycleEvent currentStageCycle;
        private MainEventController currentEventController = null;
        private DateTime startTime;
        private TimeSpan requireTime;
        private GameTimer timerForNextStageCycle = null;

        void Start()
        {
            //GameController.Instance.RegisterOnLoadGame(OnLoadGame);
            // TimeControllerSystem.Instance.RegisterOnNewMonth(UpdateMonthlyEvent);
            // TimeControllerSystem.Instance.RegisterOnNewWeek(UpdateMonthlyEvent);
        }

        private void OnLoadGame()
        {
            //сycleEventsSave = _gameData.CycleEventsModel;
            //if (сycleEventsSave.CheckRecordDate(START_DATETIME_CYCLE))
            //{
            //    currentStageCycle = (StageCycleEvent)сycleEventsSave.GetRecordInt(CURRENT_CYCLE_STAGE);
            //    startTime = сycleEventsSave.GetRecordDate(START_DATETIME_CYCLE);
            //}
            //else
            //{
            //    DataCycleEvent data = Client.Instance.GetDataCurrentCycleEvent();
            //    currentStageCycle = (StageCycleEvent)data.Stage;
            //    startTime = FunctionHelp.StringToDateTime(data.StartTime);
            //}

            //OpenCurrentCycleStage();
            //timerForNextStageCycle =
            //    TimerScript.Timer.StartTimer(FunctionHelp.GetLeftSecondsToEnd(startTime, requireTime), NextCycleStage);
        }

        [ContextMenu("NextCycleStage")]
        private void NextCycleStage()
        {
            int current = (int)currentStageCycle;
            current = current < View.EventPanels.Count - 1 ? current + 1 : 0;
            currentStageCycle = (StageCycleEvent)current;
            //сycleEventsSave.SetRecordDate(START_DATETIME_CYCLE, DateTime.Today);
            //сycleEventsSave.SetRecordInt(CURRENT_CYCLE_STAGE, (int)currentStageCycle);
            OpenCurrentCycleStage();
            timerForNextStageCycle =
                TimerScript.Timer.StartTimer(FunctionHelp.GetLeftSecondsToEnd(startTime, requireTime), NextCycleStage);
        }

        private void OpenCurrentCycleStage()
        {
            requireTime = new TimeSpan(cycleTimeDay, 0, 0, 0);
            currentEventController = View.EventPanels.Find(x => x.StageCycleEvent == currentStageCycle);
            currentEventController.Open(startTime, requireTime);
        }
    }
}