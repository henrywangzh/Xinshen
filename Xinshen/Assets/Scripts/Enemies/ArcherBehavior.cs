using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
// https://www.youtube.com/watch?v=QzitQSLhfG0

public class ArcherBehavior : MonoBehaviour
{
    [SerializeField] Transform player; //location of player
    Rigidbody rb; //Rigidbody of enemy
    
    [SerializeField] GameObject Arrow; // Prefab of an arrow to be shot
    private GameObject ShotArrow; // The arrow actually shot
    [SerializeField] public Vector3 ArrowSpawnOffset = new Vector3(0, 1, 0); // offset arrow spawn so it spawns where it's shot
    [SerializeField] public Quaternion ArrowRotation;
    [SerializeField] public float SphereCastRadius = 1f; // a sphere roughly the size of the arrow itself
    
    [SerializeField] float speed = 5f; //Patrol speed of archer
    [SerializeField] float shootingSpeed = 5f; //Frequency of shooting arrows
    [SerializeField] float combatRange = 400f; // range of shooting arrows

    private RaycastHit hit;

    private float dist;
    private Vector3 vectorTowardsPlayer;
    private bool enemyDetected = false;
    private bool attacking = false;
    private bool patrolling = false;

    // Start is called before the first frame update
    void Start() {
        patrolling = true;
        StartCoroutine(Patrol());
    }

    // Update is called once per frame
    void Update() {
        
        //Distance and vector from player
        dist = Vector3.Distance(transform.position, player.position);
        vectorTowardsPlayer = player.position - transform.position;

        enemyDetected = Detection();
        // Debug.Log("Enemy Detected: " + enemyDetected);

        //If enemy detected and not already attacking, stop patrolling and attack, else patrol if not already
        if (enemyDetected && !attacking) {
            StopCoroutine(Patrol());
            patrolling = false;
            StartCoroutine(Attack());
        } else if (!patrolling) {
            StartCoroutine(Patrol());
        }
    }

    // Check in range and spherecast to check for LoS, ArrowSpawnOffset may create weird edge cases unsure yet
    private bool Detection() {
        Ray ray = new Ray(transform.position + ArrowSpawnOffset, vectorTowardsPlayer);
        return dist <= combatRange && Physics.SphereCast(ray, SphereCastRadius, out hit, dist);
    }

    // Attack at a regular interval
    IEnumerator Attack() {

        attacking = true;

        while (enemyDetected) {
            Shoot();
            WaitForSeconds wait = new WaitForSeconds(shootingSpeed);
            yield return wait;
        }
        
        // enemy is no longer detected
        attacking = false;
        StopCoroutine(Attack());
    }
    
    // Shoot an arrow at the player at a regular interval
    void Shoot() {
        ShotArrow = Instantiate(Arrow, transform.position + ArrowSpawnOffset, ArrowRotation);
        
    }

    IEnumerator Patrol() {
        
        while (patrolling) {
            // patrol
        }

        // Patrolling should be automatically stopped when an enemy is detected. 
        yield return null;

    }

    // Patrol an area
    void MakeAPatrolLoop() {
        
    }
}
