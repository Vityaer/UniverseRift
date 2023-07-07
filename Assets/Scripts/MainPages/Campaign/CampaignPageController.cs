using Campaign;
using UiExtensions.MainPages;
using UniRx;
using UnityEngine;
using VContainer.Unity;
using VContainerUi.Messages;
using VContainerUi.Model;

namespace MainPages.Campaign
{
    public class CampaignPageController : UiMainPageController<CampaignPageView>, IInitializable
    {

        public new void Initialize()
        {
            base.Initialize();
            View.CampaignButton.OnClickAsObservable().Subscribe(_ => OpenCampaignChapter()).AddTo(Disposables);
        }

        private void OpenCampaignChapter()
        {
            UiMessagesPublisher.OpenWindowPublisher.OpenWindow<CampaignController>(openType: OpenType.Exclusive);
        }
    }
}
