using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TD.Player;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace TD.UI
{
    public class GameUIManager : MonoBehaviour
    {
        static private GameObject selectedTower;
        private ShopTower shopTower;
        [SerializeField] private InputActionReference clickAction, holdAction, pointerPosition;
        [SerializeField] private Transform startingPosition;
        [SerializeField] private TextMeshProUGUI goldText;
        [SerializeField] private TextMeshProUGUI healthText;
        [SerializeField] private UpgradeUI upgradeUI;
        [SerializeField] private int startingGold = 100;
        static private int gold = 99999;
        static private int health = 100;
        private bool isPaused = false;
        private float timeScale = 1;
        public static GameUIManager instance;
        private void Awake()
        {
            instance = this;
            gold = startingGold;
        }
        private void Update()
        {
            if (isPaused)
            {
                Time.timeScale = 0;
            }
            else
            {
                Time.timeScale = timeScale;
            }
            goldText.text = gold.ToString();
            if (clickAction.action.triggered)
            {
                // Check if mouse is over ui 
                if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
                {
                    return;
                }
                // Get vector2 position of the mouse
                Ray ray = Camera.main.ScreenPointToRay(pointerPosition.action.ReadValue<Vector2>());
                RaycastHit[] hits;
                hits = Physics.RaycastAll(ray.origin, ray.direction, 100f);
                foreach (RaycastHit hit in hits)
                {
                    if (hit.transform.tag == "Tower" && !hit.collider.isTrigger)
                    {
                        Debug.Log("Tower selected");
                        upgradeUI.OpenUpgradePanel(hit.transform.gameObject.GetComponent<Upgrades>(),pointerPosition.action.ReadValue<Vector2>());
                    }
                }
            }
            if (holdAction.action.IsInProgress())
            {
                Debug.Log("Hold");
                if (selectedTower != null)
                {
                    Ray ray = Camera.main.ScreenPointToRay(pointerPosition.action.ReadValue<Vector2>());
                    RaycastHit[] hits;
                    hits = Physics.SphereCastAll(ray.origin, 1.5f, ray.direction);
                    if (!selectedTower.GetComponent<Tower>().IsWaterTower())
                    {// Make sure the tower is not placed on top of the path or another tower
                        foreach (RaycastHit hit in hits)
                        {
                            // If the tower is over water return
                            if (hit.collider.tag == "Water")
                            {
                                return;
                            }
                            // If the tower is over the path return
                            if (hit.collider.gameObject.layer == 6)
                            {
                                return;
                            }
                            // If the tower is on top of another tower return
                            if (hit.collider.tag == "Tower" && hit.collider.gameObject != selectedTower && !hit.collider.isTrigger)
                            {
                                return;
                            }
                        }
                        foreach (RaycastHit hit in hits)
                        {
                            if (hit.collider.tag == "TowerPlot")
                            {
                                selectedTower.transform.position = hit.point;
                            }
                        }
                    }
                    else
                    {
                        foreach (RaycastHit hit in hits)
                        {
                            // If the tower is on top of another tower return
                            if (hit.collider.tag == "Tower" && hit.collider.gameObject != selectedTower && !hit.collider.isTrigger)
                            {
                                return;
                            }
                        }
                        foreach (RaycastHit hit in hits)
                        {
                            if (hit.collider.tag == "Water")
                            {
                                selectedTower.transform.position = hit.point;
                            }
                        }
                    }
                }
            }
            else
            {
                if (selectedTower != null)
                {
                    if (selectedTower.transform.position == startingPosition.position)
                    {
                        Destroy(selectedTower);
                        shopTower = null;
                    }
                    else
                    {
                        GameUIManager.AddGold(-shopTower.GetTowerCost());
                        SetCollidersEnabled(true);
                        selectedTower.GetComponent<Tower>().enabled = true;
                        selectedTower = null;
                        shopTower = null;
                    }
                }
            }
        }
        private void SetCollidersEnabled(bool enabled)
        {
            Collider[] childColliders = selectedTower.GetComponentsInChildren<Collider>(true);
            Collider[] parentColliders = selectedTower.GetComponents<Collider>();

            foreach (Collider collider in childColliders)
            {
                collider.enabled = enabled;
            }
            foreach (Collider collider in parentColliders)
            {
                collider.enabled = enabled;
            }
        }
        public void SetSelectedTower(ShopTower tower)
        {
            if (tower.GetTowerCost() > gold)
            {
                return;
            }

            shopTower = tower;
            selectedTower = Instantiate(tower.GetTowerPrefab(), startingPosition.position, Quaternion.identity);
            selectedTower.GetComponent<Tower>().enabled = false;
            Debug.Log("Tower selected");
        }
        public static int GetGold()
        {
            return gold;
        }
        public static void AddGold(int amount)
        {
            gold += amount;
        }
        public static void RemoveHealth(int amount)
        {
            health -= amount;
            instance.healthText.text = health.ToString();

            if (health <= 0)
            {
                health = 100;
                Debug.Log("Game Over");
                MainMenu.instance.OpenMenu();
                SceneManager.LoadScene(0);
            }
        }
        public static GameObject GetSelectedTower()
        {
            return selectedTower;
        }
        public void PauseGame(bool pause)
        {
            isPaused = pause;
        }
        public void SetTimeScale(float scale)
        {
            timeScale = scale;
        }
    }
}