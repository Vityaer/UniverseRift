using Db.CommonDictionaries;
using Editor.Common;
using Models.City.Forges;
using Sirenix.OdinInspector;
using System.Linq;
using UnityEngine;
using Utils;

namespace Pages.Buildings.Forge
{
    public class ForgePageEditor : BasePageEditor
    {
        private CommonDictionaries _dictionaries;

        public ForgePageEditor(CommonDictionaries commonDictionaries)
        {
            _dictionaries = commonDictionaries;
            Init();
        }

        public override void Init()
        {
            base.Init();
            ForgeModel forgeModel;
            if (_dictionaries.Buildings.ContainsKey(nameof(ForgeModel)))
            {
                forgeModel = _dictionaries.Buildings[nameof(ForgeModel)] as ForgeModel;
            }
            else
            {
                forgeModel = new ForgeModel();
                _dictionaries.Buildings.Add(nameof(ForgeModel), forgeModel);
            }

            Forge = new ForgeModelEditor(forgeModel, _dictionaries);
            DataExist = true;
        }

        public override void Save()
        {
            _dictionaries.Buildings[nameof(ForgeModel)] = Forge.GetModel();
            var buildings = _dictionaries.Buildings.ToList();
            EditorUtils.Save(buildings);
            base.Save();
        }

        [ShowInInspector]
        [ShowIf(nameof(DataExist))]
        [HorizontalGroup("3")]
        [LabelText("Forge")]
        [PropertyOrder(2)]
        public ForgeModelEditor Forge;
    }
}
