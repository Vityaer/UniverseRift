using City.TrainCamp;
using Editor.Common;
using Models.City.Mines;
using Models.Data.Inventories;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Editor.Pages.City.Mines
{
    [HideReferenceObjectPicker]
    public class MineModelEditor : BaseModelEditor<MineModel>
    {
        public MineModelEditor(MineModel model)
        {
            _model = model;
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

        private Sprite _sprite;

        [ShowInInspector]
        [HorizontalGroup("2")]
        [LabelText("SpritePath")]
        [PreviewField(100, ObjectFieldAlignment.Left)]
        public Sprite SpritePath
        {
            get
            {
                if (_sprite == null)
                {
                    _sprite = AssetDatabase.LoadAssetAtPath<Sprite>(_model.SpritePath);
                }

                return _sprite;
            }
            set
            {
                _sprite = value;
                var path = AssetDatabase.GetAssetPath(_sprite);
                _model.SpritePath = path;
            }
        }

        [ShowInInspector]
        [HorizontalGroup("3")]
        [LabelText("IncomesContainer")]
        [LabelWidth(150)]
        public CostLevelUpContainer IncomesContainer
        {
            get => _model.IncomesContainer;
            set => _model.IncomesContainer = value;
        }

        [ShowInInspector]
        [HorizontalGroup("4")]
        [LabelText("CostLevelUpContainer")]
        [LabelWidth(150)]
        public CostLevelUpContainer CostLevelUpContainer
        {
            get => _model.CostLevelUpContainer;
            set => _model.CostLevelUpContainer = value;
        }

        [ShowInInspector]
        [HorizontalGroup("5")]
        [LabelText("CreateCost")]
        [LabelWidth(150)]
        public List<ResourceData> CreateCost
        {
            get => _model.CreateCost;
            set => _model.CreateCost = value;
        }
    }
}
