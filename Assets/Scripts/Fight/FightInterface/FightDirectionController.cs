using Fight.HeroControllers.Generals;
using System.Collections.Generic;
using UnityEngine;
using VContainerUi.Abstraction;

namespace Fight.FightInterface
{
    public class FightDirectionController : UiController<FightDirectionView>
    {
        public MelleeAtackDirectionController melleeAttackController;
        private HeroController heroController;
        private List<HeroController> listWaits = new List<HeroController>();

        //public MelleeAtackDirectionController melleeAttackController => SelectDirection.melleeAttackController;

        void Start()
        {
            //FightController.Instance.RegisterOnFinishFight(CloseControllers);
            //FightController.Instance.RegisterOnEndRound(ClearData);
        }

        public void WaitTurn()
        {
            //if (GridController.PlayerCanController)
            //{
            //    FightController.Instance.WaitTurn();
            //    listWaits.Add(FightController.Instance.GetCurrentHero());
            //}
        }

        public void StartDefend()
        {
            //if (GridController.PlayerCanController)
            //{
            //    FightController.Instance.GetCurrentHero().StartDefend();
            //}
        }

        public void UseSpell()
        {
            //FightController.Instance.GetCurrentHero().UseSpecialSpell();
        }

        //API


        private void HeroChangeStamina(int stamina)
        {
            Debug.Log($"HeroChangeStamina: {stamina}");
            View.btnSpell.interactable = stamina == 100;
        }

        private void ClearController()
        {
            HeroController.UnregisterOnEndAction(ClearController);
            heroController?.statusState?.UnregisterOnChangeStamina(HeroChangeStamina);
            heroController = null;
            View.btnSpell.interactable = false;
        }

        void ClearData()
        {
            listWaits.Clear();
        }
        public void OpenControllers(HeroController heroController)
        {
            this.heroController = heroController;
            View.panelControllers.gameObject.SetActive(true);
            HeroController.RegisterOnEndAction(ClearController);
            View.btnWait.interactable = !listWaits.Contains(heroController);
            View.btnSpell.gameObject.SetActive(heroController.SpellExist);
            heroController.statusState.RegisterOnChangeStamina(HeroChangeStamina);
            HeroChangeStamina(heroController.Stamina);

        }

        public void CloseControllers()
        {
            View.panelControllers.gameObject.SetActive(false);
        }
    }
}