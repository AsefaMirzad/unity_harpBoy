using UnityEngine;

public class HitZone : MonoBehaviour
{
    [Header("Hit Detection Settings")]
    public float perfectRange = 0.3f;
    public float goodRange = 0.6f;
    public float normalRange = 1.0f;

    [Header("Visual Feedback")]
    public bool showDebugGizmos = true;
    public Color perfectColor = new Color(1f, 0.8f, 0f, 0.5f);
    public Color goodColor = Color.green;
    public Color normalColor = Color.blue;

    private void Start()
    {
        gameObject.tag = "HitZone";
        if (!GetComponent<Collider2D>())
        {
            BoxCollider2D col = gameObject.AddComponent<BoxCollider2D>();
            col.isTrigger = true;
            col.size = new Vector2(2f, normalRange * 2);
        }
    }

    public string GetHitQuality(Vector3 position)
    {
        float distance = Mathf.Abs(position.y - transform.position.y);

        if (distance <= perfectRange) return "Perfect";
        if (distance <= goodRange) return "Good";
        if (distance <= normalRange) return "Normal";
        return "Miss";
    }

    public float GetDistanceFromCenter(Vector3 position)
    {
        return Mathf.Abs(position.y - transform.position.y);
    }

    void OnDrawGizmos()
    {
        if (!showDebugGizmos) return;

        Vector3 center = transform.position;

        Gizmos.color = perfectColor;
        Gizmos.DrawCube(center, new Vector3(2f, perfectRange * 2, 0.1f));

        Gizmos.color = goodColor;
        Gizmos.DrawWireCube(center, new Vector3(2f, goodRange * 2, 0.1f));

        Gizmos.color = normalColor;
        Gizmos.DrawWireCube(center, new Vector3(2f, normalRange * 2, 0.1f));
    }
}