using Assets.Scripts.ClientServices;
using Common;
using Common.Resourses;
using Cysharp.Threading.Tasks;
using Models.Common;
using Models.Data.Players;
using Network.DataServer;
using Network.DataServer.Messages;
using System;
using UiExtensions.Scroll.Interfaces;
using UniRx;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using VContainerUi.Messages;
using VContainerUi.Model;
using VContainerUi.Services;

namespace City.Panels.Registrations
{
    public class RegisterationPanelController : UiPanelController<RegistrationPanelView>, IInitializable, IStartable, IDisposable
    {
        private const int NAME_LENGTH_MIN = 3;

        [Inject] private readonly IUiMessagesPublisherService _uiMessagesPublisher;
        [Inject] private readonly ResourceStorageController _resourceStorageController;
        [Inject] private readonly GameController _gameController;
        [Inject] private readonly CommonGameData _commonGameData;

        private PlayerInfoData _playerInfo;

        public void Initialize()
        {
            View.InputFieldNewNamePlayer.onValueChanged.AddListener(OnChangeNewName);
            View.StartRegistrationButton.OnClickAsObservable().Subscribe(_ => StartRegistration()).AddTo(Disposables);
            _gameController.OnLoadedGameData.Subscribe(_ => CheckRegistration()).AddTo(Disposables);
        }

        private void CheckRegistration()
        {
            if (_commonGameData.Player.PlayerInfoData.Id == 0)
            {
                _uiMessagesPublisher.OpenWindowPublisher.OpenWindow<RegisterationPanelController>(openType: OpenType.Additive);
            }
        }

        private void OnChangeNewName(string newName)
        {
            if (!newName.Equals(_playerInfo))
            {
                var enoughLength = newName.Length > NAME_LENGTH_MIN;
                View.StartRegistrationButton.interactable = enoughLength;
            }
            else
            {
                View.StartRegistrationButton.interactable = false;
            }
        }

        private void StartRegistration()
        {
            var newName = View.InputFieldNewNamePlayer.text;
            View.InputFieldNewNamePlayer.interactable = false;
            View.StartRegistrationButton.interactable = false;
            CreateAccount(newName).Forget();
        }

        public async UniTaskVoid CreateAccount(string name)
        {
            var message = new PlayerRegistration { Name = name };
            var result = await DataServer.PostData(message);

            if (int.TryParse(result, out int id))
            {
                _commonGameData.Player.PlayerInfoData.Id = id;
                _commonGameData.Player.PlayerInfoData.Name = name;
                GetStartPack();
                _gameController.SaveGame();
                Close();
            }
            else
            {
                View.InputFieldNewNamePlayer.interactable = true;
                View.StartRegistrationButton.interactable = true;
            }
        }

        private void GetStartPack()
        {
            //Вынести в эдитор
            _resourceStorageController.AddResource(new GameResource(ResourceType.SimpleHireCard, 10));
            _resourceStorageController.AddResource(new GameResource(ResourceType.Diamond, 100));
            _resourceStorageController.AddResource(new GameResource(ResourceType.CoinFortune, 5));
        }

        public override void Dispose()
        {
            View.InputFieldNewNamePlayer.onValueChanged.RemoveAllListeners();
            base.Dispose();
        }

    }
}