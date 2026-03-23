using System;
using City.Panels.SubjectPanels.Common;
using Common.Db.CommonDictionaries;
using Common.Inventories.Resourses;
using Common.Inventories.Splinters;
using Sirenix.Serialization;
using UIController;
using UIController.Inventory;
using UIController.ItemVisual;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Utils.DataCheckUtils;
using VContainer;

namespace City.Buildings.Market
{
    public class MarketProductController : MonoBehaviour
    {
        [OdinSerialize] private BaseMarketProduct _marketProduct;
        [SerializeField] private GameObject _soldOutPanel;
        [SerializeField] private ButtonCostController _buttonCost;
        [SerializeField] private SubjectCell CellProduct;

        private SubjectDetailController m_subjectDetailController;
        private CommonDictionaries m_commonDictionaries;

        private readonly CompositeDisposable m_disposables = new CompositeDisposable();
        private Action m_callback = null;

        public ButtonCostController ButtonCost => _buttonCost;
        public string SubjectId;

        [Inject]
        private void Construct(SubjectDetailController subjectDetailController, CommonDictionaries commonDictionaries)
        {
            m_commonDictionaries = commonDictionaries.ThrowIfArgumentNull();
            m_subjectDetailController = subjectDetailController.ThrowIfArgumentNull();
            CellProduct.OnSelect.Subscribe(ShowDataProduct).AddTo(m_disposables);
        }

        private void ShowDataProduct(SubjectCell cell)
        {
            m_subjectDetailController.ShowData(cell.Subject);
        }

        private void Start()
        {
            ButtonCost.OnClick.Subscribe(_ => Buy()).AddTo(m_disposables);
        }

        public void SetScroll(ScrollRect scrollRect)
        {
            CellProduct.Init(scrollRect, m_commonDictionaries);
        }

        public void SetData<T>(string productId, T product, Action callback) where T : BaseMarketProduct
        {
            SubjectId = productId;

            m_callback = callback;
            _marketProduct = product;
            ButtonCost.SetCost(product.Cost);
            switch (product)
            {
                case MarketProduct<GameItem> item:
                    CellProduct.SetData(item.Subject);
                    break;
                case MarketProduct<GameResource> item:
                    CellProduct.SetData(item.Subject);
                    break;
                case MarketProduct<GameSplinter> item:
                    CellProduct.SetData(item.Subject);
                    break;
            }

            UpdateUI();
        }

        private void UpdateUI()
        {
            if (_marketProduct.CountLeftProduct == _marketProduct.CountMaxProduct)
            {
                ButtonCost.Disable();
                _soldOutPanel.SetActive(true);
            }
            else
            {
                _soldOutPanel.SetActive(false);
                ButtonCost.Enable();
            }
        }

        public void Recovery()
        {
            _marketProduct.Recovery();
            UpdateUI();
        }

        public void Buy(int count = 1)
        {
            m_callback?.Invoke();
        }

        public void FinishBuy(int count = 1)
        {
            if (count + _marketProduct.CountLeftProduct > _marketProduct.CountMaxProduct)
                count = _marketProduct.CountMaxProduct - _marketProduct.CountLeftProduct;
            _marketProduct.GetProduct(count);
            UpdateUI();
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public void SetPurchaseCount(int purchaseCount)
        {
            _marketProduct.SetPurchaseCount(purchaseCount);
            UpdateUI();
        }

        private void OnDestroy()
        {
            m_callback = null;
            m_disposables.Dispose();
        }
    }
}