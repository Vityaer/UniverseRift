using System;
using UnityEngine;
using UnityEngine.Pool;

namespace Utils
{
    public class WrapperPool<T> where T : MonoBehaviour
    {
        private readonly ObjectPool<T> m_pool;
        private readonly Action<T> m_actionOnCreate;
        private readonly T m_prefab;
        private readonly Transform m_parent;

        private int m_currentCountInWork = 0;

        public int CurrentCountInWork => m_currentCountInWork;

        public WrapperPool(T prefab, Action<T> actionOnCreate, Transform parent = null)
        {
            m_pool = new ObjectPool<T>(Create, actionOnRelease: ActionOnRelease);
            m_actionOnCreate = actionOnCreate;
            m_prefab = prefab;
            m_parent = parent;
        }

        public virtual T Get()
        {
            T result = m_pool.Get();
            result.gameObject.SetActive(true);
            m_currentCountInWork += 1;
            return result;
        }

        public virtual void Release(T obj)
        {
            obj.gameObject.SetActive(false);
            m_currentCountInWork -= 1;
            m_pool.Release(obj);
        }

        private void ActionOnRelease(T obj)
        {
            obj.gameObject.SetActive(false);
        }

        private T Create()
        {
            T result = UnityEngine.Object.Instantiate(m_prefab, m_parent);

            m_actionOnCreate?.Invoke(result);

            return result;
        }
    }
}