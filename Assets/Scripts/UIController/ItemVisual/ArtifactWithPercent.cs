using Common.Db.CommonDictionaries;
using Models;
using TMPro;
using UIController.Inventory;
using UiExtensions.Misc;
using UnityEngine;
using UnityEngine.UI;
using Utils.DataCheckUtils;

namespace UIController.ItemVisual
{
    public class ArtifactWithPercent : ScrollableUiView<GameRandomSplinterData>
    {
        [SerializeField] private SubjectCell m_artifactInfo;
        [SerializeField] private  TMP_Text m_textPercent;

        private ScrollRect m_scrollRect;
        private CommonDictionaries m_dictionaries;
        
        public void Init(ScrollRect scrollRect, CommonDictionaries dictionaries)
        {
            m_scrollRect = scrollRect.ThrowIfArgumentNull();
            m_dictionaries = dictionaries.ThrowIfArgumentNull();
        }

        protected override void OnSetData()
        {
            float percent = Data.Percentage * 100;
            string number = percent switch
            {
                > 10 => percent.ToString("N1"),
                > 1 => percent.ToString("N2"),
                _ => percent.ToString("N3")
            };

            m_textPercent.text = string.Concat(number, "%");
            
            m_artifactInfo.Init(m_scrollRect, m_dictionaries);
            
            if (m_dictionaries.Items.TryGetValue(Data.Model.ModelId, out ItemModel artifact))
            {
                GameItem gameItem = new(artifact);
                m_artifactInfo.SetData(gameItem);
            }

            base.OnSetData();
        }
    }
}