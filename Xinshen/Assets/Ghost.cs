using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class Ghost : Interactable
{
    public float range = 5f;  // The radius of the spherical range
    public float speed = 2f;  // The speed at which the object moves
    public float smoothTime = 1f;  // The smooth time for dampening

    private Vector3 startPosition;
    private Vector3 targetPosition;
    private Vector3 velocity;

    DialogueRunner runner;
    [SerializeField] string dialogueNode;
    enum states { Initial, SummonedSpirit, DoingQuest, FinishedQuest };
    states currentState = states.Initial;

    void Start()
    {
        startPosition = transform.position;
        runner = FindObjectOfType<DialogueRunner>();
        runner.onDialogueComplete.AddListener(ResetInteractState);
        currentState = states.Initial;
    }

    void Update()
    {
        SetRandomTargetPosition();

        // Move towards the target position using SmoothDamp
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime, speed);
    }

    void SetRandomTargetPosition()
    {
        // Generate a random direction within the unit sphere
        Vector3 randomDirection = Random.insideUnitSphere;

        // Calculate the target position based on the random direction and range
        targetPosition = startPosition + randomDirection * range;
    }

    void ResetInteractState()
    {
        currentState = states.Initial;
        Cursor.lockState = CursorLockMode.Locked;
        // Destroy(spiritObject);
    }

    [YarnCommand("Destroy")]
    public void DestroyWisp()
    {
        Destroy(gameObject);
    }

    override protected void Interact()
    {
        Debug.Log("Interacting");
        if (currentState == states.Initial)
        {
            //spiritObject = Instantiate(spirit, transform.position, Quaternion.identity);
            //spiritObject.transform.position = new Vector3(transform.position.x, transform.position.y + 3, transform.position.z);
            currentState = states.SummonedSpirit;
            Cursor.lockState = CursorLockMode.None;
            runner.StartDialogue(dialogueNode);
        }
    }
}