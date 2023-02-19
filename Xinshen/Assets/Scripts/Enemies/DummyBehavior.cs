using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyBehavior : Enemy
{
    [SerializeField] Collider sword;

    // Start is called before the first frame update
    void Start()
    {
        
    }

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

    public override void Die()
    {
        Debug.Log("ahhhh im dying");
        Destroy(gameObject);
    }
}
