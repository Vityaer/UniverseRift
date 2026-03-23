using Common;
using Common.Resourses;
using Models.Common;
using Models.Data.Inventories;
using System;
using System.Collections.Generic;
using System.Linq;
using Common.Inventories.Resourses;
using UniRx;
using VContainer;
using VContainer.Unity;

namespace ClientServices
{
    public class ResourceStorageController : IInitializable, IDisposable
    {
        [Inject] private readonly GameController m_gameController;
        [Inject] private readonly CommonGameData m_commonGameData;

        private readonly CompositeDisposable m_disposables = new();
        private readonly Dictionary<ResourceType, GameResource> m_resources = new();

        public Dictionary<ResourceType, GameResource> Resources => m_resources;

        public void Initialize()
        {
            m_gameController.OnLoadedGameData.Subscribe(_ => OnLoadGame()).AddTo(m_disposables);
            m_gameController.OnGameSave.Subscribe(_ => OnSave()).AddTo(m_disposables);
        }

        private void OnLoadGame()
        {
            var resourcesDatas = m_commonGameData.Resources;
            foreach (var typeResource in (ResourceType[])Enum.GetValues(typeof(ResourceType)))
            {
                ResourceData data = resourcesDatas?.Find(resource => resource.Type == typeResource);
                GameResource resource = data != null
                    ? new GameResource(typeResource, data.Amount.Mantissa, data.Amount.E10)
                    : new GameResource(typeResource);

                if (!m_resources.TryGetValue(typeResource, out GameResource storageResource))
                {
                    m_resources.Add(typeResource, resource);
                }
                else
                {
                    storageResource.AddResource(resource);
                }
            }
        }

        private void OnSave()
        {
            List<ResourceData> data = new();
            foreach (GameResource resource in m_resources.Values)
            {
                if (!resource.Amount.EqualsZero())
                    data.Add(new ResourceData() { Type = resource.Type, Amount = resource.Amount });
            }

            m_commonGameData.Resources = data;
        }

        public IDisposable Subscribe(ResourceType type, Action<GameResource> action)
        {
            if (!m_resources.ContainsKey(type))
            {
                m_resources.Add(type, new GameResource(type));
            }
            return m_resources[type].OnChangeResource.Subscribe(action);

        }

        public GameResource GetResource(ResourceType type)
        {
            if (!m_resources.ContainsKey(type))
            {
                m_resources.Add(type, new GameResource(type));
            }

            return m_resources[type];
        }

        public bool CheckResource(GameResource resource)
        {
            if (!m_resources.ContainsKey(resource.Type))
            {
                m_resources.Add(resource.Type, new GameResource(resource.Type));
            }

            return m_resources[resource.Type].CheckCount(resource);
        }

        public bool CheckResource(List<GameResource> resources) =>
            resources.All(CheckResource);

        public void AddResource(GameResource resource)
        {
            if (!m_resources.TryGetValue(resource.Type, out GameResource storageResource))
            {
                m_resources.Add(resource.Type, resource);
                return;
            }

            storageResource.AddResource(resource);
        }

        public void AddResource(List<GameResource> listResource)
        {
            foreach (GameResource resource in listResource)
            {
                AddResource(resource);
            }
        }

        public void SubtractResource(GameResource resource)
        {
            if (!m_resources.ContainsKey(resource.Type))
            {
                m_resources.Add(resource.Type, new GameResource(resource.Type));
            }

            m_resources[resource.Type].SubtractResource(resource);
        }

        public void SubtractResource(List<GameResource> listResource)
        {
            foreach (GameResource resource in listResource)
            {
                SubtractResource(resource);
            }
        }

        void IDisposable.Dispose()
        {
            m_disposables.Dispose();
        }

    }
}
