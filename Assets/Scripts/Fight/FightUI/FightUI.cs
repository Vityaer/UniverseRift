using Assets.Scripts.Fight.FightUI;
using Fight.Grid;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Fight.FightUI
{
    public class FightUI : MonoBehaviour
    {

        public FightDirectionsController SelectDirection;
        private List<HeroController> listWaits = new List<HeroController>();
        public Button btnSpell, btnWait;
        public RectTransform panelControllers;
        private HeroController heroController;
        public MelleeAtackDirectionController melleeAttackController => SelectDirection.melleeAttackController;
        private static FightUI instance;
        public static FightUI Instance => instance;

        void Awake()
        {
            instance = this;
        }

        void Start()
        {
            FightController.Instance.RegisterOnFinishFight(CloseControllers);
            FightController.Instance.RegisterOnEndRound(ClearData);
        }

        public void WaitTurn()
        {
            if (GridController.PlayerCanController)
            {
                FightController.Instance.WaitTurn();
                listWaits.Add(FightController.Instance.GetCurrentHero());
            }
        }

        public void StartDefend()
        {
            if (GridController.PlayerCanController)
            {
                FightController.Instance.GetCurrentHero().StartDefend();
            }
        }

        public void UseSpell()
        {
            FightController.Instance.GetCurrentHero().UseSpecialSpell();
        }

        //API
        public void OpenControllers(HeroController heroController)
        {
            this.heroController = heroController;
            panelControllers.gameObject.SetActive(true);
            HeroController.RegisterOnEndAction(ClearController);
            btnWait.interactable = !listWaits.Contains(heroController);
            btnSpell.gameObject.SetActive(heroController.SpellExist);
            heroController.statusState.RegisterOnChangeStamina(HeroChangeStamina);
            HeroChangeStamina(heroController.Stamina);

        }

        private void HeroChangeStamina(int stamina)
        {
            Debug.Log($"HeroChangeStamina: {stamina}");
            btnSpell.interactable = stamina == 100;
        }

        public void CloseControllers()
        {
            panelControllers.gameObject.SetActive(false);
        }

        private void ClearController()
        {
            HeroController.UnregisterOnEndAction(ClearController);
            heroController?.statusState?.UnregisterOnChangeStamina(HeroChangeStamina);
            heroController = null;
            btnSpell.interactable = false;
        }

        void ClearData()
        {
            listWaits.Clear();
        }
    }
}