using Editor.Common;
using Models.Heroes;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace Editor.Pages.Heroes.Race
{
    [HideReferenceObjectPicker]
    public class RaceModelEditor : BaseModelEditor<RaceModel>
    {
        public RaceModelEditor(RaceModel model)
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
    }
}
