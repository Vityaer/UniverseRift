using Common.Resourses;
using System;

namespace UIController.Rewards
{
    [Serializable]
    public class PosibleRewardResource : PosibleRewardObject
    {
        public TypeResource subject = TypeResource.Diamond;
        public Resource GetResource { get { return new Resource(subject); } }
    }
}