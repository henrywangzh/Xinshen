using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] int damage;
    [SerializeField] Collider hitbox;
    int timer;

    public void EnableHitbox(int duration = 0)
    {
        hitbox.enabled = true;
        if (timer < duration) { timer = duration; }
    }

    public void DisableHitbox()
    {
        hitbox.enabled = false;
        timer = 0;
    }

    private void FixedUpdate()
    {
        if (timer > 0)
        {
            if (timer == 1) { hitbox.enabled = false; }
            timer--;
        }
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.layer == 8)
        {
            PlayerHP.TakeDamage(damage);
        }
    }
}
