using City.Buildings.Tavern;
using Models.Achievments;
using Models.Data;
using UniRx;
using VContainer;

namespace City.Achievements.ActionGameAchievments
{
    public class FriendHireAchievment : GameAchievment
    {
        [Inject] private readonly TavernController _tavernController;

        protected override void Subscribe()
        {
            _tavernController.ObserverFriendHire.Subscribe(AddProgress).AddTo(Disposables);
        }
    }
}
