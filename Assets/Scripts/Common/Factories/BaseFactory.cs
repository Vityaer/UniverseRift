using UnityEngine;
using VContainer;

namespace Common.Factories
{
    public abstract class BaseFactory<T> where T : Object
    {
        protected readonly IObjectResolver ObjectResolver;

        protected BaseFactory(IObjectResolver objectResolver)
        {
            ObjectResolver = objectResolver;
        }

        public virtual T Create(T prefab, Vector3 position = new Vector3(), Quaternion rotation = new Quaternion())
        {
            var obj = Object.Instantiate(prefab, position, rotation);
            ObjectResolver.Inject(obj);
            return obj;
        }
    }
}