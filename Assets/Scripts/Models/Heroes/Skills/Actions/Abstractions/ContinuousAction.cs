using Fight.Common.Rounds;
using Models.Heroes.Actions;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Heroes.Skills.Actions.Imps.SimpleActions
{
    public abstract class ContinuousAction : AbstractAction
    {
        [LabelWidth(150)] public DropOrSum RepeatCall;
        [LabelWidth(150)] public List<Round> Rounds = new();
    }
}
