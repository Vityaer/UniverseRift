using Common;
using Db.CommonDictionaries;
using Editor.Common;
using Editor.Pages.Heroes;
using Editor.Pages.Heroes.Race;
using Editor.Pages.Items;
using Editor.Pages.Items.Set;
using Editor.Pages.Items.Type;
using Editor.Pages.Rating;
using Editor.Units;
using Misc.Json.Impl;
using Pages.Heroes.Vocation;
using Sirenix.OdinInspector.Editor;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Utils;

namespace Editor.Windows
{
    public class TDEditorMainWindow : OdinMenuEditorWindow
    {
        private CommonDictionaries _dictionaries;
        private ConfigVersion ConfigVersion;
        
        private List<BasePageEditor> _allPages;
        private HeroPageEditor _heroPageEditor;
        private RacePageEditor _racePageEditor;
        private VocationPageEditor _vocationPageEditor;
        private ItemPageEditor _itemPageEditor;
        private ItemSetPageEditor _itemSetPageEditor;
        private ItemTypePageEditor _itemTypePageEditor;
        private RarityPageEditor _rarityPageEditor;
        private RatingPageEditor _ratingPageEditor;

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
                Saving();
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
            _tree.Add("Heroes/Race Editor", _racePageEditor);
            _tree.Add("Rating/Vocation Editor", _vocationPageEditor);
            _tree.Add("Items/Items Editor", _itemPageEditor);
            _tree.Add("Items/Set Editor", _itemSetPageEditor);
            _tree.Add("Items/Type Editor", _itemTypePageEditor);
            _tree.Add("Rarity/Rarities Editor", _rarityPageEditor);
            _tree.Add("Rating/Rating Editor", _ratingPageEditor);
        }

        private async void InitPages()
        {
            ConfigVersion = EditorUtils.Load<ConfigVersion>();
            if (ConfigVersion == null)
            {
                ConfigVersion = new ConfigVersion();
            }

            Debug.Log($"Config loaded. Current version: {ConfigVersion.Version}");

            var converter = new JsonConverter();
            _dictionaries = new CommonDictionaries(converter);
            await _dictionaries.Init();

            _allPages = new List<BasePageEditor>();
            
            _heroPageEditor = new HeroPageEditor(_dictionaries);
            _allPages.Add(_heroPageEditor);
            
            _itemPageEditor = new ItemPageEditor(_dictionaries);
            _allPages.Add(_itemPageEditor);

            _rarityPageEditor = new RarityPageEditor(_dictionaries);
            _allPages.Add(_rarityPageEditor);

            _itemSetPageEditor = new ItemSetPageEditor(_dictionaries);
            _allPages.Add(_itemSetPageEditor);

            _itemTypePageEditor = new ItemTypePageEditor(_dictionaries);
            _allPages.Add(_itemTypePageEditor);

            _ratingPageEditor = new RatingPageEditor(_dictionaries);
            _allPages.Add(_ratingPageEditor);

            _racePageEditor = new RacePageEditor(_dictionaries);
            _allPages.Add(_racePageEditor);

            _vocationPageEditor =  new VocationPageEditor(_dictionaries);
            _allPages.Add(_vocationPageEditor);
        }

        private void OnValueSaved()
        {
            EditorUtils.Save(ConfigVersion);
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
                Saving();
            }
        }

        private void Saving()
        {
            ConfigVersion.Version++;
            ConfigVersion.FilesCount = _allPages.Count;

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