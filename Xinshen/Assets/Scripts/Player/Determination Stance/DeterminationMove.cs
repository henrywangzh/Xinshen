using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeterminationMove : MonoBehaviour
{
	private DeterminationScriptController controller;
	private DeterminationInput inputHandler;
  private Rigidbody rb;

	[SerializeField] private Transform cam;
	[SerializeField] private float walkSpeed = 2f; // walk movement speed
	[SerializeField] private float runSpeed = 8f; // run movement speed
	[SerializeField] private float rotSpeed = 10f; // rotation speed

	private float currSpeed; // current speed

	private void Awake()
	{
		controller = GetComponent<DeterminationScriptController>();
		inputHandler = GetComponent<DeterminationInput>();
    rb = GetComponent<Rigidbody>();
	}

		// Start is called before the first frame update
	void Start()
	{
		currSpeed = walkSpeed;
	}

	private void Update()
	{
		Vector2 inputVec = inputHandler.getInputVectorNorm();
		float forwardDuration = inputHandler.getForwardDuration();

		// Start running after 0.5 sec
		currSpeed = forwardDuration > 0.5f ? runSpeed : walkSpeed;

		// Update rigidbody velocity
		Vector3 moveDir = (new Vector3(cam.forward.x, 0, cam.forward.z).normalized * inputVec.y + new Vector3(cam.right.x, 0, cam.right.z).normalized * inputVec.x);
		moveDir.y = 0;
		
		Vector3 moveVelocity = moveDir * currSpeed;
		rb.velocity = moveVelocity + new Vector3(0, rb.velocity.y, 0);;

		// Update speed
		// currSpeed = moveSpeed * 1.5f;
		// anim.SetFloat("xInput", 0);
		// anim.SetFloat("yInput", Mathf.Sqrt(yinput*yinput + xinput*xinput) * 2f);

		// Update player rotation
		Vector3 forwardDir = Vector3.RotateTowards(transform.forward, new Vector3(rb.velocity.x, 0, rb.velocity.z), rotSpeed * Time.deltaTime, 0f);
		transform.forward = forwardDir;
	}
}
