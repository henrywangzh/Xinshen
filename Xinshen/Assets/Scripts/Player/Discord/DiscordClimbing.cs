using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DiscordClimbing : MonoBehaviour
{
    [Header("References")]
    public Transform orientation;
    public Rigidbody rb;
    public LayerMask whatIsWall;


    [Header("Climbing")]
    public float climbSpeed;
    public float maxClimbTime;
    private float climbTimer;

    private bool isClimbing;

    [Header("Detection")]
    public float detectionLength;
    public float sphereCastRadius;
    public float maxWallLookAngle;
    private float wallLookAngle;

    private RaycastHit frontWallHit;
    private bool wallInFront;

    // Update is called once per frame
    private void Update()
    {
        WallCheck();
        StateMachine();

        if (isClimbing)
            ClimbingMovement();
    }

    private void StateMachine()
    {
        //State 1 - Climbing
        if (wallInFront && Input.GetKey(KeyCode.W) && wallLookAngle < maxWallLookAngle)
        {
            if (!isClimbing)
                StartClimbing();
        }
        //state 3 - Not climbing
        else
        {
            if (isClimbing)
                StopClimbing();

        }
    }
    private void WallCheck()
    {
        wallInFront = Physics.SphereCast(transform.position, sphereCastRadius, 
            orientation.forward, out frontWallHit, detectionLength, whatIsWall);
        wallLookAngle = Vector3.Angle(orientation.forward, -frontWallHit.normal);

    }
    private void StartClimbing()
    {
        isClimbing = true;
    }
    private void StopClimbing()
    {
        isClimbing = false;
    }
    private void ClimbingMovement()
    {
        rb.velocity = new Vector3(rb.velocity.x, climbSpeed, rb.velocity.z);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }
}
