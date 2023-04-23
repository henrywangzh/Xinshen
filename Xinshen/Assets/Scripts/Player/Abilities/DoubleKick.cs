using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleKick : MonoBehaviour
{
    Animator anim;
    PlayerAnimHandler animHandler;
    PlayerWeapon weapon;

    [SerializeField] int knockback = 20;
    [SerializeField] int stunFrames = 100;

    int kb;
    int stun;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        animHandler = GetComponent<PlayerAnimHandler>();
        weapon = animHandler.weaponCollider.gameObject.GetComponent<PlayerWeapon>();
    }

    private void OnEnable()
    {
        animHandler.AdjustSlashHitbox(1.2f);
        anim.Play("DoubleKick");
        kb = weapon.knockbackPower;
        stun = weapon.stunPower;
        weapon.knockbackPower = knockback;
        weapon.stunPower = stunFrames;
    }

    private void OnDisable()
    {
        animHandler.AdjustSlashHitbox();
        weapon.knockbackPower = kb;
        weapon.stunPower = stun;
    }
}
