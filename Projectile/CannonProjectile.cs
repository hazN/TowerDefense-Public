using System.Collections;
using System.Collections.Generic;
using TD.Control;
using Unity.VisualScripting;
using UnityEngine;
namespace TD.Player
{
    public class CannonProjectile : Projectile
    {
        public bool isClusterBomb;
        public float explosionRadius;
        [SerializeField] private GameObject clusterBombPrefab;
        [SerializeField] private GameObject explosionEffectPrefab;
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Enemy") || other.gameObject.layer == 6)
            {
                // Play the explosion effect
                Destroy(Instantiate(explosionEffectPrefab, this.gameObject.transform.position, Quaternion.identity), 2f);

                // Sphere cast to check for enemies in the explosion radius
                Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
                foreach (Collider collider in colliders)
                {
                    if (collider.gameObject.CompareTag("Enemy"))
                    {
                        collider.gameObject.GetComponent<EnemyController>().TakeDamage(damage);
                    }
                }
                // If the projectile has the cluster bomb upgrade, create 3 more projectiles
                if (isClusterBomb)
                {
                    isClusterBomb = false;
                    for (int i = 0; i < 3; i++)
                    {
                        GameObject clusterBomb = Instantiate(clusterBombPrefab, transform.position, Quaternion.identity);
                        clusterBomb.GetComponent<CannonCluster>().Fire(0.5f, 2f, damage * 0.25f);
                        // Send the cluster bomb in a random direction towards up and to the sides and to quickly fall down
                        clusterBomb.GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(-1f, 1f), 1, Random.Range(-1f, 1f)) * 5, ForceMode.Impulse);
                    }
                }

                // Destroy the projectile on collision with anything other than a tower
                if (!other.gameObject.CompareTag("Tower"))
                    Destroy(gameObject);
            }
        }
    }
}