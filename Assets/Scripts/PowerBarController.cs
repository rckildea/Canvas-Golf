using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using Random = System.Random;

public class PowerBarController : MonoBehaviour
{

    private RawImage powerBar;
    public RawImage powerTicker;
    public RawImage accuracyTicker;
    private RectTransform powerTickerRect;
    private RectTransform accuracyTickerRect;
    private float accuracyTickerEndPoint = 0;
    private float powerBarStartX = 0;
    private float powerBarEndX = 0;
    
    private GameObject golfBall;
    private BallMovementHandler _ballMovementHandler;
    

    enum SwingState {None, BackSwing, ForwardSwing, Travel, Reset, NoPowerSet};
    private SwingState currentState;

    public float power = 0.0f;
    public float accuracy = 0.0f;

    private void Awake()
    {
        powerBar = GetComponent<RawImage>();
        golfBall = GameObject.Find("Golf Ball");
        _ballMovementHandler = golfBall.GetComponent<BallMovementHandler>();
        
        powerTickerRect = powerTicker.GetComponent<RectTransform>();
        accuracyTickerRect = accuracyTicker.GetComponent<RectTransform>();

        accuracyTicker.enabled = false;
        
        // Calculate the corners of the powerBar to get its highest x location
        Vector3[] pbVector = new Vector3[4];
        powerBar.GetComponent<RectTransform>().GetWorldCorners(pbVector);

        powerBarStartX = pbVector[0].x;
        powerBarEndX = pbVector[3].x - powerTickerRect.rect.width / 2;
        accuracyTickerEndPoint = powerBarStartX - 3 * accuracyTickerRect.rect.width;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case SwingState.None:
                break;
            case SwingState.BackSwing:
                movePowerTicker();
                break;
            case SwingState.ForwardSwing:
                moveAccuracyTicker();
                break;
            case SwingState.Travel:
                LaunchBall();
                break;
            case SwingState.Reset:
                ResetSwing();
                break;
            case SwingState.NoPowerSet:
                NoPowerSet();
                break;
        }

        if (Input.GetButtonDown("Swing")) NextStep();
        if (Input.GetButtonDown("Reset")) ResetBall();

    }

    private void NextStep()
    {
        if (currentState != SwingState.Travel && currentState != SwingState.Reset && currentState != SwingState.NoPowerSet)
        {
            currentState++;
            if (currentState == SwingState.BackSwing)
            {
                StartBackSwing();
            }
            if (currentState == SwingState.ForwardSwing)
            {
                SetPower();
                StartForwardSwing();
            }

            if (currentState == SwingState.Travel)
            {
                SetAccuracy();
                LaunchBall();
            }
            
        }
    }
    
    private void ResetBall()
    {
        _ballMovementHandler.Reset();
    }

    private void StartBackSwing()
    {
        movePowerTicker();
    }

    private void StartForwardSwing()
    {
        accuracyTicker.enabled = true;
        accuracyTicker.transform.SetPositionAndRotation(new Vector3(powerTickerRect.position.x, accuracyTickerRect.position.y), Quaternion.Euler(0,0,180f));
        moveAccuracyTicker();
    }

    private void movePowerTicker()
    {
        powerTickerRect.Translate(400.0f * Time.deltaTime, 0f, 0f);
        if (powerTickerRect.position.x >= powerBarEndX)
        {
            currentState = SwingState.NoPowerSet;
            NoPowerSet();
        }
    }

    private void moveAccuracyTicker()
    {
        accuracyTickerRect.Translate(400.0f * Time.deltaTime, 0f,  0f); // This value is not negative because the model has been flipped 180 degrees
        if (accuracyTickerRect.position.x <= accuracyTickerEndPoint)
        {
            accuracy = 20;
            currentState = SwingState.Travel;
        }
    }

    private void LaunchBall()
    {
        _ballMovementHandler.SetAttributes(power, accuracy);
        Debug.Log("Power: " + power + "   Accuracy: " + accuracy);
        currentState = SwingState.Reset;
    }


    private void NoPowerSet()
    {
        powerTickerRect.Translate(-500.0f * Time.deltaTime, 0f, 0f);
        if (powerTickerRect.position.x <= powerBarStartX)
        {
            powerTickerRect.position = new Vector3(powerBarStartX, powerTickerRect.position.y);
            currentState = SwingState.None;
        }
    }
    private void ResetSwing()
    {
        power = 0;
        accuracy = 0;
        powerTickerRect.position = new Vector3(powerBarStartX, powerTickerRect.position.y);
        currentState = SwingState.None;
        accuracyTicker.enabled = false;
    }
    
    private void SetPower()
    {
        power = powerTickerRect.position.x + powerTickerRect.rect.width / 2 - powerBarStartX;
    }
    private void SetAccuracy()
    {
        accuracy = 100 - Math.Abs(accuracyTickerRect.position.x - accuracyTickerRect.rect.width / 2 - powerBarStartX);
        if (accuracy < 97.0)
        {
            Random ran = new Random();
            accuracy += ran.Next(-3, 3);
            if (accuracy < 20)
            {
                accuracy = 20;
            }
                
        }
    }
}
