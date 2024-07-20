using Models.Common;
using Models.Data.Players;

namespace Common.Players
{
    public class PlayersStorage
    {
        private CommonGameData _commonGameData;

        public PlayersStorage(CommonGameData commonGameData)
        {
            _commonGameData = commonGameData;
        }

        public PlayerData GetPlayerData(int playerId)
        {
            PlayerData result = null;
            if (_commonGameData.CommunicationData.PlayersData.TryGetValue(playerId, out var playerData))
            {
                return playerData;
            }

            return result;
        }
    }
}
