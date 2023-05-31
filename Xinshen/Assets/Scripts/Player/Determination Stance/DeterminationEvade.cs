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
    [SerializeField] private float guardDamageReduction = 0.7f;
    [SerializeField] private float parryWindow = 0.2f;
    float dr = 0;
    float parryTimer = 0;

    private void Start()
    {
        PlayerHP.PlayerHit.AddListener(OnPlayerHit);
    }

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
        dr = 1f;
        parryTimer = 0f;
        PlayerHP.SetDamageReduction(dr);
    }

    void OnPlayerHit()
    {
        // If this is called, player would be guarding, so discord meter drops 
        GlobalVariableManager.AddStanceMeter(StancesScriptController.Stance.discord, -20);

        if (parryTimer < parryWindow)  // Perfect guard 
        {
            GlobalVariableManager.AddStanceMeter(StancesScriptController.Stance.flow, 17);
        } 
        
        GlobalVariableManager.AddStanceMeter(StancesScriptController.Stance.frustration, 10);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            controller.switchState.Invoke("move");
            animator.SetTrigger("DeterminationGuard");
            controller.CheckStanceSwitch();
        }

        if (parryTimer < parryWindow)
        {
            parryTimer += Time.deltaTime;
        } else
        {
            dr = Mathf.Lerp(dr, guardDamageReduction, Time.deltaTime * 2);
            PlayerHP.SetDamageReduction(dr);
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

    private void OnDisable()
    {
        PlayerHP.SetDamageReduction(0);
        animHandler.DeterminationSwordGuard(false);
    }
}
