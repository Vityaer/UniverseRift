using System;
using UnityEngine;

namespace Common
{
    [Serializable]
    public class BigDigit
    {
        [SerializeField] protected float count = 0;
        public float Count { get => count; set => count = value; }
        [SerializeField] protected int e10 = 0;
        public int E10 { get => e10; set => e10 = value; }
        public BigDigit() { count = 0; e10 = 0; }
        public BigDigit(float count = 0f, int e10 = 0)
        {
            this.count = count;
            this.e10 = e10;
            NormalizeDigit();
        }



        //API
        public override string ToString() { return FunctionHelp.BigDigit(Count, E10); }
        public bool CheckCount(float count, int e10)
        {
            bool result = false;
            if (this.e10 != e10)
            {
                if (this.e10 > e10)
                {
                    if (this.count * (float)Mathf.Pow(10, this.e10 - e10) >= count)
                        result = true;
                }
                else
                {
                    if (this.count >= count * (float)Mathf.Pow(10, e10 - this.e10))
                        result = true;
                }
            }
            else
            {
                if (this.count >= count)
                {
                    result = true;
                }
            }
            return result;
        }
        public bool CheckCount(BigDigit otherDigit) { return CheckCount(otherDigit.Count, otherDigit.E10); }
        public void Add(float count, float e10 = 0)
        {
            if (count > 0 && e10 >= 0)
            {
                this.count += count * (float)Mathf.Pow(10f, e10 - this.e10);
                NormalizeDigit();
            }
        }
        public void Add(BigDigit digit) { Add(digit.Count, digit.E10); }
        public void Subtract(float count, float e10 = 0)
        {
            if (count > 0 && e10 >= 0)
            {
                this.count -= count * (float)Mathf.Pow(10f, e10 - this.e10);
                NormalizeDigit();
            }
        }
        public bool EqualsZero() { return count == 0 && e10 == 0; }
        public void Clear()
        {
            count = 0;
            e10 = 0;
        }

        //Core	
        public void NormalizeDigit()
        {
            while (count > 1000)
            {
                e10 += 3;
                count *= 0.001f;
            }
            while (count < 1 && e10 > 0)
            {
                e10 -= 3;
                count *= 1000f;
            }
            if (e10 == 0)
            {
                count = Mathf.Round(count);
            }
            else
            {
                count = Mathf.Round(count * 1000f) * 0.001f;
            }
        }
        public float ToFloat()
        {
            return count * (float)Mathf.Pow(10f, e10);
        }
        //Operators
        public static BigDigit operator *(BigDigit res, float k)
        {
            BigDigit result = new BigDigit(res.Count * k, res.E10);
            result.NormalizeDigit();
            return result;
        }
        public static BigDigit operator *(BigDigit res, int k)
        {
            BigDigit result = new BigDigit(res.Count * k, res.E10);
            result.NormalizeDigit();
            return result;
        }

        public static BigDigit operator /(BigDigit a, BigDigit b)
        {
            BigDigit result = new BigDigit(0, 0);
            if (b.Count != 0)
            {
                result.Count = a.Count / b.Count;
                result.E10 = a.E10 - b.E10;
            }
            return result;
        }
        public static bool operator >(BigDigit a, BigDigit b) { return a.CheckCount(b); }
        public static bool operator <(BigDigit a, BigDigit b) { return b.CheckCount(a); }
    }
}