using Editor.Windows;
using Sirenix.OdinInspector.Editor;
using UnityEditor;

namespace Assets.Editor.Connections
{
    public class EditorConnectionTests : OdinMenuEditorWindow
    {
        private ConnectionPage _connectionPage = new ConnectionPage();

        private OdinMenuTree _tree;

        [MenuItem("Connection_Editor/Main _%#C")]
        public static void OpenWindow()
        {
            GetWindow<EditorConnectionTests>().Show();
        }

        protected override OdinMenuTree BuildMenuTree()
        {
            _tree = new OdinMenuTree();
            FillTree();
            return _tree;
        }

        private void FillTree()
        {
            _tree.Selection.SupportsMultiSelect = false;
            _tree.Add("Connection", _connectionPage);
        }
    }
}