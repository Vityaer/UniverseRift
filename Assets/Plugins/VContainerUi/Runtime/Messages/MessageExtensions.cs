using MessagePipe;
using VContainerUi.Interfaces;
using VContainerUi.Model;

namespace VContainerUi.Messages
{
    public static class MessageExtensions
    {
        public static void OpenWindow<TWindow>(this IPublisher<MessageOpenWindow> publisher,
            UiScope scope = UiScope.Local, OpenType openType = OpenType.Exclusive)
            where TWindow : IPopUp
            => publisher.Publish(new MessageOpenWindow(typeof(TWindow), scope, openType));

        public static void BackWindow(this IPublisher<MessageBackWindow> publisher)
            => publisher.Publish(new MessageBackWindow());

        public static void CloseWindow<TWindow>(this IPublisher<MessageCloseWindow> publisher) where TWindow : IPopUp =>
            publisher.Publish(new MessageCloseWindow(typeof(TWindow)));
    }
}