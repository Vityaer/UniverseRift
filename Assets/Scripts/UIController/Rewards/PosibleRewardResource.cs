using Common.Resourses;
using System;

namespace UIController.Rewards
{
    [Serializable]
    public class PosibleRewardResource : PosibleRewardObject
    {
        public ResourceType subject = ResourceType.Diamond;
        public GameResource GetResource { get { return new GameResource(subject); } }
    }
}