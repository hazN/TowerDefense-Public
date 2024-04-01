using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace TD.Player
{
    public class CannonTower : Tower
    {
        internal override void FireTower(Vector3 direction)
        {
            // Fire the turret
            GameObject projectile = Instantiate(projectilePrefab.gameObject, projectileSpawnPoint.transform.position, Quaternion.identity);

            // Rotate the projectile transform up to face the target
            projectile.transform.up = direction;
            var projectileStats = projectile.GetComponent<Projectile>();

            // Check if the cannon has the special upgrade (cluster bomb)
            if (upgrades.SpecialUpgrade)
            {
                CannonProjectile cannonProjectile = projectile.GetComponent<CannonProjectile>();
                cannonProjectile.isClusterBomb = true;
            }

            projectile.GetComponent<Projectile>().SetDamage(projectileStats.GetDamage() * upgrades.DamageModifier);
            projectile.GetComponent<Rigidbody>().AddForce(direction.normalized * projectileStats.GetSpeed(), ForceMode.Impulse);

            // Play the fire sound
            fireSound.Play();
        }
    }
}