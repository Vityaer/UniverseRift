using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "NewSplinter", menuName = "Custom ScriptableObject/Splinter", order = 52)]

[System.Serializable]
public class SplintersList : SerializedScriptableObject
{
    [SerializeField]
    private Dictionary<string, Splinter> _splinters = new Dictionary<string, Splinter>();

    public Splinter GetSplinter(string ID)
    {
        Splinter result = null;

        if (_splinters.ContainsKey(ID))
        {
            result = _splinters[ID];
        }
        else
        {
            InfoHero hero = TavernScript.Instance.GetInfoHero(ID);
            result = new Splinter(hero);
        }

        if (result == null)
        {
            Debug.Log("Not found splinter with id = " + ID.ToString());
            result = _splinters.ElementAt(0).Value;
        }
        return result;
    }

}
