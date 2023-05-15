using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Ravage :  Enemy
{

    // TODO:
    // 1. Implement the lunge action
    // 2. Implement the stabbing action
    // 3. Take care of direction change
    // 4. Take care of the animation

    [SerializeField] Transform player; // location of player
    [SerializeField] Collider rapier;
    [SerializeField] float attack_range = 5;
    [SerializeField] float combat_range = 10;
    [SerializeField] float detection_range = 20;
    [SerializeField] Vector3 chestOffset = new Vector3(0, 1.84f, 0); // currently defaulted to roughly at the chest

    Rigidbody rb; // rigidbody of enemy
    Animator anim;

    [SerializeField] float moveSpeed = 2f; // patrol speed of ravage
    [SerializeField] float lungeSpeed = 10f; // speed of lunge
    [SerializeField] float lungeCD = 5f; // cooldown of lunge

    float distance;
    bool isAngered;
    float attackTimer;
    float freezeTimer;
    Vector3 targetPosition;
    int layerMask;

    void Start() {
        attackTimer = 0;
        attackTimer = lungeCD;
        rb = GetComponent<Rigidbody>();
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if(player == null)
        {
            player = playerObject.transform;
        }
        targetPosition = player.position;
        layerMask = ~LayerMask.GetMask(playerObject.layer.ToString());
    }

    public override void Die()
    {
        Debug.Log("ahhhh im dying");
        Destroy(gameObject);
    }

    
    private void StabbingAttack()
    {
        Debug.Log("Stabbing");
        StartAttack();
        // anim.Play("RavageStab");
    }
    
    private void Lunge()
    {
        Debug.Log("Lunge");
        // Move to player
        // this.transform.position = Vector3.MoveTowards(this.transform.position, player.transform.position, 20 * moveSpeed * Time.deltaTime);
        
        // ray cast to ground bypassing player from targetPosition xz
        // if hit, set targetPosition to hit point
        Vector3 raySource = targetPosition;
        RaycastHit hit;
        Vector3 groundCoord = targetPosition;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, Mathf.Infinity, layerMask))
        {
            // Get the ground coordinate from the hit point
            groundCoord = hit.point;
        }

        Vector3 direction = groundCoord - this.transform.position;
        this.transform.position = targetPosition - direction.normalized;
        StabbingAttack();
        attackTimer = lungeCD;
        freezeTimer = 0.5f;
    } 

    public void StartAttack()
    {
        rapier.enabled = true;
    }

    public void EndAttack()
    {
        rapier.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        EndAttack();
        attackTimer -= Time.deltaTime;
        freezeTimer -= Time.deltaTime;
        if (freezeTimer > 0)
        {
            return;
        }

        distance = Vector3.Distance(targetPosition, this.transform.position);

        if (distance <= combat_range)
        {
            isAngered = true; //switch on the enemy moving mode
        } 

        if(distance > detection_range)
        {
            isAngered = false; //The enemy stop chasing after the player
        }
        else {
            lookAtPlayer();
        }


        if (isAngered && distance >= attack_range)
        {
            // Move to player
            this.transform.position = Vector3.MoveTowards(this.transform.position, player.transform.position, moveSpeed * Time.deltaTime);
        }
        else // The player is in the attack range of the enemy
        {
            //need to implement the lunge action
            //call the Lunge function
            if(attackTimer >= 0.5f){
                targetPosition = player.position;
            }
            if(attackTimer <= 0)
            {
                Lunge();
            }
            if (attackTimer <= 1f && attackTimer > 0){
                Debug.Log("Preping");
            }
        }
    }
    private void lookAtPlayer()
    {
        Vector3 direction = targetPosition + chestOffset - this.transform.position;
        direction.y = 0;
        this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(direction), 0.1f);
    }
}
