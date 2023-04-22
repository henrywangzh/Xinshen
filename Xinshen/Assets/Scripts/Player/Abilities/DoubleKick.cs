using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleKick : MonoBehaviour
{
    Animator anim;
    PlayerAnimHandler animHandler;

    private void OnEnable()
    {
        if (anim == null)
        {
            anim = GetComponent<Animator>();
            animHandler = GetComponent<PlayerAnimHandler>();
        }
        animHandler.AdjustSlashHitbox(1.2f);
        anim.Play("DoubleKick");
    }

    private void OnDisable()
    {
        animHandler.AdjustSlashHitbox();
    }
}
