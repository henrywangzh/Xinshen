using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    [SerializeField] int maxHP = 100;
    [SerializeField] int hp;
    [SerializeField] public int damage = 20;

    static GameObject slashFXObj;

    Transform trfm;

    private void Start()
    {
        trfm = transform;

        hp = maxHP;
    }

    // Can assign negative number to heal
    public virtual void TakeDamage(int dmg)
    {
        TakeDamage(dmg, true);
    }

    public virtual void TakeDamage(int dmg, bool doSlashFX = true)
    {
        if (doSlashFX)
        {
            Instantiate(slashFXObj, transform.position + Vector3.up * 1, transform.rotation);
        }

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


    public static void AssignSlashFXObj(GameObject obj)
    {
        slashFXObj = obj;
    }
}
