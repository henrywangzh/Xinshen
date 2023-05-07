using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeterminationAttack : MonoBehaviour
{
	Animator anim;
	FlowScriptController controller;
	Rigidbody rb;
	PlayerWeapon weapon;
	[SerializeField] Collider weaponCollider;

	private int comboCount = 0;  // Corresponds with animation state name
	private int maxCombo = 3;  // Highest animation number we have

	void Start()
	{
		anim = GetComponent<Animator>();
		controller = GetComponent<FlowScriptController>();
		rb = GetComponent<Rigidbody>();
		weapon = weaponCollider.gameObject.GetComponent<PlayerWeapon>();
	}

	private void OnEnable()
	{
		if (anim == null)
			anim = GetComponent<Animator>();
		if (rb != null)
			rb.velocity = Vector3.zero;
		GlobalVariableManager.Damage = 25;
	}

	// Update is called once per frame
	void Update()
	{
		// Attack Combos
		if (Input.GetMouseButtonDown(0)) { Attack(); }

		// Block/Parry
		if (Input.GetKeyDown(KeyCode.LeftShift))
		{
			Debug.Log("Block");
		}
	}

	public void StartSwing() { weaponCollider.enabled = true; }
	public void EndSwing() { weaponCollider.enabled = false; }

	public void Attack()
	{
		comboCount++;
		if (comboCount <= maxCombo)
		{
			Debug.Log("Attack #" + comboCount);
			// anim.Play("DeterminationAttack_1"); // _" + comboCount);
		}
		else
		{
			comboCount = 0;
		}
		anim.SetBool("Combo", comboCount >= 1);
	}

	public void EndAttack()
	{
		if (this.isActiveAndEnabled)
			controller.switchState.Invoke("move");
	}

	void SetVelocityFwd(float vel)
	{
		rb.velocity = transform.forward * vel;
	}

	void Cleanup()
	{
		weaponCollider.enabled = false;
		weapon.SetPSEmission(false);
		anim.SetBool("Combo", false);
		SetVelocityFwd(0);
	}
}
