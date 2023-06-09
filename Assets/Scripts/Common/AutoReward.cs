using Common.Resourses;
using Sirenix.Serialization;
using System;
using System.Collections.Generic;
using UIController.Inventory;
using UIController.Rewards;
using UnityEngine;

namespace GeneralObject
{
    [Serializable]
    public class AutoReward
    {
        [OdinSerialize] public ListResource resources = new ListResource(new Resource(TypeResource.Gold), new Resource(TypeResource.ContinuumStone), new Resource(TypeResource.Exp));
        [OdinSerialize] public List<PosibleRewardResource> listPosibleResources = new List<PosibleRewardResource>();
        [OdinSerialize] public List<PosibleRewardItem> listPosibleItems = new List<PosibleRewardItem>();
        [OdinSerialize] public List<PosibleRewardSplinter> listPosibleSplinters = new List<PosibleRewardSplinter>();

        public Reward GetCaculateReward(int countTact)
        {
            Reward reward = new Reward();
            foreach (Resource res in resources.List) reward.GetListResource.List.Add((Resource)res.Clone() * countTact);
            GetPosibleResource(reward, countTact);
            GetPosibleItem(reward, countTact);
            GetPosibleSplinter(reward, countTact);
            return reward;
        }
        int result = 0;
        float currentPosible = 0f;
        Resource workResource = null;
        private void GetPosibleResource(Reward reward, int countTact)
        {
            Debug.Log("resources");
            result = 0;
            for (int i = 0; i < listPosibleResources.Count; i++)
            {
                currentPosible = listPosibleResources[i].Posibility / 1000f;
                for (int j = 0; j < countTact; j++)
                {
                    if (UnityEngine.Random.Range(0f, 100f) < currentPosible)
                    {
                        result += 1;
                        Debug.Log("выпал ресурс, уже " + result.ToString() + " шт.");
                    }
                }
                Debug.Log(listPosibleResources[i].subject.ToString() + " count: " + result.ToString());
                if (result > 0)
                {
                    // result = Math.Min(result, Math.Max((int) listPosibleItems[i].Posibility / 3, 1));
                    workResource = listPosibleResources[i].GetResource;
                    workResource.AddResource(result);
                    reward.GetListResource.List.Add(workResource);
                }
            }
        }
        Item workItem = null;
        private void GetPosibleItem(Reward reward, int countTact)
        {
            Debug.Log("items");
            result = 0;
            for (int i = 0; i < listPosibleItems.Count; i++)
            {
                currentPosible = listPosibleItems[i].Posibility / 1000f;
                for (int j = 0; j < countTact; j++)
                {
                    if (UnityEngine.Random.Range(0f, 100f) < currentPosible)
                    {
                        result += 1;
                        Debug.Log("выпал предмет, уже " + result.ToString() + " шт.");
                    }
                }
                Debug.Log(listPosibleItems[i].ID.ToString() + " count: " + result.ToString());

                if (result > 0)
                {
                    // result = Math.Min(result, Math.Max((int) listPosibleItems[i].Posibility / 3, 1));
                    workItem = listPosibleItems[i].GetItem;
                    workItem.Amount = result;
                    reward.AddItem(workItem);
                }
            }
        }
        Splinter workSplinter = null;
        private void GetPosibleSplinter(Reward reward, int countTact)
        {
            Debug.Log("splinters");
            result = 0;
            for (int i = 0; i < listPosibleSplinters.Count; i++)
            {
                currentPosible = listPosibleSplinters[i].Posibility / 1500f;
                for (int j = 0; j < countTact; j++)
                {
                    if (UnityEngine.Random.Range(0f, 100f) < currentPosible)
                    {
                        result += 1;
                        Debug.Log("выпал сплинтер, уже " + result.ToString() + " шт.");
                    }
                }
                Debug.Log(listPosibleSplinters[i].ID.ToString() + " count: " + result.ToString());
                if (result > 0)
                {
                    // result = Math.Min(result, Math.Max((int) listPosibleItems[i].Posibility / 3, 1));
                    workSplinter = listPosibleSplinters[i].GetSplinter;
                    workSplinter.Amount = result;
                    reward.AddSplinter(workSplinter);
                }
            }
        }

        public int Count
        {
            get
            {
                return listPosibleResources.Count + listPosibleItems.Count + listPosibleSplinters.Count;
            }
        }
        public void GetListPosibleRewards(List<PosibleRewardObject> result)
        {
            result.AddRange(listPosibleResources);
            result.AddRange(listPosibleItems);
            result.AddRange(listPosibleSplinters);

        }
    }
}