namespace UIController.Rewards.PosibleRewards
{
    public class PosibleGameObject
    {
        public string ModelId;
        public float Posibility;

        public PosibleGameObject(string id, float percent = 1f)
        {
            ModelId = id;
            Posibility = percent;
        }
    }
}