using Db.CommonDictionaries;
using Editor.Common;
using Models.City.Mines;
using Sirenix.OdinInspector;
using System.Linq;
using Utils;

namespace Pages.Buildings.Mines.Settings
{
    public class MineSettingsPageEditor : BasePageEditor
    {
        private CommonDictionaries _dictionaries;
        private string NAME => nameof(MineSettingsPageEditor);

        public MineSettingsPageEditor(CommonDictionaries commonDictionaries)
        {
            _dictionaries = commonDictionaries;
            Init();
        }

        public override void Init()
        {
            base.Init();
            MineBuildingModel magicCircleBuildingModel;
            if (_dictionaries.Buildings.ContainsKey(NAME))
            {

                magicCircleBuildingModel = _dictionaries.Buildings[NAME] as MineBuildingModel;
            }
            else
            {
                magicCircleBuildingModel = new MineBuildingModel();
                magicCircleBuildingModel.Id = NAME;
                _dictionaries.Buildings.Add(NAME, magicCircleBuildingModel);
            }

            MineSettings = magicCircleBuildingModel;
            MineSettings.SetCommonDictionary(_dictionaries);
            DataExist = true;
        }

        public override void Save()
        {
            _dictionaries.Buildings[NAME] = MineSettings;
            var buildings = _dictionaries.Buildings.Values.ToList();

            EditorUtils.Save(buildings);
            base.Save();
        }

        [ShowInInspector]
        [ShowIf(nameof(DataExist))]
        [HorizontalGroup("3")]
        [LabelText("Mine Settings")]
        [PropertyOrder(2)]
        public MineBuildingModel MineSettings;
    }
}
