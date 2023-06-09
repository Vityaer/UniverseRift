using UnityEngine;

namespace Models
{
    //Player info
    [System.Serializable]
    public class PlayerInfoModel
    {
        [SerializeField] private string name = string.Empty;
        [SerializeField] private int level = 1;
        [SerializeField] private int _playerId = 1;
        [SerializeField] private int vipLevel;

        public int IDGuild, IDAvatar, IDServer;
        public string Name => name;
        public int Level => level;
        public int VipLevel => vipLevel;
        public int PlayerId => _playerId;
        public void LevelUP()
        {
            level += 1;
        }
        public void SetNewName(string newName) { this.name = newName; }
        public void SetNewAvatar(int IDAvatar) { this.IDAvatar = IDAvatar; }

        public PlayerInfoModel() { }
        public void Register(string name, int playerId)
        {
            this.name = name;
            _playerId = playerId;
            level = 1;
            IDGuild = 0;
            vipLevel = 0;
        }
    }
}
