namespace Models.Heroes
{
    [System.Serializable]
    public class ResistanceModel : ICloneable
    {
        public float MagicResistance;
        public float CritResistance;
        public float PoisonResistance;
        public float StunResistance;
        public float PetrificationResistance;
        public float FreezingResistance;
        public float AstralResistance;
        public float DumbResistance;
        public float SilinceResistance;
        public float EfficiencyHeal = 1f;
        public object Clone()
        {
            return new ResistanceModel
            {
                MagicResistance = this.MagicResistance,
                CritResistance = this.CritResistance,
                PoisonResistance = this.PoisonResistance,
                EfficiencyHeal = this.EfficiencyHeal,
                StunResistance = this.StunResistance,
                PetrificationResistance = this.PetrificationResistance,
                FreezingResistance = this.FreezingResistance,
                AstralResistance = this.AstralResistance,
                SilinceResistance = this.SilinceResistance,
                DumbResistance = this.DumbResistance
            };
        }

    }
}
