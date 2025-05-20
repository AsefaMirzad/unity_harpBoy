// Updated Arrow.cs to check for null references
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public float speed = 3f;
    public KeyCode assignedKey;

    private bool inHitZone = false;
    private float distanceToCenter;

    void Update()
    {
        transform.Translate(Vector3.down * speed * Time.deltaTime, Space.World);

        // Check if arrow is out of screen
        if (transform.position.y < -6f)
        {
            // Safe null checks
            if (ScoreManager.Instance != null)
            {
                ScoreManager.Instance.Miss();
            }
            if (HitEffectManager.Instance != null)
            {
                HitEffectManager.Instance.ShowMiss(transform.position);
            }
            Destroy(gameObject);
            return; // Added to prevent further execution
        }

        if (inHitZone && Input.GetKeyDown(assignedKey))
        {
            if (ScoreManager.Instance != null)
            {
                if (distanceToCenter < 0.2f)
                {
                    ScoreManager.Instance.Perfect();
                    if (HitEffectManager.Instance != null)
                    {
                        HitEffectManager.Instance.ShowPerfect(transform.position);
                    }
                }
                else if (distanceToCenter < 0.5f)
                {
                    ScoreManager.Instance.Good();
                    if (HitEffectManager.Instance != null)
                    {
                        HitEffectManager.Instance.ShowGood(transform.position);
                    }
                }
                else
                {
                    ScoreManager.Instance.Miss();
                    if (HitEffectManager.Instance != null)
                    {
                        HitEffectManager.Instance.ShowMiss(transform.position);
                    }
                }
            }
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("HitZone"))
        {
            inHitZone = true;
            distanceToCenter = Vector2.Distance(transform.position, other.transform.position);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("HitZone"))
        {
            inHitZone = false;
        }
    }
}