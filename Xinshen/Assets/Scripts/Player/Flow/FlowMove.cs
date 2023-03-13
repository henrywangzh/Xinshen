using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowMove : MonoBehaviour
{
    FlowScriptController controller;
    Rigidbody rb;
    [SerializeField] public Transform cam;
    Animator anim;
    [SerializeField] bool targLocked = false;

    // TODO: use global variable manager instead of local variable
    [SerializeField] float moveSpeed = 3f;
    [SerializeField] float turnSpeed = 5f;

    float speed;
    float dashTimer = 0;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        controller = GetComponent<FlowScriptController>();
        speed = moveSpeed;
        dashTimer = 0;
    }

    public void SetDashCD(float dashCD)
    {
        dashTimer = dashCD;
    }

    // Update is called once per frame
    void Update()
    {
        float xinput = Input.GetAxis("Horizontal");
        float yinput = Input.GetAxis("Vertical");

        Vector3 moveDirection = (new Vector3(cam.forward.x, 0, cam.forward.z).normalized * yinput + new Vector3(cam.right.x, 0, cam.right.z).normalized * xinput).normalized * speed;
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
            transform.forward = Vector3.RotateTowards(transform.forward, new Vector3(moveDirection.x, 0, moveDirection.z), turnSpeed * Time.deltaTime, 0f);
            // transform.forward = Vector3.Lerp(transform.forward, new Vector3(rb.velocity.x, 0, rb.velocity.z), 0.1f);
        }
        
        if (dashTimer > 0)
        {
            dashTimer -= Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && dashTimer <= 0)
        {
            controller.switchState.Invoke("evade");
            anim.Play("FlowEvade");
            PlayerHP.SetInvulnerable(16);
        }
        if (Input.GetMouseButtonDown(0))
        {
            controller.switchState.Invoke("attack");
            anim.Play("SlashCombo1_1");
            rb.velocity = Vector3.zero;
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            controller.switchState.Invoke("cross");
            anim.Play("FlowCrossSlash");
        }
    }
}
