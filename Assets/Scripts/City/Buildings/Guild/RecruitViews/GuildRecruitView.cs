using Common.Players;
using Models.Data.Players;
using Network.DataServer.Models.Guilds;
using TMPro;
using UiExtensions.Misc;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace City.Buildings.Guild.RecruitViews
{
    public class GuildRecruitView : ScrollableUiView<RecruitData>
    {
        [SerializeField] private Image _icon;
        [SerializeField] private TMP_Text _name;

        private PlayersStorage _playersStorage;
        private PlayerData _playerData;

        [Inject]
        private void Construct(PlayersStorage playersStorage)
        {
            _playersStorage = playersStorage;
        }

        public override void SetData(RecruitData data, ScrollRect scrollRect)
        {
            base.SetData(data, scrollRect);
            _playerData = _playersStorage.GetPlayerData(data.PlayerId);
            UpdateUi();
        }

        private void UpdateUi()
        {
            if (Data == null)
                return;

            _name.text = $"{_playerData.Name}";
        }
    }
}
