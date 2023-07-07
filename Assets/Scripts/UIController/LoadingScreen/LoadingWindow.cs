using Ui.LoadingScreen.Loading;
using Ui.LoadingScreen.ProgressBar;
using VContainer;
using VContainerUi.Abstraction;

namespace Ui.LoadingScreen
{
    public class LoadingWindow : WindowBase
    {
        public override string Name => nameof(LoadingWindow);

        public LoadingWindow(IObjectResolver container) : base(container)
        {
        }

        protected override void AddControllers()
        {
            AddController<ProgressBarController>();
            AddController<LoadingController>();
        }
    }
}