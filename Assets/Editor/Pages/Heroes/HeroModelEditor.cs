using Db.CommonDictionaries;
using Editor.Common;
using Models.Heroes;
using Models.Heroes.Evolutions;
using Models.Heroes.HeroCharacteristics;
using Sirenix.OdinInspector;
using System.Linq;

namespace Editor.Pages.Heroes
{
    [HideReferenceObjectPicker]
    public class HeroModelEditor : BaseModelEditor<HeroModel>
    {
        private CommonDictionaries _dictionaries;

        private string[] _allResistances => _dictionaries.Resistances.Select(c => c.Value).Select(r => r.Id).ToArray();

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
            set => _model.Id = value;
        }

        [ShowInInspector]
        [LabelText("GeneralInfo")]
        [LabelWidth(150)]
        public GeneralInfoHero General
        {
            get => _model.General;
            set => _model.General = value;
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
        
    }
}
