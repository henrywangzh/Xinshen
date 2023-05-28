using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    [SerializeField] int _knockbackPower;
    [SerializeField] int _stunPower;
    [SerializeField] int _traumaPower;
    public int knockbackPower
    {
        get { return _knockbackPower; }
        set
        {
            if (value >= 0)
                _knockbackPower = value;
        }
    }
    public int stunPower
    {
        get { return _stunPower; }
        set
        {
            if (value >= 0)
                _stunPower = value;
        }
    }

    public int traumaPower
    {
        get { return _traumaPower; }
        set
        {
            if (value >= 0)
                _traumaPower = value;
        }
    }

    [SerializeField] ParticleSystem ps;
    [SerializeField] GameObject flowSword;
    [SerializeField] GameObject determinationSword;
    SwordFragController determinationWeapon;

    ParticleSystem.EmissionModule emitter;

    // Start is called before the first frame update
    void Start()
    {
        emitter = ps.emission;
        SetPSEmission(false);
        determinationWeapon = determinationSword.GetComponent<SwordFragController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ToggleDeterminationGuard(bool on)
    {
        determinationWeapon.ToggleGuard(on);
    }

    public void SetPSEmission(bool start)
    {
        if (start)
        {
            // emitter.enabled = true;
            ps.Emit(8);
        }
        else
        {
            // emitter.enabled = false;
            // ps.Stop(true, ParticleSystemStopBehavior.StopEmitting);
            // Debug.Log("Attempting stop");
        }
            
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Enemy>() != null)
        {
            if (other.gameObject.GetComponent<Enemy>().TakeDamage(GlobalVariableManager.Damage, true, true, _traumaPower)){
                // heal player to full if enemy is killed in frenzy mode
                if (GlobalVariableManager.FrenzyMode){
                    PlayerHP.Heal();
                }
            }
            else {
                other.gameObject.GetComponent<Enemy>().TakeKnockback(PlayerHP.torsoTrfm.position + PlayerHP.torsoTrfm.forward * -2, _knockbackPower, _stunPower);
            }
        }
    }
}
