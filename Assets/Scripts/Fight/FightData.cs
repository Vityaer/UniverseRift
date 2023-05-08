using UnityEngine;

public class FightData : MonoBehaviour
{
    public float lastDamage;
    public delegate void Del(float damage);
    public Del delsLastDamage;
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

    private static FightData instance;
    public static FightData Instance { get => instance; }
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }
}
