using City.Buildings.Mails.LetterPanels;
using Models.Misc;
using UiExtensions.Misc;
using UiExtensions.Scroll.Interfaces;
using UniRx;

namespace City.Buildings.Mails
{
    public class MailPanelController : UiPanelController<MailPanelView>
    {
        private readonly LetterPanelController _letterPanelController;

        private DynamicUiList<LetterView, LetterData> _lettetsWrapper;

        public MailPanelController(LetterPanelController letterPanelController)
        {
            _letterPanelController = letterPanelController;
        }

        public override void Start()
        {
            _lettetsWrapper = new(View.LetterPrefab, View.Content, View.Scroll, OnSelectLetterView);
            base.Start();
            View.GetAllButton.OnClickAsObservable().Subscribe(_ => GetAllRewards()).AddTo(Disposables);
            View.DeleteAllButton.OnClickAsObservable().Subscribe(_ => DeleteAllLetters()).AddTo(Disposables);
        }

        private void OnSelectLetterView(LetterView letterView)
        {
            _letterPanelController.ShowLetter(letterView);
        }

        protected override void OnLoadGame()
        {
            var receiveLetters = CommonGameData.CommunicationData.LetterDatas
                .FindAll(letter => letter.ReceiverPlayerId == CommonGameData.PlayerInfoData.Id);
            _lettetsWrapper.ShowDatas(receiveLetters);
        }

        private void GetAllRewards()
        {
        }

        private void DeleteAllLetters()
        {
        }
    }
}