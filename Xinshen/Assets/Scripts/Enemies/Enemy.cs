using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    [SerializeField] int maxHP = 100;
    [SerializeField] int hp;
    [SerializeField] public int damage = 20;

    private void Start()
    {
        hp = maxHP;
    }

    // Can assign negative number to heal
    public virtual void TakeDamage(int dmg)
    {
        hp -= dmg;
        if (hp > maxHP)
        {
            hp = maxHP;
        }
        if (hp <= 0)
        {
            hp = 0;
            try
            {
                Die();
            } catch
            {
                Debug.LogError("Abstract function Die() is not given an override! Please define an override function for Die() in the class inheriting from Enemy");
            }
            
        }
    }

    public abstract void Die();
}
