using System.Collections.Generic;
using System.Linq;
using Common.Db.CommonDictionaries;
using Sirenix.OdinInspector;

namespace Models.Inventory.Splinters
{
    public class RandomSplinterModel
    {
        [ListDrawerSettings(ShowItemCount = true, ShowIndexLabels = true, Expanded = true, DraggableItems = false)]
        [LabelText("ModelId")]
        [ValueDropdown(nameof(m_currenTypeElements), IsUniqueList = true, DropdownWidth = 250,
            SortDropdownItems = true)]
        public string ModelId;

        public int Valuation;

        private List<string> m_currenTypeElements;

        private CommonDictionaries m_commonDictionaries;

        public RandomSplinterModel()
        {
        }

        public RandomSplinterModel(string modelId, int valuation)
        {
            ModelId = modelId;
            Valuation = valuation;
        }

        public void SetData(CommonDictionaries commonDictionaries, SplinterType splinterType)
        {
            m_commonDictionaries = commonDictionaries;
            m_currenTypeElements = splinterType == SplinterType.Hero
                ? m_commonDictionaries.Heroes.Select(l => l.Value).Select(r => r.Id).ToList()
                : m_commonDictionaries.Items.Select(l => l.Value).Select(r => r.Id).ToList();
        }
    }
}