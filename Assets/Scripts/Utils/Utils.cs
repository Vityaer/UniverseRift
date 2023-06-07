using Assets.Scripts.Campaign;
using System;

namespace GameUtils
{
    public static class Utils
    {
        public static int CastNameToId(string name)
        {
            return (int) Enum.Parse(typeof(Unit), name);
        }
    }
}
