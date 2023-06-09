namespace Editor.Common
{
    public class BaseModelEditor<T>
    {
        protected T _model;

        public T GetModel()
        {
            return _model;
        }
    }
}
