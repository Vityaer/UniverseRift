using Common.Db.CommonDictionaries;
using Common.Players;
using Models.Data.Players;
using UIController.Misc.Widgets;
using UnityEngine;
using UnityEngine.UI;
using Utils.DataCheckUtils;
using VContainer;

namespace UiExtensions.Misc
{
    public class PlayerScrollableUiView<T> : ScrollableUiView<T>
    {
        [SerializeField] private PlayerInfoWidget _playerInfoWidget;

        private PlayersStorage m_playersStorage;
        private PlayerData m_playerData;
        private CommonDictionaries m_commonDictionaries;

        [Inject]
        private void Construct(PlayersStorage playersStorage, CommonDictionaries commonDictionaries)
        {
            m_playersStorage = playersStorage.ThrowIfArgumentNull();
            m_commonDictionaries = commonDictionaries.ThrowIfArgumentNull();
        }

        public virtual void SetData(T data, ScrollRect scrollRect, int playerId)
        {
            m_playerData = m_playersStorage.GetPlayerData(playerId);
            _playerInfoWidget.SetData(m_playerData);
            base.SetData(data, scrollRect, m_commonDictionaries);
        }
    }
}
