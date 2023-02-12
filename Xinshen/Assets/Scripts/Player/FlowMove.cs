using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowMove : MonoBehaviour
{
    FlowScriptController controller;
    Rigidbody rb;
    [SerializeField] Transform cam;
    Animator anim;
    [SerializeField] bool targLocked = false;

    // TODO: use global variable manager instead of local variable
    [SerializeField] float moveSpeed = 3f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        controller = GetComponent<FlowScriptController>();               
    }

    // Update is called once per frame
    void Update()
    {
        float xinput = Input.GetAxis("Horizontal");
        float yinput = Input.GetAxis("Vertical");

        Vector3 moveDirection = (cam.forward * yinput + cam.right * xinput) * moveSpeed;
        moveDirection.y = 0;
        moveDirection += new Vector3(0, rb.velocity.y, 0);
        rb.velocity = moveDirection;

        if (targLocked)
        {

            anim.SetFloat("xInput", xinput);
            anim.SetFloat("yInput", yinput);
        }
        else
        {
            anim.SetFloat("xInput", 0);
            anim.SetFloat("yInput", Mathf.Abs(yinput));
            transform.forward = Vector3.Lerp(transform.forward, new Vector3(rb.velocity.x, 0, rb.velocity.z), 0.1f);
        }
        
        

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            controller.switchState.Invoke("evade");
        }
        if (Input.GetMouseButtonDown(0))
        {
            controller.switchState.Invoke("attack");
        }
    }
}