using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] Transform pivot, rotate;
    [SerializeField] float xMouse, yMouse, verticalOffset, xSensitivity, ySensitivity;
    [SerializeField] Transform focus;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        yMouse = Input.GetAxis("Mouse Y");      //right now camera is flipped, and rotates indefintley, so fix that and set to max 90 degrees


        xMouse = Input.GetAxis("Mouse X");

        pivot.Rotate(new Vector3(-yMouse, 0, 0) * ySensitivity);
        rotate.Rotate(new Vector3(0, xMouse, 0) * xSensitivity);
        rotate.position = focus.position + Vector3.up * 2;


    }
}
