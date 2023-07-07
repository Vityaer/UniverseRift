using Models;
using Models.Common.BigDigits;
using System;
using UIController.Inventory;
using UnityEngine;

namespace Common
{
    [Serializable]
    public class BaseObject : BaseModel
    {
        public string Name;

        protected Sprite sprite = null;

        public int Amount;
        public override string ToString()
            => Amount.ToString();

        public virtual Sprite Image => sprite;

        public BaseObject()
        {
            Amount = 0;
        }

        public virtual bool EqualsZero
            => Amount == 0;

        public bool IsNull() =>
            sprite == null;


        public virtual void Add(int count)
        {
            Amount += count;
        }

        public virtual void Remove(int count)
        {
            Amount -= count;
        }

        public virtual bool CheckCount(int count) 
            => Amount >= count;
    }
}