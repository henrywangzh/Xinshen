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
    Rigidbody rb; // rigidbody of enemy
    Animator anim;

    [SerializeField] float moveSpeed = 2f; // patrol speed of ravage
    [SerializeField] float lungeSpeed = 10f; // speed of lunge
    [SerializeField] float lungeCD = 5f; // cooldown of lunge

    float distance;
    bool isAngered;
    float timer;
    float attackTimer;

    void Start() {
        attackTimer = 0;
        timer = lungeCD;
        rb = GetComponent<Rigidbody>();
        if(player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }

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
        
        Vector3 direction = player.position - this.transform.position;
        this.transform.position = player.position - direction.normalized;
        StabbingAttack();
        timer = lungeCD;
    } 

    public void StartAttack()
    {
        attackTimer = 1f;
        rapier.enabled = true;
    }

    public void EndAttack()
    {
        rapier.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        distance = Vector3.Distance(player.position, this.transform.position);
        attackTimer -= Time.deltaTime;
        if (attackTimer <= 0)
        {
            EndAttack();
        }
        if (distance <= combat_range)
        {
            isAngered = true; //switch on the enemy moving mode
        } 

        if(distance > detection_range)
        {
            isAngered = false; //The enemy stop chasing after the player
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
            timer -= Time.deltaTime;
            if(timer <= 0)
            {
                Lunge();
            }
        }
    }   
}
