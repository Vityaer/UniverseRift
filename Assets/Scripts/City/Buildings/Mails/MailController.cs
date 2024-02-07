using City.Buildings.Mails.LetterPanels;
using Models.Misc;
using UiExtensions.Misc;
using UiExtensions.Scroll.Interfaces;
using UniRx;

namespace City.Buildings.Mails
{
    public class MailController : UiPanelController<MailView>
    {
        private readonly LetterPanelController _letterPanelController;

        private DynamicUiList<LetterView, LetterData> _lettetsWrapper;

        public MailController(LetterPanelController letterPanelController)
        {
            _letterPanelController = letterPanelController;
        }

        private void OnSelectLetterView(LetterView letterView)
        {
            _letterPanelController.ShowLetter(letterView);
        }

        public override void Start()
        {
            _lettetsWrapper = new(View.LetterPrefab, View.Content, View.Scroll, OnSelectLetterView);
            base.Start();
            View.GetAllButton.OnClickAsObservable().Subscribe(_ => GetAllRewards()).AddTo(Disposables);
            View.DeleteAllButton.OnClickAsObservable().Subscribe(_ => DeleteAllLetters()).AddTo(Disposables);
        }

        protected override void OnLoadGame()
        {
            //_lettetsWrapper.ShowDatas();
        }

        private void GetAllRewards()
        {
        }

        private void DeleteAllLetters()
        {
        }
    }
}