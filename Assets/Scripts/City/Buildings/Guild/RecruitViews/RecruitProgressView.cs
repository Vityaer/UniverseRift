using Common.Db.CommonDictionaries;
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

        private PlayersStorage m_playersStorage;
        private PlayerData m_playerData;

        [Inject]
        private void Construct(PlayersStorage playersStorage)
        {
            m_playersStorage = playersStorage;
        }

        public override void SetData(RecruitData data, ScrollRect scrollRect, CommonDictionaries dictionaries)
        {
            base.SetData(data, scrollRect, dictionaries);
            m_playerData = m_playersStorage.GetPlayerData(data.PlayerId);
            UpdateUi();
        }

        private void UpdateUi()
        {
            if (Data == null)
                return;

            _numPlace.text = $"{transform.GetSiblingIndex() + 1}";
            _name.text = $"{m_playerData.Name}";

            var damage = new BigDigit(Data.ResultMantissa, Data.ResultE10);
            _result.text = $"{damage}";
            Debug.Log($"update ui: {damage}");
        }
    }
}
