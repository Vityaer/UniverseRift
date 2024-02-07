using City.Buildings.Friends.FriendViews;
using Ui.Misc.Widgets;
using UnityEngine;
using UnityEngine.UI;

namespace City.Buildings.Friends
{
    public class FriendsPanelView : BasePanel
    {
        public Button OpenListAvailablePlayerAsFriendsButton;
        public Button OpenListFriendshipRequestsButton;
        public Button SendAndReceiveFriendHeartsButton;

        public Image FriendshipRequestNews;

        public ScrollRect Scroll;
        public RectTransform Content;
        public FriendView FriendPrefab;
    }
}
