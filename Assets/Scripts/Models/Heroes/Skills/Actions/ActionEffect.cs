using Fight;
using Fight.HeroControllers.Generals;
using Fight.Misc;
using Fight.Rounds;
using Models.Heroes.Skills.Actions.Effects;
using System.Collections.Generic;
using UnityEngine;

namespace Models.Heroes.Actions
{
    [System.Serializable]
    public partial class ActionEffect
    {
        public TypeEffect typeAction;
        public EffectSimpleAction simpleAction;
        public EffectBuff effectBuff;
        public EffectDebuff effectDebuff;
        public EffectDots effectDots;
        public EffectChangeCharacteristic effectChangeCharacteristic;
        public EffectStatus effectStatus;
        public EffectMark effectMark;
        public EffectOther effectOther;
        public EffectSpecial effectSpecial;

        public SideTarget sideTarget;
        public TypeSelect typeSelect = TypeSelect.Order;
        public int countTarget;
        public RecalculateMethodTarget recalculateTarget;
        public float chance = 100f;
        public float amount;
        public RoundTypeNumber typeNumber;
        public DropOrSum RepeatCall;
        public List<Round> rounds = new List<Round>();
        [Space]
        private List<HeroController> listTarget = new List<HeroController>();
        private HeroController master;
        public HeroController Master { get => master; set => master = value; }
        public void SetNewTarget(List<HeroController> listTarget)
        {
            this.listTarget.Clear();
            if (listTarget.Count == 0 || recalculateTarget != RecalculateMethodTarget.OldTargets)
            {
                Side side = master.Side;
                if (sideTarget != SideTarget.Select)
                {
                    if (sideTarget != SideTarget.I)
                    {
                        if (sideTarget == SideTarget.Friend) { if (side == Side.Left) { side = Side.Right; } else { side = Side.Left; } }
                        FightController.Instance.ChooseEnemies(side, countTarget, this.listTarget, typeSelect);
                    }
                    else
                    {
                        this.listTarget.Add(master);
                    }
                }
                else
                {
                    this.listTarget = listTarget;
                }
                switch (recalculateTarget)
                {
                    case RecalculateMethodTarget.AddTargets:
                        listTarget.AddRange(this.listTarget);
                        break;
                    case RecalculateMethodTarget.NewTargets:
                        listTarget = this.listTarget;
                        break;
                    case RecalculateMethodTarget.OldTargets:
                        listTarget = this.listTarget;
                        break;
                }
            }
            else
            {
                this.listTarget = listTarget;
            }
        }

        public void ExecuteAction()
        {
            switch (typeAction)
            {
                case TypeEffect.SimpleAction:
                    ExecuteSimpleAction();
                    break;
                case TypeEffect.Buff:
                    ExecuteBuff();
                    break;
                case TypeEffect.Debuff:
                    ExecuteDebuff();
                    break;
                case TypeEffect.Dots:
                    ExecuteDots();
                    break;
                case TypeEffect.Mark:
                    ExecuteMark();
                    break;
                case TypeEffect.ChangeCharacteristic:
                    ExecuteChangeCharacteristic();
                    break;
                case TypeEffect.StatusHero:
                    ExecuteStatusHero();
                    break;
                case TypeEffect.Special:
                    ExecuteSpecial();
                    break;
                case TypeEffect.Other:
                    ExecuteOther();
                    break;
            }
        }

        public void GetListForSpell(List<HeroController> listTarget)
        {
            SetNewTarget(listTarget);
            if (sideTarget != SideTarget.I)
            {
                master.listTarget = this.listTarget;
            }
        }

    }
}