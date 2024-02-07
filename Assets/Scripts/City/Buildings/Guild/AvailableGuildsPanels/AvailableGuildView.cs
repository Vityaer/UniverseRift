using Network.DataServer.Models;
using System;
using TMPro;
using UiExtensions.Misc;
using UnityEngine;
using UnityEngine.UI;

namespace City.Buildings.Guild.AvailableGuildsPanels
{
    public class AvailableGuildView : ScrollableUiView<GuildData>
    {
        [SerializeField] private TMP_Text _name;
        [SerializeField] private TMP_Text _recruitCount;
        [SerializeField] private TMP_Text _level;

        public override void SetData(GuildData data, ScrollRect scrollRect)
        {
            Data = data;
            Scroll = scrollRect;
            Button.interactable = true;
            UpdateUi();
        }

        public void SetUsed()
        {
            Button.interactable = false;
        }

        private void UpdateUi()
        {
            _name.text = Data.Name;
            _recruitCount.text = $"{Data.CurrentPlayerCount} / {Data.MaxPlayerCount}";
            _level.text = $"Level: {Data.Level}";
        }
    }
}
