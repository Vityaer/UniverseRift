using City.Buildings.Friends;
using Models.Common.BigDigits;
using UniRx;
using UnityEngine;
using VContainer;

namespace City.Achievements.ActionGameAchievments
{
    public class SendFriendHeartAchievment : GameAchievment
    {
        [Inject] private readonly FriendsPanelController _friendsPanelController;

        protected override void Subscribe()
        {
            _friendsPanelController.OnSendHearts.Subscribe(SendHeart).AddTo(Disposables);
        }

        private void SendHeart(int count)
        {
            AddProgress(new BigDigit(count));
        }
    }
}
