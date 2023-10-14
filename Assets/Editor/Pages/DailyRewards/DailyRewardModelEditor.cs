using Editor.Common;
using Models.Achievments;
using Models.Data.Dailies;
using Models.Data.Inventories;
using Sirenix.OdinInspector;
using System.Collections.Generic;

namespace Editor.Pages.DailyRewards
{
    public class DailyRewardModelEditor : BaseModelEditor<DailyRewardModel>
    {
        public DailyRewardModelEditor(DailyRewardModel model)
        {
            _model = model;
        }

        [ShowInInspector]
        [HorizontalGroup("1")]
        [LabelText("Id")]
        [LabelWidth(200)]
        public string Id
        {
            get => _model.Id;
            set => _model.Id = value;
        }

        [ShowInInspector]
        [HorizontalGroup("2")]
        [LabelText("Rewards")]
        [LabelWidth(200)]
        public List<InventoryBaseItem> Rewards
        {
            get => _model.Rewards;
            set => _model.Rewards = value;
        }
    }
}
