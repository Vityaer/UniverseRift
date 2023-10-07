using Assets.Editor.Pages.Locations;
using Common;
using Db.CommonDictionaries;
using Editor.Common;
using Editor.Pages.Achievments;
using Editor.Pages.Buildings.Mines.MineRestrictions;
using Editor.Pages.Buildings.TaskBoards;
using Editor.Pages.Campaigns;
using Editor.Pages.City.Mines;
using Editor.Pages.DailyRewards;
using Editor.Pages.Heroes;
using Editor.Pages.Heroes.Race;
using Editor.Pages.Items;
using Editor.Pages.Items.Set;
using Editor.Pages.Mall.Market;
using Editor.Pages.Mall.Products;
using Editor.Pages.Rating;
using Editor.Pages.Rewards;
using Editor.Pages.Splinters;
using Editor.Units;
using Misc.Json.Impl;
using Pages.Buildings.Forge;
using Pages.Buildings.FortuneWheels;
using Pages.City.ChallengeTower;
using Pages.Heroes.CostLevelUp;
using Pages.Heroes.Vocation;
using Pages.Items.Relations;
using Sirenix.OdinInspector.Editor;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Utils;

namespace Editor.Windows
{
    public class EditorMainWindow : OdinMenuEditorWindow
    {
        private CommonDictionaries _dictionaries;
        private ConfigVersion ConfigVersion;

        private List<BasePageEditor> _allPages;
        private HeroPageEditor _heroPageEditor;
        private RacePageEditor _racePageEditor;
        private VocationPageEditor _vocationPageEditor;
        private ItemPageEditor _itemPageEditor;
        private ItemSetPageEditor _itemSetPageEditor;
        private ItemRelationPageEditor _itemRelationPageEditor;
        private RarityPageEditor _rarityPageEditor;
        private RatingPageEditor _ratingPageEditor;
        private CampaignPageEditor _campaignPageEditor;
        private LocationPageEditor _locationPageEditor;
        private ProductPageEditor _productPageEditor;
        private SplinterPageEditor _splinterPageEditor;
        private MarketPageEditor _marketPageEditor;
        private MinePageEditor _minePageEditor;
        private StorageChallangePageEditor _storageChallangePageEditor;
        private CostLevelUpContainerPageEditor _costLevelUpContainerPageEditor;
        private ForgePageEditor _forgePageEditor;
        private RewardPageEditor _rewardPageEditor;
        private GameTaskEditor _gameTaskModelEditor;
        private FortuneRewardEditor _fortuneRewardEditor;
        private AchievementPageEditor _achievementPageEditor;
        private DailyRewardPageEditor _dailyRewardPageEditor;
        private MineRestrictionPageEditor _mineRestrictionPageEditor;

        private OdinMenuTree _tree;

        [MenuItem("TD_Editor/Main _%#T")]
        public static void OpenWindow()
        {
            GetWindow<EditorMainWindow>().Show();
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
            _tree.Add("Heroes/Heroes", _heroPageEditor);
            _tree.Add("Heroes/Race", _racePageEditor);
            _tree.Add("Costs/Costs Editor", _costLevelUpContainerPageEditor);
            _tree.Add("Heroes/Vocation", _vocationPageEditor);
            _tree.Add("Inventory/Items/Items", _itemPageEditor);
            _tree.Add("Inventory/Items/Set", _itemSetPageEditor);
            _tree.Add("Inventory/Items/Item Relation", _itemRelationPageEditor);
            _tree.Add("Inventory/Splinters", _splinterPageEditor);
            _tree.Add("Rarity/Rarities", _rarityPageEditor);
            _tree.Add("Rating/Rating", _ratingPageEditor);
            _tree.Add("Fights/Ñampaign", _campaignPageEditor);
            _tree.Add("Fights/Storage Challange", _storageChallangePageEditor);
            _tree.Add("Fights/Location", _locationPageEditor);
            _tree.Add("Mall/Markets", _marketPageEditor);
            _tree.Add("Mall/Products", _productPageEditor);
            _tree.Add("City/Mines/Main Editor", _minePageEditor);
            _tree.Add("City/Mines/restrictions", _mineRestrictionPageEditor);
            _tree.Add("Rewards/Reward Editor", _rewardPageEditor);
            _tree.Add("Taskboard/Task Editor", _gameTaskModelEditor);
            _tree.Add("FortuneWheel/Fortune reward Editor", _fortuneRewardEditor);
            _tree.Add("Achievements/Achievement Editor", _achievementPageEditor);
            _tree.Add("Daily/Reward Editor", _dailyRewardPageEditor);
            
            //_tree.Add("City/Buildings/Forge Editor", _forgePageEditor);
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

            _ratingPageEditor = new RatingPageEditor(_dictionaries);
            _allPages.Add(_ratingPageEditor);

            _racePageEditor = new RacePageEditor(_dictionaries);
            _allPages.Add(_racePageEditor);

            _vocationPageEditor = new VocationPageEditor(_dictionaries);
            _allPages.Add(_vocationPageEditor);

            _campaignPageEditor = new CampaignPageEditor(_dictionaries);
            _allPages.Add(_campaignPageEditor);

            _locationPageEditor = new LocationPageEditor(_dictionaries);
            _allPages.Add(_locationPageEditor);

            _productPageEditor = new ProductPageEditor(_dictionaries);
            _allPages.Add(_productPageEditor);

            _marketPageEditor = new MarketPageEditor(_dictionaries);
            _allPages.Add(_marketPageEditor);

            _minePageEditor = new MinePageEditor(_dictionaries);
            _allPages.Add(_minePageEditor);

            _costLevelUpContainerPageEditor = new CostLevelUpContainerPageEditor(_dictionaries);
            _allPages.Add(_costLevelUpContainerPageEditor);

            _itemRelationPageEditor = new ItemRelationPageEditor(_dictionaries);
            _allPages.Add(_itemRelationPageEditor);

            _rewardPageEditor = new RewardPageEditor(_dictionaries);
            _allPages.Add(_rewardPageEditor);

            _gameTaskModelEditor = new GameTaskEditor(_dictionaries);
            _allPages.Add(_gameTaskModelEditor);

            _fortuneRewardEditor = new FortuneRewardEditor(_dictionaries);
            _allPages.Add(_fortuneRewardEditor);
            //_forgePageEditor = new ForgePageEditor(_dictionaries);
            //_allPages.Add(_forgePageEditor);

            _achievementPageEditor = new AchievementPageEditor(_dictionaries);
            _allPages.Add(_achievementPageEditor);

            _dailyRewardPageEditor = new DailyRewardPageEditor(_dictionaries);
            _allPages.Add(_dailyRewardPageEditor);

            _storageChallangePageEditor = new StorageChallangePageEditor(_dictionaries);
            _allPages.Add(_storageChallangePageEditor);

            _mineRestrictionPageEditor = new MineRestrictionPageEditor(_dictionaries);
            _allPages.Add(_mineRestrictionPageEditor);

        }

        private void OnValueSaved()
        {
            EditorUtils.Save(ConfigVersion);
        }

        protected override void OnDestroy()
        {
            _allPages?.Clear();
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