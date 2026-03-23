using System;
using UnityEngine;

namespace Editor.Common
{
    public class BaseModelEditor<T>
    {
        [NonSerialized] protected T m_model;

        public T GetModel()
        {
            return m_model;
        }
    }
}
