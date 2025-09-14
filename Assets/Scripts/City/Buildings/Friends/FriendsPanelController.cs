using System;
using System.Collections.Generic;
using System.Linq;
using City.Buildings.Friends.FriendViews;
using City.Buildings.Friends.Panels.AvailableFriends;
using City.Buildings.Friends.Panels.FriendRequests;
using City.Panels.PlayerInfoPanels;
using Cysharp.Threading.Tasks;
using Misc.Json;
using Models.Misc;
using Models.Misc.Communications;
using Network.DataServer;
using Network.DataServer.Messages.Friendships;
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
        [Inject] private readonly FriendRequestsPanelController m_friendRequestsPanelController;
        [Inject] private readonly IJsonConverter m_jsonConverter;
        [Inject] private readonly PlayerMiniInfoPanelController m_playerMiniInfoPanelController;

        private DynamicUiList<FriendView, FriendshipData> m_friendsWrapper;
        private readonly ReactiveCommand<int> m_onSendHearts = new();
        private readonly ReactiveCommand<int> m_onReceiveHearts = new();

        public IObservable<int> OnSendHearts => m_onSendHearts;
        public IObservable<int> OnReceiveHearts => m_onReceiveHearts;

        public override void Start()
        {
            m_friendRequestsPanelController.OnAgreeRequestFriendship.Subscribe(CreateFriend).AddTo(Disposables);

            m_friendsWrapper = new(View.FriendPrefab, View.Content, View.Scroll, OnSelectFriend);
            View.OpenListFriendshipRequestsButton.OnClickAsObservable().Subscribe(_ => OpenFriendshipRequestPanel())
                .AddTo(Disposables);
            View.OpenListAvailablePlayerAsFriendsButton.OnClickAsObservable().Subscribe(_ => OpenAvailablePlayers())
                .AddTo(Disposables);
            View.SendAndReceiveFriendHeartsButton.OnClickAsObservable()
                .Subscribe(_ => SendAndReceiveFriendHearts().Forget()).AddTo(Disposables);
            base.Start();
        }

        protected override void OnLoadGame()
        {
            m_friendsWrapper.ShowDatas(CommonGameData.CommunicationData.FriendshipDatas);

            foreach (var view in m_friendsWrapper.Views)
            {
                var data = view.GetData;
                var friendId =
                    data.FirstPlayerId == CommonGameData.PlayerInfoData.Id
                        ? data.SecondPlayerId
                        : data.FirstPlayerId;

                view.SetData(CommonGameData.PlayerInfoData.Id,
                    CommonGameData.CommunicationData.PlayersData[friendId]);
            }

            CheckNews();
            base.OnLoadGame();
        }

        private void CheckNews()
        {
            var newsExist = CommonGameData.CommunicationData.FriendshipRequests.Count > 0
                            || CheckNotReceivedHeartsExist();
            View.FriendshipRequestNews.enabled = newsExist;
            OnNewsStatusChangeInternal.Execute(newsExist);
        }

        private bool CheckNotReceivedHeartsExist()
        {
            return CommonGameData.CommunicationData.FriendshipDatas.Any(data =>
                (data.FirstPlayerId == CommonGameData.PlayerInfoData.Id && data.PresentForFirstPlayer ! &&
                 data.FirstPlayerRecieved)
                || (data.SecondPlayerId == CommonGameData.PlayerInfoData.Id && data.PresentForSecondPlayer ! &&
                    data.SecondPlayerRecieved)
            );
        }

        private void OpenAvailablePlayers()
        {
            MessagesPublisher.OpenWindowPublisher.OpenWindow<AvailableFriendsPanelController>(
                openType: OpenType.Additive);
        }

        private void OpenFriendshipRequestPanel()
        {
            MessagesPublisher.OpenWindowPublisher
                .OpenWindow<FriendRequestsPanelController>(openType: OpenType.Additive);
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
            var view = m_friendsWrapper.AddElement(friendship);
            view.SetData(CommonGameData.PlayerInfoData.Id,
                CommonGameData.CommunicationData.PlayersData[request.SenderPlayerId]);
        }

        private void OnSelectFriend(FriendView friendView)
        {
            var friendshipData = friendView.GetData;

            var playerData = CommonGameData.PlayerInfoData.Id.Equals(friendshipData.FirstPlayerId)
                ? CommonGameData.CommunicationData.PlayersData[friendshipData.SecondPlayerId]
                : CommonGameData.CommunicationData.PlayersData[friendshipData.FirstPlayerId];

            m_playerMiniInfoPanelController.ShowPlayerData(playerData);
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
                var heartsContainer = m_jsonConverter.Deserialize<HeartsContainer>(result);

                ShowSendedHearts(heartsContainer.SendedHeartIds);
                ShowReceivedHearts(heartsContainer.ReceivedHeartIds);
                CheckNews();
            }
        }

        private void ShowSendedHearts(List<int> sendedFriendIds)
        {
            m_onSendHearts.Execute(sendedFriendIds.Count);
            foreach (var id in sendedFriendIds)
            {
                var view = m_friendsWrapper.Views.Find(x =>
                    x.GetData.FirstPlayerId == id || x.GetData.SecondPlayerId == id);
                if (view == null)
                    continue;

                view.ShowSendedHeart();
            }
        }

        private void ShowReceivedHearts(List<int> receivedHeartIds)
        {
            m_onReceiveHearts.Execute(receivedHeartIds.Count);
            foreach (var id in receivedHeartIds)
            {
                var view = m_friendsWrapper.Views.Find(x =>
                    x.GetData.FirstPlayerId == id || x.GetData.SecondPlayerId == id);
                if (view == null)
                    continue;

                view.ShowReceivedHeart();
            }
        }
    }
}