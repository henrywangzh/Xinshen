using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimHandler : MonoBehaviour
{
    Animator anim;
    FlowScriptController controller;
    Rigidbody rb;
    [SerializeField] public BoxCollider weaponCollider;
    [SerializeField] ParticleSystem leftLegTrail;
    [SerializeField] ParticleSystem rightLegTrail;
    public PlayerWeapon weapon;
    bool canCancel = false;
    bool lockPhysics = false;
    Vector3 commandVelocity;
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
        commandVelocity = Vector3.zero;
        originalHbWidth = weaponCollider.size.x;
        originalHbHeight = weaponCollider.size.y;
        originalHbLength = weaponCollider.size.z;
        originalZOffset = weaponCollider.center.z;
        Debug.Log(originalHbHeight);
    }

    public void PlayFootstep()
    {
        if (AudioManager.audioManager != null && GlobalVariableManager.OnGround)
        {
            AudioManager.audioManager.playSound("RunningOnGrass");
            // FOR MING: only play sound if we are moving and if we are on the ground
            //if (moveDirection.magnitude >= 0.2 && onGround)
            //{
            //    AudioManager.audioManager.playRepeatedSound("WalkingOnGrass");
            //}
            //else
            //{
            //    AudioManager.audioManager.stopRepeatedSound();
            //}
        }
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

        if (lockPhysics)
        {
            rb.velocity = commandVelocity;
        }
    }
    
    public void LockPhysics(bool locked)
    {
        lockPhysics = locked;
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
        if(AudioManager.audioManager != null)
        {
            AudioManager.audioManager.playSound("OneSlashNoHit");
        }
        weaponCollider.enabled = true;
        weapon.SetPSEmission(true);
    }

    public void EndSwing()
    {
        weaponCollider.enabled = false;
        weapon.SetPSEmission(false);
    }

    #region FlowDualWield

    int baseDmg = 0;

    public void SwingFlowDagger(int enable)
    {
        if (enable == 1)
        {
            baseDmg = GlobalVariableManager.Damage;
            GlobalVariableManager.Damage = baseDmg;
        }
        {
            GlobalVariableManager.Damage = baseDmg;
        }
        weapon.ToggleFlowSlash(1, 0, enable == 1);
        weaponCollider.enabled = enable == 1;
    }

    public void SwingFlowSpear(int enable)
    {
        if (enable == 1)
        {
            baseDmg = GlobalVariableManager.Damage;
            GlobalVariableManager.Damage = baseDmg * 2;
        }
        {
            GlobalVariableManager.Damage = baseDmg;
        }
        weapon.ToggleFlowSlash(0, 1, enable == 1);
        weaponCollider.enabled = enable == 1;
    }

    public void SwingFlowSword(int enable)
    {
        if (enable == 1)
        {
            baseDmg = GlobalVariableManager.Damage;
            GlobalVariableManager.Damage = (int) (baseDmg * 1.2f);
        }
        {
            GlobalVariableManager.Damage = baseDmg;
        }
        weapon.ToggleFlowSlash(0, 0, enable == 1);
        weaponCollider.enabled = enable == 1;
    }

    #endregion

    public void DeterminationSwordGuard(bool guard)
    {
        weapon.ToggleDeterminationGuard(guard);
    }

    // 0 - left, 1 - right, 2 - both
    public void StartKick(int leg)
    {
        weaponCollider.enabled = true;
        switch (leg)
        {
            case 0:
                leftLegTrail.Emit(10);
                break;
            case 1:
                rightLegTrail.Emit(10);
                break;
            case 2:
                leftLegTrail.Emit(10);
                rightLegTrail.Emit(10);
                break;
        }
    }

    // 0 - left, 1 - right, 2 - both
    public void EndKick(int leg)
    {
        weaponCollider.enabled = false;
    }

    public void SetFwdVelocity(float vel)
    {
        rb.velocity = transform.forward * vel;
        commandVelocity = transform.forward * vel;
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
