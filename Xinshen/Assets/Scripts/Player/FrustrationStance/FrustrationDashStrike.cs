using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrustrationDashStrike : MonoBehaviour
{
    [SerializeField] Collider weaponCollider;
    [SerializeField] float dashRange = 10f;
    [SerializeField] float dashSpeed = 5f;
    Rigidbody rb;

    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
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

    private void OnEnable()
    {
        FrustChargeAtk();
    }

    public void FrustChargeAtk()
    {
        // If locked on, dash towards target and perform an attack. If not, dash forward and perform an attack
        StartCoroutine(ChargeAtk());
    }

    IEnumerator ChargeAtk()
    {
        Vector3 targetPos;
        if (GlobalVariableManager.LockedTarget != null)
        {
            Debug.Log("Starting");
            AlignToTarget();
            if (Vector3.Distance(transform.position, GlobalVariableManager.LockedTarget.position) < dashRange)
            {
                targetPos = GlobalVariableManager.LockedTarget.position;
            }
            else
            {
                Vector3 fwd = GlobalVariableManager.LockedTarget.position - transform.position;
                fwd = (fwd - new Vector3(0, fwd.y, 0)).normalized;
                targetPos = transform.position + fwd * dashRange;
            }
            Debug.Log(targetPos);
        }
        else
        {
            targetPos = transform.position + transform.forward * dashRange;
        }
        rb.velocity = transform.forward * dashSpeed;
        yield return new WaitUntil(() => Vector3.Distance(transform.position, targetPos) <= 0.2f);
        rb.velocity = Vector3.zero;
        weaponCollider.enabled = true;
        weaponCollider.enabled = false;
        weaponCollider.enabled = true;
        weaponCollider.enabled = false;
    }

}
