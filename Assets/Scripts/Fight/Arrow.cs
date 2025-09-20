using Fight.Common.Strikes;
using System;
using Fight.Common.HeroControllers.Generals;
using UniRx;
using UnityEngine;

namespace Fight.Common
{
    public class Arrow : MonoBehaviour
    {
        protected Rigidbody rb;
        protected Transform tr;
        private bool isDone = false;

        protected HeroController target;
        public float speed = 10f;

        private ReactiveCommand<HeroController> _onReachTarget = new();

        public IObservable<HeroController> OnReachTarget => _onReachTarget;

        void Awake()
        {
            tr = GetComponent<Transform>();
            rb = GetComponent<Rigidbody>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.GetComponent<HeroController>() == target)
            {
                if (isDone == false)
                {
                    isDone = true;
                    CollisionTarget(target);
                }
            }
        }

        protected virtual void CollisionTarget(HeroController target)
        {
            _onReachTarget.Execute(target);
            OffArrow();
        }

        public void OffArrow()
        {
            Destroy(gameObject);
        }

        public virtual void SetTarget(HeroController target)
        {
            this.target = target;
            Vector3 dir = (target.transform.position - tr.position).normalized;
            rb.linearVelocity = dir * speed;
            tr.forward = dir;
        }
    }
}