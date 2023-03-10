using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimHandler : MonoBehaviour
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


    // Update is called once per frame
    void Update()
    {
        if (targOrientation != Vector3.zero)
        {
            transform.forward = Vector3.Lerp(transform.forward, targOrientation, 0.2f);
        }

        if (Vector3.Distance(transform.forward, targOrientation) < 0.2f)
        {
            targOrientation = Vector3.zero;
        }
    }

    public void StartSwing()
    {
        weaponCollider.enabled = true;
        weapon.SetPSEmission(true);
    }

    public void EndSwing()
    {
        weaponCollider.enabled = false;
        weapon.SetPSEmission(false);
    }

    public void EndAttack()
    {
        if (this.isActiveAndEnabled)
            controller.switchState.Invoke("move");
    }

    public void OrientTowardsInput()
    {
        if (GlobalVariableManager.LockedTarget != null)
        {
            targOrientation = GlobalVariableManager.LockedTarget.position - transform.position;
        }
        else
        {
            float inputX = Input.GetAxis("Horizontal");
            float inputY = Input.GetAxis("Vertical");

            targOrientation = (cam.forward * inputY + cam.right * inputX);
        }
        targOrientation = (targOrientation - new Vector3(0, targOrientation.y, 0)).normalized;
    }
}
