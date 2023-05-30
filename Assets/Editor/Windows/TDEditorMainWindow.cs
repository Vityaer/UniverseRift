using Editor.Common;
using Editor.Pages;
using Misc.Json.Impl;
using Sirenix.OdinInspector.Editor;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Editor.Windows
{
    public class TDEditorMainWindow : OdinMenuEditorWindow
    {
        private List<BasePageEditor> _allPages;
        private HeroPageEditor _heroPageEditor;
        private OdinMenuTree _tree;

        [MenuItem("TD_Editor/Main _%#T")]
        public static void OpenWindow()
        {
            GetWindow<TDEditorMainWindow>().Show();
        }

        protected override void DrawMenu()
        {
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Save All",
                    new GUILayoutOption[] { GUILayout.Width(100), GUILayout.Height(40) }))
            {
                foreach (var page in _allPages)
                {
                    page.Save();
                }

                OnValueSaved();
                InitPages();
            }

            if (GUILayout.Button("Load All",
                    new GUILayoutOption[] { GUILayout.Width(100), GUILayout.Height(40) }))
            {
                ForceMenuTreeRebuild();
            }

            GUILayout.EndHorizontal();
            base.DrawMenu();
        }

        protected override OdinMenuTree BuildMenuTree()
        {
            InitPages();
            _tree = new OdinMenuTree();
            FillTree();
            return _tree;
        }

        private void FillTree()
        {
            _tree.Selection.SupportsMultiSelect = false;
            _tree.Add("Heroes/Heroes Editor", _heroPageEditor);
        }

        private async void InitPages()
        {
            var converter = new JsonConverter();

            _allPages = new List<BasePageEditor>();
            _heroPageEditor = new HeroPageEditor();

        }

        private void OnValueSaved()
        {
            //EditorUtils.Save(ConfigVersion);
        }

        protected override void OnDestroy()
        {
            _allPages.Clear();
            _allPages = null;
            base.OnDestroy();
        }

        protected override void OnGUI()
        {
            base.OnGUI();
            var condition = Event.current.type == EventType.KeyUp
                            && Event.current.modifiers == EventModifiers.Control
                            && Event.current.keyCode == KeyCode.S;

            if (condition)
            {
                Debug.Log("Save configs start");
                foreach (var page in _allPages)
                {
                    page.Save();
                }

                OnValueSaved();
                InitPages();

                Debug.Log("Save configs complete");
            }
        }
    }
}