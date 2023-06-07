using Fight.HeroControllers.Generals;
using System.Collections.Generic;
using UnityEngine;

namespace Fight
{
    public class FightEffectController : MonoBehaviour
    {
        [Header("Simple Effects")]
        public GameObject healPrefab;
        private List<GameObject> healGameObjects;
        [Header("Constant Effects")]
        public Color statePetrification;
        public Color stateFreezing;
        public Color stateAstral;
        public Color stateClear;
        public GameObject stateStun;
        public GameObject stateSilence;

        [Header("Dots")]
        public GameObject dotPoisonPrefab;
        public GameObject dotBleendingPrefab;
        public GameObject dotRotPrefab;
        public GameObject dotCorrosionPrefab;
        public GameObject dotCombustionPrefab;

        public static FightEffectController Instance { get; private set; }

        void Awake()
        {
            Instance = this;
        }

        //API
        public void CreateHealObject(Transform parent)
        {
            Instantiate(healPrefab, parent.position, Quaternion.identity, parent);
        }
        public void CreateDot(Transform parent, DotType dot)
        {
            switch (dot)
            {
                case DotType.Poison:
                    Instantiate(dotPoisonPrefab, parent.position, Quaternion.identity, parent);
                    break;
                case DotType.Bleending:
                    Instantiate(dotBleendingPrefab, parent.position, Quaternion.identity, parent);
                    break;
                case DotType.Rot:
                    Instantiate(dotRotPrefab, parent.position, Quaternion.identity, parent);
                    break;
                case DotType.Corrosion:
                    Instantiate(dotCorrosionPrefab, parent.position, Quaternion.identity, parent);
                    break;
                case DotType.Combustion:
                    Instantiate(dotCombustionPrefab, parent.position, Quaternion.identity, parent);
                    break;
            }
        }

        public void CastEffectStateOnHero(GameObject hero, State state)
        {
            HeroController heroScript = hero.GetComponent<HeroController>();
            switch (state)
            {
                case State.Stun:
                    Instantiate(stateStun, hero.transform.position, Quaternion.identity, hero.transform);
                    break;
                case State.Astral:
                    heroScript.GetSpriteRenderer.color = stateAstral;
                    break;
                case State.Freezing:
                    heroScript.GetSpriteRenderer.color = stateFreezing;
                    break;
                case State.Silence:
                    Instantiate(stateStun, hero.transform.position, Quaternion.identity, hero.transform);
                    break;
                case State.Petrification:
                    heroScript.GetSpriteRenderer.color = statePetrification;
                    break;

            }
        }
        public void ClearEffectStateOnHero(GameObject hero, State state)
        {
            hero.GetComponent<HeroController>().GetSpriteRenderer.color = stateClear;
            switch (state)
            {
                case State.Silence:
                    Destroy(hero.transform.Find("SilencePrefab(Clone)").gameObject);
                    break;
                case State.Stun:
                    Destroy(hero.transform.Find("StunPrefab(Clone)").gameObject);
                    break;
            }
        }
    }
}