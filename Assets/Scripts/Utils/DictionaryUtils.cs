using Models;
using System.Collections.Generic;
using System.Linq;

namespace Utils
{
    public static class DictionaryUtils
    {
        public static string[] GetArrayIds<T>(Dictionary<string, T> datas)
            where T : BaseModel
        {
            var result = datas.Values.Select(r => r.Id).ToArray();
            return result;
        }
    }
}
