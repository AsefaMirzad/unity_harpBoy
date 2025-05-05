using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    // Start is called before the first frame update
    public float speed = 5f;

    void Update()
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
            Playermovement player = other.GetComponent<Playermovement>();
            if (player != null)
            {
                if (!player.IsJumping())
                {
                    Debug.Log("?? Hindernis getroffen (2D)!");
                    // TODO: Leben abziehen
                }
                else
                {
                    Debug.Log("?? Spieler hat das Hindernis übersprungen (2D).");
                }

                Destroy(gameObject); // Optional
            }
        }
    }
}
