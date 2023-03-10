using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject[] damageNumberObj;
    public const int RED = 0, BLUE = 1;
    [SerializeField] GameObject slashFXObj;
    [SerializeField] Transform canvasTrfm;
    public static GameManager self;
    // Start is called before the first frame update
    void Awake()
    {
        self = GetComponent<GameManager>();
        Enemy.AssignSlashFXObj(slashFXObj);
    }

    public static void InstantiateDamageNumber(Vector3 position, int value, int color = 0)
    {
        Instantiate(self.damageNumberObj[color], Vector3.zero, Quaternion.identity).GetComponent<DamageNumber>().Init(value, position);
    }

    public static void SetCanvasChild(Transform trfm)
    {
        trfm.SetParent(self.canvasTrfm, false);
    }
}
