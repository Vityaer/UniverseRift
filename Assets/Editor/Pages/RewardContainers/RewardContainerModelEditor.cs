using Editor.Common;
using Models.Rewards;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using Common.Db.CommonDictionaries;
using UIController.Rewards;

namespace Pages.RewardContainers
{
    public class RewardContainerModelEditor : BaseModelEditor<RewardContainerModel>
    {
        private CommonDictionaries _commonDictionaries;

        public RewardContainerModelEditor(RewardContainerModel model, CommonDictionaries commonDictionaries)
        {
            m_model = model;
            _commonDictionaries = commonDictionaries;
            foreach (var reward in m_model.Rewards)
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
            get => m_model.Id;
            set => m_model.Id = value;
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
            get => m_model.Rewards;
            set => m_model.Rewards = value;
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
