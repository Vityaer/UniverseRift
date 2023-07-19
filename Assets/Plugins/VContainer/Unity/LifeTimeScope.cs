using System.Collections.Generic;
using UnityEngine;
using VContainer.Extensions;

namespace VContainer.Unity
{
    [DefaultExecutionOrder(-5000)]
    public class IdleGameLifetimeScope : LifetimeScope
    {
        [SerializeField] private List<ScriptableObjectInstaller> _scriptableObjectInstallers = new List<ScriptableObjectInstaller>();
        [SerializeField] private List<MonoInstaller> _monoInstallers = new List<MonoInstaller>();

        protected override void Configure(IContainerBuilder builder)
        {
            foreach (var installer in _monoInstallers)
            {
                installer.Install(builder);
            }

            foreach (var installer in _scriptableObjectInstallers)
            {
                installer.Install(builder);
            }

        }
    }
}
