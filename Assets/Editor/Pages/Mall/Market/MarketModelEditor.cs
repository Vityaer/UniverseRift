using Editor.Common;
using Models.City.Markets;
using Sirenix.OdinInspector;

namespace Editor.Pages.Mall.Market
{
    public class MarketModelEditor : BaseModelEditor<MarketModel>
    {
        public MarketModelEditor(MarketModel model)
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
