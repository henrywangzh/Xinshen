using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
// https://www.youtube.com/watch?v=QzitQSLhfG0
// Following a path code taken from Studio Intro Tutorials for Tower Defense 

public class ArcherBehavior : MonoBehaviour
{
    [SerializeField] Transform player; // location of player
    [SerializeField] Vector3 chestOffset = new Vector3(0, 1.84f, 0); // currently defaulted to roughly at the chest
    Rigidbody rb; // rigidbody of enemy
    
    [SerializeField] List<Transform> pathCheckpoints;
    int curCheckpoint = 0;
    Transform curTarg;

    [SerializeField] GameObject arrow; // Prefab of an arrow to be shot
    GameObject shotArrow; // The arrow actually shot
    [SerializeField] float arrowOffsetMultiple = 1.7f; // how far the arrow spawns from the archer in the direction the archer is facing
    [SerializeField] float arrowVerticalOffset = 1.5f; // vertical offset of the arrow
    Vector3 arrowSpawnOffset;
    // [SerializeField] float sphereCastRadius = 0.6f; // a sphere roughly the size of the arrow itself

    [SerializeField] float moveSpeed = 2f; // patrol speed of archer
    [SerializeField] float shootingSpeed = 2f; // frequency of shooting arrows
    [SerializeField] float combatRange = 50f; // range of shooting arrows
    [SerializeField] public float arrowMoveSpeed = 2f; // speed of arrow
    
    [SerializeField] Quaternion arrowRotation = Quaternion.identity; // rotation of the arrow so it's facing the right way

    RaycastHit hit;

    float dist;
    Vector3 vectorTowardsPlayer; // calculated as vector from arrow's vertical spawn offset to player's chest
    
    bool enemyDetected = false;
    bool attacking = false;
    // bool patrolling = false;

    // Start is called before the first frame update
    void Start() {
        rb = GetComponent<Rigidbody>();
        curTarg = pathCheckpoints[curCheckpoint];
    }

    // Update is called once per frame
    void Update() {
        
        Quaternion targetRotation;
        Vector3 lookingDirection;

        if (!attacking) {
            // Rotate archer to look in direction it's moving
            lookingDirection = curTarg.position - transform.position;
            lookingDirection.Normalize();
            lookingDirection.y = 0;
            targetRotation = Quaternion.LookRotation(lookingDirection);
            targetRotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 360 * Time.deltaTime);
            rb.MoveRotation(targetRotation);
        } else {
            lookingDirection = vectorTowardsPlayer;
            lookingDirection.Normalize();
            lookingDirection.y = 0;
            targetRotation = Quaternion.LookRotation(lookingDirection);
            targetRotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 360 * Time.deltaTime);
            rb.MoveRotation(targetRotation);
        }

        // Set arrowSpawnOffset in the direction the archer is facing
        Vector3 facing = transform.forward;
        facing.Normalize();
        arrowSpawnOffset = new Vector3(facing.x * arrowOffsetMultiple, arrowVerticalOffset, facing.z * arrowOffsetMultiple);

        //Distance and vector from player
        dist = Vector3.Distance((transform.position + arrowSpawnOffset), (player.position + chestOffset));
        vectorTowardsPlayer = (player.position + chestOffset) - (transform.position + arrowSpawnOffset);

        enemyDetected = Detection();
        // Debug.Log(hit.collider)

        // If no enemy detected and not attacking, patrol, and if not attacking but enemy is detected, attack
        if (!enemyDetected && !attacking) {

            transform.position = Vector3.MoveTowards(transform.position,
                new Vector3(curTarg.position.x, transform.position.y, curTarg.position.z), moveSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, new Vector3(curTarg.position.x, transform.position.y, curTarg.position.z)) < 0.1f) {
                SwitchCheckpoints();
            }

        } else if (enemyDetected && !attacking) {
            attacking = true;
            StartCoroutine(Attack());
        }
    }

    // Check in range and raycast to check for LoS
    private bool Detection() {

        if (dist > combatRange) {
            return false;
        }

        bool hitSomething = Physics.Raycast(transform.position + arrowSpawnOffset, vectorTowardsPlayer, out hit, dist + 1);
        // Debug.Log("Hit something: " + hitSomething);
        // Debug.Log("Hit: " + hit);
        
        if (hitSomething && hit.collider.tag == "Player") {
            return true;
        }

        return false;
    }

    // Attack at a regular interval
    IEnumerator Attack() {

        attacking = true;

        while (enemyDetected) {
            WaitForSeconds wait = new WaitForSeconds(shootingSpeed);
            yield return wait;
            Shoot();
        }
        
        // enemy is no longer detected
        attacking = false;
        StopCoroutine(Attack());
    }
    
    // Shoot an arrow at the player at a regular interval
    void Shoot() {
        shotArrow = Instantiate(arrow, transform.position + arrowSpawnOffset, arrowRotation);
        Rigidbody arrowRb = shotArrow.GetComponent<Rigidbody>();
        arrowRb.velocity = Vector3.Normalize(vectorTowardsPlayer) * arrowMoveSpeed;
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
