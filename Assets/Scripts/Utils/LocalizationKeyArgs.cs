namespace Utils
{
    public class LocalizationKeyArgs
    {
        public LocalizationKeyArgs(string key, ModelType type)
        {
            Key = key;
            Type = type;
        }

        public string Key;
        public ModelType Type;
    }
}
