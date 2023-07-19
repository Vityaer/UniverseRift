using Db.CommonDictionaries;
using System.Collections.Generic;
using Ui.Misc.Widgets;
using UIController.Cards;
using UIController.Rewards.PosibleRewards;
using UiExtensions.Scroll.Interfaces;
using UnityEngine;
using VContainer;

namespace City.Panels.PosibleHeroes
{
    public class PosibleHeroesPanelController : UiPanelController<PosibleHeroesPanelView>
    {
        [Inject] private readonly CommonDictionaries _commonDictionaries; 
        public List<CardWithPercent> cardsWithPercent = new List<CardWithPercent>();

        public void SetData(PosibleRewardData rewardInfo)
        {
            //CheckCountAvailableCard(rewardInfo.PosibilityObjectRewards.Count);
            //FillData(rewardInfo);
        }

        private void CheckCountAvailableCard(int requireCount)
        {
            if (cardsWithPercent.Count < requireCount)
                for (int i = cardsWithPercent.Count; i < requireCount; i++)
                    cardsWithPercent.Add(Object.Instantiate(View.Prefab, View.Content));
        }

        private void FillData(PosibleRewardData rewardInfo)
        {
            //for (int i = 0; i < rewardInfo.PosibilityObjectRewards.Count; i++)
            //{
            //    var name = rewardInfo.PosibilityObjectRewards[i].ModelId;
            //    cardsWithPercent[i].SetData(_commonDictionaries.Heroes[name], rewardInfo.PosibleNumObject(i));
            //}
            //for (int i = rewardInfo.PosibilityObjectRewards.Count; i < cardsWithPercent.Count; i++)
            //{
            //    cardsWithPercent[i].Hide();
            //}
        }
    }
}