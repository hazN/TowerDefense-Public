using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TD.UI
{
    public class ShopTower : MonoBehaviour
    {
        [SerializeField] private GameObject towerPrefab;
        [SerializeField] private int towerCost;
        private GameUIManager gameUIManager;
        private Button button;
        private void Awake()
        {
            gameUIManager = FindObjectOfType<GameUIManager>();
            button = GetComponent<Button>();
        }
        private void Update()
        {
            if (GameUIManager.GetGold() < towerCost)
            {
                button.interactable = false;
            }
            else
            {
                button.interactable = true;
            }
        }
        public void OnClick()
        {
            gameUIManager.SetSelectedTower(this);
        }
        public GameObject GetTowerPrefab()
        {
            return towerPrefab;
        }
        public int GetTowerCost()
        {
            return towerCost;
        }
    }
}