using Effects;
using Fight.Common.HeroControllers.Generals.Attacks;
using Fight.Common.HeroControllers.Generals.Movements;
using Sirenix.OdinInspector;
using UnityEngine;
using Utils.Development;

namespace Fight.Common.HeroControllers.VisualModels
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(OutlineController))]
    public class HeroVisualModel : MonoBehaviour, ICreatable
    {
        [field: SerializeField] public AbstractAttack AttackController { get; private set; }
        [field: SerializeField] public Sprite Avatar { get; private set; }
        [field: SerializeField] public Animator Animator { get; private set; }
        [field: SerializeField] public OutlineController OutlineController { get; private set; }

        [SerializeField] private bool _overrideMovement;
        [ShowIf("_overrideMovement")][SerializeField] private AbstractMovement _movementController;

        public bool OverrideMovement => _overrideMovement;
        public AbstractMovement MovementController => _movementController;

        public void Activate()
        {
            gameObject.SetActive(true);
        }

        public void Deactivate()
        {
            gameObject.SetActive(false);
        }

        public void OnCreateComponent()
        {
#if UNITY_EDITOR
            UnityEditor.Undo.RecordObject(this, "Get components");
            Animator = GetComponent<Animator>();
            OutlineController = GetComponent<OutlineController>();
            AttackController = GetComponent<AbstractAttack>();
            UnityEditor.PrefabUtility.RecordPrefabInstancePropertyModifications(this);
#endif
        }
    }
}
