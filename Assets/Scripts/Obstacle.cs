using UnityEngine;
using System.Collections;

public class Obstacle : MonoBehaviour
{
    [Header("Obstacle Settings")]
    public float speed = 5f;
    public int damage = 1;
    public bool destroyOnHit = true;

    [Header("Visual Feedback")]
    public Color hitColor = Color.red;
    public float hitEffectDuration = 0.5f;

    private bool hasHitPlayer = false;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        // Make sure obstacle has proper setup
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Ensure we have a collider
        Collider2D col = GetComponent<Collider2D>();
        if (col == null)
        {
            BoxCollider2D boxCol = gameObject.AddComponent<BoxCollider2D>();
            boxCol.isTrigger = true;
        }

        // Make sure it has the Obstacle tag
        gameObject.tag = "Obstacle";
    }

    void Update()
    {
        // Move the obstacle down
        transform.Translate(Vector3.down * speed * Time.deltaTime, Space.World);

        // Destroy if out of screen
        if (transform.position.y < -6f)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if we hit the player or player's hitzone
        if (!hasHitPlayer && (other.CompareTag("Player") || other.CompareTag("HitZone")))
        {
            ProcessObstacleHit(other.gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Also check collision (not just trigger)
        if (!hasHitPlayer && (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("HitZone")))
        {
            ProcessObstacleHit(collision.gameObject);
        }
    }

    private void ProcessObstacleHit(GameObject hitObject)
    {
        hasHitPlayer = true;

        Debug.Log("Obstacle hit player!");

        // Deal damage to score
        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.ObstacleHit();
        }

        // Show hit effect at collision point
        if (HitEffectManager.Instance != null)
        {
            HitEffectManager.Instance.ShowMiss(transform.position);
        }

        // Visual feedback on player
        Playermovement player = hitObject.GetComponentInParent<Playermovement>();
        if (player == null)
            player = hitObject.GetComponent<Playermovement>();

        //if (player != null)
        //{
        //    player.ShowHitEffect(hitColor);
        //}

        // Camera shake effect (optional)
        StartCoroutine(CameraShake());

        // Destroy or animate obstacle
        if (destroyOnHit)
        {
            StartCoroutine(DestroyWithEffect());
        }
        else
        {
            StartCoroutine(PassThroughEffect());
        }
    }

    private IEnumerator CameraShake()
    {
        Camera mainCam = Camera.main;
        if (mainCam == null) yield break;

        Vector3 originalPos = mainCam.transform.position;
        float elapsed = 0f;
        float duration = 0.2f;
        float magnitude = 0.1f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            mainCam.transform.position = originalPos + new Vector3(x, y, 0);

            elapsed += Time.deltaTime;
            yield return null;
        }

        mainCam.transform.position = originalPos;
    }

    private IEnumerator DestroyWithEffect()
    {
        // Flash and scale effect
        float timer = 0f;
        Vector3 originalScale = transform.localScale;
        Color originalColor = spriteRenderer != null ? spriteRenderer.color : Color.white;

        while (timer < hitEffectDuration)
        {
            timer += Time.deltaTime;
            float progress = timer / hitEffectDuration;

            // Flash between red and white
            if (spriteRenderer != null)
            {
                spriteRenderer.color = Color.Lerp(hitColor, originalColor, Mathf.PingPong(timer * 10f, 1f));
            }

            // Scale down
            transform.localScale = originalScale * (1f - progress);

            // Rotate
            transform.rotation = Quaternion.Euler(0, 0, progress * 360f);

            yield return null;
        }

        Destroy(gameObject);
    }

    private IEnumerator PassThroughEffect()
    {
        // Just flash without destroying
        if (spriteRenderer != null)
        {
            Color originalColor = spriteRenderer.color;
            spriteRenderer.color = hitColor;
            yield return new WaitForSeconds(0.2f);
            spriteRenderer.color = originalColor;
        }
    }
}