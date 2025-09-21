using Common;
using Common.Resourses;
using Models.Common;
using Models.Data.Inventories;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Common.Inventories.Resourses;
using UIController.ControllerPanels.AlchemyPanels;
using UniRx;
using VContainer;
using VContainer.Unity;

namespace ClientServices
{
    public class ResourceStorageController : IInitializable, IDisposable
    {
        [Inject] private readonly GameController _gameController;
        [Inject] private readonly CommonGameData _commonGameData;

        private CompositeDisposable _disposables = new CompositeDisposable();
        private Dictionary<ResourceType, GameResource> _resources = new Dictionary<ResourceType, GameResource>();

        public Dictionary<ResourceType, GameResource> Resources => _resources;

        public void Initialize()
        {
            _gameController.OnLoadedGameData.Subscribe(_ => OnLoadGame()).AddTo(_disposables);
            _gameController.OnGameSave.Subscribe(_ => OnSave()).AddTo(_disposables);
        }

        private void OnLoadGame()
        {
            var resourcesDatas = _commonGameData.Resources;
            foreach (var typeResource in (ResourceType[])Enum.GetValues(typeof(ResourceType)))
            {
                GameResource resource;
                var data = resourcesDatas?.Find(resource => resource.Type == typeResource);
                if (data != null)
                {
                    resource = new GameResource(typeResource, data.Amount.Mantissa, data.Amount.E10);
                }
                else
                {
                    resource = new GameResource(typeResource);
                }

                if (!_resources.ContainsKey(typeResource))
                {
                    _resources.Add(typeResource, resource);
                }
                else
                {
                    _resources[typeResource].AddResource(resource);
                }
            }
        }

        private void OnSave()
        {
            var data = new List<ResourceData>();
            foreach (var resource in _resources.Values)
            {
                if (!resource.Amount.EqualsZero())
                    data.Add(new ResourceData() { Type = resource.Type, Amount = resource.Amount });
            }

            _commonGameData.Resources = data;
        }

        public IDisposable Subscribe(ResourceType type, Action<GameResource> action)
        {
            if (!_resources.ContainsKey(type))
            {
                _resources.Add(type, new GameResource(type));
            }
            return _resources[type].OnChangeResource.Subscribe(action);

        }

        public GameResource GetResource(ResourceType type)
        {
            if (!_resources.ContainsKey(type))
            {
                _resources.Add(type, new GameResource(type));
            }

            return _resources[type];
        }

        public bool CheckResource(GameResource resource)
        {
            if (!_resources.ContainsKey(resource.Type))
            {
                _resources.Add(resource.Type, new GameResource(resource.Type));
            }

            return _resources[resource.Type].CheckCount(resource);
        }

        public bool CheckResource(List<GameResource> resources) =>
            resources.All(resource => CheckResource(resource));

        public void AddResource(GameResource resource)
        {
            if (!_resources.ContainsKey(resource.Type))
            {
                _resources.Add(resource.Type, resource);
                return;
            }

            _resources[resource.Type].AddResource(resource);
        }

        public void AddResource(List<GameResource> listResource)
        {
            foreach (var resource in listResource)
            {
                AddResource(resource);
            }
        }

        public void SubtractResource(GameResource resource)
        {
            if (!_resources.ContainsKey(resource.Type))
            {
                _resources.Add(resource.Type, new GameResource(resource.Type));
            }

            _resources[resource.Type].SubtractResource(resource);
        }

        public void SubtractResource(List<GameResource> listResource)
        {
            foreach (var resource in listResource)
            {
                SubtractResource(resource);
            }
        }

        public void Dispose()
        {
            _disposables.Dispose();
        }

    }
}
