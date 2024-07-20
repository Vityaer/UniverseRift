using Common.Players;
using Models.Data.Players;
using UIController.Misc.Widgets;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace UiExtensions.Misc
{
    public class PlayerScrollableUiView<T> : ScrollableUiView<T>
    {
        [SerializeField] private PlayerInfoWidget _playerInfoWidget;

        private PlayersStorage _playersStorage;
        private PlayerData _playerData;

        [Inject]
        private void Construct(PlayersStorage playersStorage)
        {
            _playersStorage = playersStorage;
        }

        public virtual void SetData(T data, ScrollRect scrollRect, int playerId)
        {
            _playerData = _playersStorage.GetPlayerData(playerId);
            _playerInfoWidget.SetData(_playerData);
            base.SetData(data, scrollRect);
        }
    }
}
