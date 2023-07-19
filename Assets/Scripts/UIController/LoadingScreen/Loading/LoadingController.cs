using Db.CommonDictionaries;
using Network.Misc;
using Ui.LoadingScreen.ProgressBar;
using UniRx;
using Utils;
using VContainer.Unity;
using VContainerUi.Abstraction;
using VContainerUi.Interfaces;

namespace Ui.LoadingScreen.Loading
{
    public class LoadingController : UiController<LoadingView>, IStartable
    {
        private readonly ProgressBarController _progressBarController;
        private readonly ProgressBarView _progressBarView;
        private readonly CompositeDisposable _disposable = new CompositeDisposable();
#if UNITY_EDITOR
        private const int FILES_COUNT = 29;
#else
        private const int FILES_COUNT = 30;
#endif
        private float _progressFileDelta;

        public LoadingController(CommonDictionaries _commonDictionaries, ProgressBarController progressBarController, ProgressBarView progressBarView)
        {
            _progressBarView = progressBarView;
            _progressBarController = progressBarController;
            _commonDictionaries.OnStartDownloadFiles.Subscribe(_ => OnStartDownload()).AddTo(_disposable);
            _commonDictionaries.OnFinishDownloadFiles.Subscribe(_ => OnFinishDownLoad()).AddTo(_disposable);
        }

        public void Start()
        {
            ((IUiView)_progressBarView).SetParent(View.transform);
        }

        public void OnStartDownload()
        {
            TextUtils.DownloadProgress.Subscribe(ShowDownloadProgress).AddTo(_disposable);
            _progressBarController.Open();
            _progressFileDelta = 1f / FILES_COUNT;
        }

        public void ShowDownloadProgress(FileLoadingProgress newFileProgress)
        {
            var newProgress = _progressFileDelta * (newFileProgress.Progress + newFileProgress.FileIndex - 1);
            _progressBarController.ChangeProgress(newProgress);
        }

        private void OnFinishDownLoad()
        {
            _progressBarController.Close();
        }

        public void Dispose()
        {
            _disposable?.Dispose();
        }


    }
}