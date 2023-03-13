using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowAttack : MonoBehaviour
{
    Animator anim;
    FlowScriptController controller;
    Rigidbody rb;
    [SerializeField] Collider weaponCollider;
    PlayerWeapon weapon;
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
        weapon = weaponCollider.gameObject.GetComponent<PlayerWeapon>();
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

        if (Input.GetKeyDown(KeyCode.R))
        {
            CrossSlash();
        }

        //if (targOrientation != Vector3.zero)
        //{
        //    transform.forward = Vector3.Lerp(transform.forward, targOrientation, 0.2f);
        //}
    }

    /*
    void StartSwing()
    {
        weaponCollider.enabled = true;
        weapon.SetPSEmission(true);
    }
    void EndSwing()
    {
        weaponCollider.enabled = false;
        weapon.SetPSEmission(false);
    }
    */

    public void SetCombo(int combo)
    {
        anim.SetBool("Combo", combo >= 1);
    }

    public void EndAttack()
    {
        if (this.isActiveAndEnabled)
            controller.switchState.Invoke("move"); 
    }

    void SetVelocityFwd(float vel)
    {
        rb.velocity = transform.forward * vel;
    }

    void Cleanup()
    {
        weaponCollider.enabled = false;
        weapon.SetPSEmission(false);
        anim.SetBool("Combo", false);
        SetVelocityFwd(0);
    }

    void DodgeCancel()
    {
        Cleanup();
        anim.Play("FlowEvade");
        controller.switchState.Invoke("evade");
    }

    void CrossSlash()
    {
        Cleanup();
        anim.Play("FlowCrossSlash");
        controller.switchState.Invoke("cross");
    }

    //public void OrientTowardsInput()
    //{
    //    if (GlobalVariableManager.LockedTarget != null)
    //    {
    //        targOrientation = transform.position - GlobalVariableManager.LockedTarget.position;
    //    }
    //    else
    //    {
    //        float inputX = Input.GetAxis("Horizontal");
    //        float inputY = Input.GetAxis("Vertical");

    //        targOrientation = (cam.forward * inputY + cam.right * inputX);
    //    }
    //    targOrientation = (targOrientation - new Vector3(0, targOrientation.y, 0)).normalized;
    //}
}
