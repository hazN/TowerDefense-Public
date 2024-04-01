using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace TD.Player
{
    public class WizardTower : Tower
    {
        internal override void FireTower(Vector3 direction)
        {
            // Check if the wizard has the special upgrade (multi-heatsinking shots)
            if (upgrades.SpecialUpgrade)
            {
                // Get three random enemies
                GameObject enemy1 = enemiesInRange[Random.Range(0, enemiesInRange.Count)];
                GameObject enemy2 = enemiesInRange[Random.Range(0, enemiesInRange.Count)];
                GameObject enemy3 = enemiesInRange[Random.Range(0, enemiesInRange.Count)];

                // Fire the turret
                GameObject projectile = Instantiate(projectilePrefab.gameObject, projectileSpawnPoint.transform.position, Quaternion.identity);
                GameObject projectile2 = Instantiate(projectilePrefab.gameObject, projectileSpawnPoint.transform.position, Quaternion.identity);
                GameObject projectile3 = Instantiate(projectilePrefab.gameObject, projectileSpawnPoint.transform.position, Quaternion.identity);

                // Get the rigidbodies of the projectiles and add force to them towards the enemies
                projectile.GetComponent<Rigidbody>().AddForce((enemy1.transform.position - projectile.transform.position).normalized * projectile.GetComponent<Projectile>().GetSpeed(), ForceMode.Impulse);
                projectile2.GetComponent<Rigidbody>().AddForce((enemy2.transform.position - projectile2.transform.position).normalized * projectile2.GetComponent<Projectile>().GetSpeed(), ForceMode.Impulse);
                projectile3.GetComponent<Rigidbody>().AddForce((enemy3.transform.position - projectile3.transform.position).normalized * projectile3.GetComponent<Projectile>().GetSpeed(), ForceMode.Impulse);
                // Play the fire sound
                fireSound.Play();
            }
            else base.FireTower(direction);
        }
    }
}