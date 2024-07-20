using Common.Players;
using Models.Common.BigDigits;
using Models.Data.Players;
using Network.DataServer.Models.Guilds;
using TMPro;
using UiExtensions.Misc;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace City.Buildings.Guild.RecruitViews
{
    public class RecruitProgressView : ScrollableUiView<RecruitData>
    {
        [SerializeField] private TMP_Text _numPlace;
        [SerializeField] private Image _icon;
        [SerializeField] private TMP_Text _name;
        [SerializeField] private TMP_Text _result;

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

            _numPlace.text = $"{transform.GetSiblingIndex() + 1}";
            _name.text = $"{_playerData.Name}";

            var damage = new BigDigit(Data.ResultMantissa, Data.ResultE10);
            _result.text = $"{damage}";
            Debug.Log($"update ui: {damage}");
        }
    }
}
