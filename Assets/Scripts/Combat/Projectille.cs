using Attributes;
using Core;
using UnityEngine;

namespace Combat
{
    public class Projectille : MonoBehaviour
    {
        [SerializeField] private float speed = 2f;
        [SerializeField] private float lifeAfterImpact = 0.2f;
        [SerializeField] private float maxLifeTime = 7f;
        [SerializeField] private bool isHoming = true;
        [SerializeField] private GameObject hitEffect;
        [SerializeField] private GameObject[] destroyOnHit; 
    
        private float damage = 0f;
        private Health target;
        private GameObject instigator;

        private void Start()
        {
            transform.LookAt(GetAimLocation());
        }

        void Update()
        {
            if (target == null) return;
            if (isHoming && !target.IsDead())
            {
                transform.LookAt(GetAimLocation());
            }

            transform.Translate(Vector3.forward*speed*Time.deltaTime);
        }

        public void SetTarget( Health target, float damage, GameObject instigator)
        {
            this.target = target;
            this.damage = damage;
            this.instigator = instigator;
        
            Destroy(gameObject, maxLifeTime);
        }
        private Vector3 GetAimLocation()
        {
            CapsuleCollider targetCapsule = target.GetComponent<CapsuleCollider>();
            if (targetCapsule == null)
            {
                return target.transform.position;
            }
            return target.transform.position + Vector3.up * targetCapsule.height/2;
        }

        private void OnTriggerEnter(Collider other)
        {
        
            if (other.GetComponent<Health>() != target) return;
            if (target.IsDead()) return;
            target.TakeDamage(instigator, damage);

            speed = 0;
            if (hitEffect != null)
                Instantiate(hitEffect, GetAimLocation(),transform.rotation);

            foreach (GameObject toDestroy in destroyOnHit)
            {
                Destroy(toDestroy);
            }
            Destroy(gameObject, lifeAfterImpact);
        }
    }
}
