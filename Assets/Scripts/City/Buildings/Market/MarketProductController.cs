using City.Panels.SubjectPanels.Common;
using Common;
using Common.Inventories.Splinters;
using Common.Resourses;
using Cysharp.Threading.Tasks;
using Sirenix.Serialization;
using System;
using UIController;
using UIController.Inventory;
using UIController.ItemVisual;
using UniRx;
using UnityEngine;
using VContainer;

namespace City.Buildings.Market
{
    public class MarketProductController : MonoBehaviour
    {
        [OdinSerialize] private BaseMarketProduct _marketProduct;
        [SerializeField] private GameObject _soldOutPanel;
        [SerializeField] private ButtonCostController _buttonCost;
        [SerializeField] private SubjectCell CellProduct;

        private SubjectDetailController _subjectDetailController;

        private BaseObject _subject;
        private CompositeDisposable _disposables = new CompositeDisposable();
        private Action _callback = null;

        public ButtonCostController ButtonCost => _buttonCost;
        public string SubjectId;

        [Inject]
        private void Construct(SubjectDetailController subjectDetailController)
        {
            _subjectDetailController = subjectDetailController;
            CellProduct.OnSelect.Subscribe(ShowDataProduct).AddTo(_disposables);
        }

        private void ShowDataProduct(SubjectCell cell)
        {
            _subjectDetailController.ShowData(cell.Subject);
        }

        private void Start()
        {
            ButtonCost.OnClick.Subscribe(_ => Buy()).AddTo(_disposables);
        }

        public void SetData<T>(string productId, T product, Action callback) where T : BaseMarketProduct
        {
            SubjectId = productId;

            _callback = callback;
            _marketProduct = product;
            ButtonCost.SetCost(product.Cost);
            switch (product)
            {
                case MarketProduct<GameItem> item:
                    _subject = item.Subject;
                    CellProduct.SetData(item.Subject);
                    break;
                case MarketProduct<GameResource> item:
                    _subject = item.Subject;
                    CellProduct.SetData(item.Subject);
                    break;
                case MarketProduct<GameSplinter> item:
                    _subject = item.Subject;
                    CellProduct.SetData(item.Subject);
                    break;
            }
            UpdateUI();
        }

        public void UpdateUI()
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
            _callback?.Invoke();
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
            _callback = null;
            _disposables.Dispose();
        }
    }
}