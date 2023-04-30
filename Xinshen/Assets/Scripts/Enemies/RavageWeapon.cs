using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RavageWeapon : MonoBehaviour
{
    [SerializeField] Enemy wielder;
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
        if (other.gameObject.CompareTag("Player"))
        {
            int dmg = wielder.damage;
            PlayerHP.TakeDamage(dmg);
        }
    }
}
