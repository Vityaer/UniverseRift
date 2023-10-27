using Db.CommonDictionaries;
using Editor.Common;
using Models.Rewards;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UIController.Rewards;

namespace Pages.RewardContainers
{
    public class RewardContainerModelEditor : BaseModelEditor<RewardContainerModel>
    {
        private CommonDictionaries _commonDictionaries;

        public RewardContainerModelEditor(RewardContainerModel model, CommonDictionaries commonDictionaries)
        {
            _model = model;
            _commonDictionaries = commonDictionaries;
            foreach (var reward in _model.Rewards)
            {
                reward.CommonDictionaries = _commonDictionaries;
            }
        }

        [ShowInInspector]
        [LabelText("Id")]
        [HorizontalGroup("1")]
        [LabelWidth(150)]
        public string Id
        {
            get => _model.Id;
            set => _model.Id = value;
        }

        [ShowInInspector]
        [HorizontalGroup("2")]
        [LabelText("Rewards")]
        [LabelWidth(250)]
        [ListDrawerSettings(Expanded = false,
      NumberOfItemsPerPage = 20,
          CustomRemoveElementFunction = nameof(RemoveReward), CustomAddFunction = nameof(AddReward))]
        public List<RewardModel> Rewards
        {
            get => _model.Rewards;
            set => _model.Rewards = value;
        }

        protected void AddReward()
        {
            var newRewardModel = new RewardModel();
            newRewardModel.CommonDictionaries = _commonDictionaries;
            Rewards.Add(newRewardModel);
        }

        private void RemoveReward(RewardModel light, object b, List<RewardModel> lights)
        {
            Rewards.Remove(light);
        }
    }
}
