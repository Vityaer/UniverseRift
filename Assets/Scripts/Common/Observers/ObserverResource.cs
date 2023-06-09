using Common.Resourses;
using System;

namespace Common.Observers
{
    public class ObserverResource
    {
        public TypeResource typeResource;
        public Action<Resource> delObserverResource;
        public ObserverResource(TypeResource type)
        {
            typeResource = type;
        }

        public void RegisterOnChangeResource(Action<Resource> d)
        {
            delObserverResource += d;
        }
        public void UnRegisterOnChangeResource(Action<Resource> d)
        {
            delObserverResource -= d;
        }
        public void ChangeResource(Resource res)
        {
            if (delObserverResource != null)
                delObserverResource(res);
        }
    }
}
