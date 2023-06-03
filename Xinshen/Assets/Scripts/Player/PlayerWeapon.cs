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
    [SerializeField] GameObject discordSword;
    [SerializeField] GameObject determinationSword;

    SwordFragController determinationWeapon;

    // Flow shenanigans 
    [SerializeField] GameObject lFlowDagger;  // Reverse gripped, FYI
    [SerializeField] ParticleSystem daggerTrail;
    ParticleSystem daggerAppearfx;
    // [SerializeField] GameObject lFlowSword;
    [SerializeField] GameObject rFlowSword;  // Normally enabled
    ParticleSystem swordAppearfx;
    [SerializeField] GameObject rFlowSpear;
    [SerializeField] ParticleSystem spearTrail;
    ParticleSystem spearAppearfx;

    ParticleSystem.EmissionModule emitter;

    // Start is called before the first frame update
    void Start()
    {
        emitter = ps.emission;
        SetPSEmission(false);
        determinationWeapon = determinationSword.GetComponent<SwordFragController>();
        daggerAppearfx = lFlowDagger.GetComponent<ParticleSystem>();
        swordAppearfx = rFlowSword.GetComponent<ParticleSystem>();
        spearAppearfx = rFlowSpear.GetComponent<ParticleSystem>(); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ToggleWeapon(StancesScriptController.Stance stance)
    {
        switch (stance)
        {
            case StancesScriptController.Stance.discord:
                discordSword.SetActive(true);
                determinationSword.SetActive(false);
                // rFlowSword.SetActive(false);
                break;
            case StancesScriptController.Stance.determination:
                discordSword.SetActive(false);
                determinationSword.SetActive(true);
                break;
            case StancesScriptController.Stance.frustration:
                discordSword.SetActive(false);
                determinationSword.SetActive(false);
                break;
            case StancesScriptController.Stance.flow:
                discordSword.SetActive(false);
                determinationSword.SetActive(false);
                break;
        }
    }

    public void ToggleFlowSlash(int left = 0, int weaponType = 0, bool enable = true)
    {
        /*
         * (0, 0) - right, sword
         * (0, 1) - right, spear
         * (1, 0) - left, dagger
         * (1, 1) - left, sword (No sword for now)
         */
        if (left > 0)
        {
            if (enable)
            {
                // lFlowDagger.enabled = true;
                lFlowDagger.SetActive(true);
                daggerAppearfx.Play();
                daggerTrail.Emit(8);
            } else
            {
                // daggerAppearfx.Play();
                lFlowDagger.SetActive(false);
            }
        } else
        {
            if (weaponType == 0)
            {
                if (enable)
                {
                    rFlowSword.SetActive(true);
                    swordAppearfx.Play();
                    ps.Emit(8);
                }
                else
                {
                    // swordAppearfx.Play();
                    rFlowSword.SetActive(false);
                }
            } else
            {
                if (enable)
                {
                    rFlowSpear.SetActive(true);
                    spearAppearfx.Play();
                    spearTrail.Emit(8);
                }
                else
                {
                    // spearAppearfx.Play();
                    rFlowSpear.SetActive(false);
                }
            }
        }
        
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
            if (GlobalVariableManager.Stance == StancesScriptController.Stance.frustration)
            {
                GlobalVariableManager.AddStanceMeter(StancesScriptController.Stance.flow, 7);  // 16 hits to fill
                GlobalVariableManager.SetStanceMeter(StancesScriptController.Stance.discord, 0);  // Reset discord buildup
            } else if (GlobalVariableManager.Stance == StancesScriptController.Stance.discord)
            {
                GlobalVariableManager.AddStanceMeter(StancesScriptController.Stance.flow, 17);  // 6 hits to fill
                GlobalVariableManager.AddStanceMeter(StancesScriptController.Stance.frustration, 9);  // 12 hits to fill
            }
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
