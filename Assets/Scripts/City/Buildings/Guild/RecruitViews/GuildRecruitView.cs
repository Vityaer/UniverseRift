using City.Buildings.PlayerPanels.PlayerMiniPanels;
using Common.Db.CommonDictionaries;
using Common.Players;
using DG.Tweening;
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
        [SerializeField] private PlayerAvatarView _playerAvatar;
        [SerializeField] private TMP_Text _name;
        [SerializeField] private TMP_Text _placeText;

        [SerializeField] private Image _raidBossCheckimage;
        [SerializeField] private Image _donateCheckImage;
        [SerializeField] private Image _enterCheckImage;

        [SerializeField] private float _imageCheckOff;
        [SerializeField] private float _imageCheckOn;

        private PlayersStorage m_playersStorage;
        private PlayerData m_playerData;

        [Inject]
        private void Construct(PlayersStorage playersStorage)
        {
            m_playersStorage = playersStorage;
        }

        public override void SetData(RecruitData data, ScrollRect scrollRect, CommonDictionaries commonDictionaries)
        {
            base.SetData(data, scrollRect, commonDictionaries);
            m_playerData = m_playersStorage.GetPlayerData(data.PlayerId);
            UpdateUi();
        }

        private void UpdateUi()
        {
            if (Data == null)
                return;

            _name.text = $"{m_playerData.Name}";
            _placeText.text = $"{transform.GetSiblingIndex() + 1}";
            _playerAvatar.SetData(m_playerData);

            _raidBossCheckimage.DOFade(Data.TodayRaidBoss ? _imageCheckOn : _imageCheckOff, 0f);
            _donateCheckImage.DOFade(Data.TodayDonate ? _imageCheckOn : _imageCheckOff, 0f);
            _enterCheckImage.DOFade(Data.TodayEnter ? _imageCheckOn : _imageCheckOff, 0f);
        }
    }
}
