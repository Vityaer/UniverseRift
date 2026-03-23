using Cysharp.Threading.Tasks;
using Models.Misc;
using Network.DataServer;
using Network.DataServer.Messages.Friendships;
using System;
using Common.Db.CommonDictionaries;
using UiExtensions.Misc;
using UiExtensions.Panels;
using UniRx;
using VContainer;

namespace City.Buildings.Friends.Panels.FriendRequests
{
    public class FriendRequestsPanelController : UiPanelController<FriendRequestsPanelView>
    {
        [Inject] private readonly CommonDictionaries m_commonDictionaries;
        
        private DynamicUiList<RequestFriendView, FriendshipRequest> m_requestsWrapper;
        private readonly ReactiveCommand<FriendshipRequest> m_onAgreeRequestFriendship = new();

        public IObservable<FriendshipRequest> OnAgreeRequestFriendship => m_onAgreeRequestFriendship;

        protected override void OnLoadGame()
        {
            m_requestsWrapper = new(View.RequestPrefab, View.Content, View.Scroll, m_commonDictionaries, onCreate: OnCreateRequestFriendshipView);
            m_requestsWrapper.ShowDatas(CommonGameData.CommunicationData.FriendshipRequests);

            foreach (var view in m_requestsWrapper.Views)
            {
                var data = view.GetData;
                view.SetData(CommonGameData.CommunicationData.PlayersData[data.SenderPlayerId]);
            }

            base.OnLoadGame();
        }

        private void OnCreateRequestFriendshipView(RequestFriendView view)
        {
            view.OnAgreeFriendship.Subscribe(request => AgreeRequestFriendship(request).Forget()).AddTo(Disposables);
            view.OnDeniedFriendship.Subscribe(request => DeniedRequestFriendship(request).Forget()).AddTo(Disposables);
        }

        private async UniTaskVoid DeniedRequestFriendship(RequestFriendView view)
        {
            var message = new DeniedFriendRequestMessage
            {
                PlayerId = CommonGameData.PlayerInfoData.Id,
                RequestId = view.GetData.Id
            };

            var result = await DataServer.PostData(message);

            if (!string.IsNullOrEmpty(result))
            {
                m_requestsWrapper.RemoveElement(view);
            }
        }

        private async UniTaskVoid AgreeRequestFriendship(RequestFriendView view)
        {
            var message = new ApplyFriendRequestMessage
            {
                PlayerId = CommonGameData.PlayerInfoData.Id,
                RequestId = view.GetData.Id
            };

            var result = await DataServer.PostData(message);

            if (!string.IsNullOrEmpty(result))
            {
                m_requestsWrapper.RemoveElement(view);
                m_onAgreeRequestFriendship.Execute(view.GetData);
            }
        }
    }
}
