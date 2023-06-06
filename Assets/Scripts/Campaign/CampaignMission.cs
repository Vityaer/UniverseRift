using UnityEngine;

[System.Serializable]
public class CampaignMission : Mission, ICloneable
{
    [Header("Auto fight reward")]
    [SerializeField] private AutoReward _autoFightReward;

    public AutoReward AutoFightReward => _autoFightReward;

    public object Clone()
    {
        return new CampaignMission
        {
            Name = this.Name,
            ListEnemy = this.listEnemy,
            WinReward = (Reward)this.WinReward.Clone(),
            _autoFightReward = this._autoFightReward,
            Location = this.Location
        };
    }
}
