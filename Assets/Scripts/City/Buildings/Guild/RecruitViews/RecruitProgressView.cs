using Network.DataServer.Models.Guilds;
using TMPro;
using UiExtensions.Misc;
using UnityEngine;
using UnityEngine.UI;

namespace City.Buildings.Guild.RecruitViews
{
    public class RecruitProgressView : ScrollableUiView<RecruitData>
    {
        [SerializeField] private TMP_Text _numPlace;
        [SerializeField] private Image _icon;
        [SerializeField] private TMP_Text _name;
        [SerializeField] private TMP_Text _result;

        public override void SetData(RecruitData data, ScrollRect scrollRect)
        {
            Data = data;
            UpdateUi();
        }

        private void UpdateUi()
        {
            if (Data == null)
                return;

            _numPlace.text = $"{transform.GetSiblingIndex() + 1}";
            _name.text = $"{Data.Id}";
            _result.text = "123K";
        }
    }
}
