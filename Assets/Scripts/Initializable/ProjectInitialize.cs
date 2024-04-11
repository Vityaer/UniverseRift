using UnityEngine;
using VContainer.Unity;

namespace Initializable
{
    public class ProjectInitialize : IInitializable
    {

        public void Initialize()
        {
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
#if !UNITY_EDITOR
            Application.targetFrameRate = 60;
#endif
        }
    }
}