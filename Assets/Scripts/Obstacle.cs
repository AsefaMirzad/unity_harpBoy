using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public float speed = 5f;
    public AudioSource hitSound;

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
                    Debug.Log("Hindernis getroffen (2D)!");

                    // Play hit sound
                    if (hitSound != null)
                        hitSound.Play();

                    // Register obstacle hit with ScoreManager
                    if (ScoreManager.Instance != null)
                        ScoreManager.Instance.ObstacleHit();
                }
                else
                {
                    Debug.Log("Spieler hat das Hindernis übersprungen (2D).");
                }

                Destroy(gameObject);
            }
        }
    }
}