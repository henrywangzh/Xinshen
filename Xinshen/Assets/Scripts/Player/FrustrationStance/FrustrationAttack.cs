using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrustrationAttack : MonoBehaviour
{
    Animator anim;
    FrustrationScriptController controller;
    Rigidbody rb;
    [SerializeField] [Tooltip("For debugging purposes only! Sets the locked target to this.")] Transform debugTarget;
    //[SerializeField] Collider weaponCollider;
    [SerializeField] GameObject[] afterimages;

    int comboCount = 1;  // Corresponds with animation state name
    int maxCombo = 8;  // Highest animation number we have 
    bool comboReady = false;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        controller = GetComponent<FrustrationScriptController>();
        rb = GetComponent<Rigidbody>();
        GlobalVariableManager.LockedTarget = debugTarget;
    }
    private void OnEnable()
    {
        if (anim == null)
            anim = GetComponent<Animator>();
        comboCount = 1;
        anim.Play("FrustrationAtk" + comboCount);
        if (rb != null)
            rb.velocity = Vector3.zero;
        GlobalVariableManager.Damage = 10;
        AlignToTarget();
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
                anim.Play("FrustrationAtk" + comboCount);
                afterimages[comboCount - 1].SetActive(true);
            }
            comboReady = false;
        }
    }

    public void ReadyCombo()
    {
        comboReady = true;
    }
    /*
    public void StartSwing()
    {
        weaponCollider.enabled = true;
    }

    public void EndSwing()
    {
        weaponCollider.enabled = false;
    }
    */
    public void SetFwdVelocityFrust(float vel)
    {
        Debug.Log("Setting velocity");
        rb.velocity = transform.forward * vel;
    }

    public void EndAttack()
    {
        if (this.isActiveAndEnabled)
            controller.switchState.Invoke("move");
    }
}
