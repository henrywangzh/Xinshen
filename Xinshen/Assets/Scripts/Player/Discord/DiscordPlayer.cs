using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ProBuilder.AutoUnwrapSettings;

public class DiscordPlayer : MonoBehaviour
{
    //Tasks:
    //implement walk right, left, and roll forwards (buggy!), as well as changing speed after running
    //holding w against a wall will move the player into climbing mode (certain walls, ropes, ladders)
    //falling for more than 1 second causes the player to enter falling mode, and perform a landing animation upon hitting the ground.option for fall damage.


    //SerializeField allows you to create variables in Inspector
    //Speed
    [SerializeField]
    private float speed = 5f;
    //Rotate
    [SerializeField]
    private float turnSpeed = 200f;      //turnSpeed gives the degree of rotation.  Ex: 45 degrees
    //Jump
    [SerializeField]
    int jump = 0;
    [SerializeField]
    bool canJump;
    //Attack
    //[SerializeField]

    //Attacking
    private bool isAttacking1 = false;
    private bool isAttacking2 = false;

    

    //Changing to running
    public float runningTime = 0.5f;        //time to hold W in order to start running
    private bool isRunning = false;         //check if player is running
    private float timeHeld = 0f;            //time that W is being held down
    public float runningSpeed = 5f;        //run speed

    [SerializeField]
    private float animationFinishTime = 0.9f;

    //Rolling
    private bool isRolling = false;
    public float rollSpeed = 5f;

    //Create animator variable to allow access to animations in script
    private Animator animator;


    public void Attack()
    {
        animator.SetBool("attack1", false);
    }

    //function is called when object HITS any object
    private void OnCollisionEnter(Collision collision)
    {
        canJump = true;
    }
    //when you stop touching floor, OnCollisionExit() is automatcally called
    private void OnCollisionExit(Collision collision)
    {
        canJump = false;
    }

    //public CharacterController characterController;

    // Start is called before the first frame update
    void Start()
    {
        //transform.position = new Vector3(3,5,5);    //this is how you edit x, y, z values
        canJump = true;

        //Get animator component
        animator = GetComponent<Animator>();

        //get character controller
        //characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        var velocity = Vector3.forward * Input.GetAxis("Vertical") * speed;
        //movement - Vertical is W and S key by default
        transform.Translate(velocity * Time.deltaTime);
        animator.SetFloat("Speed", velocity.z);     //.z is ."zed", this is the forward and backward axis, allowing us negative/positive values


        //Rotate character, and walk
        //".up" because we want to rotate player around the up axis
        transform.Rotate(Vector3.up, Input.GetAxis("Horizontal") * Time.deltaTime * turnSpeed);

        //Testing Movement
        //float x = Input.GetAxis("Horizontal"); //A = -1, D = 1
        //float z = Input.GetAxis("Vertical");
        //Vector3 movement = new Vector3(x, 0, z);
        //You need to get a reference to the Ridigibody component attched to the game to access it
        //Rigidbody ridigbody = GetComponent<Rigidbody>();


        //GETTING CHARACTER TO MOVE FASTER AFTER RUNNING IS NOT IMPLEMENTED!!!!!!!!!!!

        //"holding w for more than 0.5 seconds will shift the player into running mode"
        if (Input.GetKey(KeyCode.W))
        {
            //if W is pressed, timeHeld is incremented by time
            timeHeld += Time.deltaTime;
            if (!isRunning && timeHeld >= runningTime)
            {
                animator.SetBool("isRunning", true);
                //transform.Translate(velocity * Time.deltaTime * 10);
                isRunning = true;
                //transform.forward * runningSpeed * Time.deltaTime);
                //rigidbody.velocity = movement * runningSpeed;
                //GetComponent<Rigidbody>().velocity = movement * runningSpeed;

                //animator.SetFloat("timeHeld");
                //start running animation
                //transform.Translate();
            }
        }
        else
        {
            animator.SetBool("isRunning", false);
            isRunning = false;
            timeHeld = 0f;
            //stop running animation
        }

        //"pressing shift will cause the player to perform a roll in the current movement direction"
        if (Input.GetKey(KeyCode.LeftShift))
        {
            //transform.Rotate(Vector3.forward * rollSpeed * Time.deltaTime);
            isRolling = true;
            animator.SetBool("isRolling", true);
            //transform.Translate(Vector3.forward * 10);
        }
        else
        {
            isRolling = false;
            animator.SetBool("isRolling", false);
        }

        //left cliking for attack
        if (Input.GetMouseButtonDown(0))
        {
            //isAttacking1 = true;
            animator.SetBool("attack1", true);

        }
        //animator.SetBool("attack1", false);


        /*if (timeHeld < 0)
        {
            isAttacking1 = false;
            animator.SetBool("isAttacking1", false);
        }*/

        /*while (isAttacking1 == true)
        {
            if (Input.GetMouseButtonDown(0))
            {
                animator.SetBool("isAttacking2", true);
            }
        }
        animator.SetBool("isAttacking2", false);
*/
        //
        /*if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            animator.;
        }*/

        /*//Day one testing
        float x = Input.GetAxis("Horizontal"); //A = -1, D = 1
         float z = Input.GetAxis("Vertical");*/

        //movement key
        if (Input.GetKeyDown(KeyCode.Space) && canJump == true)
        {
            GetComponent<Rigidbody>().velocity = new Vector3(GetComponent<Rigidbody>().velocity.x, jump, GetComponent<Rigidbody>().velocity.z);
            //< component >     velocity is a value of rigidbody

            //GetComponent<Rigidbody>().velocity.x      //can access current position

        }
        //create a hitbox on bottom of player to fix double jumping on objects

        ///*
       /* Vector3 move = new Vector3(x, 0, z);
        transform.position += move * Time.deltaTime * speed;        //by time*/
    }
}
