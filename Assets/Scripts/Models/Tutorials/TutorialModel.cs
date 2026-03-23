using System.Collections.Generic;

namespace Models.Tutorials
{
    public class TutorialModel : BaseModel
    {
        public List<ITutorialStage> Stages = new List<ITutorialStage>();
    }
}