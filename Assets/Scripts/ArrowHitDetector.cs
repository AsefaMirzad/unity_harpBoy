using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowHitDetector : MonoBehaviour
{
    public float hitRange = 1.0f; // Bereich, in dem ein Pfeil getroffen werden kann
    public AudioClip hitSound; // Sound, der bei einem Treffer abgespielt wird
    private AudioSource audioSource; // AudioSource-Komponente

    void Start()
    {
        // Füge eine AudioSource-Komponente hinzu, falls nicht vorhanden
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow)) TryHit(KeyCode.LeftArrow);
        if (Input.GetKeyDown(KeyCode.UpArrow)) TryHit(KeyCode.UpArrow);
        if (Input.GetKeyDown(KeyCode.DownArrow)) TryHit(KeyCode.DownArrow);
        if (Input.GetKeyDown(KeyCode.RightArrow)) TryHit(KeyCode.RightArrow);
    }

    void TryHit(KeyCode key)
    {
        Arrow[] arrows = FindObjectsOfType<Arrow>();
        foreach (Arrow arrow in arrows)
        {
            float distance = Vector2.Distance(transform.position, arrow.transform.position);
            if (distance <= hitRange && arrow.assignedKey == key)
            {
                Destroy(arrow.gameObject);
                Debug.Log("Treffer! " + key);

                // Spiele den Sound ab, wenn einer zugewiesen wurde
                if (hitSound != null)
                {
                    audioSource.PlayOneShot(hitSound);
                }
                break;
            }
        }
    }
}