using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class SonBossBehaviour : MonoBehaviour
{
    [SerializeField] private Transform player;

    public float LookAtSpeed;
    public float approachSpeed;
    public float decisionTime;
    private Coroutine LookCoroutine;
    private Rigidbody rb;
    private float startTime;
    private float elapsed;
    Collider col;
    Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        col = GetComponent<Collider>();
        startTime = Time.timeSinceLevelLoad;

    }

    // Update is called once per frame
    void Update()
    {
        // face player
        transform.forward += ((player.position - transform.position) - transform.forward) * LookAtSpeed;
        elapsed = (Time.timeSinceLevelLoad - startTime) % decisionTime;
        if (elapsed >= 1)
        {
            AttackPlayer();
        }

    }

    public void AttackPlayer()
    {
        transform.position = approachSpeed * (player.position - transform.position);
    }
}
