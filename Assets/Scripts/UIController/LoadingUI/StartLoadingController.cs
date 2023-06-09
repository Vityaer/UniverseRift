using Common;
using TMPro;
using UnityEngine;

namespace UIController.LoadingUI
{
    public class StartLoadingController : MonoBehaviour
    {
        public GameObject panel;
        public TimeSlider sliderLoadingGame;
        private TextMeshProUGUI textCurrentStageLoading;

        [SerializeField] private Vector2Int data = new Vector2Int(0, 0);

        public void Close()
        {
            sliderLoadingGame.UnregisterOnFillSliderInMax(Close);
            GameController.Instance.UnregiterOnRegisterOnLoadGame(ChangeLoadedStage);
            panel.SetActive(false);
        }

        private void ChangeLoadedStage(Vector2Int newData)
        {
            Debug.Log("load");
            data = newData;
            sliderLoadingGame.SetData(newData.x, newData.y);
        }

        void Start()
        {
#if UNITY_EDITOR_WIN
            Close();
#else
			//panel.SetActive(true);
			//PlayerController.Instance.RegiterOnRegisterOnLoadGame(ChangeLoadedStage);
			//sliderLoadingGame.RegisterOnFillSliderInMax(Close);
            Close();
#endif
        }

        void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Debug.Log("twice component on " + gameObject.name);
            }
        }

        private static StartLoadingController instance;
        public static StartLoadingController Instance => instance;
    }
}