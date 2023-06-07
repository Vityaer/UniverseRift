using System.Collections.Generic;
using UnityEngine;

namespace Common.Resourses
{
    [System.Serializable]
    public class ListResource : ICloneable
    {

        [SerializeField]
        private List<Resource> resources = new List<Resource>();
        public List<Resource> List { get => resources; set => resources = value; }
        public int Count { get => List.Count; }
        public bool CheckResource(Resource res)
        {
            return CheckResource(new ListResource(res));
        }
        public bool CheckResource(ListResource resources)
        {
            bool result = true;
            foreach (Resource res in resources.List)
            {
                if (GetResource(res.Name).CheckCount(res) == false)
                {
                    result = false;
                    break;
                }
            }
            return result;
        }
        public void AddResource(Resource res)
        {
            AddResource(new ListResource(res));
        }
        public void AddResource(ListResource listResource)
        {
            foreach (Resource res in listResource.List)
            {
                GetResource(res.Name).AddResource(res);
            }
        }
        public void AddResource(List<Resource> listResource)
        {
            foreach (Resource res in listResource)
            {
                GetResource(res.Name).AddResource(res);
            }
        }
        public void SubtractResource(Resource res)
        {
            SubtractResource(new ListResource(res));
        }
        public void SubtractResource(ListResource resources)
        {
            foreach (Resource res in resources.List)
            {
                GetResource(res.Name).SubtractResource(res);
            }
        }
        public void SubtractResource(List<Resource> resources)
        {
            foreach (Resource res in resources)
            {
                GetResource(res.Name).SubtractResource(res);
            }
        }
        public void SetResource(Resource resource)
        {
            var selectResource = GetResource(resource.Name);
            selectResource = resource;
            resources.Add(selectResource);
        }
        public Resource GetResource(TypeResource name)
        {
            Resource result = resources.Find(x => x.Name == name);
            if (result == null) { result = new Resource(name); resources.Add(result); }
            return result;
        }


        public ListResource(Resource res)
        {
            SetResource(res);
        }
        public ListResource(params Resource[] resources)
        {
            for (int i = 0; i < resources.Length; i++)
            {
                SetResource(resources[i]);
            }
        }
        public ListResource(List<Resource> listResource)
        {
            foreach (Resource res in listResource)
            {
                SetResource(res);
            }
        }
        public ListResource(ListResource listResource)
        {
            foreach (Resource res in listResource.List)
            {
                SetResource(res);
            }
        }
        public ListResource() { }
        public void Clear()
        {
            resources.Clear();
        }

        //Operators
        public static ListResource operator *(ListResource resources, float k)
        {
            ListResource result = new ListResource(resources);
            for (int i = 0; i < result.List.Count; i++)
            {
                result.List[i] = result.List[i] * k;
            }
            return result;
        }


        public object Clone()
        {
            List<Resource> listRes = new List<Resource>();
            for (int i = 0; i < resources.Count; i++)
                listRes.Add((Resource)resources[i].Clone());
            return new ListResource { resources = listRes };
        }
    }
}