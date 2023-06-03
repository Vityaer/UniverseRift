using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GameWindows
{
    public class ScreenSizeChecker : UIBehaviour, IDisposable
    {
        private CancellationTokenSource _tokenSource;
        public readonly ReactiveCommand OnWindowChanged = new ReactiveCommand();

        private float _screenWidth = 0f;

#if UNITY_EDITOR
        protected override void Start()
        {
            _tokenSource = new CancellationTokenSource();
            CheckScreenSize(_tokenSource).Forget();
        }

        protected override void OnRectTransformDimensionsChange()
        {
            base.OnRectTransformDimensionsChange();
            OnChangeScreenSize(_tokenSource).Forget();
        }

        private async UniTaskVoid CheckScreenSize(CancellationTokenSource cancellationTokenSource)
        {
            while (true)
            {
                if (_screenWidth != Screen.width)
                {
                    _screenWidth = Screen.width;
                    OnChangeScreenSize(_tokenSource).Forget();
                }
                await UniTask.Yield(cancellationTokenSource.Token);
            }
        }

        private async UniTaskVoid OnChangeScreenSize(CancellationTokenSource cancellationTokenSource)
        {
            await UniTask.Yield(cancellationTokenSource.Token);
            OnWindowChanged.Execute();
        }

#endif
        public void Dispose()
        {
            if (_tokenSource != null)
            {
                _tokenSource.Cancel();
                _tokenSource.Dispose();
            }
        }
    }
}