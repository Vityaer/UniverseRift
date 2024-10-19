using City.Buildings.Friends.FriendViews;
using City.Buildings.Friends.Panels.AvailableFriends;
using City.Buildings.Friends.Panels.FriendRequests;
using City.Panels.PlayerInfoPanels;
using Cysharp.Threading.Tasks;
using Misc.Json;
using Models.Common;
using Models.Data.Players;
using Models.Misc;
using Models.Misc.Communications;
using Network.DataServer;
using Network.DataServer.Messages.Friendships;
using System;
using System.Collections.Generic;
using UiExtensions.Misc;
using UiExtensions.Scroll.Interfaces;
using UniRx;
using UnityEngine;
using VContainer;
using VContainerUi.Messages;
using VContainerUi.Model;

namespace City.Buildings.Friends
{
    public class FriendsPanelController : UiPanelController<FriendsPanelView>
    {
        [Inject] private readonly FriendRequestsPanelController _friendRequestsPanelController;
        [Inject] private readonly IJsonConverter _jsonConverter;
        [Inject] private readonly PlayerMiniInfoPanelController _playerMiniInfoPanelController;

        private DynamicUiList<FriendView, FriendshipData> _friendsWrapper;
        private ReactiveCommand<int> _onSendHearts = new();
        private ReactiveCommand<int> _onReceiveHearts = new();

        public IObservable<int> OnSendHearts => _onSendHearts;
        public IObservable<int> OnReceiveHearts => _onReceiveHearts;

        public override void Start()
        {
            _friendRequestsPanelController.OnAgreeRequestFriendship.Subscribe(CreateFriend).AddTo(Disposables);

            _friendsWrapper = new(View.FriendPrefab, View.Content, View.Scroll, OnSelectFriend);
            View.OpenListFriendshipRequestsButton.OnClickAsObservable().Subscribe(_ => OpenFriendshipRequestPanel()).AddTo(Disposables);
            View.OpenListAvailablePlayerAsFriendsButton.OnClickAsObservable().Subscribe(_ => OpenAvailablePlayers()).AddTo(Disposables);
            View.SendAndReceiveFriendHeartsButton.OnClickAsObservable().Subscribe(_ => SendAndReceiveFriendHearts().Forget()).AddTo(Disposables);
            base.Start();
        }

        protected override void OnLoadGame()
        {
            _friendsWrapper.ShowDatas(CommonGameData.CommunicationData.FriendshipDatas);

            foreach (var view in _friendsWrapper.Views)
            {
                var data = view.GetData;
                var friendId =
                    data.FirstPlayerId == CommonGameData.PlayerInfoData.Id
                    ?
                    data.SecondPlayerId
                    :
                    data.FirstPlayerId;

                view.SetData(CommonGameData.PlayerInfoData.Id, CommonGameData.CommunicationData.PlayersData[friendId]);
            }

            View.FriendshipRequestNews.enabled = CommonGameData.CommunicationData.FriendshipRequests.Count > 0;
            base.OnLoadGame();
        }

        private void OpenAvailablePlayers()
        {
            MessagesPublisher.OpenWindowPublisher.OpenWindow<AvailableFriendsPanelController>(openType: OpenType.Exclusive);
        }

        private void OpenFriendshipRequestPanel()
        {
            MessagesPublisher.OpenWindowPublisher.OpenWindow<FriendRequestsPanelController>(openType: OpenType.Exclusive);
        }

        private void CreateFriend(FriendshipRequest request)
        {
            CommonGameData.CommunicationData.FriendshipRequests.Remove(request);
            View.FriendshipRequestNews.enabled = CommonGameData.CommunicationData.FriendshipRequests.Count > 0;

            var friendship = new FriendshipData()
            {
                FirstPlayerId = request.SenderPlayerId,
                SecondPlayerId = CommonGameData.PlayerInfoData.Id,
            };

            CommonGameData.CommunicationData.FriendshipDatas.Add(friendship);
            var view = _friendsWrapper.AddElement(friendship);
            view.SetData(CommonGameData.PlayerInfoData.Id, CommonGameData.CommunicationData.PlayersData[request.SenderPlayerId]);
        }

        private void OnSelectFriend(FriendView friendView)
        {
            var friendshipData = friendView.GetData;

            PlayerData playerData = null;
            if (CommonGameData.PlayerInfoData.Id.Equals(friendshipData.FirstPlayerId))
            {
                playerData = CommonGameData.CommunicationData.PlayersData[friendshipData.SecondPlayerId];
            }
            else
            {
                playerData = CommonGameData.CommunicationData.PlayersData[friendshipData.FirstPlayerId];
            }
            _playerMiniInfoPanelController.ShowPlayerData(playerData);
        }

        private async UniTaskVoid SendAndReceiveFriendHearts()
        {
            var message = new SendAndReceivedHeartAllFriendsMessage
            {
                PlayerId = CommonGameData.PlayerInfoData.Id
            };

            var result = await DataServer.PostData(message);

            if (!string.IsNullOrEmpty(result))
            {
                Debug.Log(result);
                var heartsContainer = _jsonConverter.Deserialize<HeartsContainer>(result);

                ShowSendedHearts(heartsContainer.SendedHeartIds);
                ShowReceivedHearts(heartsContainer.ReceivedHeartIds);
            }
        }

        private void ShowSendedHearts(List<int> sendedFriendIds)
        {
            Debug.Log($"send heart count: {sendedFriendIds.Count}");
            _onSendHearts.Execute(sendedFriendIds.Count);
            foreach (var id in sendedFriendIds)
            {
                var view = _friendsWrapper.Views.Find(x => x.GetData.FirstPlayerId == id || x.GetData.SecondPlayerId == id);
                if (view == null)
                    continue;

                view.ShowSendedHeart();
            }
        }

        private void ShowReceivedHearts(List<int> receivedHeartIds)
        {
            Debug.Log($"receive heart count: {receivedHeartIds.Count}");
            _onReceiveHearts.Execute(receivedHeartIds.Count);
            foreach (var id in receivedHeartIds)
            {
                var view = _friendsWrapper.Views.Find(x => x.GetData.FirstPlayerId == id || x.GetData.SecondPlayerId == id);
                if (view == null)
                    continue;

                view.ShowReceivedHeart();
            }
        }
    }
}