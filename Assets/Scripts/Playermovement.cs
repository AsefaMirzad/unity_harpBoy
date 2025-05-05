using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playermovement : MonoBehaviour
{
    // Start is called before the first frame update

    public float jumpHeight = 1.5f;
    public float jumpDuration = 0.6f;
    private bool isJumping = false;
    private Vector3 jumpStartPosition; // Speichert die Position beim Sprungbeginn
    private int jumpStartLaneIndex; // Speichert die Spur beim Sprungbeginn

    public Transform[] lanes; // Die 5 Spalten
    private int currentLaneIndex = 2; // Startet in der Mitte (Index 2)

    void Update()
    {
        // Bewegung nur erlauben, wenn nicht mitten im Sprung
        if (!isJumping)
        {
            if (Input.GetKeyDown(KeyCode.A))
                MoveLeft();
            if (Input.GetKeyDown(KeyCode.D))
                MoveRight();

            if (Input.GetKeyDown(KeyCode.Space))
            {
                StartCoroutine(Jump());
            }
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

    private IEnumerator Jump()
    {
        isJumping = true;
        jumpStartPosition = transform.position; // Speichere die Startposition
        jumpStartLaneIndex = currentLaneIndex; // Speichere die aktuelle Spur

        float elapsed = 0f;
        Vector3 jumpPeak = jumpStartPosition + Vector3.up * jumpHeight;

        // Aufwärtsbewegung
        while (elapsed < jumpDuration / 2)
        {
            // Behalte die x-Position der Startspur während des Sprungs
            Vector3 newPos = Vector3.Lerp(jumpStartPosition, jumpPeak, elapsed / (jumpDuration / 2));
            newPos.x = lanes[jumpStartLaneIndex].position.x; // Fixiere die x-Position auf die Startspur
            transform.position = newPos;
            elapsed += Time.deltaTime;
            yield return null;
        }

        // Abwärtsbewegung
        elapsed = 0f;
        while (elapsed < jumpDuration / 2)
        {
            // Behalte die x-Position der Startspur während des Sprungs
            Vector3 newPos = Vector3.Lerp(jumpPeak, jumpStartPosition, elapsed / (jumpDuration / 2));
            newPos.x = lanes[jumpStartLaneIndex].position.x; // Fixiere die x-Position auf die Startspur
            transform.position = newPos;
            elapsed += Time.deltaTime;
            yield return null;
        }

        // Zurück zur ursprünglichen Position (mit möglicher neuer Spur)
        transform.position = new Vector3(
            lanes[currentLaneIndex].position.x,
            jumpStartPosition.y,
            jumpStartPosition.z
        );

        isJumping = false;
    }

    public bool IsJumping()
    {
        return isJumping;
    }
}
