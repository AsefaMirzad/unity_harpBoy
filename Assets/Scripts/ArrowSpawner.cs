using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowSpawner : MonoBehaviour
{
    [Header("Arrow Settings")]
    public GameObject arrowPrefab;
    public Transform[] lanes;
    public float spawnInterval = 1.5f;

    [Header("Obstacle Settings")]
    public GameObject obstaclePrefab;  // Assign your obstacle prefab here
    public float obstacleChance = 0.2f; // 20% chance to spawn obstacle instead of arrow
    public float obstacleSpeed = 5f;    // Speed of obstacles when spawned

    [Header("Difficulty Settings")]
    public bool increaseDifficulty = true;
    public float minSpawnInterval = 0.8f;
    public float difficultyRamp = 0.05f; // How quickly the game gets harder

    private float timer = 0f;
    private float gameTime = 0f;
    private int lastLaneIndex = -1;

    void Start()
    {
        // Check if prefabs are assigned
        if (arrowPrefab == null)
            Debug.LogError("Arrow prefab not assigned in ArrowSpawner!");

        if (obstaclePrefab == null)
            Debug.LogError("Obstacle prefab not assigned in ArrowSpawner!");
    }

    void Update()
    {
        // Track game time for difficulty scaling
        gameTime += Time.deltaTime;

        // Adjust difficulty over time
        float currentInterval = spawnInterval;
        if (increaseDifficulty)
        {
            currentInterval = Mathf.Max(minSpawnInterval, spawnInterval - (gameTime * difficultyRamp / 60f));
        }

        // Timer for spawning
        timer += Time.deltaTime;
        if (timer >= currentInterval)
        {
            // Decide whether to spawn arrow or obstacle
            float random = Random.value; // Random value between 0 and 1

            if (random < obstacleChance)
                SpawnObstacle();
            else
                SpawnArrow();

            timer = 0f;
        }
    }

    void SpawnArrow()
    {
        // Select a random lane
        int randomLane = Random.Range(0, lanes.Length);
        Vector3 spawnPosition = new Vector3(
            lanes[randomLane].position.x,
            transform.position.y,
            0
        );

        // Instantiate arrow
        GameObject arrow = Instantiate(arrowPrefab, spawnPosition, Quaternion.identity);

        // Set arrow properties
        KeyCode randomKey = RandomKey();
        Arrow arrowComponent = arrow.GetComponent<Arrow>();

        if (arrowComponent != null)
            arrowComponent.assignedKey = randomKey;
        else
            Debug.LogError("Arrow prefab does not have Arrow component!");

        // Set arrow rotation based on key
        float rotationZ = 0f;
        switch (randomKey)
        {
            case KeyCode.UpArrow:
                rotationZ = 90f;  // Pointing up
                break;
            case KeyCode.DownArrow:
                rotationZ = -90f; // Pointing down
                break;
            case KeyCode.LeftArrow:
                rotationZ = 180f; // Pointing left
                break;
            case KeyCode.RightArrow:
                rotationZ = 0f;   // Pointing right (default)
                break;
        }
        arrow.transform.rotation = Quaternion.Euler(0, 0, rotationZ);
    }

    void SpawnObstacle()
    {
        // Avoid spawning in the same lane as the previous obstacle if possible
        int randomLane;
        if (lanes.Length > 1)
        {
            do
            {
                randomLane = Random.Range(0, lanes.Length);
            } while (randomLane == lastLaneIndex);

            lastLaneIndex = randomLane;
        }
        else
        {
            randomLane = 0;
        }

        // Set spawn position
        Vector3 spawnPosition = new Vector3(
            lanes[randomLane].position.x,
            transform.position.y,
            0
        );

        // Instantiate obstacle
        GameObject obstacle = Instantiate(obstaclePrefab, spawnPosition, Quaternion.identity);

        // Set obstacle speed if it has the Obstacle component
        Obstacle obstacleComponent = obstacle.GetComponent<Obstacle>();
        if (obstacleComponent != null)
        {
            obstacleComponent.speed = obstacleSpeed;
        }
        else
        {
            Debug.LogWarning("Obstacle prefab doesn't have Obstacle component!");
        }
    }

    KeyCode RandomKey()
    {
        KeyCode[] keys = { KeyCode.LeftArrow, KeyCode.UpArrow, KeyCode.DownArrow, KeyCode.RightArrow };
        return keys[Random.Range(0, keys.Length)];
    }

    // Public method to control spawning (can be called from GameManager)
    public void SetDifficulty(float obstacleRate, float spawnRate)
    {
        obstacleChance = Mathf.Clamp01(obstacleRate); // Clamp between 0-1
        spawnInterval = Mathf.Max(0.5f, spawnRate);   // Don't allow too fast spawning
    }

    // Pause/resume spawning
    public void PauseSpawning()
    {
        enabled = false;
    }

    public void ResumeSpawning()
    {
        enabled = true;
    }
}