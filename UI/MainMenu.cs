using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TD.UI
{
    public class MainMenu : MonoBehaviour
    {
        [Serializable]
        public class Level
        {
            public int levelIndex;
            public bool isCompleted = false;
            public Button button;
        }

        [SerializeField] private List<Level> levels = new List<Level>();
        [SerializeField] private Transform MenuUI;
        public static MainMenu instance;
        private void Awake()
        {
            if (instance != null)
            {
                Destroy(gameObject);
                return;
            }
            DontDestroyOnLoad(this);
            instance = this;
        }
        private void Start()
        {
            for (int i = 0; i < levels.Count; i++)
            {
                int index = levels[i].levelIndex;
                levels[i].button.onClick.AddListener(() => LoadLevel(index));
            }
            ReloadUI();
        }
        private void ReloadUI()
        {
            for (int i = 0; i < levels.Count; i++)
            {
                levels[i].button.interactable = isLevelUnlocked(i);
                levels[i].isCompleted = isLevelCompleted(i+1);
            }
        }
        public void OpenMenu()
        {
            MenuUI.gameObject.SetActive(true);
            ReloadUI();
        }
        public void LoadLevel(int sceneIndex)
        {
            MenuUI.gameObject.SetActive(false);
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneIndex);
        }
        private bool isLevelUnlocked(int index)
        {
            // Check if there is a previous level
            if (index == 0)
            {
                return true;
            }
            // Otherwise check if the previous level is completed
             return levels[index-1].isCompleted;
        }
        private bool isLevelCompleted(int index)
        {
            bool isCompleted = (PlayerPrefs.GetInt(index + " completed") != 0);
            return isCompleted;
        }
        public void CompleteLevel(int index)
        {
            Debug.Log("Level " + index + " completed");
            PlayerPrefs.SetInt(index + " completed", 1);
            levels[index-1].isCompleted = true;
            PlayerPrefs.Save();
            ReloadUI();
        }
        public void ResetProgress()
        {
            int i = 1;
            foreach (Level level in levels)
            {
                PlayerPrefs.DeleteKey(i++ + " completed");
                level.isCompleted = false;
                level.button.interactable = false;
            }
            PlayerPrefs.Save();
            ReloadUI();
        }
    }
}