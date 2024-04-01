using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using TD.UI;
using UnityEngine;
namespace TD.Player
{
    public class Upgrades : MonoBehaviour
    {
        [BoxGroup("Speed")]
        [SerializeField][Tooltip("How much to increase speed by percentage")] private float speedPerUpgrade = 1.25f;
        [BoxGroup("Speed")]
        [SerializeField] private int speedCost = 10;
        [BoxGroup("Speed")]
        [SerializeField] private int maxSpeedUpgrades = 4;
        [BoxGroup("Damage")]
        [SerializeField][Tooltip("How much to increase damage by percentage")] private float damagePerUpgrade = 1.25f;
        [BoxGroup("Damage")]
        [SerializeField] private int damageCost = 10;
        [BoxGroup("Damage")]
        [SerializeField] private int maxDamageUpgrades = 4;
        [BoxGroup("Range")]
        [SerializeField][Tooltip("How much to increase range by percentage")] private float rangePerUpgrade = 1.25f;
        [BoxGroup("Range")]
        [SerializeField] private int rangeCost = 50;
        [BoxGroup("Range")]
        [SerializeField] private int maxRangeUpgrades = 4;
        [BoxGroup("Special")]
        [SerializeField] private int specialCost = 100;
        [BoxGroup("Special")]
        [SerializeField] private string specialID = "TRI-SHOT";
        [SerializeField] private float speedModifier = 1f;
        [SerializeField] private float damageModifier = 1f;
        [SerializeField] private float rangeModifier = 1f;
        [SerializeField] private bool specialUpgrade = false;

        public float SpeedModifier { get => speedModifier; set => speedModifier = value; }
        public float DamageModifier { get => damageModifier; set => damageModifier = value; }
        public float RangeModifier { get => rangeModifier; set => rangeModifier = value; }
        public bool SpecialUpgrade { get => specialUpgrade; set => specialUpgrade = value; }
        public string SpecialID { get => specialID; set => specialID = value; }

        public void UpgradeSpeed()
        {
            speedModifier *= speedPerUpgrade;
            GameUIManager.AddGold(-GetSpeedCost());
            speedCost *= 2;
            maxSpeedUpgrades--;
        }
        public void UpgradeDamage()
        {
            damageModifier *= damagePerUpgrade;
            GameUIManager.AddGold(-GetDamageCost());
            damageCost *= 2;
            maxDamageUpgrades--;
        }
        public void UpgradeRange()
        {
            rangeModifier *= rangePerUpgrade;
            GameUIManager.AddGold(-GetRangeCost());
            rangeCost *= 2;
            maxRangeUpgrades--;
        }
        public void UpgradeSpecial()
        {
            specialUpgrade = true;
            GameUIManager.AddGold(-GetSpecialCost());
            specialUpgrade = true;
        }

        public int GetSpeedCost()
        {
            return speedCost;
        }
        public int GetDamageCost()
        {
            return damageCost;
        }
        public int GetRangeCost()
        {
            return rangeCost;
        }
        public int GetSpecialCost()
        {
            return specialCost;
        }
        public int GetMaxSpeedUpgrades()
        {
            return maxSpeedUpgrades;
        }
        public int GetMaxDamageUpgrades()
        {
            return maxDamageUpgrades;
        }
        public int GetMaxRangeUpgrades()
        {
            return maxRangeUpgrades;
        }
    }
}