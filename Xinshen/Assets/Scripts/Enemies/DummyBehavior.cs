using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyBehavior : Enemy
{
    [SerializeField] Collider sword;
    [SerializeField] ParticleSystem ps;

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartAttack()
    {
        sword.enabled = true;
    }

    public void EndAttack()
    {
        sword.enabled = false;
    }

    public override void TakeDamage(int dmg)
    {
        base.TakeDamage(dmg);
        ps.Play();
    }

    public override void Die()
    {
        Debug.Log("ahhhh im dying");
        Destroy(gameObject);
    }
}
