using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowEvade : MonoBehaviour
{
    FlowScriptController controller;
    Rigidbody rb;
    Animator anim;
    Vector3 destination;
    
    [SerializeField] float dashSpeed = 5;
    [SerializeField] float totalDashTime = 0.5f;

    [SerializeField] GameObject disappearFX;
    [SerializeField] GameObject appearFX;
    [SerializeField] GameObject mesh;
    [SerializeField] GameObject weapon;
    [SerializeField] GameObject trail;

    [SerializeField] Transform cam;

    float currentDashTime = 0f;
    
    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<FlowScriptController>();
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        if (rb == null)
        {
            rb = GetComponent<Rigidbody>();
            anim = GetComponent<Animator>();
        }
        if (rb)
        {
            float xinput = Input.GetAxis("Horizontal");
            float yinput = Input.GetAxis("Vertical");

            Vector3 inputDir = (new Vector3(cam.forward.x, 0, cam.forward.z) * yinput + new Vector3(cam.right.x, 0, cam.right.z) * xinput).normalized;

            if (inputDir.magnitude > 0)
            {
                rb.velocity = inputDir * dashSpeed;
                // transform.forward = inputDir;
            }
            else
            {
                rb.velocity = -1 * transform.forward * dashSpeed;
            }

            disappearFX.SetActive(true);
            appearFX.SetActive(false);
            mesh.SetActive(false);
            weapon.SetActive(false);
            trail.SetActive(true);
            anim.SetBool("Combo", false);
        }

        // Now we've totally evaded
    }
    private void OnDisable()
    {
        // reset dash timer when done
        currentDashTime = 0f;
        disappearFX.SetActive(false);
        appearFX.SetActive(true);
        mesh.SetActive(true);
        weapon.SetActive(true);
        // trail.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {
        currentDashTime += Time.deltaTime;
        if(currentDashTime >= totalDashTime || Input.GetKeyUp(KeyCode.LeftShift))
        {
            controller.switchState.Invoke("move");
            anim.Play("FlowMove");
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // controller.switchState.Invoke("move");
        }
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            // controller.switchState.Invoke("jump");
        }
    }
}
