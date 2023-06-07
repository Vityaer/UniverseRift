using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UIController.Inventory
{
    [CreateAssetMenu(fileName = "NewSplinter", menuName = "Custom ScriptableObject/Splinter", order = 52)]

    [System.Serializable]
    public class SplintersList : SerializedScriptableObject
    {
        [SerializeField]
        private Dictionary<string, SplinterModel> _splinters = new Dictionary<string, SplinterModel>();

        public SplinterModel GetSplinter(string ID)
        {
            SplinterModel result = null;

            if (_splinters.ContainsKey(ID))
            {
                result = _splinters[ID];
            }
            else
            {
                HeroModel hero = Tavern.Instance.GetInfoHero(ID);
                result = new SplinterModel(hero);
            }

            if (result == null)
            {
                Debug.Log("Not found splinter with id = " + ID.ToString());
                result = _splinters.ElementAt(0).Value;
            }
            return result;
        }

    }
}