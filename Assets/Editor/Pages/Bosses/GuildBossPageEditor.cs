using Assets.Editor.Pages.Bosses;
using Editor.Common;
using Models.Guilds;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using Common.Db.CommonDictionaries;
using Utils;

namespace Editor.Pages.Bosses
{

    public class GuildBossPageEditor : BasePageEditor
    {
        private CommonDictionaries _dictionaries;
        private List<GuildBossContainer> _guildBossContainers => _dictionaries.GuildBossContainers.Select(l => l.Value).ToList();

        public GuildBossPageEditor(CommonDictionaries commonDictionaries)
        {
            _dictionaries = commonDictionaries;
            Init();
        }

        public override void Init()
        {
            base.Init();
            GuildBossModelEditors = _guildBossContainers.Select(f => new GuildBossModelEditor(f, _dictionaries)).ToList();
            DataExist = true;
        }

        public override void Save()
        {
            var containers = GuildBossModelEditors.Select(r => r.GetModel()).ToList();

            EditorUtils.Save(containers);
            base.Save();
        }

        protected override void AddElement()
        {
            base.AddElement();
            var id = UnityEngine.Random.Range(0, 99999).ToString();
            _dictionaries.GuildBossContainers.Add(id, new GuildBossContainer() { Id = id });
            GuildBossModelEditors.Add(new GuildBossModelEditor(_dictionaries.GuildBossContainers[id], _dictionaries));
        }

        private void RemoveElements(GuildBossModelEditor light, object b, List<GuildBossModelEditor> lights)
        {
            var targetElement = GuildBossModelEditors.First(e => e == light);
            var id = targetElement.Model.Id;
            _dictionaries.GuildBossContainers.Remove(id);
            GuildBossModelEditors.Remove(targetElement);
        }

        [ShowInInspector]
        [ListDrawerSettings(HideRemoveButton = false, DraggableItems = false, Expanded = true,
            NumberOfItemsPerPage = 20,
            CustomRemoveElementFunction = nameof(RemoveElements), CustomAddFunction = nameof(AddElement))]
        [ShowIf(nameof(DataExist))]
        [HorizontalGroup("3")]
        [LabelText("Guild Boss Containers")]
        [PropertyOrder(2)]
        [Searchable(FilterOptions = SearchFilterOptions.ValueToString)]
        public List<GuildBossModelEditor> GuildBossModelEditors = new List<GuildBossModelEditor>();
    }
}
