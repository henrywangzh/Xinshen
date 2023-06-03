using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    // A boolean to check if the item can be interacted with 
    public bool isInteractable = false;
    private Collider collider;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // If can be interacted with and player presses 'E'
        if (isInteractable && Input.GetKeyDown(KeyCode.E))
        {
            Interact();
        }
    }

    abstract protected void Interact();

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isInteractable = true;
            collider = other;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isInteractable = false;
        }
    }
    
    
}
