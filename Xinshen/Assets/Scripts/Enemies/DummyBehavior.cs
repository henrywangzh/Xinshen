using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyBehavior : Enemy
{
    [SerializeField] Collider sword;
    [SerializeField] ParticleSystem ps;
    Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    protected override void Start()
    {
        base.Start();
        anim = GetComponent<Animator>();
        onAtkInterrupt.AddListener(HandleInterrupt);
        onStun.AddListener(HandleStun);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void HandleStun()
    {
        // Implement a longer stun
        StartCoroutine(FallAndRecover());
    }

    void HandleInterrupt()
    {
        // Interrupt the current attack (for this just cut to animation)
        anim.Play("DummyImpact");
    }

    IEnumerator FallAndRecover()
    {
        anim.Play("DummyDown");
        yield return new WaitUntil(() => !IsStunned());
        anim.Play("DummyGetup");
    }

    public void StartAttack()
    {
        sword.enabled = true;
    }

    public void EndAttack()
    {
        sword.enabled = false;
    }

    public override void Die()
    {
        Debug.Log("ahhhh im dying");
        Destroy(gameObject);
    }
}
