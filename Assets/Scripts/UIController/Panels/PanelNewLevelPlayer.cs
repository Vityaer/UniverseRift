using Common;
using TMPro;
using UIController.MessagePanels;
using UIController.Rewards;

public class PanelNewLevelPlayer : RewardPanel
{
    public TextMeshProUGUI textNewLevel;
    public void Open(Reward reward)
    {
        int newLevel = GameController.Instance.player.GetPlayerInfo.Level;
        textNewLevel.text = newLevel.ToString();
        base.Open(reward);
    }
}
