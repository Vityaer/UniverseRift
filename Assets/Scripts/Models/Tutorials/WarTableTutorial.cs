using System.Threading;
using Campaign;
using City.Panels.BlackedPanels;
using City.Panels.DialogPanels;
using City.Panels.TutorialTouchPanels;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using Fight.Windows.WarTable;
using MainPages.Campaign;
using Sirenix.OdinInspector;
using UniRx;
using UnityEngine;
using VContainer;

namespace Models.Tutorials
{
    public class WarTableTutorial : ITutorialStage
    {
        [Inject] private CampaignPageView m_campaignPageView;
        [Inject] private WarTableController m_warTableController;
        [Inject] private WarTableView m_warTableView;
        [Inject] private BlackedPanelController m_blackedPanelController;
        [Inject] private TutorialTouchController m_tutorialTouchController;
        [Inject] private DialogPanelController m_dialogPanelController;
        [Inject] private CampaignController m_campaignController;

        [LabelWidth(200)]
        public string OpenCampaignDialogId;
        [LabelWidth(200)]
        public string SelectMissionDialogId;
        [LabelWidth(200)]
        public string CreateTeamDialogId;
        [LabelWidth(200)]
        public string StartFightDialogId;

        public async UniTask PlayAsync(CancellationToken token)
        {
            await m_dialogPanelController.ShowDialog(OpenCampaignDialogId, token);
            await m_blackedPanelController.SelectView(m_campaignPageView.CampaignButton.GetComponent<RectTransform>());
            m_tutorialTouchController.ShowTouch(m_campaignPageView.CampaignButton.GetComponent<RectTransform>());
            await m_campaignPageView.CampaignButton.OnClickAsAsyncEnumerable().FirstAsync(token);
            m_tutorialTouchController.HideTouch();
            
            await m_dialogPanelController.ShowDialog(SelectMissionDialogId, token);
            await UniTask.DelayFrame(3, cancellationToken: token);
            RectTransform button = m_campaignController.MissionControllers[0].btnGoFight.GetComponent<RectTransform>();
            m_tutorialTouchController.ShowTouch(button);
            await m_blackedPanelController.SelectView(button);
            await m_campaignController.MissionControllers[0].OnClickMission.First().ToUniTask(cancellationToken: token);
            m_tutorialTouchController.HideTouch();

            await m_dialogPanelController.ShowDialog(CreateTeamDialogId, token);
            await UniTask.DelayFrame(3, cancellationToken: token);
            m_tutorialTouchController.ShowTouch(m_warTableView.ListCardPanel.Cards[0].GetComponent<RectTransform>());
            await m_blackedPanelController.SelectView(m_warTableView.ListCardPanel.Cards[0].GetComponent<RectTransform>());
            await m_warTableView.ListCardPanel.OnSelect.First().ToUniTask(cancellationToken: token);
            m_tutorialTouchController.HideTouch();

            m_tutorialTouchController.ShowTouch(m_warTableView.ListCardPanel.Cards[1].GetComponent<RectTransform>());
            await m_blackedPanelController.SelectView(m_warTableView.ListCardPanel.Cards[1].GetComponent<RectTransform>());
            await m_warTableView.ListCardPanel.OnSelect.First().ToUniTask(cancellationToken: token);
            m_tutorialTouchController.HideTouch();

            await m_dialogPanelController.ShowDialog(StartFightDialogId, token);
            m_tutorialTouchController.ShowTouch(m_warTableView.StartFightButton.GetComponent<RectTransform>());
            await m_blackedPanelController.SelectView(m_warTableView.StartFightButton.GetComponent<RectTransform>());
            await m_warTableView.StartFightButton.OnClickAsAsyncEnumerable().FirstAsync(token);
            m_tutorialTouchController.HideTouch();
            m_blackedPanelController.HideSelect();
        }
    }
}