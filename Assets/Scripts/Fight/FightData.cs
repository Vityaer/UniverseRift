using UnityEngine;

namespace Fight.Common
{
    public class FightData : MonoBehaviour
    {
        public float lastDamage;
        public delegate void Del(float damage);
        public Del delsLastDamage;

        public static FightData Instance { get; private set; }

        void Awake()
        {
            Instance = this;
        }

        public void RegisterOnLastDamage(Del d)
        {
            delsLastDamage += d;
        }

        public void UnRegisterOnLastDamage(Del d)
        {
            delsLastDamage += d;
        }

        public void GetLastDamage()
        {
            if (delsLastDamage != null)
                delsLastDamage(lastDamage);
        }




    }
}