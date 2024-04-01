using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using TD.Control;
using UnityEngine;
namespace TD.Player
{
    public abstract class Projectile : MonoBehaviour
    {
        [BoxGroup("Projectile Stats")]
        [SerializeField] protected float damage;
        [BoxGroup("Projectile Stats")]
        [SerializeField] protected float speed;
        [BoxGroup("Trail Settings")]
        [SerializeField] protected float trailLife;
        [BoxGroup("Trail Settings")]
        [SerializeField] protected float trailStartWidth;
        [BoxGroup("Trail Settings")]
        [SerializeField] protected float trailEndWidth;
        protected TrailRenderer trailRenderer;

        public void SetDamage(float damage)
        {
            this.damage = damage;

            trailRenderer = GetComponent<TrailRenderer>();
            if (trailRenderer != null)
            {
                trailRenderer.time = trailLife;
                trailRenderer.startWidth = trailStartWidth;
                trailRenderer.endWidth = trailEndWidth;
            }
        }
        public void SetModifiers(float speedModifier, float damageModifier)
        {
            speed *= speedModifier;
            damage *= damageModifier;
        }
        public float GetSpeed()
        {
            return speed;
        }
        public float GetDamage()
        {
            return damage;
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Enemy"))
            {
                other.gameObject.GetComponent<EnemyController>().TakeDamage(damage);

                // Destroy the projectile on collision with anything other than a tower
                if (!other.gameObject.CompareTag("Tower"))
                    Destroy(gameObject);
            }
        }
    }
}