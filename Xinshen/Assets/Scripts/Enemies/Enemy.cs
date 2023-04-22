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

    EnemyCamp enemyCamp;

    protected void Start()
    {
        trfm = transform;

        hp = maxHP;

        InvokeRepeating("InvokedFixedUpdate", .02f, .02f);
    }

    // Can assign negative number to heal
    public virtual bool TakeDamage(int dmg)
    {
        return TakeDamage(dmg, true, true);
    }

    public virtual bool TakeDamage(int dmg, bool doSlashFX = true, bool doDamageNumber = true)
    {
        if (doSlashFX) { Instantiate(slashFXObj, transform.position + Vector3.up * centerYOffset, transform.rotation); }
        if (doDamageNumber) { GameManager.InstantiateDamageNumber(transform.position + Vector3.up * centerYOffset, dmg, GameManager.BLUE); }

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
            return true;
        }
        return false;
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
    public void TakeKnockback(Vector3 source, float power, int stunFrames = -1)
    {
        GetComponent<Rigidbody>().velocity = (trfm.position - source).normalized * power;
        if (stunFrames < 0)
            stunned = (int)(power * 2);
        else
            stunned = stunFrames;
    }

    public void InvokedFixedUpdate()
    {
        if (stunned > 0)
        {
            stunned--;
        }
    }

    public void AssignToCamp(EnemyCamp enemyCamp)
    {
        this.enemyCamp = enemyCamp;
    }

    public void OnDestroy()
    {
        if(enemyCamp != null)
        {
            enemyCamp.removeEnemy(this);
        }
    }
}
