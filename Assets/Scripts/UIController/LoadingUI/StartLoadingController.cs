using TMPro;
using UnityEngine;
using VContainer.Unity;
using VContainerUi.Abstraction;

namespace UIController.LoadingUI
{
    public class StartLoadingController : UiController<StartLoadingView>, IStartable
    {
        private const float LOADING_TIME = 2f;

        [SerializeField] private Vector2Int data = new Vector2Int(0, 0);

        public void Start()
        {
#if UNITY_EDITOR_WIN
            View.gameObject.SetActive(false);
#else
            View.gameObject.SetActive(false);
#endif
        }

        private void ChangeLoadedStage(Vector2Int newData)
        {
            Debug.Log("load");
            data = newData;
            View.LoadingGameSlider.SetData(newData.x, newData.y);
        }

    }
}