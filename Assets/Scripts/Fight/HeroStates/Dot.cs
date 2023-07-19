using Fight.Common.Strikes;
using Fight.HeroControllers.Generals;
using Fight.Rounds;
using System.Collections.Generic;

namespace Fight.HeroStates
{
    public class Dot
    {
        public DotType type;
        public List<Round> rounds;
        public void Update(List<Round> extraRounds)
        {
            bool find = false;
            List<bool> flags = new List<bool>(this.rounds.Count) { false };
            foreach (Round curRound in extraRounds)
            {
                find = false;
                for (int i = 0; i < this.rounds.Count; i++)
                {
                    if ((this.rounds[i].typeNumber == curRound.typeNumber) && (flags[i] == false))
                    {
                        this.rounds[i].Add(curRound.amount);
                        find = true;
                        flags[i] = true;
                        break;
                    }
                }
                if (find == false) this.rounds.Add((Round)curRound.Clone());
            }
        }
        public void RoundFinish(HeroController heroController)
        {
            if (rounds.Count > 0)
            {
                heroController.ApplyDamage(new Strike(rounds[0].amount, 0, rounds[0].typeNumber, TypeStrike.Clean));
                rounds.RemoveAt(0);
            }
        }
        public bool IsFinish { get => (rounds.Count == 0); }

        public Dot(DotType type, List<Round> newRounds)
        {
            this.type = type;
            this.rounds = new List<Round>();
            foreach (Round round in newRounds)
            {
                this.rounds.Add((Round)round.Clone());
            }
        }
    }
}
