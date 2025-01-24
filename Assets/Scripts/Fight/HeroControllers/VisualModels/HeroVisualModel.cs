using Effects;
using Fight.HeroControllers.Generals.Attacks;
using Fight.HeroControllers.Generals.Movements;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Fight.HeroControllers.VisualModels
{
    public class HeroVisualModel : MonoBehaviour
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
    }
}
