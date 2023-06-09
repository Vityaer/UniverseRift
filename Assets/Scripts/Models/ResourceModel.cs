using Common;
using Common.Resourses;

namespace Models
{
    //Resource
    [System.Serializable]
    public class ResourceModel
    {
        public TypeResource type;
        public BigDigit amount;
        public ResourceModel(Resource res)
        {
            this.type = res.Name;
            this.amount = res.Amount;
        }
        public ResourceModel(TypeResource typeResource)
        {
            this.type = typeResource;
            this.amount = new BigDigit();
        }
    }
}
