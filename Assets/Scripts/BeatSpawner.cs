using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatSpawner : MonoBehaviour
{
    // Start is called before the first frame update

    [Header("Obstacle-Varianten")]
    public GameObject[] obstaclePrefabs;      // Liste aller möglichen Obstacle-Varianten

    [Header("Spawnpunkte")]
    public Transform[] spawnPositions;        // Lane-Positionen (Transform-Objekte)

    [Header("Beat-Steuerung")]
    public float bpm = 120f;                  // Beats per Minute
    public float beatOffset = 12f;             // Optionaler Takt-Offset in Sekunden

    [Header("Audio")]
    public AudioSource music;

    private float beatInterval;
    private float timer;

    void Start()
    {
        beatInterval = 60f / bpm;
        timer = beatInterval + beatOffset;

        if (music != null)
            music.Play();
    }

    void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            SpawnObstacle();
            timer = beatInterval;
        }
    }

    void SpawnObstacle()
    {
        if (obstaclePrefabs.Length == 0 || spawnPositions.Length == 0)
            return;

        // Zufälliges Prefab wählen
        GameObject selectedPrefab = obstaclePrefabs[Random.Range(0, obstaclePrefabs.Length)];

        // Zufällige Lane wählen
        Transform spawnPoint = spawnPositions[Random.Range(0, spawnPositions.Length)];

        // Instanziieren
        Instantiate(selectedPrefab, spawnPoint.position, Quaternion.identity);
    }
}
