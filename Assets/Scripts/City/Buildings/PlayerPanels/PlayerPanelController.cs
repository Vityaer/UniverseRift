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
using LocalizationSystems;
using UI.Utils.Localizations.Extensions;
using System.Collections.Generic;

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
        [Inject] private readonly ILocalizationSystem _localizationSystem;

        private PlayerData _playerInfo;
        private GameResource _requireExpForLevel;
        private GameResource _currentExp;
        private ReactiveCommand<BigDigit> _onLevelUp = new();
        private CompositeDisposable _disposables = new CompositeDisposable();

        public IObservable<BigDigit> OnLevelUp => _onLevelUp;
        public PlayerData PlayerInfoData => _playerInfo;

        public override void Start()
        {
            View.AvatarButton.OnClickAsObservable().Subscribe(_ => OpenAvatarsPanel()).AddTo(_disposables);
            _avatarPanelDetailsController.OnSelectNewAvatar.Subscribe(ChangeAvatar).AddTo(_disposables);
            base.Start();
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
            _playerInfo.AvatarPath = avatar.Path;
            View.Avatar.sprite = SpriteUtils.LoadSprite(avatar.Path);
        }

        private void UpdateData()
        {
            View.Name.text = _localizationSystem.GetLocalizedContainer("MyPlayerName")
                .WithArguments(new List<object> {_playerInfo.Name})
                .GetLocalizedString();

            View.PlayerId.text = _localizationSystem.GetLocalizedContainer("MyPlayerId")
                .WithArguments(new List<object> { _playerInfo.Id })
                .GetLocalizedString();

            View.Level.text = _localizationSystem.GetLocalizedContainer("MyPlayerLevel")
                .WithArguments(new List<object> { _playerInfo.Level })
                .GetLocalizedString();

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

            View.Level.text = _localizationSystem.GetLocalizedContainer("MyPlayerLevel")
                .WithArguments(new List<object> { _playerInfo.Level })
                .GetLocalizedString();

            _onLevelUp.Execute(new BigDigit(_playerInfo.Level));
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
    }
}