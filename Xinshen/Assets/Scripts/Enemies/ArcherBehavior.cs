using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherBehavior : MonoBehaviour
{
    [SerializeField] Transform player; //location of player
    [SerializeField] Rigidbody rb; //Rigidbody of enemy
    
    public Arrow ArrowPrefab; // Prefab of an arrow to be shot
    [SerializeField] public Vector3 ArrowSpawnOffset = new Vector3(0, 1, 0); // offset arrow spawn so it spawns where it's shot
    
    [SerializeField] float speed = 5f; //Patrol speed of archer
    [SerializeField] float shootingSpeed = 5f; //Frequency of shooting arrows
    [SerializeField] float combatRange = 500f; // range of shooting arrows

    private bool enemyDetected = false;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update() {
        
        //Distance from player
        float dist = Vector3.Distance(transform.position, player.position);

        //If in range (inclusive), detect enemy
        enemyDetected = dist <= combatRange;

        //If enemy detected, shoot
        if (enemyDetected) {
            Shoot();
        }
    }
    
    //Shoot an arrow at the player
    void Shoot() {
        
    }
}
