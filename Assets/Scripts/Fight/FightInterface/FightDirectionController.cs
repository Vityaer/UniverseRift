using System;
using System.Collections.Generic;
using Fight.Common.HeroControllers.Generals;
using UniRx;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using VContainerUi.Abstraction;

namespace Fight.Common.FightInterface
{
    public class FightDirectionController : UiController<FightDirectionView>, IInitializable, IDisposable
    {
        //[Inject] private readonly FightController _fightController;

        private HeroController heroController;
        private List<HeroController> listWaits = new List<HeroController>();
        private CompositeDisposable _disposables = new CompositeDisposable();

        public MelleeAtackDirectionController MelleeAttackController => View.melleeAttackController;

        public void Initialize()
        {
            //_fightController.OnFinishFight.Subscribe(_ => CloseControllers()).AddTo(_disposables);
            //_fightController.OnEndRound.Subscribe(_ => ClearData()).AddTo(_disposables);
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
            View.btnSpell.interactable = stamina == 100;
        }

        private void ClearController()
        {
            HeroController.UnregisterOnEndAction(ClearController);
            heroController?.StatusState?.UnregisterOnChangeStamina(HeroChangeStamina);
            heroController = null;
            View.btnSpell.interactable = false;
        }

        public void ClearData()
        {
            listWaits.Clear();
        }

        public void OpenControllers(HeroController heroController)
        {
            this.heroController = heroController;
            View.panelControllers.SetActive(true);
            HeroController.RegisterOnEndAction(ClearController);
            View.btnWait.interactable = !listWaits.Contains(heroController);
            View.btnSpell.gameObject.SetActive(heroController.SpellExist);
            heroController.StatusState.RegisterOnChangeStamina(HeroChangeStamina);
            HeroChangeStamina(heroController.Stamina);

        }

        public void CloseControllers()
        {
            View.panelControllers.SetActive(false);
        }

        public void Dispose()
        {
            _disposables.Dispose();
        }

    }
}