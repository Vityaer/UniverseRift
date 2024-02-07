using City.Panels.Confirmations;
using Cysharp.Threading.Tasks;
using Models.Data.Players;
using Network.DataServer;
using Network.DataServer.Messages.Friendships;
using System;
using UiExtensions.Scroll.Interfaces;
using UniRx;
using UnityEngine;
using Utils;
using VContainer;

namespace City.Panels.OtherPlayers.MainPanels
{
    public class OtherPlayerMainPanelController : UiPanelController<OtherPlayerMainPanelView>
    {
        [Inject] private readonly ConfirmationPanelController _confirmationPanelController; 

        private PlayerData _playerData;
        private IDisposable _mainButtonDisposable;

        public void ShowPlayer(int id)
        {
            if (!CommonGameData.CommunicationData.PlayersData.TryGetValue(id, out _playerData))
            {
                Debug.LogError($"player with id:{id} not found.");
                return;
            }

            UpdateUi();
        }

        private void UpdateUi()
        {
            View.Nickname.text = _playerData.Name;
            View.Level.text = $"{_playerData.Level}";
            View.Avatar.sprite = SpriteUtils.LoadSprite(_playerData.AvatarPath);

            var friend = CommonGameData.CommunicationData.FriendshipDatas.Find(data => data.FirstPlayerId == _playerData.Id || data.SecondPlayerId == _playerData.Id);
            
            _mainButtonDisposable?.Dispose();

            if (friend == null)
            {
                if (CommonGameData.CommunicationData.FriendshipDatas.Count < Constants.Game.MAX_FRIENDS_COUNT)
                {
                    View.MainButton.gameObject.SetActive(true);
                    View.MainActionText.text = "Add in friend";
                    _mainButtonDisposable = View.MainButton.OnClickAsObservable().Subscribe(_ => AddPlayerInFriends().Forget());
                }
                else
                {
                    View.MainButton.gameObject.SetActive(false);
                }
            }
            else
            {
                View.MainButton.gameObject.SetActive(true);
                View.MainActionText.text = "Remove from friend";
                _mainButtonDisposable = View.MainButton.OnClickAsObservable().Subscribe(_ => CheckConfirmRemovePlayer());
            }
        }

        private async UniTaskVoid AddPlayerInFriends()
        {
            var message = new CreateFriendRequestMessage
            {
                PlayerId = CommonGameData.PlayerInfoData.Id,
                OtherPlayerId = _playerData.Id
            };

            var result = await DataServer.PostData(message);

            if (!string.IsNullOrEmpty(result))
            {
                View.MainButton.interactable = false;
            }
        }

        private void CheckConfirmRemovePlayer()
        {
            var question = "Remove player?";
            _confirmationPanelController.Show(question, () => RemovePlayerFromFriends().Forget(), null);
        }

        private async UniTaskVoid RemovePlayerFromFriends()
        {
            var friendship = CommonGameData.CommunicationData.FriendshipDatas.Find(data => data.FirstPlayerId == _playerData.Id || data.SecondPlayerId == _playerData.Id);
            var message = new BreakFriendshipMessage
            {
                PlayerId = CommonGameData.PlayerInfoData.Id,
                FriendshipId = friendship.Id
            };

            var result = await DataServer.PostData(message);

            if (!string.IsNullOrEmpty(result))
            {
                View.MainButton.interactable = false;
            }
        }

        public override void Dispose()
        {
            _mainButtonDisposable?.Dispose();
            base.Dispose();
        }
    }
}
