using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace TD.Player
{
    public class BalisstaTower : Tower
    {
        internal override void FireTower(Vector3 direction)
        {
            // Check if the balissta has the special upgrade (triple shot)
            if (upgrades.SpecialUpgrade)
            {   
                // Fire the turret
                GameObject projectile = Instantiate(projectilePrefab.gameObject, projectileSpawnPoint.transform.position, Quaternion.identity);
                GameObject projectile2 = Instantiate(projectilePrefab.gameObject, projectileSpawnPoint.transform.position, Quaternion.identity);
                GameObject projectile3 = Instantiate(projectilePrefab.gameObject, projectileSpawnPoint.transform.position, Quaternion.identity);

                // Rotate the projectile transform up to face the target, offset the direction of the projectiles to create a
                Vector3 direction2 = direction + Vector3.forward * 1.5f;
                Vector3 direction3 = direction - Vector3.forward * 1.5f;
                projectile.transform.up = direction;
                projectile2.transform.up = direction2;
                projectile3.transform.up = direction3;

                var projectileStats = projectile.GetComponent<Projectile>();
                projectile.GetComponent<Projectile>().SetDamage(projectileStats.GetDamage() * upgrades.DamageModifier);
                projectile2.GetComponent<Projectile>().SetDamage(projectileStats.GetDamage() * upgrades.DamageModifier);
                projectile3.GetComponent<Projectile>().SetDamage(projectileStats.GetDamage() * upgrades.DamageModifier);
                projectile.GetComponent<Rigidbody>().AddForce(direction.normalized * projectileStats.GetSpeed(), ForceMode.Impulse);
                projectile2.GetComponent<Rigidbody>().AddForce(direction2.normalized * projectileStats.GetSpeed(), ForceMode.Impulse);
                projectile3.GetComponent<Rigidbody>().AddForce(direction3.normalized * projectileStats.GetSpeed(), ForceMode.Impulse);
                // Play the fire sound
                fireSound.Play();
            }
            else base.FireTower(direction);
        }
    }
}
