using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatternSpawner : MonoBehaviour
{
    [System.Serializable]
    public class ObstaclePattern
    {
        public float beatTime;              // Zeitpunkt in Sekunden
        public int laneIndex;               // Lane 0-4
        public GameObject obstaclePrefab;   // Prefab-Referenz
    }

    public List<ObstaclePattern> patterns = new List<ObstaclePattern>();
    public Transform[] lanes;
    public AudioSource music;

    private float songTime;
    private int nextPatternIndex = 0;

    void Start()
    {
        if (music != null)
            music.Play();
    }

    void Update()
    {
        songTime += Time.deltaTime;

        // Prüfe, ob nächste Platzierung fällig ist
        while (nextPatternIndex < patterns.Count && songTime >= patterns[nextPatternIndex].beatTime)
        {
            SpawnPattern(patterns[nextPatternIndex]);
            nextPatternIndex++;
        }
    }

    void SpawnPattern(ObstaclePattern pattern)
    {
        if (pattern.laneIndex < 0 || pattern.laneIndex >= lanes.Length)
            return;

        Transform lane = lanes[pattern.laneIndex];
        Instantiate(pattern.obstaclePrefab, lane.position, Quaternion.identity);
    }
}
