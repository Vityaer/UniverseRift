using Common.Db.CommonDictionaries;
using Editor.Common;
using Models.City.Markets;
using Sirenix.OdinInspector;

namespace Editor.Pages.Mall.Market
{
    public class MarketModelEditor : BaseModelEditor<MarketModel>
    {
        public MarketModelEditor(MarketModel model, CommonDictionaries dictionaries)
        {
            _model = model;
            _model.CommonDictionaries = dictionaries;
        }

        [ShowInInspector]
        [HorizontalGroup("1")]
        [LabelWidth(150)]
        public MarketModel Model
        {
            get => _model;
            set => _model = value;
        }
    }
}
