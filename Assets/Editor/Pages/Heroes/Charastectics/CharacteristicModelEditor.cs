using Editor.Common;
using Models.Heroes.HeroCharacteristics.Abstractions;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace Assets.Editor.Pages.Heroes.Charastectics
{
    public class CharacteristicModelEditor : BaseModelEditor<CharacteristicModel>
    {
        public CharacteristicModelEditor(CharacteristicModel model)
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

        [ShowInInspector]
        [HorizontalGroup("2")]
        [LabelText("Name")]
        [LabelWidth(150)]
        public string Name
        {
            get => _model.Name;
            set => _model.Name = value;
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
