using Editor.Common;
using Models.Inventory.Splinters;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using Common.Db.CommonDictionaries;

namespace Editor.Pages.Splinters
{
    public class SplinterModelEditor : BaseModelEditor<SplinterModel>
    {
        private List<string> _currenTypeElements;
        private CommonDictionaries _commonDictionaries;

        private List<string> _items => _commonDictionaries.Items.Select(l => l.Value).Select(r => r.Id).ToList();
        private List<string> _heroes => _commonDictionaries.Heroes.Select(l => l.Value).Select(r => r.Id).ToList();

        public SplinterModelEditor(SplinterModel model, CommonDictionaries commonDictionaries)
        {
            _model = model;
            _commonDictionaries = commonDictionaries;
        }

        [ShowInInspector]
        [HorizontalGroup("1")]
        [LabelText("Id")]
        [LabelWidth(150)]
        public string Id
        {
            get => _model.Id;
            set => _model.Id = value;
        }

        [ShowInInspector]
        [HorizontalGroup("2")]
        [LabelText("Type")]
        [LabelWidth(150)]
        public SplinterType Type
        {
            get
            {
                switch (_model.SplinterType)
                {
                    case SplinterType.Hero:
                        _currenTypeElements = _heroes;
                        break;
                    case SplinterType.Item:
                        _currenTypeElements = _items;
                        break;
                    case SplinterType.RandomReward:
                        break;
                }
                return _model.SplinterType;
            }
            set
            {
                switch (value)
                {
                    case SplinterType.Hero:
                        _currenTypeElements = _heroes;
                        break;
                    case SplinterType.Item:
                        _currenTypeElements = _items;
                        break;
                    case SplinterType.RandomReward:
                        break;
                }

                _model.SplinterType = value;
            }
        }

        [ShowInInspector]
        [HorizontalGroup("3")]
        [ListDrawerSettings(ShowItemCount = true, ShowIndexLabels = true, Expanded = true, DraggableItems = false)]
        [LabelText("ModelId")]
        [ValueDropdown(nameof(_currenTypeElements), IsUniqueList = true, DropdownWidth = 250, SortDropdownItems = true)]
        public string ModelId
        {
            get
            {
                var result = _model.ModelId;
                if (string.IsNullOrEmpty(result))
                {
                    result = _currenTypeElements.FirstOrDefault();
                }

                return result;
            }
            set => _model.ModelId = value;
        }

        [ShowInInspector]
        [HorizontalGroup("4")]
        [LabelText("RequireCount")]
        [LabelWidth(150)]
        public int RequireCount
        {
            get => _model.RequireCount;
            set => _model.RequireCount = value;
        }


    }
}
