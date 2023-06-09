using UnityEditor;
using UnityEngine;
using Utils.Development;

namespace Utils
{
    [InitializeOnLoad]
    public class EditorOnComponentAdded
    {
        static EditorOnComponentAdded()
        {
            ObjectFactory.componentWasAdded -= HandleComponentAdded;
            ObjectFactory.componentWasAdded += HandleComponentAdded;

            EditorApplication.quitting -= OnEditorQuiting;
            EditorApplication.quitting += OnEditorQuiting;
        }

        private static void HandleComponentAdded(Component obj)
        {
            if(obj is ICreatable)
            {
                ((ICreatable)obj).OnCreateComponent();
            }
        }

        private static void OnEditorQuiting()
        {
            ObjectFactory.componentWasAdded -= HandleComponentAdded;
            EditorApplication.quitting -= OnEditorQuiting;
        }
    }
}