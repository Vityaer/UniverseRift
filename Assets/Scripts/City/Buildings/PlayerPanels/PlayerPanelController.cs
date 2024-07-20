using City.Buildings.PlayerPanels.AvatarPanels;
using City.Buildings.PlayerPanels.AvatarPanels.AvatarPanelDetails;
using City.Panels.NewLevels;
using ClientServices;
using Common.Resourses;
using Cysharp.Threading.Tasks;
using Db.CommonDictionaries;
using Misc.Json;
using Models.Common;
using Models.Common.BigDigits;
using Models.Data.Players;
using Models.Misc.Avatars;
using Network.DataServer;
using Network.DataServer.Messages.Players;
using System;
using UIController.Rewards;
using UiExtensions.Scroll.Interfaces;
using UniRx;
using Utils;
using VContainer;
using VContainerUi.Model;
using VContainerUi.Messages;

namespace City.Buildings.PlayerPanels
{
    public class PlayerPanelController : UiPanelController<PlayerPanelView>
    {
        [Inject] private readonly CommonGameData _commonGameData;
        [Inject] private readonly ResourceStorageController _resourceStorageController;
        [Inject] private readonly CommonDictionaries _dictionaries;
        [Inject] private readonly PlayerNewLevelPanelController _playerNewLevelPanelController;
        [Inject] private readonly IJsonConverter _jsonConverter;
        [Inject] private readonly AvatarPanelDetailsController _avatarPanelDetailsController;
        [Inject] private readonly AvatarPanelController _avatarPanelController;

        private PlayerData _playerInfo;
        private GameResource _requireExpForLevel;
        private GameResource _currentExp;
        private ReactiveCommand<BigDigit> _onLevelUp = new();
        private CompositeDisposable _disposables = new CompositeDisposable();

        public IObservable<BigDigit> OnLevelUp => _onLevelUp;
        public PlayerData PlayerInfoData => _playerInfo;

        public override void OnShow()
        {
            View.AvatarButton.OnClickAsObservable().Subscribe(_ => OpenAvatarsPanel()).AddTo(_disposables);
            _avatarPanelDetailsController.OnSelectNewAvatar.Subscribe(ChangeAvatar).AddTo(_disposables);
            base.OnShow();
        }

        private void OpenAvatarsPanel()
        {
            MessagesPublisher.OpenWindowPublisher.OpenWindow<AvatarPanelController>(openType: OpenType.Exclusive);
        }

        protected override void OnLoadGame()
        {
            _playerInfo = _commonGameData.PlayerInfoData;
            _resourceStorageController.Subscribe(ResourceType.Exp, ChangeExp).AddTo(_disposables);
            UpdateData();
        }

        private void ChangeAvatar(AvatarModel avatar)
        {
            View.Avatar.sprite = SpriteUtils.LoadSprite(avatar.Path);
        }

        private void UpdateData()
        {
            View.Name.text = $"name: {_playerInfo.Name}";
            View.PlayerId.text = $"id: {_playerInfo.Id}";
            View.Level.text = $"{_playerInfo.Level}";
            View.Avatar.sprite = SpriteUtils.LoadSprite(_playerInfo.AvatarPath);
            UpdateExp();
        }

        private void UpdateExp()
        {
            _currentExp = _resourceStorageController.Resources[ResourceType.Exp];
            _requireExpForLevel = GetRequireExpForLevel(_playerInfo.Level);
            View.SliderExp.SetAmount(_currentExp, _requireExpForLevel);
        }

        public void LevelUp()
        {
            _playerInfo.Level += 1;
            View.Level.text = $"{_playerInfo.Level}";
            _onLevelUp.Execute(new BigDigit(_playerInfo.Level, 0));
        }

        public void ChangeExp(GameResource newExp)
        {
            UpdateExp();
            if (newExp.CheckCount(_requireExpForLevel))
            {
                TryLevelUp().Forget();
            }
        }

        private async UniTaskVoid TryLevelUp()
        {
            var message = new PlayerNewLevelMessage { PlayerId = _playerInfo.Id };
            var result = await DataServer.PostData(message);
            var reward = _jsonConverter.Deserialize<RewardModel>(result);

            LevelUp();
            _playerNewLevelPanelController.SetData(reward);
            UpdateExp();
        }

        private GameResource GetRequireExpForLevel(int level)
        {
            return _dictionaries.CostContainers["PlayerLevels"].GetCostForLevelUp(level)[0];
        }

        public void SaveNewName(string newName)
        {
            //playerInfo.SetNewName(newName);
            //Utils.TextUtils.Save(_сommonGameData);
        }

    }
}