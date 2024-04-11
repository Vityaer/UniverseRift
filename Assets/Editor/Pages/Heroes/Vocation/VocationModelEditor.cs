using Editor.Common;
using Models.Heroes;
using Newtonsoft.Json;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace Editor.Pages.Heroes.Vocation
{
    [HideReferenceObjectPicker]
    public class VocationModelEditor : BaseModelEditor<VocationModel>
    {
        public VocationModelEditor(VocationModel model)
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
        [HorizontalGroup("3")]
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
