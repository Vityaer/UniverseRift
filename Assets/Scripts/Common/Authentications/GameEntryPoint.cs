using City.Panels.Registrations;
using Cysharp.Threading.Tasks;
using Db.CommonDictionaries;
using Models.Common;
using Network.DataServer;
using System;
using UniRx;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Common.Authentications
{
    public class GameEntryPoint : IInitializable, IDisposable
    {
        private const string PLAYER_ID_KEY = "PlayerId";

        [Inject] private readonly CommonGameData _commonGameData;
        [Inject] private readonly RegistrationPanelController _registrationPanelController;
        [Inject] private readonly RegistrationPanelView _registrationPanelView;
        [Inject] private readonly CommonDictionaries _dictionaries;

        private CompositeDisposable _disposables = new();

        public async void Initialize()
        {
            await _dictionaries.Init();

            DataServer.OnError.Subscribe(OnErrorLoadData).AddTo(_disposables);
            _commonGameData.OnLoadedData.Subscribe(_ => DisposeSubscribers()).AddTo(_disposables);
            if (!_registrationPanelView.OverrideId)
            {
                var playerId = GetPlayerId();
                if (playerId > 0)
                {
                    _commonGameData.Init(playerId).Forget();
                }
                else
                {
                    _registrationPanelController.OpenPanelRegistration();
                }
            }
            else
            {
                _commonGameData.Init(_registrationPanelView.PlayerId).Forget();
            }
        }

        private void DisposeSubscribers()
        {
            _disposables?.Dispose();
            _disposables = null;
        }

        private void OnErrorLoadData(string error)
        {
            if (error.Equals("Player not found"))
            {
                Debug.Log("re registration");
                _registrationPanelController.OpenPanelRegistration();
            }
        }

        private int GetPlayerId()
        {
            return PlayerPrefs.HasKey(PLAYER_ID_KEY) ? PlayerPrefs.GetInt(PLAYER_ID_KEY) : -1;
        }

        public void Dispose()
        {
            DisposeSubscribers();
        }
    }
}
