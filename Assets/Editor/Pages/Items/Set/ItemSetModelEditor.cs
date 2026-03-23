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
            m_model = model;
        }

        [ShowInInspector]
        [HorizontalGroup("1")]
        [LabelText("Id")]
        [LabelWidth(50)]
        public string Id
        {
            get => m_model.Id;
            set => m_model.Id = value;
        }

        [ShowInInspector]
        [HorizontalGroup("2")]
        [LabelText("Name")]
        [LabelWidth(50)]
        public string Name
        {
            get => m_model.Name;
            set => m_model.Name = value;
        }
        
        [ShowInInspector]
        [HorizontalGroup("3")]
        [LabelText("Rating")]
        [LabelWidth(50)]
        public int Rating
        {
            get => m_model.Rating;
            set => m_model.Rating = value;
        }
        
        [ShowInInspector]
        [HorizontalGroup("Item")]
        [VerticalGroup("Item/Left")]
        [BoxGroup("Item/Left/Common")]
        [LabelText("Bonuses")]
        [PropertyOrder(4)]
        [LabelWidth(110)]
        public List<Bonus> ExtraBonuses
        {
            get => m_model.ExtraBonuses;
            set => m_model.ExtraBonuses = value;
        }
    }
}

