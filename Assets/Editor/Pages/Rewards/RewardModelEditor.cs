using Db.CommonDictionaries;
using Editor.Common;
using Models.Data.Inventories;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UIController.Rewards;

namespace Pages.Rewards
{
    public class RewardModelEditor : BaseModelEditor<RewardModel>
    {
        private CommonDictionaries _dictionaries;

        public RewardModelEditor(RewardModel model, CommonDictionaries _dictionaries)
        {
            _model = model;
        }

        [ShowInInspector]
        [LabelText("Id")]
        [LabelWidth(100)]
        public string Id
        {
            get => _model.Id;
            set => _model.Id = value;
        }

        [ShowInInspector]
        [LabelText("Resources")]
        [LabelWidth(250)]
        public List<ResourceData> Resources
        {
            get => _model.Resources;
            set => _model.Resources = value;
        }

        [ListDrawerSettings(HideRemoveButton = false, DraggableItems = false, Expanded = true,
        NumberOfItemsPerPage = 20,
        CustomRemoveElementFunction = nameof(RemoveItem), CustomAddFunction = nameof(AddItem))]
        public List<ItemData> Items
        {
            get => _model.Items;
            set => _model.Items = value;
        }

        protected void AddItem()
        {
            Items.Add(new ItemData(_dictionaries));
        }

        private void RemoveItem(ItemData light, object b, List<ItemData> lights)
        {
            Items.Remove(light);
        }

        public List<SplinterData> Splinters
        {
            get => _model.Splinters;
            set => _model.Splinters = value;
        }

    }
}
