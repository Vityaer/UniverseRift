using Editor.Common;
using Models.Inventory.Splinters;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using Common.Db.CommonDictionaries;
using UnityEditor;
using UnityEngine;

namespace Editor.Pages.Splinters
{
    public class SplinterModelEditor : BaseModelEditor<SplinterModel>
    {
        private List<string> m_currenTypeElements;
        private readonly CommonDictionaries m_commonDictionaries;
        private Sprite m_sprite;

        private List<string> Items => m_commonDictionaries.Items.Select(l => l.Value).Select(r => r.Id).ToList();
        private List<string> Heroes => m_commonDictionaries.Heroes.Select(l => l.Value).Select(r => r.Id).ToList();

        public SplinterModelEditor(SplinterModel model, CommonDictionaries commonDictionaries)
        {
            m_model = model;
            m_commonDictionaries = commonDictionaries;
            
            SetData();
        }

        [ShowInInspector]
        [HorizontalGroup("1")]
        [LabelText("Id")]
        [LabelWidth(150)]
        public string Id
        {
            get => m_model.Id;
            set => m_model.Id = value;
        }

        [ShowInInspector]
        [HorizontalGroup("2")]
        [LabelText("Type")]
        [LabelWidth(150)]
        public SplinterType Type
        {
            get
            {
                switch (m_model.SplinterType)
                {
                    case SplinterType.Hero:
                        m_currenTypeElements = Heroes;
                        break;
                    case SplinterType.Item:
                        m_currenTypeElements = Items;
                        break;
                }
                return m_model.SplinterType;
            }
            set
            {
                switch (value)
                {
                    case SplinterType.Hero:
                        m_currenTypeElements = Heroes;
                        break;
                    case SplinterType.Item:
                        m_currenTypeElements = Items;
                        break;
                }

                m_model.SplinterType = value;
                SetData();
            }
        }

        [ShowInInspector]
        [HorizontalGroup("3")]
        [LabelText("Rewards")]
        [ListDrawerSettings(HideRemoveButton = false, DraggableItems = false, Expanded = true,
            NumberOfItemsPerPage = 5,
            CustomRemoveElementFunction = nameof(RemoveElements), CustomAddFunction = nameof(AddElement))]
        public List<RandomSplinterModel> ModelIds
        {
            get => m_model.ModelIds;
            set => m_model.ModelIds = value;
        }


        [ShowInInspector]
        [HorizontalGroup("4")]
        [LabelText("RequireCount")]
        [LabelWidth(150)]
        public int RequireCount
        {
            get => m_model.RequireCount;
            set => m_model.RequireCount = value;
        }


        private void SetData()
        {
            foreach (RandomSplinterModel model in m_model.ModelIds)
            {
                model.SetData(m_commonDictionaries, m_model.SplinterType);
            }
        }
        
        private void AddElement()
        {
            var newReward = new RandomSplinterModel();
            newReward.SetData(m_commonDictionaries, m_model.SplinterType);
            m_model.ModelIds.Add(newReward);
        }

        private void RemoveElements(RandomSplinterModel light, object b, List<RandomSplinterModel> lights)
        {
            var targetElement = m_model.ModelIds.First(e => e == light);
            m_model.ModelIds.Remove(targetElement);
        }
        
        [ShowInInspector]
        [HorizontalGroup("5")]
        [LabelText("SpritePath")]
        [ShowIf(@"ShowCustomSprite")]
        [PreviewField(100, ObjectFieldAlignment.Left)]
        public Sprite SpritePath
        {
            get
            {
                if (m_sprite == null)
                {
                    m_sprite = AssetDatabase.LoadAssetAtPath<Sprite>(m_model.ImageName);
                }

                return m_sprite;
            }
            set
            {
                m_sprite = value;
                var path = AssetDatabase.GetAssetPath(m_sprite);
                m_model.ImageName = path;
            }
        }
        
        private bool ShowCustomSprite => m_model.ModelIds.Count > 1;
    }
}
