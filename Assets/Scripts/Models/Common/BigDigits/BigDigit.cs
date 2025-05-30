using Sirenix.OdinInspector;
using System;
using UnityEngine;
using UniverseRift.Heplers.MathOperations;

namespace Models.Common.BigDigits
{
    [HideReferenceObjectPicker]
    [Serializable]
    public partial class BigDigit
    {
        public float Mantissa = 0f;
        public int E10 = 0;

        public BigDigit()
        {
        }

        public BigDigit(float count = 0f, int e10 = 0)
        {
            Mantissa = count;
            E10 = e10;
            NormalizeDigit();
        }

        //API
        public override string ToString() =>
            FunctionHelp.BigDigit(Mantissa, E10);

        public bool CheckCount(float count, int e10)
        {
            bool result = false;
            if (E10 != e10)
            {
                if (E10 > e10)
                {
                    if (Mantissa * (float)Mathf.Pow(10, E10 - e10) >= count)
                        result = true;
                }
                else
                {
                    if (Mantissa >= count * (float)Mathf.Pow(10, e10 - E10))
                        result = true;
                }
            }
            else
            {
                if (Mantissa >= count)
                {
                    result = true;
                }
            }
            return result;
        }

        public bool CheckCount(BigDigit otherDigit) =>
            CheckCount(otherDigit.Mantissa, otherDigit.E10);

        public void Add(float count, float e10 = 0)
        {
            if (count > 0 && e10 >= 0)
            {
                Mantissa += count * (float)Mathf.Pow(10f, e10 - E10);
                NormalizeDigit();
            }
        }

        public void Add(BigDigit digit) =>
            Add(digit.Mantissa, digit.E10);

        public void Subtract(BigDigit digit) =>
            Subtract(digit.Mantissa, digit.E10);

        public void Subtract(float count, float e10 = 0)
        {
            if (count > 0 && e10 >= 0)
            {
                Mantissa -= count * (float)Mathf.Pow(10f, e10 - E10);
                NormalizeDigit();
            }
        }

        public bool EqualsZero() =>
                 (Mantissa == 0 && E10 == 0);

        public void Clear()
        {
            Mantissa = 0;
            E10 = 0;
        }

        public void NormalizeDigit()
        {
            while (Mantissa > 1000)
            {
                E10 += 3;
                Mantissa *= 0.001f;
            }
            while (Mantissa < 1 && E10 > 0)
            {
                E10 -= 3;
                Mantissa *= 1000f;
            }
            if (E10 == 0)
            {
                Mantissa = CustomMath.RoundToNearestInt(Mantissa);
            }
            else
            {
                Mantissa = (float)Math.Round(Mantissa, 3);
                //Mantissa = Mathf.Round(Mantissa * 1000f) * 0.001f;
            }
        }

        public float ToFloat()
        {
            return Mantissa * Mathf.Pow(10f, E10);
        }

        public int ToInt() =>
            Mathf.RoundToInt(ToFloat());

        public BigDigit Clone()
        {
            return new BigDigit(Mantissa, E10);
        }
    }
}