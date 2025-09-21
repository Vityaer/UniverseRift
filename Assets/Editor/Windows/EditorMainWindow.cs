using System.Collections.Generic;
using Assets.Editor.Pages.Avatars;
using Assets.Editor.Pages.Heroes.Charastectics;
using Assets.Editor.Pages.Locations;
using Common;
using Common.Db.CommonDictionaries;
using Editor.Common;
using Editor.Common.Pages.Buildings.Arenas;
using Editor.Common.Pages.Buildings.Guilds;
using Editor.Common.Pages.Buildings.Sanctuaries;
using Editor.Common.Pages.Misc.WhereGetPages;
using Editor.Pages.Achievments;
using Editor.Pages.Bosses;
using Editor.Pages.Buildings.AlchemyPanels;
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
using Pages.Buildings.Mines.Settings;
using Pages.City.ChallengeTower;
using Pages.Heroes.CostLevelUp;
using Pages.Heroes.RatingUps;
using Pages.Heroes.Vocation;
using Pages.Items.Relations;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;
using Utils;

namespace Editor.Windows
{
    public class EditorMainWindow : OdinMenuEditorWindow
    {
        private CommonDictionaries m_dictionaries;
        private ConfigVersion m_configVersion;

        private List<BasePageEditor> m_allPages;
        private HeroPageEditor m_heroPageEditor;
        private RacePageEditor m_racePageEditor;
        private VocationPageEditor m_vocationPageEditor;
        private ItemPageEditor m_itemPageEditor;
        private ItemSetPageEditor m_itemSetPageEditor;
        private ItemRelationPageEditor m_itemRelationPageEditor;
        private RarityPageEditor m_rarityPageEditor;
        private RatingPageEditor m_ratingPageEditor;
        private CampaignPageEditor m_campaignPageEditor;
        private LocationPageEditor m_locationPageEditor;
        private ProductPageEditor m_productPageEditor;
        private SplinterPageEditor m_splinterPageEditor;
        private MarketPageEditor m_marketPageEditor;
        private MinePageEditor m_minePageEditor;
        private StorageChallangePageEditor m_storageChallangePageEditor;
        private CostLevelUpContainerPageEditor m_costLevelUpContainerPageEditor;
        private ForgePageEditor m_forgePageEditor;
        private RewardPageEditor m_rewardPageEditor;
        private GameTaskEditor m_gameTaskModelEditor;
        private FortuneRewardEditor m_fortuneRewardEditor;
        private AchievementPageEditor m_achievementPageEditor;
        private DailyRewardPageEditor m_dailyRewardPageEditor;
        private MineRestrictionPageEditor m_mineRestrictionPageEditor;
        private TravelRaceCircleEditor m_travelRaceCircleEditor;
        private AchievmentContainersPageEditor m_achievmentContainersPageEditor;
        private RewardContrainerPageEditor m_rewardContrainerPageEditor;
        private GuildBossPageEditor m_guildBossPageEditor;
        private AvatarPageEditor m_avatarPageEditor;
        private RatingUpPageEditor m_ratingUpPageEditor;
        private CharacteristicPageEditor m_characteristicPageEditor;
        private HireContainerPageEditor m_hireContainerPageEditor;
        private MagicCirclePageEditor m_magicCirclePageEditor;
        private GuildBuildingPageEditor m_guildBuildingPageEditor;
        private MineSettingsPageEditor m_mineSettingsPageEditor;
        private SanctuaryPageEditor m_sanctuaryPageEditor;
        private ArenaPageEditor m_arenaPageEditor;
        private WhereGetResourcePageEditor m_whereGetResourcePageEditor;
        private AlchemyPanelPageEditor m_alchemyPanelPageEditor;
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
                Saving();

            if (GUILayout.Button("Load All",
                    new GUILayoutOption[] { GUILayout.Width(100), GUILayout.Height(40) }))
                ForceMenuTreeRebuild();

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
            _tree.Add("Heroes/Heroes", m_heroPageEditor);
            _tree.Add("Heroes/Characteristics", m_characteristicPageEditor);
            _tree.Add("Heroes/Vocations", m_vocationPageEditor);
            _tree.Add("Heroes/Races", m_racePageEditor);
            _tree.Add("Heroes/Rating Requirements", m_ratingUpPageEditor);
            _tree.Add("Costs/Costs Editor", m_costLevelUpContainerPageEditor);
            _tree.Add("Inventory/Items/Items", m_itemPageEditor);
            _tree.Add("Inventory/Items/Set", m_itemSetPageEditor);
            _tree.Add("Inventory/Items/Item Relation", m_itemRelationPageEditor);
            _tree.Add("Inventory/Splinters", m_splinterPageEditor);
            _tree.Add("Rarity/Rarities", m_rarityPageEditor);
            _tree.Add("Rating/Rating", m_ratingPageEditor);
            _tree.Add("Fights/Campaign", m_campaignPageEditor);
            _tree.Add("Fights/Storage Challenge", m_storageChallangePageEditor);
            _tree.Add("Fights/Location", m_locationPageEditor);
            _tree.Add("Mall/Markets", m_marketPageEditor);
            _tree.Add("Mall/Products", m_productPageEditor);
            _tree.Add("City/Hires", m_hireContainerPageEditor);
            _tree.Add("City/Mines/Main Editor", m_minePageEditor);
            _tree.Add("City/Mines/restrictions", m_mineRestrictionPageEditor);
            _tree.Add("City/Mines/Mines Settings", m_mineSettingsPageEditor);
            _tree.Add("City/Sanctuary", m_sanctuaryPageEditor);
            _tree.Add("City/Arena", m_arenaPageEditor);
            _tree.Add("City/Travel/Main Editor", m_travelRaceCircleEditor);
            _tree.Add("City/FortuneWheel/Fortune reward Editor", m_fortuneRewardEditor);
            _tree.Add("Rewards/Reward Editor", m_rewardPageEditor);
            _tree.Add("Task board/Task Editor", m_gameTaskModelEditor);
            _tree.Add("Achievements/Achievement Editor", m_achievementPageEditor);
            _tree.Add("Achievements/Achievements container", m_achievmentContainersPageEditor);
            _tree.Add("Daily/Reward Editor", m_dailyRewardPageEditor);
            _tree.Add("Rewards/Containers", m_rewardContrainerPageEditor);
            _tree.Add("Players/Avatars", m_avatarPageEditor);
            //_tree.Add("City/Buildings/Forge Editor", _forgePageEditor);
            _tree.Add("City/Buildings/Magic Circle", m_magicCirclePageEditor);
            _tree.Add("City/Panels/Alchemy", m_alchemyPanelPageEditor);
            
            _tree.Add("City/Guild/Main", m_guildBuildingPageEditor);
            _tree.Add("City/Guild/Bosses", m_guildBossPageEditor);
            _tree.Add("Misc/Where get resources", m_whereGetResourcePageEditor);
            
        }

        private async void InitPages()
        {
            m_configVersion = EditorUtils.Load<ConfigVersion>();
            if (m_configVersion == null) m_configVersion = new ConfigVersion();

            Debug.Log($"Config loaded. Current version: {m_configVersion.Version}");

            var converter = new JsonConverter();
            m_dictionaries = new CommonDictionaries(converter);
            await m_dictionaries.Init();

            m_allPages = new List<BasePageEditor>();

            m_heroPageEditor = new HeroPageEditor(m_dictionaries);
            m_allPages.Add(m_heroPageEditor);

            m_itemPageEditor = new ItemPageEditor(m_dictionaries);
            m_allPages.Add(m_itemPageEditor);

            m_rarityPageEditor = new RarityPageEditor(m_dictionaries);
            m_allPages.Add(m_rarityPageEditor);

            m_itemSetPageEditor = new ItemSetPageEditor(m_dictionaries);
            m_allPages.Add(m_itemSetPageEditor);

            m_ratingPageEditor = new RatingPageEditor(m_dictionaries);
            m_allPages.Add(m_ratingPageEditor);

            m_racePageEditor = new RacePageEditor(m_dictionaries);
            m_allPages.Add(m_racePageEditor);

            m_vocationPageEditor = new VocationPageEditor(m_dictionaries);
            m_allPages.Add(m_vocationPageEditor);

            m_campaignPageEditor = new CampaignPageEditor(m_dictionaries);
            m_allPages.Add(m_campaignPageEditor);

            m_locationPageEditor = new LocationPageEditor(m_dictionaries);
            m_allPages.Add(m_locationPageEditor);

            m_productPageEditor = new ProductPageEditor(m_dictionaries);
            m_allPages.Add(m_productPageEditor);

            m_marketPageEditor = new MarketPageEditor(m_dictionaries);
            m_allPages.Add(m_marketPageEditor);

            m_minePageEditor = new MinePageEditor(m_dictionaries);
            m_allPages.Add(m_minePageEditor);

            m_costLevelUpContainerPageEditor = new CostLevelUpContainerPageEditor(m_dictionaries);
            m_allPages.Add(m_costLevelUpContainerPageEditor);

            m_itemRelationPageEditor = new ItemRelationPageEditor(m_dictionaries);
            m_allPages.Add(m_itemRelationPageEditor);

            m_rewardPageEditor = new RewardPageEditor(m_dictionaries);
            m_allPages.Add(m_rewardPageEditor);

            m_gameTaskModelEditor = new GameTaskEditor(m_dictionaries);
            m_allPages.Add(m_gameTaskModelEditor);

            m_fortuneRewardEditor = new FortuneRewardEditor(m_dictionaries);
            m_allPages.Add(m_fortuneRewardEditor);

            //_forgePageEditor = new ForgePageEditor(_dictionaries);
            //_allPages.Add(_forgePageEditor);

            m_achievementPageEditor = new AchievementPageEditor(m_dictionaries);
            m_allPages.Add(m_achievementPageEditor);

            m_dailyRewardPageEditor = new DailyRewardPageEditor(m_dictionaries);
            m_allPages.Add(m_dailyRewardPageEditor);

            m_storageChallangePageEditor = new StorageChallangePageEditor(m_dictionaries);
            m_allPages.Add(m_storageChallangePageEditor);

            m_mineRestrictionPageEditor = new MineRestrictionPageEditor(m_dictionaries);
            m_allPages.Add(m_mineRestrictionPageEditor);

            m_travelRaceCircleEditor = new TravelRaceCircleEditor(m_dictionaries);
            m_allPages.Add(m_travelRaceCircleEditor);

            m_splinterPageEditor = new SplinterPageEditor(m_dictionaries);
            m_allPages.Add(m_splinterPageEditor);

            m_achievmentContainersPageEditor = new AchievmentContainersPageEditor(m_dictionaries);
            m_allPages.Add(m_achievmentContainersPageEditor);

            m_rewardContrainerPageEditor = new RewardContrainerPageEditor(m_dictionaries);
            m_allPages.Add(m_rewardContrainerPageEditor);

            m_guildBossPageEditor = new GuildBossPageEditor(m_dictionaries);
            m_allPages.Add(m_guildBossPageEditor);

            m_avatarPageEditor = new AvatarPageEditor(m_dictionaries);
            m_allPages.Add(m_avatarPageEditor);

            m_ratingUpPageEditor = new RatingUpPageEditor(m_dictionaries);
            m_allPages.Add(m_ratingUpPageEditor);

            m_characteristicPageEditor = new CharacteristicPageEditor(m_dictionaries);
            m_allPages.Add(m_characteristicPageEditor);

            m_hireContainerPageEditor = new HireContainerPageEditor(m_dictionaries);
            m_allPages.Add(m_hireContainerPageEditor);

            m_magicCirclePageEditor = new MagicCirclePageEditor(m_dictionaries);
            m_allPages.Add(m_magicCirclePageEditor);
            
            m_guildBuildingPageEditor = new GuildBuildingPageEditor(m_dictionaries);
            m_allPages.Add(m_guildBuildingPageEditor);

            m_mineSettingsPageEditor = new MineSettingsPageEditor(m_dictionaries);
            m_allPages.Add(m_mineSettingsPageEditor);
            
            m_sanctuaryPageEditor = new SanctuaryPageEditor(m_dictionaries);
            m_allPages.Add(m_sanctuaryPageEditor);
            
            m_arenaPageEditor = new ArenaPageEditor(m_dictionaries);
            m_allPages.Add(m_arenaPageEditor);
            
            m_whereGetResourcePageEditor = new WhereGetResourcePageEditor(m_dictionaries);
            m_allPages.Add(m_whereGetResourcePageEditor);
            
            m_alchemyPanelPageEditor = new AlchemyPanelPageEditor(m_dictionaries);
            m_allPages.Add(m_alchemyPanelPageEditor);
        }

        private void OnValueSaved()
        {
            EditorUtils.Save(m_configVersion);
        }

        protected override void OnDestroy()
        {
            m_allPages?.Clear();
            m_allPages = null;
            base.OnDestroy();
        }

        protected override void OnImGUI()
        {
            base.OnImGUI();
            var condition = Event.current.type == EventType.KeyUp
                            && Event.current.modifiers == EventModifiers.Control
                            && Event.current.keyCode == KeyCode.S;

            if (condition) Saving();
        }

        private void Saving()
        {
            m_configVersion.Version++;
            m_configVersion.FilesCount = m_allPages.Count;

            Debug.Log("Save configs start");
            foreach (var page in m_allPages) page.Save();

            OnValueSaved();
            InitPages();

            Debug.Log("Save configs complete");
        }
    }
}