using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace UIController.Inventory
{
    [CreateAssetMenu(fileName = "NewItem", menuName = "Custom ScriptableObject/Item", order = 53)]
    [System.Serializable]
    public class ItemsList : SerializedScriptableObject
    {
        [Header("Forge")]
        [SerializeField] private Dictionary<string, Item> Items = new Dictionary<string, Item>();

        public Item GetItem(string ID)
        {
            Item result = null;
            if (Items.ContainsKey(ID))
            {
                result = Items[ID];
            }
            else
            {
                Debug.Log("не нашли такого предмета " + ID.ToString());
            }

            return result;
        }
    }
}