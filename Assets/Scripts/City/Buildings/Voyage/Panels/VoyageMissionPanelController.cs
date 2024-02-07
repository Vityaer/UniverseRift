using City.Buildings.Voyage.Panels;
using Common.Rewards;
using Models.Fights.Campaign;
using System;
using UiExtensions.Scroll.Interfaces;
using UniRx;
using UnityEngine;
using VContainerUi.Messages;
using VContainerUi.Model;

namespace City.Buildings.Voyage
{
    public class VoyageMissionPanelController : UiPanelController<VoyageMissionPanelView>
    {
        private Action _action;

        public override void Start()
        {
            View.MainButton.OnClickAsObservable().Subscribe(_ => OpenMission()).AddTo(Disposables);
            base.Start();
        }

        public void ShowInfo(GameReward winReward, StatusMission status, int index, Action action)
        {
            _action = action;
            View.rewardController.ShowReward(winReward);

            View.textNameMission.text = $"Mission {index + 1}";
            View.StatusMissionText.text = $"{status}";
            switch (status)
            {
                case StatusMission.NotOpen:
                    View.MainButton.interactable = false;
                    break;
                case StatusMission.Open:
                    View.MainButton.interactable = true;
                    break;
                case StatusMission.Complete:
                    View.MainButton.interactable = false;
                    break;
            }
            MessagesPublisher.OpenWindowPublisher.OpenWindow<VoyageMissionPanelController>(openType: OpenType.Exclusive);
        }

        public void OpenMission()
        {
            Close();
            _action?.Invoke();
        }
    }
}