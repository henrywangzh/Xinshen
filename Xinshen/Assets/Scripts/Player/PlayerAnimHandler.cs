using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimHandler : MonoBehaviour
{
    Animator anim;
    FlowScriptController controller;
    Rigidbody rb;
    [SerializeField] BoxCollider weaponCollider;
    PlayerWeapon weapon;
    bool canCancel = false;
    Transform cam;
    Vector3 targOrientation;

    float originalHbWidth;
    float originalHbLength;
    float originalHbHeight;
    float originalZOffset;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        controller = GetComponent<FlowScriptController>();
        rb = GetComponent<Rigidbody>();
        cam = GetComponent<FlowMove>().cam;
        weapon = weaponCollider.gameObject.GetComponent<PlayerWeapon>();
        targOrientation = Vector3.zero;
        originalHbWidth = weaponCollider.size.x;
        originalHbHeight = weaponCollider.size.y;
        originalHbLength = weaponCollider.size.z;
        originalZOffset = weaponCollider.center.z;
        Debug.Log(originalHbHeight);
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

    // Adjusts the dimensions of the hitbox as a percent of the original. Hitbox will always be adjusted to be centered in front of player.
    public void AdjustSlashHitbox(float length = 1f, float width = 1f, float height = 1f)
    {
        float newLen = length * originalHbLength;
        // Z offset = size diff / 2
        float newZ = (newLen - originalHbLength) / 2 + originalZOffset;
        float newWidth = width * originalHbWidth;
        float newHeight = height * originalHbHeight;

        weaponCollider.center = new Vector3(0, 0, newZ);
        weaponCollider.size = new Vector3(newWidth, newHeight, newLen);
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

    public void SetFwdVelocity(float vel)
    {
        rb.velocity = transform.forward * vel;
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
