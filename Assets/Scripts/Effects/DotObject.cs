using DG.Tweening;
using System;
using UnityEngine;

namespace Effects
{
    public class DotObject : MonoBehaviour, IDisposable
    {
        private Tween _tween;

        void Start()
        {
            _tween = transform.Find("Sprite").GetComponent<SpriteRenderer>().DOFade(0f, duration: 1f).OnComplete(() => Destroy(gameObject));
        }

        public void Dispose()
        {
            _tween.Kill();
        }
    }
}