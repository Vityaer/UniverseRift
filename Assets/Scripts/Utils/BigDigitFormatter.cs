using System;

namespace Utils
{
    public class BigDigitFormatter : IFormatProvider, ICustomFormatter
    {
        public object GetFormat(Type formatType)
        {
            if (formatType == typeof(ICustomFormatter))
                return this;
            else
                return null;
        }

        public string Format(string format, object arg, IFormatProvider formatProvider)
        {
            // Check whether this is an appropriate callback
            if (!this.Equals(formatProvider))
                return null;

            string numericString = arg.ToString();

            if (format.Equals("BD"))
            {
                if (numericString.Length <= 4)
                {
                    return numericString;
                }
                else
                {
                    var digit = float.Parse(numericString);
                    var exponent = 0;
                    while (digit > 1000)
                    {
                        exponent += 3;
                        digit /= 1000;
                    }
                    var result = Math.Round(digit, 1).ToString();
                    switch (exponent)
                    {
                        case 3:
                            result = $"{result}K";
                            break;
                        case 6:
                            result = $"{result}M";
                            break;
                        case 9:
                            result = $"{result}B";
                            break;
                    }
                    return result;
                }
            }
            return numericString;
        }
    }
}