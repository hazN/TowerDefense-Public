using System.Collections;
using System.Collections.Generic;
using TD.Sounds;
using UnityEngine;
using UnityEngine.AI;

namespace TD.Player
{
    public class Tower : MonoBehaviour
    {
        [SerializeField] private GameObject body;
        [SerializeField] private GameObject turret;
        [SerializeField] private float turnSpeed = 15f;
        [SerializeField] private float angleTurningAccuracy = 80f;
        protected List<GameObject> enemiesInRange = new List<GameObject>();
        protected GameObject target;
        [SerializeField] protected Projectile projectilePrefab;
        [SerializeField] protected float fireRate = 1f;
        [SerializeField] protected GameObject projectileSpawnPoint;
        private bool isReloading = false;
        protected Upgrades upgrades;
        [SerializeField] private SphereCollider rangeCollider;
        [SerializeField] protected SoundEffectSO fireSound;
        [SerializeField] protected bool isWaterTower = false;
        private void Awake()
        {
            upgrades = GetComponent<Upgrades>();
        }
        private void Update()
        {
            rangeCollider.radius = upgrades.RangeModifier;
            // If we have no target, return
            if (target == null) return;

            // Rotate the body horizontally to face the target
            Vector3 direction = target.transform.position - body.transform.position;
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            Vector3 rotation = Quaternion.Lerp(body.transform.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles;
            body.transform.rotation = Quaternion.Euler(0f, rotation.y, 0f);

            // Rotate the turret vertically to face the target
            float distance = Vector3.Distance(turret.transform.position, target.transform.position);
            Quaternion turretLookRotation = Quaternion.LookRotation(-direction);
            Vector3 turretRotation = Quaternion.Lerp(turret.transform.rotation, turretLookRotation, Time.deltaTime * turnSpeed).eulerAngles;
            turret.transform.rotation = Quaternion.Euler(turretRotation.x, turretRotation.y, turretRotation.z);

            if (enemiesInRange.Count > 0)
            {
                Fire();
            }
          
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Enemy"))
            {
                enemiesInRange.Add(other.gameObject);
                UpdateTarget();
            }
        }
        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Enemy"))
            {
                enemiesInRange.Remove(other.gameObject);
                target = null;
                UpdateTarget();
            }
        }
        private void UpdateTarget()
        {
            // If we already have a target, return
            if (target != null) return;

            // Otherwise, find the cloest enemy in range
            GameObject closestEnemy = null;
            float closestDistance = Mathf.Infinity;

            foreach (var enemy in enemiesInRange)
            {
                if (enemy == null) continue;

                float distance = Vector3.Distance(transform.position, enemy.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestEnemy = enemy;
                }
            }

            // If we found an enemy, set it as the target
            if (closestEnemy != null)
                target = closestEnemy;
            else target = null;
        }
        private void Fire()
        {
            // Check cooldown
            if (isReloading) return;

            // Direction from target to projectile spawn influenced by agent speed
            Vector3 direction = target.transform.position - projectileSpawnPoint.transform.position + Vector3.up * 2f;
            // Use agent speed to predict the target's position
            float distance = Vector3.Distance(target.transform.position, projectileSpawnPoint.transform.position);
            float time = distance / projectilePrefab.GetSpeed();
            direction += target.GetComponent<NavMeshAgent>().velocity * time;

            // Call the fire tower method that can be overriden by child classes
            FireTower(direction);

            // Start the reload coroutine
            StartCoroutine(Reload());
        }
        internal virtual void FireTower(Vector3 direction)
        {
            // Fire the turret
            GameObject projectile = Instantiate(projectilePrefab.gameObject, projectileSpawnPoint.transform.position, Quaternion.identity);

            // Rotate the projectile transform up to face the target
            projectile.transform.up = direction;
            var projectileStats = projectile.GetComponent<Projectile>();
            projectile.GetComponent<Projectile>().SetDamage(projectileStats.GetDamage() * upgrades.DamageModifier);
            projectile.GetComponent<Rigidbody>().AddForce(direction.normalized * projectileStats.GetSpeed(), ForceMode.Impulse);

            // Play the fire sound
            fireSound.Play();
        }
        private IEnumerator Reload()
        {
            isReloading = true;
            float reloadTime = fireRate / upgrades.SpeedModifier;
            yield return new WaitForSeconds(reloadTime);
            isReloading = false;
        }

        public void RemoveEnemy(GameObject enemy)
        {
            if (enemiesInRange.Contains(enemy))
            {
                enemiesInRange.Remove(enemy);
            }
            target = null;
            UpdateTarget();
        }
        public bool IsWaterTower()
        {
            return isWaterTower;
        }
    }
}