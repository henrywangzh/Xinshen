using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    //this is a way to make a first person point of view, where you make the camera a child of the player parent

    [SerializeField] float mouseY;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Update() // Update is called once per frame
    {
        
    }

    void FixedUpdate() //50x per sec
    {
        mouseY = Input.GetAxis("Mouse Y");      //right now camera is flipped, and rotates indefintley, so fix that and set to max 90 degrees

        transform.Rotate(Vector3.right * mouseY);
        //mouseX = Input.GetAxis("Mouse Y");
    }
}
