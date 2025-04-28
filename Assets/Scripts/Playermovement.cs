using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playermovement : MonoBehaviour
{ 
    // Start is called before the first frame update
    void Start()
    {
        
    }

    
    public Transform[] lanes; // Die 5 Spalten
    private int currentLaneIndex = 2; // Startet in der Mitte (Index 2)

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
            MoveLeft();
        if (Input.GetKeyDown(KeyCode.D))
            MoveRight();
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
}
