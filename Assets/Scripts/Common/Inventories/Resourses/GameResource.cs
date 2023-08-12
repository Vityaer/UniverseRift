using Models.Common.BigDigits;
using Models.Data.Inventories;
using System;
using UniRx;
using UnityEngine;
using UniverseRift.Heplers.MathOperations;

namespace Common.Resourses
{
    [Serializable]
    public class GameResource : BaseObject
    {
        public readonly ResourceType Type;
        public readonly new BigDigit Amount;
        private ReactiveCommand<GameResource> _onChangeResource = new ReactiveCommand<GameResource>();

        public IObservable<GameResource> OnChangeResource => _onChangeResource;

        public GameResource()
        {
            Type = ResourceType.Gold;
            Amount = new BigDigit();
        }

        public GameResource(ResourceData resourceModel)
        {
            Type = resourceModel.Type;
            Amount = resourceModel.Amount;
        }

        public GameResource(ResourceType name, float count = 0f, int e10 = 0)
        {
            Type = name;
            Amount = new BigDigit(count, e10);
        }

        public GameResource(ResourceType name, BigDigit amount)
        {
            Type = name;
            Amount = amount;
        }

        public string GetTextAmount() =>
            Amount.ToString();

        public override string ToString() =>
            Amount.ToString();

        public override bool CheckCount(int count) =>
            Amount.CheckCount(count, 0);

        public bool CheckCount(int count, int e10) =>
            Amount.CheckCount(count, e10);

        public override bool EqualsZero =>
            Amount.EqualsZero();
        public bool CheckCount(GameResource res) =>
            Amount.CheckCount(res.Amount);

        public override void Add(int count)
        {
            AddResource(count);
        }

        public void AddResource(float count, float e10 = 0)
        {
            Amount.Add(count, e10);
            _onChangeResource.Execute(this);
        }

        public void AddResource(GameResource res)
        {
            Amount.Add(res.Amount);
            _onChangeResource.Execute(this);
        }

        public void SubtractResource(float count, float e10 = 0)
        {
            Amount.Subtract(count, e10);
            _onChangeResource.Execute(this);
        }

        public void SubtractResource(GameResource res)
        {
            Amount.Subtract(res.Amount);
            _onChangeResource.Execute(this);
        }

        public void Clear()
        {
            Amount.Clear();
            _onChangeResource.Execute(this);
        }

        //Operators
        public static GameResource operator *(GameResource res, float k)
        {
            GameResource result = new GameResource(res.Type, CustomMath.RoundToNearestInt(res.Amount.Mantissa * k), res.Amount.E10);
            return result;
        }

        public static bool operator <(GameResource a, GameResource b) { return b.CheckCount(a); }
        public static bool operator >(GameResource a, GameResource b) { return a.CheckCount(b); }

        //Image
        private static Sprite[] spriteAtlas;
        public override Sprite Image
        {
            get
            {
                if (sprite == null)
                {
                    if (spriteAtlas == null) spriteAtlas = Resources.LoadAll<Sprite>("UI/GameImageResource/Resources");
                    for (int i = 0; i < spriteAtlas.Length; i++)
                    {
                        if (Type.ToString().Equals(spriteAtlas[i].name))
                        {
                            sprite = spriteAtlas[i];
                            break;
                        }
                    }
                }
                return sprite;
            }
        }

        public int ConvertToInt()
        {
            return (int)(Amount.Mantissa * Mathf.Pow(10, Amount.E10));
        }
    }
}