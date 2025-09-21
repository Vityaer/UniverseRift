using Common.Resourses;
using System;
using Common.Inventories.Resourses;

namespace UIController.Rewards
{
    [Serializable]
    public class PosibleRewardResource : PosibleRewardObject
    {
        public ResourceType subject = ResourceType.Diamond;
        public GameResource GetResource { get { return new GameResource(subject); } }
    }
}