using System;
using Common.Db.CommonDictionaries;
using Network.Misc;
using Plugins.VContainerUi.Runtime.Abstraction;
using Ui.LoadingScreen.Loading;
using Ui.LoadingScreen.ProgressBar;
using UniRx;
using UnityEngine;
using Utils;
using VContainer.Unity;
using VContainerUi.Abstraction;

namespace UIController.LoadingScreen.Loading
{
    public class LoadingController : UiController<LoadingView>, IInitializable, IDisposable
    {
        private readonly ProgressBarController m_progressBarController;
        private readonly CompositeDisposable m_disposable = new();
        private readonly CommonDictionaries m_commonDictionaries;

#if UNITY_EDITOR
        private const int FILES_COUNT = 29;
#else
        private const int FILES_COUNT = 30;
#endif
        private float m_progressFileDelta;
        private bool m_isOpen;

        
        public LoadingController(CommonDictionaries commonDictionaries, ProgressBarController progressBarController)
        {
            m_progressBarController = progressBarController;
            m_commonDictionaries = commonDictionaries;
        }

        public void Initialize()
        {
            m_commonDictionaries.OnStartDownloadFiles.Subscribe(_ => OnStartDownload()).AddTo(m_disposable);
            m_commonDictionaries.OnFinishDownloadFiles.Subscribe(_ => OnFinishDownLoad()).AddTo(m_disposable);
        }

        private void OnStartDownload()
        {
            if (m_isOpen)
                return;

            m_isOpen = true;

            Show();
            TextUtils.DownloadProgress.Subscribe(ShowDownloadProgress).AddTo(m_disposable);
            m_progressBarController.Open();
            m_progressFileDelta = 1f / FILES_COUNT;
        }

        private void ShowDownloadProgress(FileLoadingProgress newFileProgress)
        {
            var newProgress = m_progressFileDelta * (newFileProgress.Progress + newFileProgress.FileIndex - 1);
            m_progressBarController.ChangeProgress(newProgress);
        }

        private void OnFinishDownLoad()
        {
            Hide();
            m_progressBarController.Close();
        }

        void IDisposable.Dispose()
        {
            m_disposable?.Dispose();
        }

    }
}