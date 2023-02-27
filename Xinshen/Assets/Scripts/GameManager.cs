using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject slashFXObj;
    // Start is called before the first frame update
    void Awake()
    {
        Enemy.AssignSlashFXObj(slashFXObj);
    }
}
