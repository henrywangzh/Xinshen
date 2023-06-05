using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deparenter : MonoBehaviour
{
    [SerializeField] Transform[] children;
    [SerializeField] Deparenter self;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < children.Length; i++)
        {
            children[i].parent = null;
        }

        Destroy(self);
    }
}
