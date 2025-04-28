using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowHitDetector : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    public float hitRange = 1.0f; // Bereich, in dem ein Pfeil getroffen werden kann

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
                break;
            }
        }
    }
}
