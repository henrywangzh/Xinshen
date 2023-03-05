using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeterminationInput : MonoBehaviour
{
	private float startTime;

	private Vector2 inputVector; 

	public Vector2 getInputVector()
	{
		inputVector = new Vector2(0, 0);

		// if (Input.GetKey(KeyCode.W)) inputVector.y = 1;
		// if (Input.GetKey(KeyCode.S)) inputVector.y = -1;

		// if (Input.GetKey(KeyCode.D)) inputVector.x = 1;
		// if (Input.GetKey(KeyCode.A)) inputVector.x = -1;

		inputVector.x = Input.GetAxis("Horizontal");
		inputVector.y = Input.GetAxis("Vertical");

		return inputVector;
	}

	public Vector2 getInputVectorNorm()
	{
		Vector2 inputVector = getInputVector();
		inputVector.Normalize();

		return inputVector;
	}

	// Return time duration that player has been moving forward
	public float getForwardDuration()
	{
		if (Input.GetKeyDown(KeyCode.W))
			startTime = Time.time;

		if (Input.GetKey("w"))
			return Time.time - startTime;
		else
			return 0f;
	}
}
