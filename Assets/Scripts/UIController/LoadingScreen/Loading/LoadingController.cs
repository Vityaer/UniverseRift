using Common.Db.CommonDictionaries;
using Network.Misc;
using Ui.LoadingScreen.ProgressBar;
using UniRx;
using Utils;
using VContainer.Unity;
using VContainerUi.Abstraction;

namespace Ui.LoadingScreen.Loading
{
    public class LoadingController : UiController<LoadingView>, IInitializable
    {
        private readonly ProgressBarController _progressBarController;
        private readonly CompositeDisposable _disposable = new CompositeDisposable();
        private readonly CommonDictionaries _commonDictionaries;

#if UNITY_EDITOR
        private const int FILES_COUNT = 29;
#else
        private const int FILES_COUNT = 30;
#endif
        private float _progressFileDelta;
        private bool _isOpen;

        public LoadingController(CommonDictionaries commonDictionaries, ProgressBarController progressBarController)
        {
            _progressBarController = progressBarController;
            _commonDictionaries = commonDictionaries;
        }

        public void Initialize()
        {
            _commonDictionaries.OnStartDownloadFiles.Subscribe(_ => OnStartDownload()).AddTo(_disposable);
            _commonDictionaries.OnFinishDownloadFiles.Subscribe(_ => OnFinishDownLoad()).AddTo(_disposable);
        }

        public void OnStartDownload()
        {
            if (_isOpen)
                return;

            _isOpen = true;

            Show();
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
            Hide();
            _progressBarController.Close();
        }

        public void Dispose()
        {
            _disposable?.Dispose();
        }

    }
}