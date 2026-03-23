using Editor.Common;
using Models.Data.Dailies;
using Models.Data.Inventories;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using Common.Db.CommonDictionaries;

namespace Editor.Pages.DailyRewards
{
    public class DailyRewardModelEditor : BaseModelEditor<DailyRewardModel>
    {
        private CommonDictionaries _dictionaries;

        public DailyRewardModelEditor(DailyRewardModel model, CommonDictionaries dictionaries)
        {
            m_model = model;
            _dictionaries = dictionaries;
        }

        [ShowInInspector]
        [HorizontalGroup("1")]
        [LabelText("Id")]
        [LabelWidth(200)]
        public string Id
        {
            get => m_model.Id;
            set => m_model.Id = value;
        }

        [Button]
        [ShowInInspector]
        [HorizontalGroup("2")]
        private void AddItem()
        {
            Rewards.Add(new ItemData(_dictionaries));
        }

        [ShowInInspector]
        [HorizontalGroup("3")]
        [LabelText("Rewards")]
        [LabelWidth(200)]
        [ListDrawerSettings(HideRemoveButton = false, DraggableItems = true, Expanded = true,
        NumberOfItemsPerPage = 20,
        CustomRemoveElementFunction = nameof(RemoveReward))]
        public List<InventoryBaseItem> Rewards
        {
            get => m_model.Rewards;
            set => m_model.Rewards = value;
        }

        private void RemoveReward(InventoryBaseItem light, object b, List<InventoryBaseItem> lights)
        {
            Rewards.Remove(light);
        }
    }
}
