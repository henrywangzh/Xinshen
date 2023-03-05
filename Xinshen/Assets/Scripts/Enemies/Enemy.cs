using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    [SerializeField] int maxHP = 100, hp;
    [SerializeField] float centerYOffset; //used to instantiate on-hit effects at object's center
    [SerializeField] public int damage = 20;

    static GameObject slashFXObj;

    Transform trfm;

    private void Start()
    {
        trfm = transform;

        hp = maxHP;

        InvokeRepeating("InvokedFixedUpdate", .02f, .02f);
    }

    // Can assign negative number to heal
    public virtual void TakeDamage(int dmg)
    {
        TakeDamage(dmg, true, true);
    }

    public virtual void TakeDamage(int dmg, bool doSlashFX = true, bool doDamageNumber = true)
    {
        if (doSlashFX) { Instantiate(slashFXObj, transform.position + Vector3.up * centerYOffset, transform.rotation); }
        if (doDamageNumber) { GameManager.InstantiateDamageNumber(transform.position + Vector3.up * centerYOffset, dmg); }

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

    int stunned;
    protected bool IsStunned()
    {
        return stunned > 0;
    }

    public static void AssignSlashFXObj(GameObject obj)
    {
        slashFXObj = obj;
    }

    //source: where the knockback is coming from (flies away from source); power: how much to knockback
    public void TakeKnockback(Vector3 source, float power)
    {
        GetComponent<Rigidbody>().velocity = (trfm.position - source).normalized * power;
        stunned = (int)(power * 2);
    }

    public void InvokedFixedUpdate()
    {
        if (stunned > 0)
        {
            stunned--;
        }
    }
}
