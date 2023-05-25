using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeterminationEvade : MonoBehaviour
{
    private DeterminationScriptController controller;
    private DeterminationInput inputHandler;
    private Rigidbody rb;
    private Animator animator;
    private Transform cam;
    private PlayerAnimHandler animHandler;

    [SerializeField] private float walkSpeed = 2f; // walk movement speed
    [SerializeField] private float rotSpeed = 10f; // rotation speed


    private void OnEnable()
    {
        if (animator == null)
        {
            controller = GetComponent<DeterminationScriptController>();
            inputHandler = GetComponent<DeterminationInput>();
            rb = GetComponent<Rigidbody>();
            animator = GetComponent<Animator>();
            cam = GetComponent<FlowMove>().cam;
            animHandler = GetComponent<PlayerAnimHandler>();
        }
        animator.SetTrigger("DeterminationGuard");
        animHandler.DeterminationSwordGuard(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            animHandler.DeterminationSwordGuard(false);
            controller.switchState.Invoke("move");
            animator.SetTrigger("DeterminationGuard");
        }

        Vector2 inputVec = inputHandler.getInputVectorNorm();

        animator.SetFloat("xInput", Mathf.Lerp(animator.GetFloat("xInput"), inputVec.x, 0.4f));
        animator.SetFloat("yInput", Mathf.Lerp(animator.GetFloat("yInput"), inputVec.y, 0.4f));

        // Update player rotation
        Vector3 forwardDir = Vector3.RotateTowards(transform.forward, /*new Vector3(rb.velocity.x, 0, rb.velocity.z)*/ new Vector3(cam.forward.x, 0f,cam.forward.z), rotSpeed * Time.deltaTime, 0f);
        transform.forward = forwardDir;

        // Update rigidbody velocity
        Vector3 moveDir = (new Vector3(cam.forward.x, 0, cam.forward.z).normalized * inputVec.y + new Vector3(cam.right.x, 0, cam.right.z).normalized * inputVec.x);
        moveDir.y = 0;

        Vector3 moveVelocity = moveDir * walkSpeed;
        rb.velocity = moveVelocity + new Vector3(0, rb.velocity.y, 0);

    }
}
