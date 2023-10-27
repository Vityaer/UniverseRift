using System.Collections.Generic;
using System.Linq;
using UnityEditor;

namespace Utils
{
    public static class InheritanceUtils
    {
        public static string[] GetAllTypeNames<T>()
        {
#if UNITY_EDITOR
            var result = new List<string>()
                .Concat(TypeCache.GetTypesDerivedFrom<T>()
                    .Where(type => !type.IsAbstract)
                    .Select(type => type.Name))
                .ToArray();

            return result;
#else
            return new string[]{};
#endif
        }
    }
}
