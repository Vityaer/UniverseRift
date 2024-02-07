using Cysharp.Threading.Tasks;
using Models.Misc;
using Network.DataServer;
using Network.DataServer.Messages.Friendships;
using System;
using UiExtensions.Misc;
using UiExtensions.Scroll.Interfaces;
using UniRx;

namespace City.Buildings.Friends.Panels.FriendRequests
{
    public class FriendRequestsPanelController : UiPanelController<FriendRequestsPanelView>
    {
        private DynamicUiList<RequestFriendView, FriendshipRequest> _requestsWrapper;
        private ReactiveCommand<FriendshipRequest> _onAgreeRequestFriendship = new();

        public IObservable<FriendshipRequest> OnAgreeRequestFriendship => _onAgreeRequestFriendship;

        protected override void OnLoadGame()
        {
            _requestsWrapper = new(View.RequestPrefab, View.Content, View.Scroll, onCreate: OnCreateRequestFriendshipView);
            _requestsWrapper.ShowDatas(CommonGameData.CommunicationData.FriendshipRequests);

            foreach (var view in _requestsWrapper.Views)
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
                _requestsWrapper.RemoveElement(view);
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
                _requestsWrapper.RemoveElement(view);
                _onAgreeRequestFriendship.Execute(view.GetData);
            }
        }
    }
}
