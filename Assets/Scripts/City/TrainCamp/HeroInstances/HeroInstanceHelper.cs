using Cysharp.Threading.Tasks;
using Fight.HeroControllers.Generals;
using UnityEngine;
using Utils.DataCheckUtils;

namespace City.TrainCamp.HeroInstances
{
    public class HeroInstanceHelper : MonoBehaviour
    {
        [SerializeField] private Camera m_camera;
        [SerializeField] private Transform m_createPoint;
        [SerializeField] private Transform m_container;

        private HeroController m_heroController;

        public Camera Camera => m_camera;
        public Transform CreatePoint => m_createPoint;

        public void EnableCamera(CustomRenderTexture renderContainerRenderTexture)
        {
            m_camera.enabled = true;
            m_camera.targetTexture = renderContainerRenderTexture;
            AdjustPerspectiveCameraToObject(m_heroController.GetComponent<Collider>()).Forget();
        }

        public void SetHeroController(HeroController heroController)
        {
            m_heroController = heroController.ThrowIfArgumentNull();
            heroController.gameObject.SetActive(true);
            heroController.transform.SetParent(m_createPoint);
            heroController.transform.localPosition = Vector3.zero;
        }

        public void DisableCamera()
        {
            m_camera.enabled = false;
            m_camera.targetTexture = null;
            m_heroController = null;
        }

        public void SetPosition(Vector3 newPosition)
        {
            m_container.position = newPosition;
        }

        private async UniTask AdjustPerspectiveCameraToObject(Collider objectCollider)
        {
            await UniTask.DelayFrame(1);
            Bounds bounds = objectCollider.bounds;
            Vector3 objectCenter = bounds.center;

            // Рассчитываем радиус окружности, охватывающей объект
            float radius = bounds.extents.magnitude;

            // Половина угла обзора в радианах
            float halfFovRad = Mathf.Deg2Rad * m_camera.fieldOfView * 0.5f;

            // Вычисляем расстояние до объекта
            float distance = radius / Mathf.Sin(halfFovRad);

            // Направление взгляда камеры (например, камера смотрит на -Z)
            Vector3 direction = m_camera.transform.forward;

            // Сдвигаем камеру так, чтобы объект был в центре и полностью входил в видимый фрустум
            m_camera.transform.position = objectCenter - direction * distance;

            // Можно зафиксировать камеру, чтобы она смотрела точно на центр объекта
            m_camera.transform.LookAt(objectCenter);

            var currentPosition = m_camera.transform.position;
            currentPosition.x = m_heroController.transform.position.x;
            m_camera.transform.position = currentPosition;
        }
    }
}