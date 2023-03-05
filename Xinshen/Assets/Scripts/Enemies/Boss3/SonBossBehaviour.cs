using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SonBossBehaviour : MonoBehaviour
{
    [SerializeField] private Transform player;
    
    private Rigidbody rb;
    Collider col;
    Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        col = GetComponent<Collider>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
