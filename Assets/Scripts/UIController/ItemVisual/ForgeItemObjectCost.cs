using TMPro;
using UnityEngine;

public class ForgeItemObjectCost : MonoBehaviour
{
    public TextMeshProUGUI textAmount;
    private Item requireItem;
    private int amountRequire;

    public void SetInfo(Item item, int amount)
    {
        this.requireItem = item;
        this.amountRequire = amount;
        CheckItems();
    }

    public void CheckItems()
    {
        int currentCount = InventoryController.Instance.HowManyThisItems(requireItem);
        string result = (currentCount >= amountRequire) ? "<color=black>" : "<color=red>";

        result = string.Concat(result, currentCount.ToString(), "</color>/", amountRequire.ToString());
        textAmount.text = result;
    }
}
