using Fight;
using Fight.HeroControllers.Generals;
using Fight.Misc;
using Fight.Rounds;
using Models.Heroes.Skills.Actions.Effects;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace Models.Heroes.Actions
{
    [System.Serializable]
    public partial class ActionEffect
    {
        public TypeEffect TypeAction;
        [ShowIf("TypeAction", TypeEffect.SimpleAction)] public EffectSimpleAction SimpleAction;
        [ShowIf("TypeAction", TypeEffect.Buff)] public EffectBuff EffectBuff;
        [ShowIf("TypeAction", TypeEffect.Debuff)] public EffectDebuff EffectDebuff;
        [ShowIf("TypeAction", TypeEffect.Dots)] public EffectDots EffectDots;
        [ShowIf("TypeAction", TypeEffect.ChangeCharacteristic)] public EffectChangeCharacteristic EffectChangeCharacteristic;
        [ShowIf("TypeAction", TypeEffect.StatusHero)] public EffectStatus EffectStatus;
        [ShowIf("TypeAction", TypeEffect.Mark)] public EffectMark EffectMark;
        [ShowIf("TypeAction", TypeEffect.Other)] public EffectOther EffectOther;
        [ShowIf("TypeAction", TypeEffect.Special)] public EffectSpecial EffectSpecial;
            
        public SideTarget SideTargetType;
        [HideIf("@this.SideTargetType == SideTarget.All || this.SideTargetType == SideTarget.I")] public TypeSelect TypeSelect = TypeSelect.Order;
        [HideIf("@this.SideTargetType == SideTarget.All || this.SideTargetType == SideTarget.I")] public int CountTarget;
        [HideIf("@this.SideTargetType == SideTarget.All || this.SideTargetType == SideTarget.I")] public RecalculateMethodTarget RecalculateTarget;
        public float Chance = 100f;
        public float Amount;
        public RoundTypeNumber TypeNumber;
        [HideIf("@this.TypeAction == TypeEffect.SimpleAction")] public DropOrSum RepeatCall;
        [HideIf("@this.TypeAction == TypeEffect.SimpleAction")] public List<Round> Rounds = new();

        [Space]
        private List<HeroController> listTarget = new();
        private HeroController _owner;

        public HeroController Owner { get => _owner; set => _owner = value; }

        public void SetNewTarget(List<HeroController> listTarget)
        {
            this.listTarget.Clear();
            if (listTarget.Count == 0 || RecalculateTarget != RecalculateMethodTarget.OldTargets)
            {
                Side side = _owner.Side;
                if (SideTargetType != SideTarget.Select)
                {
                    if (SideTargetType != SideTarget.I)
                    {
                        if (SideTargetType == SideTarget.Friend)
                        {
                            if (side == Side.Left)
                            {
                                side = Side.Right;
                            }
                            else
                            {
                                side = Side.Left;
                            }
                        }

                        _owner.FightController.ChooseEnemies(side, CountTarget, this.listTarget, TypeSelect);
                    }
                    else
                    {
                        this.listTarget.Add(_owner);
                    }
                }
                else
                {
                    this.listTarget = listTarget;
                }
                switch (RecalculateTarget)
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
            switch (TypeAction)
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
            if (SideTargetType != SideTarget.I)
            {
                _owner.ListTarget = this.listTarget;
            }
        }

    }
}