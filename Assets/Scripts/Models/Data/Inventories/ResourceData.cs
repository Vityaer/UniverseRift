using Common.Resourses;
using Models.Common.BigDigits;

namespace Models.Data
{
    public class ResourceData : BaseDataModel
    {
        public ResourceType Type;
        public BigDigit Amount = new BigDigit();
    }
}
