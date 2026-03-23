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
        [LabelText("SpritePath")]
        [PreviewField(100, ObjectFieldAlignment.Left)]
        public Sprite SpritePath
        {
            get
            {
                if (_sprite == null)
                {
                    _sprite = AssetDatabase.LoadAssetAtPath<Sprite>(m_model.MissionBackground);
                }

                return _sprite;
            }
            set
            {
                _sprite = value;
                var path = AssetDatabase.GetAssetPath(_sprite);
                m_model.MissionBackground = path;
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
                    _mapPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(m_model.MapPrefabPath);
                }

                return _mapPrefab;
            }
            set
            {
                _mapPrefab = value;
                var path = AssetDatabase.GetAssetPath(_mapPrefab);
                m_model.MapPrefabPath = path;
            }
        }
    }
}
