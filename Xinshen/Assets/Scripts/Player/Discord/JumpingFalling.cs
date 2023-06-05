using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpingFalling : MonoBehaviour
{
  /*  Animator anim;
    float lastPos;
    // Start is called before the first frame update
    void Start()
    {
        lastPos = transform.position.y;
        InvokeRepeating("checkPos", 0.01f, .5f);
        //invokeRepeating is called each x seconds

    }

    void checkPos()
    {
        float difference = transform.position.y - lastPos;
        if (difference > 0.5f)     //this means y is decreasing
        {
            anim.Play("RealFalling");
        }
        else //y stayed the same
        {
            lastPos = transform.position.y;
        }
    }*/
    // Update is called once per frame
    void Update()
    {

    }


    //this doesn't work cuz player doesn't even touch ground 
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Collidable") //tag is in inspector of the object, udner the anme
        {
            Debug.Log("enter");
            //print("enter");
        }
    } //only called on firs frame of collision
    
    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Collidable") //tag is in inspector
        {
            Debug.Log("stay");
            //print("stay");
        }
    } //called every frame of collision

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Collidable") //tag is in inspector
        {
            Debug.Log("exit");
            //print("exit");
        }
    } //called on last frame of collision
}
