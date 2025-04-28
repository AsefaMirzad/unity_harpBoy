using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowSpawner : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    public GameObject arrowPrefab;
    public Transform[] lanes;
    public float spawnInterval = 1.5f;
    private float timer = 0f;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            SpawnArrow();
            timer = 0f;
        }
    }

    void SpawnArrow()
    {
        int randomLane = Random.Range(0, lanes.Length);
        Vector3 spawnPosition = new Vector3(
            lanes[randomLane].position.x,
            transform.position.y,
            0
        );

        

        GameObject arrow = Instantiate(arrowPrefab, spawnPosition, Quaternion.identity);
        KeyCode randomKey = RandomKey();
        arrow.GetComponent<Arrow>().assignedKey = randomKey; // Zufällige Pfeiltaste setzen

        float rotationZ = 0f;
        switch (randomKey)
        {
            case KeyCode.UpArrow:
                rotationZ = 90f;  // Pfeil zeigt nach oben
                break;
            case KeyCode.DownArrow:
                rotationZ = -90f; // Pfeil zeigt nach unten
                break;
            case KeyCode.LeftArrow:
                rotationZ = 180f;  // Pfeil zeigt nach links
                break;
            case KeyCode.RightArrow:
                rotationZ = 0f;    // Pfeil zeigt nach rechts (Standard)
                break;
        }
        arrow.transform.rotation = Quaternion.Euler(0, 0, rotationZ);
    }

    KeyCode RandomKey()
    {
        KeyCode[] keys = { KeyCode.LeftArrow, KeyCode.UpArrow, KeyCode.DownArrow, KeyCode.RightArrow };
        return keys[Random.Range(0, keys.Length)];
    }

}
