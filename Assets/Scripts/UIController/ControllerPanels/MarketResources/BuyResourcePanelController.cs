using Assets.Scripts.ClientServices;
using Common.Resourses;
using Cysharp.Threading.Tasks;
using UIController.ControllerPanels.MarketResources;
using UiExtensions.Scroll.Interfaces;
using UniRx;
using VContainer;
using VContainer.Unity;

namespace UIController
{
    public class BuyResourcePanelController : UiPanelController<BuyResourcePanelView>, IStartable
    {
        [Inject] private readonly ResourceStorageController _storageController;

        private GameResource _product;
        private GameResource _cost;

        public override void Start()
        {
            base.Start();
            View.CountController.OnChangeCount.Subscribe(ChangeCount).AddTo(Disposables);
        }

        public void Open(GameResource product, GameResource cost)
        {
            _cost = cost;
            _product = product;
            ChangeCount(count: View.CountController.MinCount);
            View.MainImage.SetData(product);
        }

        private void ChangeCount(int count)
        {
            View.BuyButton.ChangeCost(_cost * count);
        }

        public void Buy()
        {
            int count = View.CountController.Count;

            var cost = _cost * count;

            if (_storageController.CheckResource(cost))
            {
                _storageController.SubtractResource(cost);
                _storageController.AddResource(_product * count);
            }
        }

    }
}