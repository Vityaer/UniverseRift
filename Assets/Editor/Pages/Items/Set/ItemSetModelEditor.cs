using System.Collections.Generic;
using Editor.Common;
using Models;
using Sirenix.OdinInspector;
using UIController.Inventory;

namespace Editor.Pages.Items.Set
{
    [HideReferenceObjectPicker]
    public class ItemSetModelEditor : BaseModelEditor<ItemSet>
    {
        public ItemSetModelEditor(ItemSet model)
        {
            _model = model;
        }

        [ShowInInspector]
        [HorizontalGroup("1")]
        [LabelText("Id")]
        [LabelWidth(50)]
        public string Id
        {
            get => _model.Id;
            set => _model.Id = value;
        }

        [ShowInInspector]
        [HorizontalGroup("2")]
        [LabelText("Name")]
        [LabelWidth(50)]
        public string Name
        {
            get => _model.Name;
            set => _model.Name = value;
        }
        
        [ShowInInspector]
        [HorizontalGroup("Item")]
        [VerticalGroup("Item/Left")]
        [BoxGroup("Item/Left/Common")]
        [LabelText("Bonuses")]
        [PropertyOrder(2)]
        [LabelWidth(110)]
        public List<Bonus> ExtraBonuses
        {
            get => _model.ExtraBonuses;
            set => _model.ExtraBonuses = value;
        }
    }
}

