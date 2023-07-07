using Common.Resourses;
using Models.Common.BigDigits;
using Models.Data.Players;
using System;
using UiExtensions.Scroll.Interfaces;
using UniRx;
using Utils;

namespace City.Buildings.PlayerPanels
{
    public class PlayerPanelController : UiPanelController<PlayerPanelView>
    {
        private PlayerInfoData _playerInfo;
        private GameResource _requireExpForLevel;
        private GameResource _currentExp;
        private ReactiveCommand<BigDigit> _onLevelUp;

        public IObservable<BigDigit> OnLevelUp => _onLevelUp;
        public PlayerInfoData PlayerInfoData => _playerInfo;

        protected override void OnLoadGame()
        {
            //_playerInfo = GameController.Instance.player.GetPlayerInfo;
            //GameController.Instance.RegisterOnChangeResource(ChangeExp, ResourceType.Exp);
            //_requireExpForLevel = GetRequireExpForLevel();
            //_currentExp = GameController.Instance.GetResource(ResourceType.Exp);
        }

        public void LevelUp()
        {
            _playerInfo.Level += 1;
            _onLevelUp.Execute(new BigDigit(_playerInfo.Level, 0));
        }

        protected void OpenPage()
        {
            UpdateMainUI();
        }

        private void UpdateMainUI()
        {
            View.Name.text = _playerInfo.Name;
            View.Level.text = _playerInfo.Level.ToString();
            View.GuildId.text = _playerInfo.IDGuild.ToString();
            View.Avatar.sprite = SpriteUtils.LoadSprite(_playerInfo.AvatarPath);
            View.SliderExp.SetAmount(_currentExp, _requireExpForLevel);
        }

        //Exp
        public void ChangeExp(GameResource newExp)
        {
            if (newExp.CheckCount(_requireExpForLevel))
            {
                //GameController.Instance.UnregisterOnChangeResource(ChangeExp, ResourceType.Exp);
                //GameController.Instance.SubtractResource(_requireExpForLevel);
                //GameController.Instance.player.LevelUP();
                //_requireExpForLevel = GetRequireExpForLevel();
                //var reward = new Reward(rewardForLevelUp.GetCostForLevelUp(playerInfo.Level));
                //MessageController.Instance.OpenPanelNewLevel(reward);
                //GameController.Instance.RegisterOnChangeResource(ChangeExp, ResourceType.Exp);
                //UpdateMainUI();
            }
        }
        //private GameResource GetRequireExpForLevel()
        //{
        //    return playerLevelList.GetCostForLevelUp(playerInfo.Level + 1).GetResource(ResourceType.Exp);
        //}

        public void SaveNewName(string newName)
        {
            //playerInfo.SetNewName(newName);
            //Utils.TextUtils.Save(_сommonGameData);
        }

    }
}