using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrustrationMove : GlobalVariableManager
{
    // Start is called before the first frame update
    FrustrationScriptController controller;
    Rigidbody rb;
    [SerializeField] Transform cam;
    Animator anim;
    [SerializeField] bool targLocked = false;

    // TODO: use global variable manager instead of local variable
    
    [SerializeField] float moveSpeed = RunningSpeed;
    
    [SerializeField] float turnSpeed = 5f;
    float speed;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        controller = GetComponent<FrustrationScriptController>();
        speed = moveSpeed; //player immediately runs
    }


    // Update is called once per frame
    void Update()
    {
        float xinput = Input.GetAxis("Horizontal");
        float yinput = Input.GetAxis("Vertical");

        Vector3 moveDirection = (new Vector3(cam.forward.x, 0, cam.forward.z) * yinput + new Vector3(cam.right.x, 0, cam.right.z) * xinput).normalized * speed;
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
            anim.SetFloat("yInput", Mathf.Sqrt(yinput*yinput + xinput*xinput) * 2f);
            transform.forward = Vector3.RotateTowards(transform.forward, new Vector3(rb.velocity.x, 0, rb.velocity.z), turnSpeed * Time.deltaTime, 0f);
            // transform.forward = Vector3.Lerp(transform.forward, new Vector3(rb.velocity.x, 0, rb.velocity.z), 0.1f);
        }

        if (Input.GetMouseButtonDown(0))
        {
            controller.switchState.Invoke("attack");
        }

    }
    
}
