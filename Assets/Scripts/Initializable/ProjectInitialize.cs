using Cysharp.Threading.Tasks;
using Db.CommonDictionaries;
using Models.Common;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Initializable
{
    public class ProjectInitialize : IInitializable
    {
        [Inject] private readonly CommonDictionaries _dictionaries;

        public void Initialize()
        {
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
#if !UNITY_EDITOR
            Application.targetFrameRate = 60;
#endif
            _dictionaries.Init().Forget();
        }
    }
}