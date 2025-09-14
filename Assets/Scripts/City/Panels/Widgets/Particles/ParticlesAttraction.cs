using System;
using AssetKits.ParticleImage;
using UniRx;
using UnityEngine;

namespace City.Panels.Widgets.Particles
{
    public class ParticlesAttraction : MonoBehaviour
    {
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private ParticleImage _particles;
        
        private ReactiveCommand<ParticlesAttraction> _onFinish = new();
        
        public IObservable<ParticlesAttraction> OnFinish => _onFinish;
        public ReactiveCommand OnParticleFinish = new();

        private CompositeDisposable _disposables = new();
        
        private void Awake()
        {
            _particles.onParticleFinish.AsObservable()
                .Subscribe(_ => OnParticleFinish.Execute())
                .AddTo(_disposables);
            
            _particles.onStop.AsObservable()
                .Subscribe(_ => _onFinish.Execute(this))
                .AddTo(_disposables);
        }

        private void OnDestroy()
        {
            _disposables.Dispose();
        }

        public void SetData(RectTransform startElement, RectTransform target)
        {
            _rectTransform.SetParent(startElement);
            _rectTransform.anchoredPosition = Vector2.zero;
            _particles.attractorTarget = target;
            _particles.Play();
        }
    }
}