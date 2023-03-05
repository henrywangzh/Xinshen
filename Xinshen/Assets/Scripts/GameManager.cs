using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject damageNumberObj;
    [SerializeField] GameObject slashFXObj;
    // Start is called before the first frame update
    void Awake()
    {
        Enemy.AssignSlashFXObj(slashFXObj);
    }

    void InstantiateDamageNumber(Vector3 position, int value)
    {
        //Instantiate(damageNumberObj, position, Quaternion.identity).GetComponent<DamageNumber>().
    }
}
