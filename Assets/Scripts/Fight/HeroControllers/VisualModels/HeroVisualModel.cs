using Effects;
using Fight.HeroControllers.Generals.Attacks;
using UnityEngine;

namespace Fight.HeroControllers.VisualModels
{
    public class HeroVisualModel : MonoBehaviour
    {
        [field: SerializeField] public AbstractAttack AttackController { get; private set; }
        [field: SerializeField] public Sprite Avatar { get; private set; }
        [field: SerializeField] public Animator Animator { get; private set; }
        [field: SerializeField] public OutlineController OutlineController { get; private set; }

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
