using VContainer;

namespace Fight.AI
{
    public class BotFactory
    {
        private readonly IObjectResolver m_resolver;

        public BotFactory(IObjectResolver objectResolver)
        {
            m_resolver = objectResolver;
        }

        public T Create<T>() where T : BaseBot, new()
        {
            var bot = new T();
            m_resolver.Inject(bot);
            return bot;
        }
    }
}