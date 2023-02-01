using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    Rigidbody rb;
    [SerializeField] float speed = 3;
    [SerializeField] Transform cameraPivot, cameraRotate;
    [SerializeField] float z, x;

    [SerializeField] float rotationSpeed = 10;
    [SerializeField] float jumpPower = 3;
    bool onGround = false;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        float right = Input.GetKey(KeyCode.D) ? 1 : 0;
        float left = Input.GetKey(KeyCode.A) ? -1 : 0;
        float forward = Input.GetKey(KeyCode.W) ? 1 : 0;
        float backward = Input.GetKey(KeyCode.S) ? -1 : 0;

        x = right + left;
        z = forward + backward;

        rb.velocity = cameraRotate.forward * z * speed + x * speed * cameraPivot.right + new Vector3(0, rb.velocity.y, 0);


        RotatePlayer();
        if(Input.GetKeyDown(KeyCode.Space) && onGround)
        {
            Jump();
        }
    }

    void RotatePlayer()
    {
        Vector3 targetDir = Vector3.zero;
        // rotate in direction of camera and based on amount of input
        targetDir = cameraRotate.forward * z;
        targetDir += cameraRotate.right * x;

        targetDir.Normalize();

        if (targetDir == Vector3.zero)
        {
            targetDir = transform.forward;
        }
        float rs = rotationSpeed; // rotation speed

        // look towards target direction through a slerp
        Quaternion tr = Quaternion.LookRotation(targetDir);
        Quaternion targetRotation = Quaternion.Slerp(transform.rotation, tr, rs * Time.deltaTime);

        transform.rotation = targetRotation;
    }
    void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, jumpPower, rb.velocity.z);
        onGround = false;
    }

    private void OnCollisionStay(Collision collision)
    {
        for (int i = 0; i < collision.contactCount; i++)
        {
            Vector3 normal = collision.GetContact(i).normal;
            float upDot = Vector3.Dot(transform.up, normal);
            if (upDot >= 0.5)
            {
                onGround = true;
            }
        }
    }
}
