namespace Fight.HeroStates
{
    //Debuff
    public class Debuff
    {
        public State state = State.Clear;
        public int countRound;
        private bool virgin = true;
        public void Update(State state, int rounds)
        {
            this.state = state;
            this.countRound = rounds;
            virgin = true;
        }
        public void RoundFinish()
        {
            if (this.virgin) { this.virgin = false; }
            else { this.countRound -= 1; }
        }
        public bool IsFinish { get => (countRound == 0); }
    }
}
