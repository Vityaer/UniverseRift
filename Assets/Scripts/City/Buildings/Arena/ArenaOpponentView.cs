using TMPro;
using UnityEngine;
public class ArenaOpponentView : MonoBehaviour
{
    [SerializeField] private AvatarView avatarController;
    [SerializeField] private TextMeshProUGUI nameText, levelText, scoreText, winCountText, loseCountText;

    private ArenaOpponent opponent;

    public void SetData(ArenaOpponent newOpponent)
    {
        opponent = newOpponent;
        UpdateUI();
    }
    private void UpdateUI()
    {
        avatarController.SetAvatar(opponent.Avatar);
        nameText.text = opponent.Name;
        levelText.text = string.Concat("Уровень: ", opponent.Level.ToString());
        winCountText.text = opponent.WinCount.ToString();
        loseCountText.text = opponent.LoseCount.ToString();
    }
    public void GoToFight()
    {
        ArenaScript.Instance.FightWithOpponentUseAI(opponent);
    }

}
