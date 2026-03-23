using Common.Db.CommonDictionaries;
using Common.Players;
using Models.Arenas;
using Models.Data.Players;
using TMPro;
using UController.Other;
using UiExtensions.Misc;
using UnityEngine;
using UnityEngine.UI;
using Utils.DataCheckUtils;
using VContainer;

namespace City.Buildings.Arena
{
    public class ArenaOpponentView : ScrollableUiView<ArenaPlayerData>
    {
        [SerializeField] private AvatarView _avatarController;
        [SerializeField] private TMP_Text _opponentName;
        [SerializeField] private TMP_Text _levelText;
        [SerializeField] private TMP_Text _scoreText;
        [SerializeField] private TMP_Text _winCountText;
        [SerializeField] private TMP_Text _loseCountText;

        private PlayerData _playerData;
        private PlayersStorage _playersStorage;

        [Inject]
        private void Construct(PlayersStorage playersStorage)
        {
            _playersStorage = playersStorage;
        }

        public override void SetData(ArenaPlayerData data, ScrollRect scrollRect, CommonDictionaries dictionaries)
        {
            Data = data;
            Scroll = scrollRect.ThrowIfArgumentNull();
            CommonDataDictionaries = dictionaries.ThrowIfArgumentNull();
            UpdateUI();
        }

        private void UpdateUI()
        {
            _playerData = _playersStorage.GetPlayerData(Data.PlayerId);
            _avatarController.SetData(_playerData);
            _opponentName.text = $"{_playerData.Name}";
            _levelText.text = $"{_playerData.Level}";
            _scoreText.text = $"{Data.Score}";
            _winCountText.text = $"{Data.WinCount}";
            _loseCountText.text = $"{Data.LoseCount}";
        }
        public void GoToFight()
        {
            //ArenaController.Instance.FightWithOpponentUseAI(opponent);
        }
    }
}