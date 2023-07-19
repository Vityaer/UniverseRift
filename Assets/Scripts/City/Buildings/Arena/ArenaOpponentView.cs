using Models.City.Arena;
using TMPro;
using UController.Other;
using UnityEngine;

namespace City.Buildings.Arena
{
    public class ArenaOpponentView : MonoBehaviour
    {
        [SerializeField] private AvatarView avatarController;
        [SerializeField] private TextMeshProUGUI nameText, levelText, scoreText, winCountText, loseCountText;

        private ArenaOpponentModel opponent;

        public void SetData(ArenaOpponentModel newOpponent)
        {
            opponent = newOpponent;
            UpdateUI();
        }
        private void UpdateUI()
        {
            //avatarController.SetAvatar(opponent.Avatar);
            nameText.text = opponent.Name;
            levelText.text = string.Concat("Уровень: ", opponent.Level.ToString());
            winCountText.text = opponent.WinCount.ToString();
            loseCountText.text = opponent.LoseCount.ToString();
        }
        public void GoToFight()
        {
            //ArenaController.Instance.FightWithOpponentUseAI(opponent);
        }

    }
}