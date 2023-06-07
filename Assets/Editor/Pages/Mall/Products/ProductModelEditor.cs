using Editor.Common;
using Models;
using Sirenix.OdinInspector;

namespace Editor.Pages.Mall.Products
{
    [HideReferenceObjectPicker]
    public class ProductModelEditor : BaseModelEditor<ProductModel>
    {
        public ProductModelEditor(ProductModel model)
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
    }
}
