using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiscordAttack : MonoBehaviour
{
    Animator anim;
    ActualDiscordScriptController controller;
    Rigidbody rb;
    [SerializeField] Collider weaponCollider;


    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        controller = GetComponent<ActualDiscordScriptController>();
        rb = GetComponent<Rigidbody>();
    }

  /*  private void OnEnable()
    {
        if (anim == null)
            anim = GetComponent<Animator>();
        //SetCombo(1);
        if (rb != null)
            rb.velocity = Vector3.zero;
        GlobalVariableManager.Damage = 25;
    }*/

    // Update is called once per frame
    void Update()
    {
        /*if (Input.GetMouseButtonDown(0))
        {
            //SetCombo(1);

        }*/

        if (Input.GetMouseButtonDown(0))
        {
            //isAttacking1 = true;
            //anim.SetBool("DiscordAttack", true);
            DiscordCombo(1);

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

    public void DiscordCombo(int attack)
    {
        anim.SetBool("DiscordAttack", attack >= 1);
    }
    public void EndDiscordAttack()
    {
        anim.SetBool("DiscordAttack", false);
        weaponCollider.enabled = false;
    }
    public void EndAttack()
    {
        if (this.isActiveAndEnabled)
            controller.switchState.Invoke("move");
    }
}

