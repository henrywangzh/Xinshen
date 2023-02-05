using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GruntBehaviour : MonoBehaviour
{
    [SerializeField] Transform player;

    [SerializeField] Rigidbody rb;

    float speed = 5;

    [SerializeField] float combatRange = 100;

    // Start is called before the first frame update
    void Start()
    {
        // rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        // Returns distance between two points
        float dist = Vector3.Distance(transform.position, player.position);

        // Set the velocity of your Rigidbody to move towards the target

        Vector3 vectorTowardsPlayer = player.position - transform.position;
        vectorTowardsPlayer.Normalize();
        if (dist <= combatRange)
        {
            rb.velocity = speed * vectorTowardsPlayer;
        }



    }
}
