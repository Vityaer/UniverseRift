using System.Collections.Generic;
using Common.Db.CommonDictionaries;
using Cysharp.Threading.Tasks;
using Misc.Json;
using Models.Misc;
using Network.DataServer;
using Network.DataServer.Messages.Friendships;
using UiExtensions.Misc;
using UiExtensions.Panels;
using VContainer;

namespace City.Buildings.Friends.Panels.AvailableFriends
{
    public class AvailableFriendsPanelController : UiPanelController<AvailableFriendsPanelView>
    {
        [Inject] private readonly IJsonConverter m_jsonConverter;
        [Inject] private readonly CommonDictionaries m_commonDictionaries;

        private DynamicUiList<AvailableFriendView, AvailableFriendData> m_availableFriendsWrapper;
        private bool m_isLoadAvailableFriends;

        public override void Start()
        {
            m_availableFriendsWrapper = new(View.AvailableFriendPrefab, View.Content,
                View.ScrollRect, m_commonDictionaries,
                view => CreateRequestFriendship(view).Forget());
            base.Start();
        }

        public override void OnShow()
        {
            if (!m_isLoadAvailableFriends)
                LoadAvailableFrieds().Forget();
            base.OnShow();
        }

        private async UniTaskVoid LoadAvailableFrieds()
        {
            m_isLoadAvailableFriends = true;
            GetAvailableFriendsMessage message = new GetAvailableFriendsMessage
            {
                PlayerId = CommonGameData.PlayerInfoData.Id
            };
            string result = await DataServer.PostData(message);
            if (!string.IsNullOrEmpty(result))
            {
                List<AvailableFriendData> availableFriendDatas =
                    m_jsonConverter.Deserialize<List<AvailableFriendData>>(result);
                m_availableFriendsWrapper.ShowDatas(availableFriendDatas);
            }
        }

        private async UniTaskVoid CreateRequestFriendship(AvailableFriendView view)
        {
            CreateFriendRequestMessage message = new CreateFriendRequestMessage
            {
                PlayerId = CommonGameData.PlayerInfoData.Id,
                OtherPlayerId = view.GetData.PlayerId
            };

            string result = await DataServer.PostData(message);

            if (!string.IsNullOrEmpty(result)) view.SetDone();
        }
    }
}