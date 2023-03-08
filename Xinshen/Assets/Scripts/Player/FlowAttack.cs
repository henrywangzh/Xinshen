using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowAttack : MonoBehaviour
{
    Animator anim;
    FlowScriptController controller;
    Rigidbody rb;
    [SerializeField] Collider weaponCollider;
    bool canCancel = false;
    Transform cam;
    Vector3 targOrientation;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        controller = GetComponent<FlowScriptController>();
        rb = GetComponent<Rigidbody>();
        cam = GetComponent<FlowMove>().cam;
        targOrientation = Vector3.zero;
    }

    private void OnEnable()
    {
        if (anim == null)
            anim = GetComponent<Animator>();
        SetCombo(1);
        if (rb != null)
            rb.velocity = Vector3.zero;
        targOrientation = Vector3.zero;
        GlobalVariableManager.Damage = 25;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SetCombo(1);
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            Debug.Log("Dodging");
            DodgeCancel();
        }

        if (targOrientation != Vector3.zero)
        {
            transform.forward = Vector3.Lerp(transform.forward, targOrientation, 0.2f);
        }
    }

    public void StartSwing()
    {
        weaponCollider.enabled = true;
    }

    public void EndSwing()
    {
        weaponCollider.enabled = false;
    }

    public void SetCombo(int combo)
    {
        anim.SetBool("Combo", combo >= 1);
    }

    public void SetFwdVelocity(float vel)
    {
        rb.velocity = transform.forward * vel;
    }

    public void EndAttack()
    {
        if (this.isActiveAndEnabled)
            controller.switchState.Invoke("move"); 
    }

    void DodgeCancel()
    {
        EndSwing();
        SetFwdVelocity(0);
        anim.Play("FlowEvade");
        controller.switchState.Invoke("evade");
    }

    public void OrientTowardsInput()
    {
        float inputX = Input.GetAxis("Horizontal");
        float inputY = Input.GetAxis("Vertical");

        targOrientation = (cam.forward * inputY + cam.right * inputX).normalized;
    }
}
