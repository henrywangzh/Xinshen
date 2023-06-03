using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeterminationAttack : MonoBehaviour
{
    Animator anim;
    PlayerAnimHandler animHandler;
    DeterminationScriptController controller;
    Rigidbody rb;
    [SerializeField] [Tooltip("For debugging purposes only! Sets the locked target to this.")] Transform debugTarget;
    //[SerializeField] Collider weaponCollider;

    int comboCount = 1;  // Corresponds with animation state name
    int maxCombo = 3;  // Highest animation number we have 
    [SerializeField] bool comboReady = false;
    [SerializeField] float chargeInterval = 0.8f;
    float heldTime = 0;
    int chargeLevel = 0;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        controller = GetComponent<DeterminationScriptController>();
        rb = GetComponent<Rigidbody>();
        GlobalVariableManager.LockedTarget = debugTarget;
    }
    private void OnEnable()
    {
        if (anim == null)
        {
            anim = GetComponent<Animator>(); 
            animHandler = GetComponent<PlayerAnimHandler>();
        }
        comboCount = 1;
        anim.Play("DeterminationAtkc" + comboCount);
        if (rb != null)
            rb.velocity = Vector3.zero;
        GlobalVariableManager.Damage = 10;
        AlignToTarget();
        comboReady = false;
        animHandler.LockPhysics(true);
        heldTime = 0;
        chargeLevel = 0;
    }

    void AlignToTarget()
    {
        if (GlobalVariableManager.LockedTarget != null)
        {
            Vector3 fwd = GlobalVariableManager.LockedTarget.position - transform.position;
            fwd = (fwd - new Vector3(0, fwd.y, 0)).normalized;
            transform.forward = fwd;
        }
    }

    // Update is called once per frame
    void Update()
    {
        heldTime += Time.deltaTime; 
        if (Input.GetMouseButtonDown(0) && comboReady)
        {
            comboCount++;
            if (comboCount <= maxCombo)
            {
                AlignToTarget();
                anim.Play("DeterminationAtkc" + comboCount);
            }
            comboReady = false;
        }
        if (heldTime > chargeInterval)
        {
            heldTime = 0;
            if (chargeLevel < 2)
            {
                ++chargeLevel;
                GlobalVariableManager.Damage = 10 + chargeLevel * 10;
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            anim.SetTrigger("DeterminationAttack"); 
            
        }
    }

    public void ChargeForward()
    {
        float vel = 5f + chargeLevel * 15f;
        animHandler.SetFwdVelocity(vel);
    }

    public void DetReadyCombo()
    {
        Debug.Log("Calling");
        comboReady = true;
        chargeLevel = 0;
        heldTime = 0;
        GlobalVariableManager.Damage = 10;
    }

    private void OnDisable()
    {
        animHandler.LockPhysics(false);
    }

}
