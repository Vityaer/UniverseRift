using Fight.Common.HeroControllers.Generals;
using Fight.Common.Misc;
using Fight.Common.Rounds;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace Models.Heroes.Actions
{
    [System.Serializable]
    public abstract class AbstractAction
    {
        [LabelWidth(150)] public SideTarget SideTargetType;
        [LabelWidth(150)] [HideIf("@this.SideTargetType == SideTarget.All || this.SideTargetType == SideTarget.I")] public TypeSelect TypeSelect = TypeSelect.Order;
        [LabelWidth(150)] [HideIf("@this.SideTargetType == SideTarget.All || this.SideTargetType == SideTarget.I")] public int CountTarget;
        [LabelWidth(150)] [HideIf("@this.SideTargetType == SideTarget.All || this.SideTargetType == SideTarget.I")] public RecalculateMethodTarget RecalculateTarget;
        [LabelWidth(150)] public float Chance = 100f;
        [LabelWidth(150)] public float Amount;
        [LabelWidth(150)] public RoundTypeNumber TypeNumber;

        [Space]
        protected List<HeroController> ListTarget = new();
        private HeroController _owner;

        public HeroController Owner { get => _owner; set => _owner = value; }

        public void SetNewTarget(List<HeroController> listTarget)
        {
            this.ListTarget.Clear();
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

                        _owner.FightController.ChooseEnemies(side, CountTarget, this.ListTarget, TypeSelect);
                    }
                    else
                    {
                        this.ListTarget.Add(_owner);
                    }
                }
                else
                {
                    this.ListTarget = listTarget;
                }

                switch (RecalculateTarget)
                {
                    case RecalculateMethodTarget.AddTargets:
                        listTarget.AddRange(this.ListTarget);
                        break;
                    case RecalculateMethodTarget.NewTargets:
                        listTarget = this.ListTarget;
                        break;
                    case RecalculateMethodTarget.OldTargets:
                        listTarget = this.ListTarget;
                        break;
                }
            }
            else
            {
                this.ListTarget = listTarget;
            }
        }

        public abstract void ExecuteAction();

        public void GetListForSpell(List<HeroController> listTarget)
        {
            SetNewTarget(listTarget);
            if (SideTargetType != SideTarget.I)
            {
                _owner.ListTarget = this.ListTarget;
            }
        }

    }
}