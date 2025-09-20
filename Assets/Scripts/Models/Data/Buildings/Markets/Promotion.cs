using Common.Db.CommonDictionaries;
using Newtonsoft.Json;

namespace Models.Data.Buildings.Markets
{
    public class Promotion : BaseModel
    {
        [JsonIgnore] public CommonDictionaries CommonDictionaries;

        public Promotion()
        {
        }

        public Promotion(CommonDictionaries commonDictionaries)
        {
            CommonDictionaries = commonDictionaries;
        }

        public string MarketName { get; set; } = string.Empty;
        public string ProductId { get; set; } = string.Empty;
    }
}
