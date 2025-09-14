using System;
using UnityEngine;
using UnityEngine.Pool;

namespace Utils
{
    public class WrapperPool<T> where T : MonoBehaviour
    {
        protected ObjectPool<T> Pool;
        protected Action<T> ActionOnCreate;
        protected T Prefab;
        protected Transform _parent;

        private int _currentCountInWork = 0;
        
        public int CurrentCountInWork => _currentCountInWork;
        
        public WrapperPool(T prefab, Action<T> actionOnCreate, Transform parent = null)
        {
            Pool = new ObjectPool<T>(Create, actionOnRelease: ActionOnRelease);
            ActionOnCreate = actionOnCreate;
            Prefab = prefab;
            _parent = parent;
        }

        public virtual T Get()
        {
            var result = Pool.Get();
            result.gameObject.SetActive(true);
            _currentCountInWork += 1;
            return result;
        }

        public virtual void Release(T obj)
        {
            obj.gameObject.SetActive(false);
            _currentCountInWork -= 1;
            Pool.Release(obj);
        }

        protected void ActionOnRelease(T obj)
        {
            obj.gameObject.SetActive(false);
        }

        protected T Create()
        {
            var result = UnityEngine.Object.Instantiate(Prefab, _parent);

            if (ActionOnCreate != null)
                ActionOnCreate(result);

            return result;
        }
    }
}
