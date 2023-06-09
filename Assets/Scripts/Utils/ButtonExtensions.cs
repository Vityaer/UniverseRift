using UnityEngine.Events;
using UnityEngine.UI;

namespace Utils
{
    public static class ButtonExtensions
    {
        public static void SetListener(this Button button, UnityAction action)
        {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(action);
        }

        public static void SwapListeners(this Button button, UnityAction oldAction, UnityAction newAction)
        {
            button.onClick.RemoveListener(oldAction);
            button.onClick.AddListener(newAction);
        }
    }
}