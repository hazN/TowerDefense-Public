using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace TD.Control
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private List<Transform> waypoints = new List<Transform>();
        [SerializeField] private List<Wave> waves = new List<Wave>();
        private int curEnemies = 0;
        private int curWave = -1;
        private void Spawn()
        {
            if (curWave >= waves.Count)
            {
                return;
            }
            // Spawn an enemy and set its waypoints
            GameObject enemy = Instantiate(waves[curWave].enemyPrefab, transform.position, Quaternion.identity);
            enemy.GetComponent<EnemyController>().SetWaypoints(waypoints);

            // Increment the current enemies count

            curEnemies++;
            // If the current enemies count is equal to the max enemies, cancel the invoke
            if (curEnemies >= waves[curWave].maxEnemies)
            {
                CancelInvoke("Spawn");
                if (waves[curWave].repeating > 1)
                {
                    waves[curWave].repeating--;
                    StartCoroutine(WaitForNextWave());
                }
            }
        }

        public void StartNextWave()
        {
            if (curWave < waves.Count)
            {
                curWave++;
                curEnemies = 0;
                InvokeRepeating("Spawn", waves[curWave].timeAfterWave, waves[curWave].spawnRate);
            }
        }
        public void StopSpawning()
        {
            CancelInvoke();
        }
        public int GetMaxEnemies()
        {
            return waves[curWave].maxEnemies * waves[curWave].repeating;
        }
        public int GetMaxWaves()
        {
            return waves.Count;
        }
        public IEnumerator WaitForNextWave()
        {
            yield return new WaitForSeconds(waves[curWave].timeAfterWave);
            curEnemies = 0;
            InvokeRepeating("Spawn", 0, waves[curWave].spawnRate);

        }
        [Serializable]
        public class Wave
        {
            [Tooltip("The enemy prefab to spawn")]
            public GameObject enemyPrefab;
            [Tooltip("The time between each enemy spawning")]
            public float spawnRate = 0.75f;
            [Tooltip("The amount of enemies to spawn (per sub-wave)")]
            public int maxEnemies = 10;
            [Tooltip("How many sub-waves to spawn")]
            public int repeating = 1;
            [Tooltip("The time to wait after this wave")]
            public float timeAfterWave = 2f;
        }
    }
}