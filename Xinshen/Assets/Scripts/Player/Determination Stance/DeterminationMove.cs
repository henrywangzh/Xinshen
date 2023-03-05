using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeterminationMove : MonoBehaviour
{
	private DeterminationScriptController controller;
	private DeterminationInput inputHandler;
  private Rigidbody rb;

	[SerializeField] private Transform cam;
	[SerializeField] private float moveSpeed = 4f; // movement speed
	[SerializeField] private float rotSpeed = 5f; // rotation speed

	private float currSpeed; // current speed

	private void Awake()
	{
		controller = GetComponent<DeterminationScriptController>();
		inputHandler = GetComponent<DeterminationInput>();
    rb = GetComponent<Rigidbody>();
	}


	private void Update()
	{
		Vector2 inputVec = inputHandler.getInputVectorNorm();

		Vector3 moveDirection = (new Vector3(cam.forward.x, 0, cam.forward.z).normalized * inputVec.y + new Vector3(cam.right.x, 0, cam.right.z).normalized * inputVec.x) * currSpeed;
		moveDirection.y = 0;
		moveDirection += new Vector3(0, rb.velocity.y, 0);
		rb.velocity = moveDirection;

		currSpeed = moveSpeed * 1.5f;
		// anim.SetFloat("xInput", 0);
		// anim.SetFloat("yInput", Mathf.Sqrt(yinput*yinput + xinput*xinput) * 2f);
		transform.forward = Vector3.RotateTowards(transform.forward, new Vector3(rb.velocity.x, 0, rb.velocity.z), rotSpeed * Time.deltaTime, 0f);

		
		// // Keep input vector separate from the move vector
		// Vector3 moveDir = new Vector3(inputVec.x, 0f, inputVec.y);
		// float moveDistance = moveSpeed * Time.deltaTime;

		// transform.forward = Vector3.Slerp(transform.forward, moveDir, rotSpeed * Time.deltaTime);
	}
}
