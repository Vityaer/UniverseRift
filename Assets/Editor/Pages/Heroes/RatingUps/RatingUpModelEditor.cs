using Editor.Common;
using Models.City.TrainCamp;
using Models.Data.Inventories;
using Models.Heroes.PowerUps;
using Sirenix.OdinInspector;
using System.Collections.Generic;

namespace Assets.Editor.Pages.Heroes.RatingUps
{
    public class RatingUpModelEditor : BaseModelEditor<RatingUpContainer>
    {
        public RatingUpModelEditor(RatingUpContainer model)
        {
            _model = model;
        }

        [ShowInInspector]
        [HorizontalGroup("1")]
        [LabelText("Id")]
        [LabelWidth(150)]
        public string Id
        {
            get => _model.Id;
            set => _model.Id = value;
        }

        [ShowInInspector]
        [HorizontalGroup("2")]
        [LabelText("Cost")]
        [LabelWidth(150)]
        public List<ResourceData> Cost
        {
            get => _model.Cost;
            set => _model.Cost = value;
        }

        [ShowInInspector]
        [HorizontalGroup("3")]
        [LabelText("RequirementHeroes")]
        [LabelWidth(150)]
        public List<RequirementHeroModel> RequirementHeroes
        {
            get => _model.RequirementHeroes;
            set => _model.RequirementHeroes = value;
        }
    }
}
