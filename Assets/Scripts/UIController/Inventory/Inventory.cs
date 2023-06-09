using Models;
using System;
using System.Collections.Generic;
using UIController.GameSystems;
using UnityEngine;

namespace UIController.Inventory
{
    [Serializable]
    public class Inventory
    {
        public List<ItemController> items = new List<ItemController>();
        public List<SplinterController> splinters = new List<SplinterController>();

        private Action OnChange;

        public void RegisterOnChange(Action d) { OnChange += d; }
        public void UnregisterOnChange(Action d) { OnChange -= d; }

        public void GetAll(List<VisualAPI> list)
        {
            list.Clear();
            foreach (ItemController controller in items) list.Add(controller as VisualAPI);
            foreach (SplinterController controller in splinters) list.Add(controller as VisualAPI);
        }

        public void GetItemAtType(string typeItems, List<VisualAPI> list)
        {
            list.Clear();
            foreach (ItemController controller in items) if (controller.item.Type == typeItems) list.Add(controller as VisualAPI);
        }

        public void Add(ItemController itemController)
        {
            Debug.Log("add item");
            ItemController workItem = items.Find(x => x.item.Id == itemController.item.Id);
            if (workItem != null)
            {
                workItem.IncreaseAmount(itemController.Amount);
            }
            else
            {
                items.Add((ItemController)itemController.Clone());
            }
            OnChange();
        }

        public void Add(SplinterController splinterController)
        {
            SplinterController workSplinter = splinters.Find(x => x.splinter.Id == splinterController.splinter.Id);
            if (workSplinter != null)
            {
                workSplinter.IncreaseAmount(splinterController.splinter.Amount);
            }
            else
            {
                splinters.Add((SplinterController)splinterController.Clone());
            }
            OnChange();
        }

        public void Add(List<ItemController> items)
        {
            for (int i = 0; i < items.Count; i++) Add(items[i]);
            OnChange();
        }

        public void Add(List<SplinterController> splinters)
        {
            for (int i = 0; i < splinters.Count; i++) Add(splinters[i]);
            OnChange();
        }

        public void RemoveSplinter(Splinter splinerForDelete)
        {
            SplinterController controller = splinters.Find(x => x.splinter == splinerForDelete);
            if (controller == null) Debug.Log("not found splinter for delete");
            splinters.Remove(controller);
            OnChange();
        }

        public Inventory(InventorySave inventorySave)
        {
            Item _item;
            Splinter _splinter;
            ItemsList itemsList = Resources.Load<ItemsList>("Items/ListItems");

            foreach (var item in inventorySave.listItem)
            {
                _item = itemsList?.GetItem(item.Id);
                if (_item != null)
                {
                    items.Add(new ItemController(_item, item.Amount));
                }
            }

            foreach (Models.SplinterModel item in inventorySave.listSplinter)
            {
                var name = item.Id;
                _splinter = SplinterSystem.Instance.GetSplinter(name);
                if (_splinter != null)
                {
                    splinters.Add(new SplinterController(_splinter, item.Amount));
                }
            }
        }

        public Inventory() { }
    }
}