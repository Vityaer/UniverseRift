using Editor.Common;
using Models.Heroes;
using Models.Heroes.Evolutions;
using Models.Heroes.HeroCharacteristics;
using Models.Heroes.Skills;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using Common.Db.CommonDictionaries;

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
            m_model = model;
            _dictionaries = commonDictionaries;

            if (m_model.General == null)
            {
                m_model.General = new GeneralInfoHero();
                m_model.Characteristics = new Characteristics();
                m_model.IncCharacts = new IncreaseCharacteristicsModel();
                m_model.Evolutions = new Evolution();
                m_model.Resistances = new StorageResistances();
            }
        }

        [ShowInInspector]
        [LabelText("Id")]
        [LabelWidth(50)]
        public string Id
        {
            get => m_model.Id;
            set
            {
                m_model.Id = value;
                m_model.General.HeroId = value;
            }
        }

        [FoldoutGroup("GeneralInfo", expanded: true)]
        public string HeroId
        {
            get => m_model.General.HeroId;
            set => m_model.General.HeroId = value;
        }

        [ShowInInspector]
        [FoldoutGroup("GeneralInfo")]
        [ValueDropdown(nameof(_allRaces), IsUniqueList = true, DropdownWidth = 250, SortDropdownItems = true)]
        public string Race
        {
            get => m_model.General.Race;
            set => m_model.General.Race = value;
        }

        [ShowInInspector]
        [FoldoutGroup("GeneralInfo")]
        [ValueDropdown(nameof(_allVocations), IsUniqueList = true, DropdownWidth = 250, SortDropdownItems = true)]
        public string ClassHero
        {
            get => m_model.General.Vocation;
            set => m_model.General.Vocation = value;
        }

        [ShowInInspector]
        [FoldoutGroup("GeneralInfo")]
        public Rare Rare
        {
            get => m_model.General.Rare;
            set => m_model.General.Rare = value;
        }

        [ShowInInspector]
        [LabelText("Characteristics")]
        [LabelWidth(150)]
        public Characteristics Characts
        {
            get => m_model.Characteristics;
            set => m_model.Characteristics = value;
        }

        [ShowInInspector]
        [LabelText("IncreaseCharacteristics")]
        [LabelWidth(150)]
        public IncreaseCharacteristicsModel IncCharacts
        {
            get => m_model.IncCharacts;
            set => m_model.IncCharacts = value;
        }

        [ShowInInspector]
        [LabelText("Evolutions")]
        [LabelWidth(150)]
        public Evolution Evolutions
        {
            get => m_model.Evolutions;
            set => m_model.Evolutions = value;
        }
        

        [ShowInInspector]
        [LabelText("Resistance")]
        [LabelWidth(150)]
        public StorageResistances Resistance
        {
            get => m_model.Resistances;
            set => m_model.Resistances = value;
        }

        [ShowInInspector]
        [LabelText("Skills")]
        [LabelWidth(150)]
        public List<SkillModel> Skills
        {
            get => m_model.Skills;
            set => m_model.Skills = value;
        }
    }
}
