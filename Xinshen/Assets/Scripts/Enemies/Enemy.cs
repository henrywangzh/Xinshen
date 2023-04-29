using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Enemy : MonoBehaviour
{
    [SerializeField] int maxHP = 100, hp;
    [SerializeField] float centerYOffset; //used to instantiate on-hit effects at object's center
    [SerializeField] public int damage = 20;
    [SerializeField] public int stunMeterMax = 100;
    [SerializeField] public int poise = 20;
    [SerializeField] public int stunDuration = 200;
    [SerializeField] int stunned;
    [SerializeField] int stunMeter = 0;

    static GameObject slashFXObj;
    protected UnityEvent onStun;
    protected UnityEvent onAtkInterrupt;

    Transform trfm;
    EnemyCamp enemyCamp;
    Rigidbody _rb;


    protected virtual void Start()
    {
        trfm = transform;
        _rb = GetComponent<Rigidbody>();
        hp = maxHP;
        onStun = new UnityEvent();
        onAtkInterrupt = new UnityEvent();

        InvokeRepeating("InvokedFixedUpdate", .02f, .02f);
    }

    public int GetStunMeter()
    {
        return stunMeter;
    }

    public virtual bool TakeDamage(int dmg)
    {
        return TakeDamage(dmg, true, true, 5);
    }

    // Can assign negative number to heal
    public virtual bool TakeDamage(int dmg, bool doSlashFX = true, bool doDamageNumber = true, int trauma = 5)
    {
        if (doSlashFX) { Instantiate(slashFXObj, transform.position + Vector3.up * centerYOffset, transform.rotation); }
        if (doDamageNumber) { GameManager.InstantiateDamageNumber(transform.position + Vector3.up * centerYOffset, dmg, GameManager.BLUE); }

        stunMeter += trauma;
        if (stunMeter > stunMeterMax)
        {
            bool prevStun = stunned > 0;
            stunMeter = 0;
            stunned = stunDuration;
            if (!prevStun)
                onStun.Invoke();
        } 
        if (!IsStunned() && trauma > poise)
        {
            onAtkInterrupt.Invoke(); // Invoke attack interrupt
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
            return true;
        }
        return false;
    }

    public abstract void Die();

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
        _rb.velocity = (trfm.position - source).normalized * power;
        bool alreadyStunned = stunned > 0;
        if (stunFrames < 0)
        {
            stunned = 0;
        }
        else if (stunFrames > 10)   
        {
            stunned = stunDuration;
        }
            
        if (stunned > 0 && !alreadyStunned)
        {
            onStun.Invoke();
        }
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
