using System;

namespace Editor.Common
{
    public class BasePageEditor
    {
        protected bool DataExist { get; set; } = false;
        public event Action OnValueSaved;

        public virtual void Init()
        {
        }

        public virtual void Save()
        {
            OnValueSaved?.Invoke();
        }

        protected virtual void AddElement()
        {
        }
        protected virtual void RemoveElement()
        {
        }
    }
}