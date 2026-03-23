using System;
using System.Collections.Generic;
using City.Panels.SubjectPanels.Common;
using Common.Db.CommonDictionaries;
using Common.Rewards;
using Hero;
using LocalizationSystems;
using Models.Fights.Campaign;
using UI.Utils.Localizations.Extensions;
using UiExtensions.Panels;
using UniRx;
using VContainer;
using VContainerUi.Messages;
using VContainerUi.Model;

namespace City.Buildings.Voyage.Panels
{
    public class VoyageMissionPanelController : UiPanelController<VoyageMissionPanelView>
    {
        [Inject] private readonly CommonDictionaries m_commonDictionaries;
        [Inject] private readonly SubjectDetailController m_subjectDetailController;
        [Inject] private readonly ILocalizationSystem m_localizationSystem;
        
        private Action m_action;

        public override void Start()
        {
            View.MainButton.OnClickAsObservable().Subscribe(_ => OpenMission()).AddTo(Disposables);
            View.rewardController.SetDetailsController(m_subjectDetailController);
            base.Start();
        }

        public void ShowInfo(MissionModel mission, StatusMission status, int index, Action action)
        {
            GameReward reward = new GameReward(mission.WinReward, m_commonDictionaries);
            m_action = action;
            View.rewardController.ShowReward(reward, m_commonDictionaries);

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
                var hero = new GameHero(m_commonDictionaries.Heroes[heroData.HeroId], heroData);
                View.UnitCells[i].SetData(hero);
            }


            MessagesPublisher.OpenWindowPublisher.OpenWindow<VoyageMissionPanelController>(openType: OpenType.Additive);
        }

        private void OpenMission()
        {
            Close();
            m_action?.Invoke();
        }
    }
}