using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyBehavior : Enemy
{
    [SerializeField] Collider sword;
    [SerializeField] ParticleSystem ps;
    Animator anim;

    bool swinging = false;

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
        poise = 20;  // Poise recovery
    }

    IEnumerator FallAndRecover()
    {
        anim.Play("DummyDown");
        yield return new WaitUntil(() => !IsStunned());
        anim.Play("DummyGetup");
    }

    IEnumerator Swing()
    {
        poise = 10;
        anim.Play("DummyAtk");
        swinging = true;
        yield return new WaitForSeconds(0.1f);
        poise = 50;
        yield return new WaitUntil(() => !swinging);
        poise = 20;
    }

    public void StartAttack()
    {
        sword.enabled = true;
    }

    public void EndAttack()
    {
        sword.enabled = false;
        swinging = false;
    }

    public override void Die()
    {
        Debug.Log("ahhhh im dying");
        Destroy(gameObject);
    }
}
