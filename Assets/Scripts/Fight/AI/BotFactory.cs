using Fight.Misc;
using UnityEngine;
using VContainer;

namespace Fight.AI
{
    public class BotFactory
    {
        private readonly IObjectResolver _resolver;

        public BotFactory(IObjectResolver objectResolver)
        {
            _resolver = objectResolver;
        }

        public T Create<T>() where T : BaseBot, new()
        {
            var bot = new T();
            _resolver.Inject(bot);
            return bot;
        }
    }
}