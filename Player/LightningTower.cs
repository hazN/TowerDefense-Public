using System.Collections;
using System.Collections.Generic;
using TD.Control;
using UnityEngine;
namespace TD.Player
{
    public class LightningTower : Tower
    {
        [SerializeField] private ParticleSystem lightningEffect;
        [SerializeField] private LayerMask enemyLayer;
        [SerializeField] private float damage = 5;
        internal override void FireTower(Vector3 direction)
        {
            // No projectile, just play the lightning effect
            lightningEffect.Play();

            // Check for the AOE upgrade
            if (upgrades.SpecialUpgrade)
            {
                UnityEngine.ParticleSystem.ShapeModule editableShape = lightningEffect.shape;
                editableShape.shapeType = ParticleSystemShapeType.Donut;
                foreach (var enemy in enemiesInRange)
                {
                    if (enemy != null)
                    {
                        enemy.GetComponent<EnemyController>().TakeDamage(upgrades.DamageModifier * damage);
                    }
                }
            }
            else
            {
                target.GetComponent<EnemyController>().TakeDamage(upgrades.DamageModifier * damage);
            }
            // Play the fire sound
            fireSound.Play();
        }
    }
}