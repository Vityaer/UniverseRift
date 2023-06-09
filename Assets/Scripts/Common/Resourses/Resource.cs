using System;
using UIController.Inventory;
using UnityEngine;

namespace Common.Resourses
{
    [Serializable]
    public class Resource : BaseObject, ICloneable, VisualAPI
    {
        public TypeResource Name;
        [SerializeField] protected BigDigit amount;
        public BigDigit Amount { get => amount; set => amount = value; }
        public float Count { get => Amount.Count; set => Amount.Count = value; }
        public int E10 { get => Amount.E10; set => Amount.E10 = value; }

        public Resource()
        {
            Name = TypeResource.Gold;
            Amount = new BigDigit();
        }

        public Resource(TypeResource name, float count = 0f, int e10 = 0)
        {
            Name = name;
            Amount = new BigDigit(count, e10);
        }

        public Resource(TypeResource name, BigDigit amount)
        {
            Name = name;
            Amount = amount;
        }

        //API
        public override string GetTextAmount() { return Amount.ToString(); }
        public override string ToString() { return Amount.ToString(); }
        public bool CheckCount(int count, int e10) { return Amount.CheckCount(count, e10); }
        public bool CheckCount(Resource res) { return Amount.CheckCount(res.Count, res.E10); }
        public void AddResource(float count, float e10 = 0) { Amount.Add(count, e10); }
        public void AddResource(Resource res) { Amount.Add(res.Count, res.E10); }
        public void SubtractResource(float count, float e10 = 0) { Amount.Subtract(count, e10); }
        public void SubtractResource(Resource res) { Amount.Subtract(res.Count, res.E10); }
        public void Clear() { Amount.Clear(); }

        public void ClearUI()
        {
            UI = null;
        }
        public VisualAPI GetVisual()
        {
            return this as VisualAPI;
        }

        //Operators
        public static Resource operator *(Resource res, float k)
        {
            Resource result = new Resource(res.Name, Mathf.Ceil(res.Count * k), res.E10);
            return result;
        }
        public static bool operator <(Resource a, Resource b) { return b.CheckCount(a); }
        public static bool operator >(Resource a, Resource b) { return a.CheckCount(b); }
        //Image
        private static Sprite[] spriteAtlas;
        public Sprite Image
        {
            get
            {
                if (sprite == null)
                {
                    if (spriteAtlas == null) spriteAtlas = Resources.LoadAll<Sprite>("UI/GameImageResource/Resources");
                    for (int i = 0; i < spriteAtlas.Length; i++)
                    {
                        if (Name.ToString().Equals(spriteAtlas[i].name))
                        {
                            sprite = spriteAtlas[i];
                            break;
                        }
                    }
                }
                return sprite;
            }
        }
        public object Clone()
        {
            return new Resource
            {
                Name = Name,
                Amount = Amount
            };
        }
        //UI
        public override string GetName() { return Name.ToString(); }
        public void ClickOnItem() { InventoryController.Instance.OpenInfoItem(this); }
        public void SetUI(ThingUI UI)
        {
            this.UI = UI;
            UpdateUI();
        }
        public void UpdateUI()
        {
            UI?.UpdateUI(Image, Amount.ToString());
        }
        public int ConvertToInt()
        {
            return (int)(Count * Mathf.Pow(10, E10));
        }

    }
}