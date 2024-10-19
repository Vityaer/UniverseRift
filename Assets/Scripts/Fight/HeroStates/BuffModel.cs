using Fight.Rounds;
using System.Collections.Generic;

namespace Fight.HeroStates
{
    [System.Serializable]
    public class BuffModel
    {
        public BuffType Type;
        public List<Round> Rounds = new List<Round>();
        public float GetCurrentAmount { get => Rounds[0].Amount; }

        public BuffModel(BuffType type, float amount, int countRound = 1, RoundTypeNumber typeNumber = RoundTypeNumber.Num)
        {
            this.Type = type;
            for (int i = 0; i < countRound; i++)
                Rounds.Add(new Round(amount, typeNumber));
        }

        public void FinishRound()
        {
            Rounds.RemoveAt(0);
        }

    }
}
