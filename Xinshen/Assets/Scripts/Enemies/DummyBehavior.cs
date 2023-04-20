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

    public override bool TakeDamage(int dmg)
    {
        bool retVal = base.TakeDamage(dmg);
        ps.Play();
        return retVal;
    }

    public override void Die()
    {
        Debug.Log("ahhhh im dying");
        Destroy(gameObject);
    }
}
