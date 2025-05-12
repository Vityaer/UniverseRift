using Fight.HeroControllers.Generals;
using System;
using UIController;
using UnityEngine;
using VContainer;
using UniRx;

namespace Fight.HeroStates
{
    public partial class HeroStatus : MonoBehaviour
    {
        private FightController _fightController;

        [SerializeField] private HeroController heroController;
        public SmoothSlider sliderHP;
        public SmoothSlider sliderStamina;
        private Vector2 delta = new Vector2(0, 30);
        private Action<int> observerStamina;
        private int stamina = 25;
        // private GameObject panelStatus;
        private float currentHP;
        private CompositeDisposable _disposables = new CompositeDisposable();
        public int Stamina => stamina;

        [Inject]
        public void Construct(FightController fightController)
        {
            _fightController = fightController;
            _fightController.OnEndRound.Subscribe(_ => RoundFinish()).AddTo(_disposables);
        }

        void Awake()
        {
            heroController = GetComponent<HeroController>();
        }

        //Helth	
        public void ChangeHP(float HP)
        {
            if (HP < currentHP)
            {
                //PoolFightTexts.Instance.ShowDamage(currentHP - HP, gameObject.transform.position);
            }
            else
            {
                //PoolFightTexts.Instance.ShowHeal(HP - currentHP, gameObject.transform.position);
            }

            currentHP = HP;
            sliderHP.ChangeValue(HP);
            if ((HP / sliderHP.MaxValue < 0.5f) && (HP / sliderHP.MaxValue > 0.3f))
            {
                //heroController.OnHPLess50();
            }
            else if (HP / sliderHP.MaxValue < 0.3f)
            {
                //heroController.OnHPLess30();
            }
        }

        public void SetMaxHealth(float maxHP)
        {
            if (currentHP == 0) currentHP = maxHP;
            sliderHP.SetMaxValue(maxHP);
        }

        public void ChangeMaxHP(float amountChange)
        {
            SetMaxHealth(sliderHP.MaxValue + amountChange);
        }

        public void Death()
        {
            sliderHP.Death();
            sliderStamina.Death();
        }
        //Stamina

        public void ChangeStamina(int addStamina)
        {
            stamina = (int)Mathf.Clamp(stamina + addStamina, 0, 100);
            sliderStamina.ChangeValue(stamina);
            OnChangeStamina(stamina);
        }

        public void RegisterOnChangeStamina(Action<int> d)
        {
            observerStamina += d;
        }

        public void UnregisterOnChangeStamina(Action<int> d)
        {
            observerStamina -= d;
        }

        private void OnChangeStamina(int num)
        {
            if (observerStamina != null)
                observerStamina(num);
        }

        private void OnDestroy()
        {
            _disposables.Dispose();
        }
    }
}