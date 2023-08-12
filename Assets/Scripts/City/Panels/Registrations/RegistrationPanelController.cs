using Assets.Scripts.ClientServices;
using ClientServices;
using Common;
using Common.Resourses;
using Common.Rewards;
using Cysharp.Threading.Tasks;
using Db.CommonDictionaries;
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
    public class RegistrationPanelController : UiPanelController<RegistrationPanelView>, IInitializable, IStartable, IDisposable
    {
        private const int NAME_LENGTH_MIN = 3;

        [Inject] private readonly IUiMessagesPublisherService _uiMessagesPublisher;
        [Inject] private readonly CommonGameData _commonGameData;
        [Inject] private readonly CommonDictionaries _commonDictionaries;
        [Inject] private readonly ClientRewardService _clientRewardService;

        private PlayerData _playerInfo;

        public new void Initialize()
        {
            View.InputFieldNewNamePlayer.onValueChanged.AddListener(OnChangeNewName);
            View.StartRegistrationButton.OnClickAsObservable().Subscribe(_ => StartRegistration()).AddTo(Disposables);
            base.Initialize();
        }

        public void OpenPanelRegistration()
        {
            Debug.Log("open registration");
            _uiMessagesPublisher.OpenWindowPublisher.OpenWindow<RegistrationPanelController>(openType: OpenType.Exclusive);
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
                _commonGameData.Init(id).Forget();
                Close();
            }
            else
            {
                View.InputFieldNewNamePlayer.interactable = true;
                View.StartRegistrationButton.interactable = true;
            }
        }

        public override void Dispose()
        {
            View.InputFieldNewNamePlayer.onValueChanged.RemoveAllListeners();
            base.Dispose();
        }

    }
}