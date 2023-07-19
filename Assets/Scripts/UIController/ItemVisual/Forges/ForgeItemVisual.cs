using City.Buildings.Forge;
using City.TrainCamp;
using Common.Resourses;
using System;
using UIController.Inventory;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace UIController.ItemVisual.Forges
{
    public class ForgeItemVisual : MonoBehaviour, IDisposable
    {
        [Header("Info")]
        public TypeMatter matter;
        public GameItemRelation Thing;
        public Image _imageBorder;
        private GameItem item;
        public SubjectCell Cell;
        private bool _selected = false;
        private CompositeDisposable _disposables = new CompositeDisposable();
        private ReactiveCommand<ForgeItemVisual> _onSelected = new ReactiveCommand<ForgeItemVisual>();

        public IObservable<ForgeItemVisual> OnSelected => _onSelected;

        private void Start()
        {
            Cell.OnSelect.Subscribe(_ => SelectItem()).AddTo(_disposables);
        }

        public void SetItem(GameItemRelation item)
        {
            Thing = item;
            Cell.SetData(Thing.Result);
        }

        public void SetItem(GameItem item)
        {
            this.item = item;
            Cell.SetData(item);
        }

        private void SelectItem()
        {
            if (matter == TypeMatter.Synthesis)
            {
                if (!_selected)
                {
                    _onSelected.Execute(this);
                }
            }
        }

        public void Diselect()
        {
            _imageBorder.enabled = false;
            _selected = false;
        }

        public void Select()
        {
            _imageBorder.enabled = true;
            _selected = true;
        }

        public void Dispose()
        {
            _disposables.Dispose();
        }
    }
}