using System;
using System.Threading;
using City.Buildings.Tavern;
using City.Panels.BlackedPanels;
using City.Panels.DialogPanels;
using City.Panels.HeroesHireResultPanels;
using City.Panels.TutorialTouchPanels;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using Sirenix.OdinInspector;
using UniRx;
using UnityEngine;
using VContainer;

namespace Models.Tutorials
{
    public class TavernHireTutorial : ITutorialStage
    {
        [LabelWidth(200)]
        public string SelectSimpleHireDialogId;
        [LabelWidth(200)]
        public string HireDialogId;
        [LabelWidth(200)]
        public HireType HireTargetType;
        
        [Inject] private TavernController m_tavernController;
        [Inject] private TavernView m_tavernView;
        [Inject] private BlackedPanelController m_blackedPanelController;
        [Inject] private TutorialTouchController m_tutorialTouchController;
        [Inject] private HeroesHireResultPanelView m_heroesHireResultPanelView;
        [Inject] private DialogPanelController m_dialogPanelController;

        private IDisposable m_disposable;
        
        public async UniTask PlayAsync(CancellationToken token)
        {
            await m_dialogPanelController.ShowDialog(SelectSimpleHireDialogId, token);
            m_tutorialTouchController.ShowTouch(m_tavernView.SimpleHireButton.GetComponent<RectTransform>());
            await m_blackedPanelController.SelectView(m_tavernView.SimpleHireButton.GetComponent<RectTransform>());
            await m_tavernView.SimpleHireButton.OnClickAsAsyncEnumerable().FirstAsync(token);
            m_tutorialTouchController.HideTouch();
            
            await m_dialogPanelController.ShowDialog(HireDialogId, token);
            m_tutorialTouchController.ShowTouch(m_tavernView.CostManyHireController.GetComponent<RectTransform>());
            await m_blackedPanelController.SelectView(m_tavernView.CostManyHireController.GetComponent<RectTransform>());

            switch (HireTargetType)
            {
                case HireType.Simple:
                    await m_tavernController.ObserverSimpleHire.First().ToUniTask(cancellationToken: token);
                    break;
                case HireType.Special:
                    await m_tavernController.ObserverSpecialHire.First().ToUniTask(cancellationToken: token);
                    break;
                case HireType.Friend:
                default:
                    await m_tavernController.ObserverFriendHire.First().ToUniTask(cancellationToken: token);
                    break;
            }
            m_tutorialTouchController.HideTouch();

            m_blackedPanelController.HideSelect();
            m_tutorialTouchController.ShowTouch(m_heroesHireResultPanelView.ButtonCloseBuilding.GetComponent<RectTransform>());
            await m_heroesHireResultPanelView.ButtonCloseBuilding.OnClickAsAsyncEnumerable().FirstAsync(token);
            m_tutorialTouchController.HideTouch();

            m_tutorialTouchController.ShowTouch(m_tavernView.ButtonCloseBuilding.GetComponent<RectTransform>());
            await m_tavernView.ButtonCloseBuilding.OnClickAsAsyncEnumerable().FirstAsync(token);
            m_tutorialTouchController.HideTouch();
        }

        public enum HireType
        {
            Simple = 0,
            Special = 1,
            Friend = 2
        }
    }
}