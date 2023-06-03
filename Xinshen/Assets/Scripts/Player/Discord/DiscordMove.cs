using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

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
        cam = GlobalVariableManager.MainCamera;
    }

    private void OnEnable()
    {
        controller.CheckStanceTransitions();
    }

    // Climbing mode flag
    [SerializeField] bool isClimbing = false;
    public float raycastDistance = 1f; // Distance of the raycast from the player
    public LayerMask wallLayer; // Layer mask for the wall objects

    // Falling mode flag
    [SerializeField] public float fallThreshold = 1f; // Threshold for detecting falling
    public float fallTimeThreshold = 1f; // Time threshold for detecting falling
    [SerializeField] private bool isFalling = false; // Flag to track if the player is currently falling
    

    // Rolling
    public float rollForce = 10.0f; // The force applied to perform the roll
    //public Animator animator; // Reference to the player's animator component
    private Vector3 currentMoveDirection; // The current movement direction of the player

    //Time since contact
    float timeSinceContact = 0f;
    private float fallStartTime; // Time when the player starts falling
    void OnCollisionEnter(Collision collision)
    {
        /* if (collision.gameObject.CompareTag("Ground"))
         {
             isFalling = false;
             timeSinceContact = 0f;
             anim.Play("DiscordMove");
         }*/
        if (isFalling && Time.time - fallStartTime >= 1f)  //player lands on surface, is no longer falling
        {
            isFalling = false;
            anim.SetBool("Falling", false);
            anim.Play("FellDown");
        }
        timeSinceContact = 0f;
    }
/*    void OnCollisionExit(Collision collision)
    {
        timeSinceContact += Time.deltaTime;
        //isFalling = false;
    }*/
    void OnCollisionStay(Collision collision)
    {
        
        if (!isFalling && timeSinceContact >= 1f)   //player wasn't falling, and now is falling
        {
            isFalling = true;
            fallStartTime = Time.time;
            anim.SetBool("Falling", true);
            anim.Play("Falling");
        }
        timeSinceContact = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        //update time since contact
        timeSinceContact += Time.deltaTime;

        // Detect input for 'W' key, also 'A', 'D'
        bool isHoldingW = Input.GetKey(KeyCode.W);
        bool isHoldingD = Input.GetKey(KeyCode.D);
        bool isHoldingA = Input.GetKey(KeyCode.A);
        bool isHoldingS = Input.GetKey(KeyCode.S);


        // Get movement
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


        //ATTACKING
        if (Input.GetMouseButtonDown(0))
        {
            controller.switchState.Invoke("discordAttack");
            //anim.Play("attack07");
            rb.velocity = Vector3.zero;
        }

        //ROLLING
        // Get the input for movement
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        // Update the current movement direction
        currentMoveDirection = new Vector3(moveHorizontal, 0.0f, moveVertical);
        // Check for input to perform roll
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            // Perform roll in the current movement direction
            PerformRoll(currentMoveDirection);
        }

        void PerformRoll(Vector3 rollDirection)
        {
            // Normalize the roll direction
            rollDirection.Normalize();

            // Apply a force to the player in the roll direction
            //rb.AddForce(rollDirection * rollForce, ForceMode.Impulse);
            //rb.AddForce(transform.forward * 2000f, ForceMode.Force);

            // Play the rolling animation
            anim.Play("TRUERoll");

            //ISSUE:  DOES NOT UPDATE PLAYER AFTER ROLLING

            //Make player invulerable ???
            PlayerHP.SetInvulnerable(2);

            //PlayerHP.SetDamageReduction(1);
            //1 = %100 damage reduction
        }


        /*//CLIMBING
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
        if (isHoldingS && isCollidingWithWall)
        {
            isClimbing = true;
            transform.position += Vector3.down * 1f * Time.deltaTime;
            anim.Play("Climbing");
        }
        if (isHoldingD && isCollidingWithWall)
        {
            transform.position += transform.right * 1f * Time.deltaTime;
            anim.Play("Climbing");
        }
        if (isHoldingA && isCollidingWithWall)
        {
            //transform.position += transform.left * 1f * Time.deltaTime;
            anim.Play("Climbing");
        }

        if (!isHoldingW && isCollidingWithWall || !isHoldingD && isCollidingWithWall || !isHoldingS && isCollidingWithWall || !isHoldingA && isCollidingWithWall)
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

            //anim.Play("Falling");
        }*/


        //check if time since on ground


        /*Collision collision ;
        {
            for (int i = 0; i < collision.contactCount; i++)
            {
                Vector3 normal = collision.GetContact(i).normal;
            }
        }*/

        //FALLING
        //only do this when player presses jump maybe
        /* if (isClimbing)
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
             //if (!isFalling && transform.position.y < fallThreshold)
             if (!isFalling && timeSinceContact > 1f)
             {
                 // Player is starting to fall, record fall start time
                 isFalling = true;
                 fallStartTime = Time.time;
                 anim.Play("Falling");
             }
            *//* else if (isFalling)
             {
                 // Player is already falling, check fall time threshold
                 float fallTime = Time.time - fallStartTime;
                 if (fallTime >= fallTimeThreshold)
                 {
                     // Player has fallen for more than the time threshold, trigger falling mode
                     // play anim.play falling animation

                     anim.Play("Falling");
                 }
             }*//*
         }*/
      /*  if (!isFalling && timeSinceContact >= 1f)
        {
            // Player is starting to fall, record fall start time
            isFalling = true;
            fallStartTime = Time.time;
            anim.Play("Falling");
            rb.useGravity = true;
        }
        if (isFalling && (timeSinceContact == 0f))
        {
            isFalling = false;
            anim.Play("FellDown");
            rb.useGravity = true;
        }*/

        //player lands
       /* if (isFalling && timeSinceContact == 0f && (fallStartTime > 0f))
        {
            isFalling = false;
            anim.SetBool("Falling", false);
            anim.Play("FellDown");
        }*/
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

    public int moveRollingPlayerForward(int rollForceReal)
    {

        rb.AddForce(transform.forward * rollForceReal, ForceMode.Impulse);
        return 0;
    }
}