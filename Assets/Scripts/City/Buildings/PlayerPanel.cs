using City.Buildings.General;
using City.TrainCamp;
using Common;
using Common.Resourses;
using MainScripts;
using Models;
using TMPro;
using UIController.ItemVisual;
using UIController.Rewards;
using UnityEngine;
using UnityEngine.UI;

namespace City.Buildings
{
    public class PlayerPanel : Building
    {
        [SerializeField] private ItemSliderController sliderLevel;
        [SerializeField] private TextMeshProUGUI nameText, levelText, IDGuildText, idText;
        [SerializeField] private CostLevelUp playerLevelList, rewardForLevelUp;
        [SerializeField] private Image avatar, outlineAvatar;

        private PlayerInfoModel playerInfo;
        private Resource requireExpForLevel, currentExp;

        public string GetName => playerInfo.Name;

        protected override void OnLoadGame()
        {
            playerInfo = GameController.Instance.player.GetPlayerInfo;
            GameController.Instance.RegisterOnChangeResource(ChangeExp, TypeResource.Exp);
            requireExpForLevel = GetRequireExpForLevel();
            currentExp = GameController.Instance.GetResource(TypeResource.Exp);
        }

        protected override void OpenPage()
        {
            UpdateMainUI();
        }

        private void UpdateMainUI()
        {
            nameText.text = playerInfo.Name;
            levelText.text = playerInfo.Level.ToString();
            IDGuildText.text = playerInfo.IDGuild.ToString();
            // avatar.sprite = playerInfo.avatar;
            sliderLevel.SetAmount(currentExp, requireExpForLevel);
        }

        //Exp
        public void ChangeExp(Resource newExp)
        {
            if (newExp.CheckCount(requireExpForLevel))
            {
                GameController.Instance.UnregisterOnChangeResource(ChangeExp, TypeResource.Exp);
                GameController.Instance.SubtractResource(requireExpForLevel);
                GameController.Instance.player.LevelUP();
                requireExpForLevel = GetRequireExpForLevel();
                var reward = new Reward(rewardForLevelUp.GetCostForLevelUp(playerInfo.Level));
                MessageController.Instance.OpenPanelNewLevel(reward);
                GameController.Instance.RegisterOnChangeResource(ChangeExp, TypeResource.Exp);
                UpdateMainUI();
            }
        }
        private Resource GetRequireExpForLevel()
        {
            return playerLevelList.GetCostForLevelUp(playerInfo.Level + 1).GetResource(TypeResource.Exp);
        }

        public void SaveNewName(string newName)
        {
            playerInfo.SetNewName(newName);
            SaveGame();
        }

    }
}