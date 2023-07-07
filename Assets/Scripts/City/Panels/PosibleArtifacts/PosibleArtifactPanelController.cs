using System.Collections.Generic;
using UIController.ItemVisual;
using UIController.Rewards.PosibleRewards;
using UiExtensions.Scroll.Interfaces;
using UnityEngine;

namespace City.Panels.PosibleArtifacts
{
    public class PosibleArtifactPanelController : UiPanelController<PosibleArtifactPanelView>
    {
        public List<ArtifactWithPercent> cardsWithPercent = new List<ArtifactWithPercent>();

        public void SetData(PosibleReward rewardInfo)
        {
            CheckCountAvailableCard(rewardInfo.PosibilityObjectRewards.Count);
            FillData(rewardInfo);
        }

        private void CheckCountAvailableCard(int requireCount)
        {
            if (cardsWithPercent.Count < requireCount)
                for (int i = cardsWithPercent.Count; i < requireCount; i++)
                    cardsWithPercent.Add(Object.Instantiate(View.Prefab, View.Content));
        }

        private void FillData(PosibleReward rewardInfo)
        {
            for (int i = 0; i < rewardInfo.PosibilityObjectRewards.Count; i++)
            {
                cardsWithPercent[i].SetData(rewardInfo.PosibilityObjectRewards[i].ModelId, rewardInfo.PosibleNumObject(i));
            }

            for (int i = rewardInfo.PosibilityObjectRewards.Count; i < cardsWithPercent.Count; i++)
            {
                cardsWithPercent[i].Hide();
            }
        }
    }
}