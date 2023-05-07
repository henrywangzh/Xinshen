using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeterminationMove : MonoBehaviour
{
	private DeterminationScriptController controller;
	private DeterminationInput inputHandler;
	private Rigidbody rb;
	private Animator animator;

	[SerializeField] private Transform cam;
	[SerializeField] private float walkSpeed = 2f; // walk movement speed
	[SerializeField] private float runSpeed = 8f; // run movement speed
	[SerializeField] private float rotSpeed = 10f; // rotation speed
	[SerializeField] bool targLocked = false;

	private float currSpeed; // current speed
	private float animMultiplier;

	private void Awake()
	{
		controller = GetComponent<DeterminationScriptController>();
		inputHandler = GetComponent<DeterminationInput>();
		rb = GetComponent<Rigidbody>();
		animator = GetComponent<Animator>();
	}

	// Start is called before the first frame update
	void Start()
	{
		currSpeed = walkSpeed;
		animMultiplier = 1f;
	}

	private void Update()
	{
		Vector2 inputVec = inputHandler.getInputVectorNorm();
		float forwardDuration = inputHandler.getForwardDuration();

		// Start running after 0.5 sec
		bool isRunning = forwardDuration > 0.5f;


		if (targLocked)
		{
			currSpeed = walkSpeed;
			animMultiplier = 1f;

			animator.SetFloat("xInput", inputVec.x);
			animator.SetFloat("yInput", inputVec.y);
		}
		else
		{
			// Update player speed
			currSpeed = isRunning ? runSpeed : walkSpeed;

			// Update animator parameters
			animator.SetFloat("xInput", 0);

			if (isRunning)
				animMultiplier = Mathf.Min(2f, animMultiplier + 2.5f * Time.deltaTime);
			else
				animMultiplier = 1f;

			animator.SetFloat("yInput", inputHandler.getInputVector().magnitude * animMultiplier);

			// Update player rotation
			Vector3 forwardDir = Vector3.RotateTowards(transform.forward, new Vector3(rb.velocity.x, 0, rb.velocity.z), rotSpeed * Time.deltaTime, 0f);
			transform.forward = forwardDir;
		}

		// Update rigidbody velocity
		Vector3 moveDir = (new Vector3(cam.forward.x, 0, cam.forward.z).normalized * inputVec.y + new Vector3(cam.right.x, 0, cam.right.z).normalized * inputVec.x);
		moveDir.y = 0;

		Vector3 moveVelocity = moveDir * currSpeed;
		rb.velocity = moveVelocity + new Vector3(0, rb.velocity.y, 0);

		// Update speed
		// currSpeed = moveSpeed * 1.5f;
		// anim.SetFloat("xInput", 0);
		// anim.SetFloat("yInput", Mathf.Sqrt(yinput*yinput + xinput*xinput) * 2f);


		if (Input.GetMouseButtonDown(0))
		{
			controller.switchState.Invoke("attack");
			animator.Play("DeterminationAttack_1");
			rb.velocity = Vector3.zero;
		}
	}
}
