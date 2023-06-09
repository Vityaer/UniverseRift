using UnityEditor;

namespace Assets.Editor.Utils
{
    public class EditorSpellVFXResolver : EditorWindow
    {
        //[MenuItem("Tools/VFX/Prefab resolve")]
        static void Init()
        {
            //string[] assetGUIDs = AssetDatabase.FindAssets("t:Object", new[] { "Assets/Resources/VFX" });

            //for (int i = 0; i < assetGUIDs.Length; i++)
            //{
            //    try
            //    {
            //        string assetFilePath = AssetDatabase.GUIDToAssetPath(assetGUIDs[i]);

            //        if (Path.GetExtension(assetFilePath).Equals(".prefab"))
            //        {
            //            var prefab = (GameObject)AssetDatabase.LoadAssetAtPath(assetFilePath, typeof(GameObject));
            //            if (prefab.TryGetComponent(out VFXView vFXView))
            //            {
            //                vFXView.OnCreateComponent();
            //                PrefabUtility.ApplyObjectOverride(prefab, assetFilePath, InteractionMode.AutomatedAction);
            //            }
            //            EditorUtility.DisplayProgressBar("Finding vfx prefab in the project", assetFilePath, (float)i / assetGUIDs.Length);
            //        }
            //    }
            //    catch
            //    {
            //        // ignored
            //    }
            //}

            //EditorUtility.ClearProgressBar();
        }
    }
}