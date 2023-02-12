using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    [SerializeField] 
    int speed, jump = 0;
    [SerializeField]
    bool canJump;


    //function is called when object HITS any object
    private void OnCollisionEnter(Collision collision)
    {
        canJump = true;
    }
    //fOR EVERYTHING


    //when you stop touching floor, OnCollisionExit() is automatcally called
    private void OnCollisionExit(Collision collision)
    {
        canJump = false;
    }

    //this allows you to create variables in Inspector


    // Start is called before the first frame update
    void Start()
    {
        //transform.position = new Vector3(3,5,5);    //this is how you edit x, y, z values
        canJump = true;
        
    }

    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxis("Horizontal"); //A = -1, D = 1
        float z = Input.GetAxis("Vertical");
        
       
            //to test if object is on ground

        if (Input.GetKeyDown(KeyCode.Space) && canJump == true)
        {
            GetComponent<Rigidbody>().velocity = new Vector3(GetComponent<Rigidbody>().velocity.x, jump, GetComponent<Rigidbody>().velocity.z);
            //< component >     velocity is a value of rigidbody

            //GetComponent<Rigidbody>().velocity.x      //can access current position

        }
        //create a hitbox on bottom of player to fix double jumping on objects




        //
        Vector3 move = new Vector3(x, 0, z);
        transform.position += move * Time.deltaTime * speed;        //by time


    }
}
