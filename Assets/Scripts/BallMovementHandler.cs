using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMovementHandler : MonoBehaviour
{
    private float power;
    private float accuracy;
    private Rigidbody ballRigidBody;
    
    // Start is called before the first frame update
    void Start()
    {
        ballRigidBody = this.GetComponent<Rigidbody>();
        ballRigidBody.sleepThreshold = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        if (power != 0)
        {
            ballRigidBody.AddForce(new Vector3(1.5f * power, 1.5f * power, 1.5f * power));
            power = 0;
        }
    }

    public void SetAttributes(float pow, float acc)
    {
        power = pow;
        accuracy = acc;
    }
}
