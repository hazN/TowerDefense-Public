using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using TD.Control;
using UnityEngine;
namespace TD.Player
{
    public class CannonCluster : MonoBehaviour
    {
        private float timeBeforeExplosion = 2f;
        private float explosionRadius;
        private float damage;
        [SerializeField] private GameObject explosionEffectPrefab;

        public void Fire(float time, float radius, float dmg)
        {
            timeBeforeExplosion = time;
            explosionRadius = radius;
            damage = dmg;
            StartCoroutine(Explode());
        }
        private IEnumerator Explode()
        {
            yield return new WaitForSeconds(timeBeforeExplosion);
            // Play the explosion effect
            Destroy(Instantiate(explosionEffectPrefab, this.gameObject.transform.position, Quaternion.identity), 2f);

            Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
            foreach (Collider collider in colliders)
            {
                if (collider.gameObject.CompareTag("Enemy"))
                {
                    collider.gameObject.GetComponent<EnemyController>().TakeDamage(damage);
                }
            }
            Destroy(gameObject);
        }
    }
}