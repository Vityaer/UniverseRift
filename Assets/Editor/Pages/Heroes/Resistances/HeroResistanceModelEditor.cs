using Sirenix.OdinInspector;

namespace Editor.Pages.Heroes.Resistances
{
    [HideReferenceObjectPicker]
    public class HeroResistanceModelEditor
    {
        private string[] _allResistances;

        public HeroResistanceModelEditor(string[] allResistances)
        {
            _allResistances = allResistances;
        }

        [ShowInInspector]
        [HorizontalGroup("4")]
        [ListDrawerSettings(ShowItemCount = true, ShowIndexLabels = true, Expanded = true, DraggableItems = false)]
        [LabelText("Name")]
        [PropertyOrder(4)]
        [ValueDropdown(nameof(_allResistances), IsUniqueList = true, DropdownWidth = 250, SortDropdownItems = true)]
        public string Name;

        [ShowInInspector]
        [HorizontalGroup("4")]
        [LabelText("Power")]
        [PropertyOrder(4)]
        public float Resistance;

    }
}
