using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using UnityEngine.AddressableAssets.ResourceLocators;
using UnityEngine.Networking;
using UnityEngine.ResourceManagement;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.ResourceManagement.Util;

namespace CustomAddresables
{
    [DisplayName("TestCustomProvider")]
    public class TestCustomProvider : IResourceProvider
    {
        public Type GetDefaultType(IResourceLocation location)
        {
            throw new NotImplementedException();
        }

        public bool CanProvide(Type type, IResourceLocation location)
        {
            throw new NotImplementedException();
        }

        public void Provide(ProvideHandle provideHandle)
        {
            throw new NotImplementedException();
        }

        public void Release(IResourceLocation location, object asset)
        {
            throw new NotImplementedException();
        }

        public string ProviderId { get; }
        public ProviderBehaviourFlags BehaviourFlags { get; }
    }
}