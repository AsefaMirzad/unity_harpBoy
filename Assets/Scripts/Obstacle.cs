using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    // Start is called before the first frame update
    public float speed = 5f;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Playermovement player = other.GetComponent<Playermovement>();
            if (player != null && !player.IsJumping())
            {
                Debug.Log("Hindernis getroffen!");
                // TODO: hier später Leben abziehen oder Effekt auslösen
            }
            else
            {
                Debug.Log("Spieler ist gesprungen und hat das Hindernis übersprungen.");
            }
        }
    }
    private void Update()
    {
        transform.position += Vector3.down * speed * Time.deltaTime;

        if (transform.position.y < -10f)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            ScoreManager.Instance.Miss(); // Minuspunkte
            Destroy(gameObject);
        }
    }
}
