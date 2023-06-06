using System.Collections;
using System.Collections.Generic;
//using Unity.VisualScripting.ReorderableList;
using UnityEngine;

public class DiscordAttack : MonoBehaviour
{
    Animator anim;
    PlayerAnimHandler animHandler;
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

    private void OnEnable()
    {
        if (anim == null)
        {
            anim = GetComponent<Animator>();
            animHandler = GetComponent<PlayerAnimHandler>();
        }
        //SetCombo(1);
        if (rb != null)
              rb.velocity = Vector3.zero;
        //GlobalVariableManager.Damage = 25;
        animHandler.LockPhysics(true);
        DiscordCombo(1);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {

            DiscordCombo(1);
        }
        //if (Input.GetMouseButtonDown(0))
        //{
        //isAttacking1 = true;
        //anim.SetBool("DiscordAttack", true);

        //DiscordCombo(1);

        //}
    }
 
    public void DiscordCombo(int attack)
    {
        anim.SetBool("DiscordAttack", attack >= 1);
    }

    public void EndDiscordAttack()
    {
        anim.SetBool("DiscordAttack", false);
        //weaponCollider.enabled = false;
        controller.switchState.Invoke("discordMove");
    }

    public void EndAttack()
    {
        if (this.isActiveAndEnabled)
            controller.switchState.Invoke("discordMove");
    }

    public float DiscordAttackMoveSpeed = 5f;
    // Move player after attack
    // Function triggered by the animation event
    public void UpdateDiscordPlayerPosition()
    {
        // Update player's position based on forward movement
        transform.position += transform.forward * DiscordAttackMoveSpeed * Time.deltaTime;
    }

    private void OnDisable()
    {
        animHandler.LockPhysics(false);
    }
}

