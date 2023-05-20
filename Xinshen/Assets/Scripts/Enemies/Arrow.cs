using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
// https://www.youtube.com/watch?v=QzitQSLhfG0

public class Arrow : MonoBehaviour {
    [SerializeField] public float AutoDestroyTime = 2f; // destroy the arrow set time after it's been shot
    [SerializeField] public int damage = 5; // damage p/arrow
    Rigidbody rb; // rigidbody of obj.

    private void Awake() {
        rb = GetComponent<Rigidbody>();
    }

    // Disables an arrow after the auto destroy time
    private void OnEnable() {
        CancelInvoke("Disable");
        Invoke("Disable", AutoDestroyTime);
    }

    private void OnTriggerEnter(Collider other) {
        Debug.Log("Collided with " + other.tag);
        if (other.tag == "Player") {
            // Gets GVM of player
            GlobalVariableManager GlobalVariableManager = other.GetComponent<GlobalVariableManager>();
            // Uses GVM to take damage for player
            PlayerHP.TakeDamage(damage);
        }

        // Disables arrow if it hits
        Disable();
        // Debug.Log("DOES THIS RUN?");
    }
    
    // Function for disabling game objects
    private void Disable() {
        Debug.Log("Disable?");
        // CancelInvoke("Disable");
        // rb.velocity = Vector3.zero;
        // this.GameObject().SetActive(false);
        Destroy(gameObject);
    }
    
}
