using System.Threading;
using UnityEngine;
using Cysharp.Threading.Tasks;

namespace UIController.FightUI.HeroStatusPanels
{
    public class HeroStatusCanvas : MonoBehaviour
    {
        private Camera _camera;

        private CancellationTokenSource m_tokenSource;

        public void Start()
        {
            _camera = Camera.main;
            m_tokenSource = new CancellationTokenSource();
            FollowCamera(m_tokenSource.Token).Forget();
        }

        private async UniTask FollowCamera(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                if (_camera != null)
                {
                    // Поворачиваем объект строго на камеру
                    var cameraPosition = _camera.transform.position;
                    var direction = transform.position - cameraPosition;

                    // Если ненулевой вектор — крутим
                    if (direction.sqrMagnitude > Mathf.Epsilon)
                    {
                        transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
                    }
                }

                // Ждём до следующего кадра
                await UniTask.Yield(PlayerLoopTiming.PostLateUpdate, cancellationToken: token);
            }
        }

        private void OnDestroy()
        {
            m_tokenSource?.Cancel();
            m_tokenSource?.Dispose();
        }
    }
}