using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrustrationEvade : MonoBehaviour
{
    FrustrationScriptController controller;
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
        controller = GetComponent<FrustrationScriptController>();
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        if (rb == null)
            rb = GetComponent<Rigidbody>();
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
            weapon.SetActive(false);
            trail.SetActive(true);
            anim.SetBool("isSliding", true);
        }

        // Now we've totally evaded
    }
    private void OnDisable()
    {
        // reset dash timer when done
        currentDashTime = 0f;
        disappearFX.SetActive(false);
        appearFX.SetActive(true);
        weapon.SetActive(true);
        // trail.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {
        currentDashTime += Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            anim.SetBool("isSliding", false);
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

