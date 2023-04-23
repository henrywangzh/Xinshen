using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiscordMove : MonoBehaviour
{
    ActualDiscordScriptController controller;
    Rigidbody rb;
    [SerializeField] Transform cam;
    Animator anim;
    [SerializeField] bool targLocked = false;

    // TODO: use global variable manager instead of local variable
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float turnSpeed = 5f;
    float speed;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        controller = GetComponent<ActualDiscordScriptController>();
        speed = moveSpeed;
    }


    // Climbing mode flag
    [SerializeField] bool isClimbing = false;
    public float raycastDistance = 1f; // Distance of the raycast from the player
    public LayerMask wallLayer; // Layer mask for the wall objects

    // Falling mode flag
    [SerializeField] public float fallThreshold = -10f; // Threshold for detecting falling
    public float fallTimeThreshold = 1f; // Time threshold for detecting falling
    [SerializeField] private bool isFalling = false; // Flag to track if the player is currently falling
    private float fallStartTime; // Time when the player starts falling

    // Update is called once per frame
    void Update()
    {
        float xinput = Input.GetAxis("Horizontal");
        float yinput = Input.GetAxis("Vertical");

        Vector3 moveDirection = (new Vector3(cam.forward.x, 0, cam.forward.z).normalized * yinput + new Vector3(cam.right.x, 0, cam.right.z).normalized * xinput) * speed;
        moveDirection.y = 0;
        moveDirection += new Vector3(0, rb.velocity.y, 0);
        rb.velocity = moveDirection;

        if (targLocked)
        {
            speed = moveSpeed;
            anim.SetFloat("xInput", xinput);
            anim.SetFloat("yInput", yinput);
        }
        else
        {
            speed = moveSpeed * 1.5f;
            anim.SetFloat("xInput", 0);
            anim.SetFloat("yInput", Mathf.Sqrt(yinput * yinput + xinput * xinput) * 2f);
            transform.forward = Vector3.RotateTowards(transform.forward, new Vector3(rb.velocity.x, 0, rb.velocity.z), turnSpeed * Time.deltaTime, 0f);
            // transform.forward = Vector3.Lerp(transform.forward, new Vector3(rb.velocity.x, 0, rb.velocity.z), 0.1f);
        }


        //Attacking
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            controller.switchState.Invoke("evade");
            PlayerHP.SetInvulnerable(16);
        }
        if (Input.GetMouseButtonDown(0))
        {
            controller.switchState.Invoke("discordAttack");
            //anim.Play("attack07");
            rb.velocity = Vector3.zero;
        }


        //Player is climbing

        // Detect input for 'W' key
        bool isHoldingW = Input.GetKey(KeyCode.W);
        // Check if player is colliding with a wall
        bool isCollidingWithWall = false;
        //collision detection:
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, raycastDistance, wallLayer))
        {
            // Player is colliding with a wall
            isCollidingWithWall = true;
            // Your custom code for handling the player collision with the wall
        }
        else
        {
            // Player is not colliding with a wall
            isCollidingWithWall = false;
            // Your custom code for handling the player not colliding with the wall
        }

        // Transition into climbing mode
        if (isHoldingW && isCollidingWithWall)
        {
            // Set climbing mode flag
            isClimbing = true;

            // Perform climbing logic
            // e.g., change player's movement, animation, physics, etc.
            transform.position += Vector3.up * 1f * Time.deltaTime;

            //play climbing animation
            anim.Play("Climbing");
        }
        else if (!isHoldingW && isCollidingWithWall)
        {
            rb.useGravity = false;  
        }
        else
        {
            // Exit climbing mode
            isClimbing = false;
            rb.useGravity = true;
            // Reset climbing logic
            // e.g., restore player's movement, animation, physics, etc.
        }


        //Player is falling
            //only do this when player presses jump maybe
        if (isClimbing)
        {
            if (isFalling)
            {
                //trigger landing animation
                isFalling = false;
                anim.Play("FellDown");
            }
        }
        else
        {
            // Player is not climbing
            if (!isFalling && transform.position.y < fallThreshold)
            {
                // Player is starting to fall, record fall start time
                isFalling = true;
                fallStartTime = Time.time;
            }
            else if (isFalling)
            {
                // Player is already falling, check fall time threshold
                float fallTime = Time.time - fallStartTime;
                if (fallTime >= fallTimeThreshold)
                {
                    // Player has fallen for more than the time threshold, trigger falling mode
                    // play anim.play falling animation

                    anim.Play("Falling");
                }
            }
        }
    }
       /* if (transform.position.y < fallThreshold)
        {
            if (!isFalling)
            {
                // Player is starting to fall, record fall start time
                isFalling = true;
                fallStartTime = Time.time;
            }
            else
            {
                // Player is already falling, check fall time threshold
                float fallTime = Time.time - fallStartTime;
                if (fallTime >= fallTimeThreshold)
                {
                    // Player has fallen for more than the time threshold, trigger falling mode
                    //anim.play falling
                    //isFalling = true;
                }
            }
        }
        else
        {
            // Player is not falling
            if (isFalling)
            {
                // Player was previously falling, trigger landing animation
                //anim.play
                isFalling = false;
            }

        }*/

}
