using Fight.Common.Strikes;
using Fight.HeroControllers.Generals;
using UnityEngine;

namespace Fight
{
    public class Arrow : MonoBehaviour
    {
        protected Rigidbody2D rb;
        protected Transform tr;
        private bool isDone = false;

        protected HeroController target;
        protected Strike strike = null;
        public float speed = 10f;

        public delegate void Del();
        protected Del delsCollision;

        void Awake()
        {
            tr = GetComponent<Transform>();
            rb = GetComponent<Rigidbody2D>();
        }

        private void OnTriggerEnter2D(Collider2D other)
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
            if (strike != null)
                target.GetDamage(strike);

            if (delsCollision != null)
                delsCollision();

            OffArrow();
        }

        public void OffArrow()
        {
            Destroy(gameObject);
        }

        public virtual void SetTarget(HeroController target, Strike strike)
        {
            this.target = target;
            this.strike = strike;
            Vector3 dir = target.GetPosition - tr.position;
            dir.Normalize();
            rb.velocity = dir * speed;
        }

        public void RegisterOnCollision(Del d)
        {
            delsCollision += d;
        }

        public void UnRegisterOnCollision(Del d)
        {
            delsCollision -= d;
        }
    }
}