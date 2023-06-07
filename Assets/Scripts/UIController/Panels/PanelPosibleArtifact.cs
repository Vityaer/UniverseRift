using System.Collections.Generic;
using UIController.ControllerPanels;
using UIController.Inventory;
using UIController.ItemVisual;
using UnityEngine;

public class PanelPosibleArtifact : BasePanelScript
{
    public Transform content;
    public GameObject prefab;
    public List<ArtifactWithPercent> cardsWithPercent = new List<ArtifactWithPercent>();

    public void Open(PosibleReward rewardInfo)
    {
        CheckCountAvailableCard(rewardInfo.posibilityObjectRewards.Count);
        FillData(rewardInfo);
        base.Open();
    }

    private void CheckCountAvailableCard(int requireCount)
    {
        if (cardsWithPercent.Count < requireCount)
            for (int i = cardsWithPercent.Count; i < requireCount; i++)
                cardsWithPercent.Add(Instantiate(prefab, transform).GetComponent<ArtifactWithPercent>());
    }

    private void FillData(PosibleReward rewardInfo)
    {
        for (int i = 0; i < rewardInfo.posibilityObjectRewards.Count; i++)
        {
            cardsWithPercent[i].SetData(rewardInfo.posibilityObjectRewards[i].ID, rewardInfo.PosibleNumObject(i));
        }

        for (int i = rewardInfo.posibilityObjectRewards.Count; i < cardsWithPercent.Count; i++)
        {
            cardsWithPercent[i].Hide();
        }
    }
}
