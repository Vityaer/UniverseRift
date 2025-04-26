using System.Linq;
using Db.CommonDictionaries;
using Models.City.AbstactBuildingModels;
using Sirenix.OdinInspector;
using Utils;

namespace Editor.Common.Pages.Buildings.Abstract
{
    public abstract class AbstractBuildingPageEditor<T> : BasePageEditor
    where T : BuildingModel, new()
    {
        private CommonDictionaries _dictionaries;
        protected abstract string Name { get; }

        public AbstractBuildingPageEditor(CommonDictionaries commonDictionaries)
        {
            _dictionaries = commonDictionaries;
            Init();
        }

        public override void Init()
        {
            base.Init();
            T magicCircleBuildingModel;
            if (_dictionaries.Buildings.ContainsKey(Name))
            {

                magicCircleBuildingModel = _dictionaries.Buildings[Name] as T;
            }
            else
            {
                magicCircleBuildingModel = new T();
                magicCircleBuildingModel.Id = Name;
                _dictionaries.Buildings.Add(Name, magicCircleBuildingModel);
            }

            BuildingModel = magicCircleBuildingModel;
            BuildingModel.SetCommonDictionary(_dictionaries);
            DataExist = true;
        }

        public override void Save()
        {
            _dictionaries.Buildings[Name] = BuildingModel;
            var buildings = _dictionaries.Buildings.Values.ToList();

            EditorUtils.Save(buildings);
            base.Save();
        }

        [ShowInInspector]
        [ShowIf(nameof(DataExist))]
        [HorizontalGroup("3")]
        [LabelText("Building")]
        [PropertyOrder(2)]
        public T BuildingModel;
    }
}