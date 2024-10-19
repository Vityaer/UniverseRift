using System;
using UnityEngine;

namespace Editor.Common
{
    public class BaseModelEditor<T>
    {
        [NonSerialized] protected T _model;

        public T GetModel()
        {
            return _model;
        }
    }
}
