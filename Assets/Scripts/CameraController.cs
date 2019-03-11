using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform golfBall, ballCenter;
    private float mouseX = 20.0f;
    private float mouseY = 40.0f;
    private float moveFrontAndBack, moveLeftAndRight;
    private float zoom = -20.0f;
    
    private float mouseSensitivity = 6.0f;
    private float moveSpeed = 2.0f;
    private float zoomSpeed = 6.0f;
    
    private float zoomMin = -2.0f;
    private float zoomMax = -30.0f;

    private float yMaxAngle = 60.0f;
    private float yMinAngle = 5.0f;
    
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        zoom += Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
        if (zoom > zoomMin) zoom = zoomMin;
        else if (zoom < zoomMax) zoom = zoomMax;
        
        this.transform.localPosition = new Vector3(0, 0, zoom);

        if (Input.GetMouseButton(1))
        {
            mouseX += Input.GetAxis("Mouse X") * mouseSensitivity;
            mouseY -= Input.GetAxis("Mouse Y") * mouseSensitivity;
        }

        mouseY = Mathf.Clamp(mouseY, yMinAngle, yMaxAngle);
        this.transform.LookAt(ballCenter);
        ballCenter.localRotation = Quaternion.Euler(mouseY, mouseX, 0);
    }
}
