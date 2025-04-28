using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
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
   
    void Update()
    {
        transform.Translate(Vector3.down * speed * Time.deltaTime, Space.World);

        if (transform.position.y < -6f) // Wenn Pfeil zu weit unten -> zerstören
            Destroy(gameObject);
    }
}
