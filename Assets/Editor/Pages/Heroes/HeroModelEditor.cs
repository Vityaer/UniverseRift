using Db.CommonDictionaries;
using Editor.Common;
using Models.Heroes;
using Models.Heroes.Evolutions;
using Models.Heroes.HeroCharacteristics;
using Models.Heroes.Skills;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;

namespace Editor.Pages.Heroes
{
    [HideReferenceObjectPicker]
    public class HeroModelEditor : BaseModelEditor<HeroModel>
    {
        private CommonDictionaries _dictionaries;

        private string[] _allResistances => _dictionaries.Resistances.Select(c => c.Value).Select(r => r.Id).ToArray();
        private string[] _allRaces => _dictionaries.Races.Select(c => c.Value).Select(r => r.Id).ToArray();
        private string[] _allVocations => _dictionaries.Vocations.Select(c => c.Value).Select(r => r.Id).ToArray();

        public HeroModelEditor(HeroModel model, CommonDictionaries commonDictionaries)
        {
            _model = model;
            _dictionaries = commonDictionaries;

            if (_model.General == null)
            {
                _model.General = new GeneralInfoHero();
                _model.Characteristics = new Characteristics();
                _model.IncCharacts = new IncreaseCharacteristicsModel();
                _model.Evolutions = new Evolution();
                _model.Resistances = new StorageResistances();
            }
        }

        [ShowInInspector]
        [LabelText("Id")]
        [LabelWidth(50)]
        public string Id
        {
            get => _model.Id;
            set
            {
                _model.Id = value;
                _model.General.HeroId = value;
            }
        }

        [FoldoutGroup("GeneralInfo", expanded: true)]
        public string HeroId
        {
            get => _model.General.HeroId;
            set => _model.General.HeroId = value;
        }

        [ShowInInspector]
        [FoldoutGroup("GeneralInfo")]
        [ValueDropdown(nameof(_allRaces), IsUniqueList = true, DropdownWidth = 250, SortDropdownItems = true)]
        public string Race
        {
            get => _model.General.Race;
            set => _model.General.Race = value;
        }

        [ShowInInspector]
        [FoldoutGroup("GeneralInfo")]
        [ValueDropdown(nameof(_allVocations), IsUniqueList = true, DropdownWidth = 250, SortDropdownItems = true)]
        public string ClassHero
        {
            get => _model.General.Vocation;
            set => _model.General.Vocation = value;
        }

        [ShowInInspector]
        [FoldoutGroup("GeneralInfo")]
        public Rare Rare
        {
            get => _model.General.Rare;
            set => _model.General.Rare = value;
        }

        [ShowInInspector]
        [LabelText("Characteristics")]
        [LabelWidth(150)]
        public Characteristics Characts
        {
            get => _model.Characteristics;
            set => _model.Characteristics = value;
        }

        [ShowInInspector]
        [LabelText("IncreaseCharacteristics")]
        [LabelWidth(150)]
        public IncreaseCharacteristicsModel IncCharacts
        {
            get => _model.IncCharacts;
            set => _model.IncCharacts = value;
        }

        [ShowInInspector]
        [LabelText("Evolutions")]
        [LabelWidth(150)]
        public Evolution Evolutions
        {
            get => _model.Evolutions;
            set => _model.Evolutions = value;
        }
        

        [ShowInInspector]
        [LabelText("Resistance")]
        [LabelWidth(150)]
        public StorageResistances Resistance
        {
            get => _model.Resistances;
            set => _model.Resistances = value;
        }

        [ShowInInspector]
        [LabelText("Skills")]
        [LabelWidth(150)]
        public List<SkillModel> Skills
        {
            get => _model.Skills;
            set => _model.Skills = value;
        }
    }
}
