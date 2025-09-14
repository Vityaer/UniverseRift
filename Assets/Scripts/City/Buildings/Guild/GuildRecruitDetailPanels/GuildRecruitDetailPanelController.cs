using Common.Players;
using Cysharp.Threading.Tasks;
using Models.Data.Players;
using Network.DataServer;
using Network.DataServer.Messages.Guilds;
using Network.DataServer.Models;
using Network.DataServer.Models.Guilds;
using UiExtensions.Scroll.Interfaces;
using UniRx;
using VContainer;
using VContainer.Unity;
using VContainerUi.Messages;
using VContainerUi.Model;
using VContainerUi.Services;

namespace City.Buildings.Guild.GuildRecruitDetailPanels
{
    public class GuildRecruitDetailPanelController : UiPanelController<GuildRecruitDetailPanelView>, IInitializable
    {
        [Inject] private readonly IUiMessagesPublisherService _uiMessagesPublisher;
        [Inject] private readonly PlayersStorage _playersStorage;

        private PlayerData _playerData;
        private RecruitData _recruitData;
        private RecruitData _myRecruitData;

        private GuildData _guildData => CommonGameData.City.GuildPlayerSaveContainer.GuildData;

        public override void Start()
        {
            View.RecruitBanButton.OnClickAsObservable().Subscribe(_ => RecruitBan().Forget()).AddTo(Disposables);
            View.LeaveGuildButton.OnClickAsObservable().Subscribe(_ => LeaveGuild().Forget()).AddTo(Disposables);
            base.Start();
        }

        public void ShowRecruitDetail(RecruitData recruitData)
        {
            _playerData = _playersStorage.GetPlayerData(recruitData.PlayerId);
            _recruitData = recruitData;
            UpdateUi();
            _uiMessagesPublisher.OpenWindowPublisher.OpenWindow<GuildRecruitDetailPanelController>(openType: OpenType.Exclusive);
        }

        private void UpdateUi()
        {
            View.PlayerAvatarView.SetData(_playerData);
            View.PlayerName.text = _playerData.Name;

            var myData = GetMyRecruitData();
            bool isLeader = _guildData.LeaderId == myData.PlayerId;
            bool isSelf = myData.PlayerId == _recruitData.PlayerId;

            bool canBan = isLeader && !isSelf;
            bool canLeave = (isLeader && !isSelf) || (!isLeader && isSelf);

            View.RecruitBanButton.gameObject.SetActive(canBan);
            View.LeaveGuildButton.gameObject.SetActive(canLeave);
        }

        private async UniTaskVoid LeaveGuild()
        {
            var message = new LeaveFromGuildMessage
            {
                PlayerId = CommonGameData.PlayerInfoData.Id,
                GuildId = _guildData.Id
            };

            var result = await DataServer.PostData(message);
            if (!string.IsNullOrEmpty(result))
            {
                CommonGameData.City.GuildPlayerSaveContainer.GuildRecruits.Remove(_recruitData);
                Close();
            }
        }

        private async UniTaskVoid RecruitBan()
        {
            var message = new GuildCreatePlayerBanMessage
            {
                PlayerId = CommonGameData.PlayerInfoData.Id,
                GuildId = _guildData.Id,
                OtherPlayerId = _recruitData.PlayerId,
                Reason = 0
            };

            var result = await DataServer.PostData(message);
            if (!string.IsNullOrEmpty(result))
            {
                CommonGameData.City.GuildPlayerSaveContainer.GuildRecruits.Remove(_recruitData);
                Close();
            }
        }

        private RecruitData GetMyRecruitData()
        {
            if (_myRecruitData == null)
            {
                _myRecruitData = CommonGameData.City.GuildPlayerSaveContainer.GuildRecruits
                    .Find(recruit => recruit.PlayerId == CommonGameData.PlayerInfoData.Id);
            }

            return _myRecruitData;
        }
    }
}
