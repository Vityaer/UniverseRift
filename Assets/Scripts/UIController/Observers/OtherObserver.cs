using Common;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UIController.Observers
{
    public class OtherObserver : MonoBehaviour
    {
        public TypeObserverOther type;
        public bool isMyabeBuy = false;

        [Header("UI")]
        public GameObject btnAddResource;
        public Image imageObserver;
        public TextMeshProUGUI textObserver;

        void Start()
        {
            btnAddResource.gameObject.SetActive(isMyabeBuy);
            RegisterOnChange();
            UpdateUI();
        }

        void RegisterOnChange()
        {
            switch (type)
            {
                case TypeObserverOther.CountHeroes:
                    GameController.Instance.RegisterOnChangeCountHeroes(UpdateUI);
                    break;
            }
        }

        public void UpdateUI()
        {
            string textCount = string.Empty;
            switch (type)
            {
                case TypeObserverOther.CountHeroes:
                    textCount = FunctionHelp.AmountFromRequireCount(GameController.Instance.GetCurrentCountHeroes, GameController.Instance.GetMaxCountHeroes);
                    break;
                case TypeObserverOther.MineEnergy:
                    textCount = "3 / 5";
                    // textCount = FunctionHelp.AmountFromRequireCount(PlayerScript.Instance.GetCurrentCountHeroes, PlayerScript.Instance.GetMaxCountHeroes);
                    break;
            }
            textObserver.text = textCount;

        }
        public void OpenPanelForBuyResource()
        {
            // MarketProduct<Resource> product = null;
            // product = MarketResourceScript.Instance.GetProductFromTypeResource(resource.Name);
            // if(product != null)
            // 	PanelBuyResourceScript.StandartPanelBuyResource.Open(
            // 		product.subject, product.cost
            // 		);
        }
    }
}