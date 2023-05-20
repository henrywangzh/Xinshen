using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeterminationAttack : MonoBehaviour
{
    Animator anim;
    DeterminationScriptController controller;
    Rigidbody rb;
    [SerializeField] [Tooltip("For debugging purposes only! Sets the locked target to this.")] Transform debugTarget;
    //[SerializeField] Collider weaponCollider;

    int comboCount = 1;  // Corresponds with animation state name
    int maxCombo = 3;  // Highest animation number we have 
    [SerializeField] bool comboReady = false;

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
            anim = GetComponent<Animator>();
        comboCount = 1;
        anim.Play("DeterminationAtkc" + comboCount);
        if (rb != null)
            rb.velocity = Vector3.zero;
        GlobalVariableManager.Damage = 30;
        AlignToTarget();
        comboReady = false;
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
        if (Input.GetMouseButtonUp(0))
        {
            anim.SetTrigger("DeterminationAttack");
        }
    }

    public void DetReadyCombo()
    {
        Debug.Log("Calling");
        comboReady = true;
    }

}
