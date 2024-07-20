using Cysharp.Threading.Tasks;
using Models.Misc;
using Network.DataServer.Messages.Friendships;
using Network.DataServer;
using UiExtensions.Misc;
using UiExtensions.Scroll.Interfaces;
using Misc.Json;
using VContainer;
using System.Collections.Generic;

namespace City.Buildings.Friends.Panels.AvailableFriends
{
    public class AvailableFriendsPanelController : UiPanelController<AvailableFriendsPanelView>
    {
        [Inject] private readonly IJsonConverter _jsonConverter;

        private DynamicUiList<AvailableFriendView, AvailableFriendData> _availableFriendsWrapper;
        private bool _isLoadAvailableFriends;

        public override void Start()
        {
            _availableFriendsWrapper = new(View.AvailableFriendPrefab, View.Content, View.ScrollRect, view => CreateRequestFriendship(view).Forget());
            base.Start();
        }

        public override void OnShow()
        {
            if(!_isLoadAvailableFriends)
                LoadAvailableFrieds().Forget();
            base.OnShow();
        }

        private async UniTaskVoid LoadAvailableFrieds()
        {
            _isLoadAvailableFriends = true;
            var message = new GetAvailableFriendsMessage
            {
                PlayerId = CommonGameData.PlayerInfoData.Id
            };
            var result = await DataServer.PostData(message);
            if (!string.IsNullOrEmpty(result))
            {
                var availableFriendDatas = _jsonConverter.Deserialize<List<AvailableFriendData>>(result);
                _availableFriendsWrapper.ShowDatas(availableFriendDatas);
            }
        }

        private async UniTaskVoid CreateRequestFriendship(AvailableFriendView view)
        {
            var message = new CreateFriendRequestMessage
            {
                PlayerId = CommonGameData.PlayerInfoData.Id,
                OtherPlayerId = view.GetData.PlayerId
            };

            var result = await DataServer.PostData(message);

            if (!string.IsNullOrEmpty(result))
            {
                view.SetDone();
            }
        }
    }
}
