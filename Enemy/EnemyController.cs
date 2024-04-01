using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using TD.Core;
using TD.Player;
using TD.Sounds;
using TD.UI;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

namespace TD.Control
{
    public class EnemyController : MonoBehaviour
    {
        private LevelManager levelManager;
        private List<Transform> waypoints = new List<Transform>();
        private int waypointIndex = 0;
        private NavMeshAgent agent;
        private bool waypointSet = false;
        private Animator animator;

        [BoxGroup("Agent Settings")]
        [SerializeField] private float agentStoppingDistance = 0.3f;
        [BoxGroup("Agent Settings")]
        [SerializeField] private float agentSpeed = 3.5f;
        [BoxGroup("Agent Settings")]
        [SerializeField] private float maxHealth = 100;
        [BoxGroup("Agent Settings")]
        [SerializeField] private Vector2 goldDrop = new Vector2(1, 5);
        [BoxGroup("Agent Settings")]
        [SerializeField] private Vector2 damage = new Vector2(1, 5);
        private float currentHealth;
        [BoxGroup("Configuration")]
        [SerializeField] private Slider healthBarPrefab;
        private Slider healthBar;
        [BoxGroup("Configuration")]
        [SerializeField] private LayerMask towerLayer;
        [BoxGroup("Configuration")]
        [SerializeField] private SoundEffectSO deathSound;

        private void Awake()
        {
            agent = GetComponent<NavMeshAgent>();
            animator = GetComponentInChildren<Animator>();
            levelManager = FindObjectOfType<LevelManager>();
        }
        private void Start()
        {
            currentHealth = maxHealth;
            healthBar = Instantiate(healthBarPrefab, transform.position, Quaternion.identity, FindObjectOfType<Canvas>().transform);
            healthBar.transform.SetParent(FindObjectOfType<Canvas>().transform);
            healthBar.maxValue = maxHealth;
            healthBar.value = maxHealth;
        }
        private void Update()
        {
            // Set the animator velocity for the blend tree
            animator.SetFloat("Velocity", agent.velocity.magnitude);
            // If waypoints are not set, return
            if (!waypointSet) return;

            // Move the health bar
            if (healthBar != null)
                healthBar.transform.position = Camera.main.WorldToScreenPoint(transform.position + Vector3.up * 1.2f);

            // If the agent has reached the destination, move to the next waypoint
            if (!agent.pathPending && agent.remainingDistance <= agentStoppingDistance)
            {
                // If the agent has reached the last waypoint, destroy the game object
                if (waypointIndex == waypoints.Count - 1)
                {
                    GameUIManager.RemoveHealth(UnityEngine.Random.Range((int)damage.x, (int)damage.y));
                    levelManager.EnemyDestroyed();
                    Destroy(healthBar.gameObject);
                    Destroy(gameObject);
                }
                // Otherwise, move to the next waypoint
                else
                {
                    waypointIndex++;
                    agent.SetDestination(waypoints[waypointIndex].position);
                }
            }
        }
        public void SetWaypoints(List<Transform> waypoints)
        {
            // Set the waypoints and agent stats
            this.waypoints = waypoints;
            agent.SetDestination(waypoints[waypointIndex].position);
            agent.speed = agentSpeed;
            agent.stoppingDistance = agentStoppingDistance;
            waypointSet = true;
        }

        public void TakeDamage(float damage)
        {
            // Decrement the health and update the health bar
            currentHealth -= damage;
            healthBar.value = currentHealth;
            // If the health is less than or equal to 0, destroy the game object
            if (currentHealth <= 0)
            {
                levelManager.EnemyDestroyed();
                Collider[] hitColliders = Physics.OverlapSphere(transform.position, 15, towerLayer);
                foreach (Collider hitCollider in hitColliders)
                {
                    if (hitCollider.gameObject.CompareTag("Tower"))
                    {
                        Tower tower = hitCollider.gameObject.GetComponent<Tower>();
                        if (tower != null)
                        {
                            tower.RemoveEnemy(gameObject);
                        }
                    }
                }
                deathSound.Play();
                GameUIManager.AddGold(UnityEngine.Random.Range((int)goldDrop.x, (int)goldDrop.y));
                Destroy(healthBar.gameObject);
                Destroy(gameObject);
            }
        }
    }
}