using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class BallMovementHandler : MonoBehaviour
{
    private float power;
    private float accuracy;
    private Rigidbody ballRigidBody;
    private Transform ballTransform;
    public Transform directionArrow, arrowCenter, arrowPoint, ballCenter;

    private float directionArrowMaxY = 50.0f;
    private float directionArrowMinY = 5.0f;
    private float arrowMovementSpeed = 2.0f;
    
    private Vector3 ballPos;
    
    float moveZ = 0, moveY = 0;

    private Vector3 offset;
    private bool ballIsMoving = false;

    private void Awake()
    {
        ballTransform = this.transform;
        ballRigidBody = this.GetComponent<Rigidbody>();
        ballRigidBody.sleepThreshold = 1f;
    }

    // Start is called before the first frame update
    void Start()
    {
        

    }

    // Update is called once per frame
    void Update()
    {
        
        if (power != 0)
        {
            ballRigidBody.AddRelativeForce(arrowPoint.forward * power);
            directionArrow.gameObject.SetActive(false);
            power = 0;
        }

        if (ballRigidBody.velocity.magnitude == 0)
        {
            directionArrow.gameObject.SetActive(true);
        }
        else
        {
            directionArrow.gameObject.SetActive(false);
        }
        RotateDirectionArrow();
        ballPos = new Vector3(ballTransform.localPosition.x, ballTransform.localPosition.y - 0.1f,
            ballTransform.localPosition.z);
        arrowCenter.localPosition = ballPos;
        arrowPoint.localPosition = ballPos;
        ballCenter.localPosition = ballPos;
    }
    
    void FixedUpdate()
    {
        Debug.Log(ballRigidBody.velocity);
        if (ballRigidBody.velocity != Vector3.zero)
        {
            ballIsMoving = true;
        }
        else if (ballIsMoving) PrepareBall();
    }

    public void SetAttributes(float pow, float acc)
    {
        power = pow;
        accuracy = acc;
    }
    private void RotateDirectionArrow()
    {
        //this.transform.localPosition = new Vector3(0, 0, zoom);

        if (Input.GetButton("Horizontal Movement"))
        {
            moveZ += Input.GetAxis("Horizontal Movement") * arrowMovementSpeed;
        }
        if (Input.GetButton("Vertical Movement"))
        {
            moveY += Input.GetAxis("Vertical Movement") * arrowMovementSpeed;
        }
        
        moveY = Mathf.Clamp(moveY, directionArrowMinY, directionArrowMaxY);
        directionArrow.LookAt(arrowCenter);
        arrowCenter.localRotation = Quaternion.Euler(0, moveZ, moveY);
    }

    public void Reset()
    {
        this.transform.position = new Vector3(-8f, 1.5f, -8f);
        ballRigidBody.velocity = Vector3.zero;
        ballRigidBody.angularVelocity = Vector3.zero;
    }

    public void PrepareBall()
    {
        // Reset the velocity
        ballRigidBody.velocity = Vector3.zero;
        ballRigidBody.angularVelocity = Vector3.zero;
        // "Pause" the physics
        ballRigidBody.isKinematic = true;
        // Do positioning, etc
        ballRigidBody.transform.rotation = Quaternion.identity;
        // Re-enable the physics
        ballRigidBody.isKinematic = false;
        ballIsMoving = false;
    }
}
