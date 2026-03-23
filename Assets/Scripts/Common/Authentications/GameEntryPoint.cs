using City.Panels.Registrations;
using Models.Common;
using Network.DataServer;
using System;
using Common.Db.CommonDictionaries;
using Cysharp.Threading.Tasks;
using Services.NewPlayerGameStarter;
using UniRx;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Common.Authentications
{
    public class GameEntryPoint : IInitializable, IDisposable
    {
        private const string PLAYER_ID_KEY = "PlayerId";

        [Inject] private readonly CommonGameData m_commonGameData;
        [Inject] private readonly NewPlayerGameStarter m_newPlayerGameStarter;
        [Inject] private readonly RegistrationPanelView m_registrationPanelView;
        [Inject] private readonly CommonDictionaries m_dictionaries;
        
        private CompositeDisposable m_disposables = new();

        public async void Initialize()
        {
            await m_dictionaries.Init();

            DataServer.OnError.Subscribe(OnErrorLoadData).AddTo(m_disposables);
            m_commonGameData.OnLoadedData.Subscribe(_ => DisposeSubscribers()).AddTo(m_disposables);
#if UNITY_EDITOR
            if (!m_registrationPanelView.OverrideId)
            {
                int playerId = GetPlayerId();
                if (playerId > 0)
                {
                    Debug.Log($"пытаемся загрузить данные для: {playerId}");
                    m_commonGameData.Init(playerId).Forget();
                }
                else
                {
                    m_newPlayerGameStarter.StartGame().Forget();
                }
            }
            else
            {
                m_commonGameData.Init(m_registrationPanelView.PlayerId).Forget();
            }
#else
            var playerId = GetPlayerId();
            if (playerId > 0)
            {
                Debug.Log($"пытаемся загрузить данные для: {playerId}");
                _commonGameData.Init(playerId).Forget();
            }
            else
            {
                m_commonGameData.Init(m_registrationPanelView.PlayerId).Forget();
            }
#endif
        }

        private void DisposeSubscribers()
        {
            m_disposables?.Dispose();
            m_disposables = null;
        }

        private void OnErrorLoadData(string error)
        {
            if (!error.Equals("Player not found"))
            {
                return;
            }
            
            m_newPlayerGameStarter.StartGame().Forget();
        }

        private static int GetPlayerId()
        {
            return PlayerPrefs.HasKey(PLAYER_ID_KEY) ? PlayerPrefs.GetInt(PLAYER_ID_KEY) : -1;
        }

        public void Dispose()
        {
            DisposeSubscribers();
        }
    }
}
