using Editor.Common;
using Models.City.Misc;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using Common.Db.CommonDictionaries;
using Utils;

namespace Pages.City.ChallengeTower
{
    public class StorageChallangePageEditor : BasePageEditor
    {
        private CommonDictionaries _dictionaries;

        private List<StorageChallengeModel> _storageChallenges => _dictionaries.StorageChallenges.Select(l => l.Value).ToList();

        public StorageChallangePageEditor(CommonDictionaries commonDictionaries)
        {
            _dictionaries = commonDictionaries;
            Init();
        }

        public override void Init()
        {
            base.Init();
            StorageChallanges = _storageChallenges.Select(f => new StorageChallangeModelEditor(f, _dictionaries)).ToList();
            DataExist = true;
        }

        public override void Save()
        {
            var units = StorageChallanges.Select(r => r.GetModel()).ToList();

            EditorUtils.Save(units);
            base.Save();
        }

        protected override void AddElement()
        {
            base.AddElement();
            var id = UnityEngine.Random.Range(0, 99999).ToString();
            _dictionaries.StorageChallenges.Add(id, new StorageChallengeModel() { Id = id });
            StorageChallanges.Add(new StorageChallangeModelEditor(_dictionaries.StorageChallenges[id], _dictionaries));
        }

        private void RemoveElements(StorageChallangeModelEditor light, object b, List<StorageChallangeModelEditor> lights)
        {
            var targetElement = StorageChallanges.First(e => e == light);
            var id = targetElement.Id;
            _dictionaries.StorageChallenges.Remove(id);
            StorageChallanges.Remove(targetElement);
        }

        [ShowInInspector]
        [ListDrawerSettings(HideRemoveButton = false, DraggableItems = false, Expanded = true,
            NumberOfItemsPerPage = 20,
            CustomRemoveElementFunction = nameof(RemoveElements), CustomAddFunction = nameof(AddElement))]
        [ShowIf(nameof(DataExist))]
        [HorizontalGroup("3")]
        [LabelText("Storage Challanges")]
        [PropertyOrder(2)]
        [Searchable(FilterOptions = SearchFilterOptions.ValueToString)]
        public List<StorageChallangeModelEditor> StorageChallanges = new List<StorageChallangeModelEditor>();
    }
}
