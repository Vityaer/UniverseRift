using System;
using TMPro;
using UniRx;

namespace UiExtensions.Misc
{
    public static class TMPInputFieldExtensions
    {
        public static IObservable<string> OnValueChangedAsObservable(this TMP_InputField inputField)
        {
            return Observable.Create<string>(observer =>
            {
                void Handler(string text) => observer.OnNext(text);
                inputField.onValueChanged.AddListener(Handler);

                return Disposable.Create(() => { inputField.onValueChanged.RemoveListener(Handler); });
            });
        }
    }
}