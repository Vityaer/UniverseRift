using System;

namespace GameUtils
{
    public static class Utils
    {
        public static string CastIdToName(int id)
        {
            return ((HeroName)id).ToString();
        }

        public static int CastNameToId(string name)
        {
            return (int) Enum.Parse(typeof(MissionEnemy), name);
        }
    }
}
