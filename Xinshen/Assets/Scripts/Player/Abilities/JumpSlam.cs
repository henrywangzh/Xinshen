using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpSlam : MonoBehaviour
{
    
    Animator anim;
    Rigidbody rb;
    private PlayerAnimHandler animhandler;

    [SerializeField] float jumpDistance = 1f;     // the distance at which the character moves forward
    [SerializeField] float jumpHeight = 1f;      // the height at which the character jumps
    Vector3 jump;

    // Start is called before the first frame update
    void Start()
    {

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
        anim.Play("JumpSlam");
        
        
    }

    private void LeapSlamJump() {
        jump = transform.forward * jumpDistance;
        jump.y += jumpHeight;
        rb.velocity = jump;
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
