using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowCrossSlash : MonoBehaviour
{
    Animator anim;
    Rigidbody rb;
    PlayerAnimHandler animhandler;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        animhandler = GetComponent<PlayerAnimHandler>();
    }

    private void OnEnable()
    {
        if (anim == null)
        {
            anim = GetComponent<Animator>();
            rb = GetComponent<Rigidbody>();
            animhandler = GetComponent<PlayerAnimHandler>();
        }
        animhandler.AdjustSlashHitbox(1.5f);
        anim.Play("FlowCrossSlash");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDisable()
    {
        animhandler.AdjustSlashHitbox();
    }
}
