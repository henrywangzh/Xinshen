using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    [SerializeField] int knockbackPower;
    [SerializeField] int stunPower;
    [SerializeField] ParticleSystem ps;
    ParticleSystem.EmissionModule emitter;
    // Start is called before the first frame update
    void Start()
    {
        emitter = ps.emission;
        SetPSEmission(false);
    }

    // Update is called once per frame
    void Update()
    {
        
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
            other.gameObject.GetComponent<Enemy>().TakeDamage(GlobalVariableManager.Damage);
            other.gameObject.GetComponent<Enemy>().TakeKnockback(PlayerHP.torsoTrfm.position + PlayerHP.torsoTrfm.forward * -2, knockbackPower, stunPower);
        }
    }
}
