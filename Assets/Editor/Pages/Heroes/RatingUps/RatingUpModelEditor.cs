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
            m_model = model;
        }

        [ShowInInspector]
        [HorizontalGroup("1")]
        [LabelText("Id")]
        [LabelWidth(150)]
        public string Id
        {
            get => m_model.Id;
            set => m_model.Id = value;
        }

        [ShowInInspector]
        [HorizontalGroup("2")]
        [LabelText("Cost")]
        [LabelWidth(150)]
        public List<ResourceData> Cost
        {
            get => m_model.Cost;
            set => m_model.Cost = value;
        }

        [ShowInInspector]
        [HorizontalGroup("3")]
        [LabelText("RequirementHeroes")]
        [LabelWidth(150)]
        public List<RequirementHeroModel> RequirementHeroes
        {
            get => m_model.RequirementHeroes;
            set => m_model.RequirementHeroes = value;
        }
    }
}
