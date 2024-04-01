using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using TD.Player;
using TMPro;
using UnityEngine;
namespace TD.UI
{
    public class UpgradeUI : MonoBehaviour
    {
        [BoxGroup("Config")]
        [SerializeField] private GameObject upgradePanel;
        [BoxGroup("Config")]
        [SerializeField] private GameObject layoutGroup;
        [BoxGroup("Config")]
        [SerializeField] private TextMeshProUGUI speedCostText, damageCostText, rangeCostText, specialCostText;
        private GameObject currentSpecialUpgrade = null;

        private Upgrades towerUpgrades = null;

        private void Start()
        {
            upgradePanel.SetActive(false);
            GameObject selectedTower = GameUIManager.GetSelectedTower();
        }

        private void Update()
        {
            if (towerUpgrades == null) return;

            speedCostText.text = towerUpgrades.GetSpeedCost().ToString();
            damageCostText.text = towerUpgrades.GetDamageCost().ToString();
            rangeCostText.text = towerUpgrades.GetRangeCost().ToString();
            specialCostText.text = towerUpgrades.GetSpecialCost().ToString();

            if (towerUpgrades.SpeedModifier >= towerUpgrades.GetMaxSpeedUpgrades())
            {
                speedCostText.text = "Max";
            }
            if (towerUpgrades.DamageModifier >= towerUpgrades.GetMaxDamageUpgrades())
            {
                damageCostText.text = "Max";
            }
            if (towerUpgrades.RangeModifier >= towerUpgrades.GetRangeCost())
            {
                rangeCostText.text = "Max";
            }
            if (towerUpgrades.SpecialUpgrade)
            {
                specialCostText.text = "Max";
            }
        }

        public void OpenUpgradePanel(Upgrades towerUpgrades, Vector2 screenPos)
        {
            // Reset the current special upgrade
            if (currentSpecialUpgrade != null)
            {
                currentSpecialUpgrade.SetActive(false);
                currentSpecialUpgrade = null;
            }

            this.towerUpgrades = towerUpgrades;

            // Adjust panel position to keep it within screen boundaries
            RectTransform panelRectTransform = upgradePanel.GetComponent<RectTransform>();
            Vector2 pivot = new Vector2(0.5f, 0.5f); // Center pivot
            Vector2 panelSize = panelRectTransform.sizeDelta * pivot;

            // Calculate screen boundaries
            Vector2 screenSize = new Vector2(Screen.width, Screen.height);
            Vector2 screenMin = panelSize;
            Vector2 screenMax = screenSize - panelSize;

            // Clamp panel position within screen boundaries
            Vector2 clampedPos = screenPos;
            clampedPos.x = Mathf.Clamp(clampedPos.x, screenMin.x, screenMax.x);
            clampedPos.y = Mathf.Clamp(clampedPos.y, screenMin.y, screenMax.y);

            // Set panel position
            upgradePanel.transform.position = clampedPos;

            // Set the special upgrade 
            Transform[] trs = GetComponentsInChildren<Transform>(true);
            foreach (Transform child in trs)
            {
                if (child.name == towerUpgrades.SpecialID)
                {
                    child.gameObject.SetActive(true);
                    currentSpecialUpgrade = child.gameObject;
                    specialCostText = child.GetComponentInChildren<TextMeshProUGUI>();
                }
            }
            upgradePanel.SetActive(true);
        }

        public void CloseUpgradePanel()
        {
            towerUpgrades = null;
            upgradePanel.SetActive(false);
        }
        public void UpdateSelection()
        {
            towerUpgrades = GameUIManager.GetSelectedTower().GetComponent<Upgrades>();
        }

        public void UpgradeSpeed()
        {
            if (towerUpgrades.GetMaxSpeedUpgrades() > 0 && GameUIManager.GetGold() >= towerUpgrades.GetSpeedCost())
            {
                towerUpgrades.UpgradeSpeed();
            }
        }
        public void UpgradeDamage()
        {
            if (towerUpgrades.GetMaxDamageUpgrades() > 0 && GameUIManager.GetGold() >= towerUpgrades.GetDamageCost())
            {
                towerUpgrades.UpgradeDamage();
            }
        }
        public void UpgradeRange()
        {
            if (towerUpgrades.GetMaxRangeUpgrades() > 0 && GameUIManager.GetGold() >= towerUpgrades.GetRangeCost())
            {
                towerUpgrades.UpgradeRange();
            }
        }
        public void UpgradeSpecial()
        {
            if (!towerUpgrades.SpecialUpgrade && GameUIManager.GetGold() >= towerUpgrades.GetSpecialCost())
            {
                towerUpgrades.UpgradeSpecial();
            }
        }
    }
}