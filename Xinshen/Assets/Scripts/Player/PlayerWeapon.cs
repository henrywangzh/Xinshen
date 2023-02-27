using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    [SerializeField] int knockbackPower;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Enemy>() != null)
        {
            other.gameObject.GetComponent<Enemy>().TakeDamage(GlobalVariableManager.Damage);
            other.gameObject.GetComponent<Enemy>().TakeKnockback(PlayerHP.torsoTrfm.position + PlayerHP.torsoTrfm.forward * -2, knockbackPower);
        }
    }
}
