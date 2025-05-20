using UnityEngine;

public class HitEffectManager : MonoBehaviour
{
    public static HitEffectManager Instance;

    [Header("Visual Effect Prefabs")]
    public GameObject PerfectEffectPrefab;
    public GameObject GoodEffectPrefab;
    public GameObject MissEffectPrefab;

    [Header("Audio")]
    public AudioSource perfectSound;
    public AudioSource goodSound;
    public AudioSource missSound;

    [Header("Fallback Colors")]
    public Color perfectColor = new Color(1f, 0.8f, 0f);  // Gold
    public Color goodColor = new Color(0f, 0.8f, 0.2f);   // Green
    public Color missColor = new Color(1f, 0.2f, 0.2f);   // Red

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void ShowPerfect(Vector3 position)
    {
        // Play sound effect
        if (perfectSound != null)
            perfectSound.Play();

        // Show visual effect
        if (PerfectEffectPrefab != null)
        {
            // Use the prefab if available
            Instantiate(PerfectEffectPrefab, position, Quaternion.identity);
        }
        else
        {
            // Create a simple text effect as fallback
            CreateTextEffect("PERFECT!", position, perfectColor);
        }
    }

    public void ShowGood(Vector3 position)
    {
        // Play sound effect
        if (goodSound != null)
            goodSound.Play();

        // Show visual effect
        if (GoodEffectPrefab != null)
        {
            // Use the prefab if available
            Instantiate(GoodEffectPrefab, position, Quaternion.identity);
        }
        else
        {
            // Create a simple text effect as fallback
            CreateTextEffect("GOOD", position, goodColor);
        }
    }

    public void ShowMiss(Vector3 position)
    {
        // Play sound effect
        if (missSound != null)
            missSound.Play();

        // Show visual effect
        if (MissEffectPrefab != null)
        {
            // Use the prefab if available
            Instantiate(MissEffectPrefab, position, Quaternion.identity);
        }
        else
        {
            // Create a simple text effect as fallback
            CreateTextEffect("MISS", position, missColor);
        }
    }

    // Creates a simple text effect if no prefab is available
    private void CreateTextEffect(string message, Vector3 position, Color color)
    {
        // Create a new GameObject
        GameObject effectObj = new GameObject("TextEffect");
        effectObj.transform.position = position;

        // Add TextMesh for 3D text
        TextMesh textMesh = effectObj.AddComponent<TextMesh>();
        textMesh.text = message;
        textMesh.color = color;
        textMesh.fontSize = 24;
        textMesh.alignment = TextAlignment.Center;
        textMesh.anchor = TextAnchor.MiddleCenter;

        // Add a script to animate and destroy
        EffectAnimation anim = effectObj.AddComponent<EffectAnimation>();
        anim.lifetime = 1.0f;
        anim.moveSpeed = 1.0f;
        anim.fadeSpeed = 1.0f;
    }
}

// Helper class for animating text effects
public class EffectAnimation : MonoBehaviour
{
    public float lifetime = 1.0f;
    public float moveSpeed = 1.0f;
    public float fadeSpeed = 1.0f;

    private TextMesh textMesh;
    private float timer = 0f;

    void Start()
    {
        textMesh = GetComponent<TextMesh>();
    }

    void Update()
    {
        // Move upward
        transform.position += Vector3.up * moveSpeed * Time.deltaTime;

        // Fade out
        if (textMesh != null)
        {
            Color color = textMesh.color;
            color.a = Mathf.Max(0, 1 - (timer / lifetime));
            textMesh.color = color;
        }

        // Count time and destroy when done
        timer += Time.deltaTime;
        if (timer >= lifetime)
        {
            Destroy(gameObject);
        }
    }
}