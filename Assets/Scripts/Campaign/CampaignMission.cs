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
            _listEnemy = this.listEnemy,
            winReward = (Reward)this.WinReward.Clone(),
            _autoFightReward = this._autoFightReward,
            location = this.location
        };
    }
}
