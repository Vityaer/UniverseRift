using UnityEngine;
using Sirenix.OdinInspector;
using Network.DataServer;
using Network.DataServer.Messages.Hires;
using Cysharp.Threading.Tasks;
using Network.DataServer.DevelopmentMessages;

namespace Initializable.Launches
{
    public class Launch : MonoBehaviour
    {
        [EnumToggleButtons, HideLabel]
        [SerializeField] private LaunchType _launchType;

#if UNITY_EDITOR
        [Button("Refresh day")]
        private async UniTaskVoid RefreshServerDay()
        {
            var message = new RefreshDayMessage();
            var result = await DataServer.PostData(message);
        }

        [Button("Next game cycle")]
        private async UniTaskVoid LoadNextGameCycle()
        {
            var message = new ChangeGameCycleMessage();
            var result = await DataServer.PostData(message);
        }
#endif
    }
}
