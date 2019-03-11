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
    public Transform directionArrow, arrowCenter;

    private float directionArrowMaxZ = 90.0f;
    private float directionArrowMinZ = 0.0f;
    
    float moveZ = 0, moveY = 0;

    private Vector3 offset;

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
            directionArrow.gameObject.SetActive(false);
            ballRigidBody.AddForce(new Vector3(1.5f * power, 1.5f * power, 1.5f * power));
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
        arrowCenter.localPosition = new Vector3(ballTransform.localPosition.x, ballTransform.localPosition.y - 0.1f, ballTransform.localPosition.z);
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
            moveZ += (Input.GetAxis("Horizontal Movement"));
        }
        if (Input.GetButton("Vertical Movement"))
        {
            moveY += (Input.GetAxis("Vertical Movement"));
        }
        
        moveY = Mathf.Clamp(moveY, 5, 50);
        directionArrow.LookAt(arrowCenter);
        arrowCenter.localRotation = Quaternion.Euler(0, moveZ, moveY);
    }

    public void Reset()
    {
        this.transform.position = new Vector3(-8f, 1.5f, -8f);
        ballRigidBody.velocity = Vector3.zero;
        ballRigidBody.angularVelocity = Vector3.zero;
    }
}
