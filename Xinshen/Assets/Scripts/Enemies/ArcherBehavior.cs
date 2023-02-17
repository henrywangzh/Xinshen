using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
// https://www.youtube.com/watch?v=QzitQSLhfG0
// Following a path code taken from Studio Intro Tutorials for Tower Defense 

public class ArcherBehavior : MonoBehaviour
{
    [SerializeField] Transform player; // location of player
    Rigidbody rb; // rigidbody of enemy
    
    [SerializeField] List<Transform> pathCheckpoints;
    int curCheckpoint = 0;
    Transform curTarg;

    [SerializeField] GameObject arrow; // Prefab of an arrow to be shot
    GameObject shotArrow; // The arrow actually shot
    [SerializeField] Vector3 arrowSpawnOffset = new Vector3(0, 1, 0); // offset arrow spawn so it spawns where it's shot
    [SerializeField] Quaternion arrowRotation = Quaternion.identity; // rotation of the arrow so it's facing the right way
    [SerializeField] float sphereCastRadius = 0.1f; // a sphere roughly the size of the arrow itself
    
    [SerializeField] float moveSpeed = 2f; // patrol speed of archer
    [SerializeField] float shootingSpeed = 5f; // frequency of shooting arrows
    [SerializeField] float combatRange = 1f; // range of shooting arrows

    RaycastHit hit;

    float dist;
    Vector3 vectorTowardsPlayer;
    
    bool enemyDetected = false;
    bool attacking = false;
    bool patrolling = false;

    // Start is called before the first frame update
    void Start() {
        rb = GetComponent<Rigidbody>();
        curTarg = pathCheckpoints[curCheckpoint];
    }

    // Update is called once per frame
    void Update() {
        
        //Distance and vector from player
        dist = Vector3.Distance(transform.position, player.position);
        vectorTowardsPlayer = player.position - transform.position;

        enemyDetected = Detection();
        // Debug.Log("Enemy Detected: " + enemyDetected);

        // If no enemy detected, patrol, else 
        if (!enemyDetected) {
            transform.position = Vector3.MoveTowards(transform.position,
                new Vector3(curTarg.position.x, transform.position.y, curTarg.position.z), moveSpeed * Time.deltaTime);
            
            if (Vector3.Distance(transform.position, new Vector3(curTarg.position.x, transform.position.y, curTarg.position.z)) < 0.1f) {
                SwitchCheckpoints();
            }
            
        } else if (enemyDetected && !attacking) {
            attacking = true;
            StartCoroutine(Attack());
        }

        //If enemy detected and not already attacking, stop patrolling and attack, else patrol if not already
        // if (enemyDetected && !attacking) {
        //     // StopCoroutine(Patrol());
        //     patrolling = false;
        //     rb.velocity = Vector3.zero;
        //     // StartCoroutine(Attack());
        // } else if (!patrolling) {
        //     patrolling = true;
        //     // StartCoroutine(Patrol());
        // }
    }

    // Check in range and spherecast to check for LoS, ArrowSpawnOffset may create weird edge cases unsure yet
    private bool Detection() {
        Ray ray = new Ray(transform.position + arrowSpawnOffset, vectorTowardsPlayer);
        return dist <= combatRange && Physics.SphereCast(ray, sphereCastRadius, out hit, dist);
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
        shotArrow = Instantiate(arrow, transform.position + arrowSpawnOffset, arrowRotation);
        Rigidbody arrowRb = shotArrow.GetComponent<Rigidbody>();
        arrowRb.velocity = Vector3.Normalize(vectorTowardsPlayer);
    }

    // IEnumerator Patrol() {
    //
    //     patrolling = true;
    //     Debug.Log("Patrolling");
    //
    //     transform.position = Vector3.MoveTowards(transform.position,
    //             new Vector3(curTarg.position.x, transform.position.y, curTarg.position.z), moveSpeed * Time.deltaTime);
    //         
    //     if (Vector3.Distance(transform.position, curTarg.position) < 0.1f) {
    //         SwitchCheckpoints();
    //     }
    //
    //     yield return StartCoroutine(Patrol());
    // }

    // Switch checkpoints to go to the next one
    void SwitchCheckpoints() {
        curCheckpoint++; // Go to next checkpoint
        
        Debug.Log(curCheckpoint);
        
        if (curCheckpoint >= pathCheckpoints.Count) { // if reached end of checkpoint
            curCheckpoint = 0;  // loop back to the first checkpoint
            curTarg = pathCheckpoints[curCheckpoint];
        } else {
            curTarg = pathCheckpoints[curCheckpoint]; // else go to next checkpoint
        }
    }
}
