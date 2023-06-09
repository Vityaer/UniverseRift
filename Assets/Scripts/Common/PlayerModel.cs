using UnityEngine;

namespace Common
{
    public class PlayerModel : BaseObject
    {
        [SerializeField] private int amount;
        public int Amount { get => amount; set => amount = value; }
        [SerializeField] protected string name = string.Empty;
        [SerializeField] public string Rating;

    }
}