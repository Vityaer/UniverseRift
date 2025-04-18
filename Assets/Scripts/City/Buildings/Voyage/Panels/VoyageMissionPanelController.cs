using City.Buildings.Voyage.Panels;
using City.Panels.SubjectPanels.Common;
using Common.Rewards;
using Db.CommonDictionaries;
using Hero;
using Models.Fights.Campaign;
using System;
using System.Collections.Generic;
using LocalizationSystems;
using UI.Utils.Localizations.Extensions;
using UiExtensions.Scroll.Interfaces;
using UniRx;
using VContainer;
using VContainerUi.Messages;
using VContainerUi.Model;

namespace City.Buildings.Voyage
{
    public class VoyageMissionPanelController : UiPanelController<VoyageMissionPanelView>
    {
        [Inject] private readonly CommonDictionaries _commonDictionaries;
        [Inject] private readonly SubjectDetailController _subjectDetailController;
        [Inject] private readonly ILocalizationSystem m_localizationSystem;
        
        private Action _action;

        public override void Start()
        {
            View.MainButton.OnClickAsObservable().Subscribe(_ => OpenMission()).AddTo(Disposables);
            View.rewardController.SetDetailsController(_subjectDetailController);
            base.Start();
        }

        public void ShowInfo(MissionModel mission, StatusMission status, int index, Action action)
        {
            var reward = new GameReward(mission.WinReward, _commonDictionaries);
            _action = action;
            View.rewardController.ShowReward(reward);

            View.NameMission.StringReference = m_localizationSystem
                .GetLocalizedContainer("VoyageMissionName")
                .WithArguments(new List<object>{{index + 1}});
            
            View.StatusMission.StringReference = m_localizationSystem
                .GetLocalizedContainer("VoyageMissionStatus")
                .WithArguments(new List<object>{status});
            
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

            View.UnitCells.ForEach(cell => cell.Clear());
            for (var i = 0; i < mission.Units.Count; i++)
            {
                var heroData = mission.Units[i];
                var hero = new GameHero(_commonDictionaries.Heroes[heroData.HeroId], heroData);
                View.UnitCells[i].SetData(hero);
            }


            MessagesPublisher.OpenWindowPublisher.OpenWindow<VoyageMissionPanelController>(openType: OpenType.Additive);
        }

        public void OpenMission()
        {
            Close();
            _action?.Invoke();
        }
    }
}