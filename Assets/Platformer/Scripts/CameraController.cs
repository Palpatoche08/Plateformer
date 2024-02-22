using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraController: MonoBehaviour
{
    public float moveSpeed = 5f;

    void Update()
    {
        MoveCamera();
    }

    void MoveCamera()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow))
        {
            Vector3 movement = new Vector3(horizontalInput, 0f, 0f) * moveSpeed * Time.deltaTime;
            transform.Translate(movement);
        }
    }
}
