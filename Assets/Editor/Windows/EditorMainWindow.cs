using Assets.Editor.Pages.Avatars;
using Assets.Editor.Pages.Heroes.Charastectics;
using Assets.Editor.Pages.Locations;
using Common;
using Db.CommonDictionaries;
using Editor.Common;
using Editor.Pages.Achievments;
using Editor.Pages.Bosses;
using Editor.Pages.Buildings.Hires;
using Editor.Pages.Buildings.MagicCircles;
using Editor.Pages.Buildings.Mines.MineRestrictions;
using Editor.Pages.Buildings.TaskBoards;
using Editor.Pages.Buildings.TravelRaceCampaigns;
using Editor.Pages.Campaigns;
using Editor.Pages.City.Mines;
using Editor.Pages.DailyRewards;
using Editor.Pages.DailyTasks;
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
using Pages.BattlePases;
using Pages.Buildings.Forge;
using Pages.Buildings.FortuneWheels;
using Pages.City.ChallengeTower;
using Pages.Heroes.CostLevelUp;
using Pages.Heroes.RatingUps;
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
        private TravelRaceCircleEditor _travelRaceCircleEditor;
        private AchievmentContainersPageEditor _achievmentContainersPageEditor;
        private RewardContrainerPageEditor _rewardContrainerPageEditor;
        private GuildBossPageEditor _guildBossPageEditor;
        private AvatarPageEditor _avatarPageEditor;
        private RatingUpPageEditor _ratingUpPageEditor;
        private CharacteristicPageEditor _characteristicPageEditor;
        private HireContainerPageEditor _hireContainerPageEditor;
        private MagicCirclePageEditor _magicCirclePageEditor;
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
            _tree.Add("Heroes/Characteristics", _characteristicPageEditor);
            _tree.Add("Heroes/Vocations", _vocationPageEditor);
            _tree.Add("Heroes/Races", _racePageEditor);
            _tree.Add("Heroes/Rating Requrements", _ratingUpPageEditor);
            _tree.Add("Costs/Costs Editor", _costLevelUpContainerPageEditor);
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
            _tree.Add("City/Hires", _hireContainerPageEditor);
            _tree.Add("City/Mines/Main Editor", _minePageEditor);
            _tree.Add("City/Mines/restrictions", _mineRestrictionPageEditor);
            _tree.Add("City/Travel/Main Editor", _travelRaceCircleEditor);
            _tree.Add("City/FortuneWheel/Fortune reward Editor", _fortuneRewardEditor);
            _tree.Add("Rewards/Reward Editor", _rewardPageEditor);
            _tree.Add("Taskboard/Task Editor", _gameTaskModelEditor);
            _tree.Add("Achievements/Achievement Editor", _achievementPageEditor);
            _tree.Add("Achievements/Achievements container", _achievmentContainersPageEditor);
            _tree.Add("Daily/Reward Editor", _dailyRewardPageEditor);
            _tree.Add("Rewards/Containers", _rewardContrainerPageEditor);
            _tree.Add("Guild/Bosses", _guildBossPageEditor);
            _tree.Add("Players/Avatars", _avatarPageEditor);
            //_tree.Add("City/Buildings/Forge Editor", _forgePageEditor);
            _tree.Add("City/Buildings/Magic Circle", _magicCirclePageEditor);
            
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

            _travelRaceCircleEditor = new TravelRaceCircleEditor(_dictionaries);
            _allPages.Add(_travelRaceCircleEditor);

            _splinterPageEditor = new SplinterPageEditor(_dictionaries);
            _allPages.Add(_splinterPageEditor);

            _achievmentContainersPageEditor = new AchievmentContainersPageEditor(_dictionaries);
            _allPages.Add(_achievmentContainersPageEditor);

            _rewardContrainerPageEditor = new RewardContrainerPageEditor(_dictionaries);
            _allPages.Add(_rewardContrainerPageEditor);

            _guildBossPageEditor = new GuildBossPageEditor(_dictionaries);
            _allPages.Add(_guildBossPageEditor);

            _avatarPageEditor = new AvatarPageEditor(_dictionaries);
            _allPages.Add(_avatarPageEditor);

            _ratingUpPageEditor = new RatingUpPageEditor(_dictionaries);
            _allPages.Add(_ratingUpPageEditor);

            _characteristicPageEditor = new CharacteristicPageEditor(_dictionaries);
            _allPages.Add(_characteristicPageEditor);

            _hireContainerPageEditor = new HireContainerPageEditor(_dictionaries);
            _allPages.Add(_hireContainerPageEditor);

            _magicCirclePageEditor = new MagicCirclePageEditor(_dictionaries);
            _allPages.Add(_magicCirclePageEditor);
            
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

        protected override void OnImGUI()
        {
            base.OnImGUI();
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