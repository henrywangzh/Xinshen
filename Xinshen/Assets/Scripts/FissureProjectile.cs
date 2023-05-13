using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FissureProjectile : MonoBehaviour
{
    [SerializeField] int duration, pillarSpawnRate;
    int spawnTimer;
    [SerializeField] float speed;
    [SerializeField] GameObject pillar;
    Transform trfm;
    // Start is called before the first frame update
    void Start()
    {
        trfm = transform;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        trfm.position += trfm.forward * speed;

        if (spawnTimer > 0) { spawnTimer--; }
        else
        {
            Instantiate(pillar, trfm.position, trfm.rotation);
            spawnTimer = pillarSpawnRate;
        }

        if (duration > 0) { duration--; } else { Destroy(gameObject); }
    }
}
