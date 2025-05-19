using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class Arrow : MonoBehaviour
{
    // Start is called before the first frame update

   
    // Start is called before the first frame update
    void Start()
    {

    }

    public float speed = 3f;
    public KeyCode assignedKey; // Welche Taste gedrückt werden muss


    private bool inHitZone = false;
    private float distanceToCenter; 

    void Update()
    {
        transform.Translate(Vector3.down * speed * Time.deltaTime, Space.World);

        if (transform.position.y < -6f)
        {
            Destroy(gameObject);
            ScoreManager.Instance.Miss();
            HitEffectManager.Instance.ShowMiss(transform.position);
        }

        if (inHitZone)
        {
            if (Input.GetKeyDown(assignedKey))
            {


                if (distanceToCenter < 0.2f)
                {
                    ScoreManager.Instance.Perfect();
                    HitEffectManager.Instance.ShowPerfect(transform.position);
                }
                else if (distanceToCenter < 0.5f)
                {
                    ScoreManager.Instance.Good();
                    HitEffectManager.Instance.ShowGood(transform.position);
                }
                else
                {
                    ScoreManager.Instance.Miss();
                    HitEffectManager.Instance.ShowMiss(transform.position);
                }

                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("HitZone"))
        {
            inHitZone = true;
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
