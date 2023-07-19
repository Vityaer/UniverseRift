using Models.Achievments;
using UniRx;
using VContainerUi.Abstraction;

namespace City.Buildings.Requirement
{
    public class BaseRequimentController : UiController<AchievmentView>
    {
        private AchievmentModel _requirement;
        private CompositeDisposable _disposables = new CompositeDisposable();

        public BaseRequimentController()
        {
        }
    }
}
