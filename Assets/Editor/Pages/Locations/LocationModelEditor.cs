using Editor.Common;
using Models.Fights.Misc;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace Editor.Pages.Locations
{
    [HideReferenceObjectPicker]
    public class LocationModelEditor : BaseModelEditor<LocationModel>
    {
        private Sprite _sprite;

        public LocationModelEditor(LocationModel model)
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
        [LabelText("SpritePath")]
        [PreviewField(100, ObjectFieldAlignment.Left)]
        public Sprite SpritePath
        {
            get
            {
                if (_sprite == null)
                {
                    _sprite = AssetDatabase.LoadAssetAtPath<Sprite>(_model.MissionBackground);
                }

                return _sprite;
            }
            set
            {
                _sprite = value;
                var path = AssetDatabase.GetAssetPath(_sprite);
                _model.MissionBackground = path;
            }
        }
        
        private GameObject _mapPrefab;

        [ShowInInspector]
        [HorizontalGroup("3")]
        [LabelText("MapPrefabPath")]
        [PreviewField(100, ObjectFieldAlignment.Left)]
        public GameObject MapPrefabPath
        {
            get
            {
                if (_mapPrefab == null)
                {
                    _mapPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(_model.MapPrefabPath);
                }

                return _mapPrefab;
            }
            set
            {
                _mapPrefab = value;
                var path = AssetDatabase.GetAssetPath(_mapPrefab);
                _model.MapPrefabPath = path;
            }
        }
    }
}
