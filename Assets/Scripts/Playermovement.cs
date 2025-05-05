using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playermovement : MonoBehaviour
{
    // Start is called before the first frame update

    public float jumpHeight = 1.5f;
    public float jumpDuration = 0.6f;
    private bool isJumping = false;
    private Vector3 startPosition;
    void Start()
    {
        startPosition = transform.position;
    }

    
    public Transform[] lanes; // Die 5 Spalten
    private int currentLaneIndex = 2; // Startet in der Mitte (Index 2)

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
            MoveLeft();
        if (Input.GetKeyDown(KeyCode.D))
            MoveRight();

        if (Input.GetKeyDown(KeyCode.Space) && !isJumping)
        {
            StartCoroutine(Jump());
        }
    }

    void MoveLeft()
    {
        if (currentLaneIndex > 0)
        {
            currentLaneIndex--;
            UpdatePosition();
        }
    }

    void MoveRight()
    {
        if (currentLaneIndex < lanes.Length - 1)
        {
            currentLaneIndex++;
            UpdatePosition();
        }
    }

    void UpdatePosition()
    {
        transform.position = new Vector3(
            lanes[currentLaneIndex].position.x,
            transform.position.y,
            transform.position.z
        );
    }


    private System.Collections.IEnumerator Jump()
    {
        isJumping = true;
        float elapsed = 0f;

        Vector3 jumpPeak = startPosition + Vector3.up * jumpHeight;

        // Aufwärtsbewegung
        while (elapsed < jumpDuration / 2)
        {
            transform.position = Vector3.Lerp(startPosition, jumpPeak, elapsed / (jumpDuration / 2));
            elapsed += Time.deltaTime;
            yield return null;
        }

        // Abwärtsbewegung
        elapsed = 0f;
        while (elapsed < jumpDuration / 2)
        {
            transform.position = Vector3.Lerp(jumpPeak, startPosition, elapsed / (jumpDuration / 2));
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = startPosition;
        isJumping = false;
    }

    public bool IsJumping()
    {
        return isJumping;
    }
}
