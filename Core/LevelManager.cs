using Sirenix.OdinInspector;
using TD.Control;
using TD.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TD.Core
{
    public class LevelManager : MonoBehaviour
    {
        [BoxGroup("References")]
        [SerializeField] private EnemySpawner spawner;
        [BoxGroup("Settings")]
        [SerializeField] private float timeBetweenWaves = 5f;
        private int currentWave = 0;
        private bool isSpawning = false;
        private int enemiesRemaining = 0;
        private float timer = 0f;

        private void Start()
        {
            StartNextWave();
        }
        private void Update()
        {
            // If enemies are spawning, reset the timer
            if (isSpawning)
            {
                timer = 0;
            }
            else
            {
                timer -= Time.deltaTime;
                if (timer <= 0)
                {
                    if (currentWave >= spawner.GetMaxWaves())
                    {
                        StopSpawning();
                    }
                    else
                    {
                        StartNextWave();
                    }
                }
            }
        }
        private void StartNextWave()
        {
            currentWave++;
            if (currentWave >= spawner.GetMaxWaves())
            {
                StopSpawning();
            }
            spawner.StartNextWave();
            enemiesRemaining = spawner.GetMaxEnemies();
            isSpawning = true;
        }

        private void StopSpawning()
        {
            spawner.StopSpawning();
            isSpawning = false;
            Debug.Log("Level Complete");
            MainMenu.instance.CompleteLevel(SceneManager.GetActiveScene().buildIndex);
            MainMenu.instance.OpenMenu();
            SceneManager.LoadScene(0);
        }
        public void EnemyDestroyed()
        {
            enemiesRemaining--;
            if (enemiesRemaining <= 0)
            {
                isSpawning = false;
                timer = timeBetweenWaves;
            }
        }
    }
}