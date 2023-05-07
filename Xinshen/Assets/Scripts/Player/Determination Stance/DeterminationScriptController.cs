using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeterminationScriptController : ScriptController
{
	StancesScriptController masterController;
	Animator anim;

	// Define sub-script references
	DeterminationMove move;
	DeterminationAttack attack;

	private void Awake()
	{
		masterController = GetComponent<StancesScriptController>();
		anim = GetComponent<Animator>();

		// Initialize the state machine
		createStates();

		// Get references to sub-scripts
		move = GetComponent<DeterminationMove>();
		attack = GetComponent<DeterminationAttack>();

		ScriptKeyPair state;

		// Move node
		string moveName = "move";
		state = new ScriptKeyPair(move, KeyCode.None);
		List<MonoBehaviour> requiredStates = new List<MonoBehaviour>();
		List<MonoBehaviour> canBeInterruptedByTheseStates = new List<MonoBehaviour>();
		Node moveNode = addNode(moveName, state, null, requiredStates, canBeInterruptedByTheseStates);

		// Attack node
		string attackName = "attack";
		state = new ScriptKeyPair(attack, KeyCode.None);
		requiredStates = new List<MonoBehaviour>();
		canBeInterruptedByTheseStates = new List<MonoBehaviour>();
		Node attackNode = addNode(attackName, state, null, requiredStates, canBeInterruptedByTheseStates);

		/* TODO:
		// Setting up evade node
		string name = "evade";
		state = new ScriptKeyPair(evade, KeyCode.None);  // Script maps to a key. If the Keycode is None then the script cannot be switched into via keybinds
		requiredStates = new List<MonoBehaviour>();
		canBeInterruptedByTheseStates = new List<MonoBehaviour>();
		Node evadeNode = addNode(name, state, null, requiredStates, canBeInterruptedByTheseStates);

		string atkname = "attack";
		state = new ScriptKeyPair(attack, KeyCode.None);  // Script maps to a key. If the Keycode is None then the script cannot be switched into via keybinds
		requiredStates = new List<MonoBehaviour>();
		canBeInterruptedByTheseStates = new List<MonoBehaviour>();
		Node attackNode = addNode(atkname, state, null, requiredStates, canBeInterruptedByTheseStates);

		moveNode.addNextAvailableStates(evadeNode);
		moveNode.addNextAvailableStates(attackNode);
		moveNode.addNextAvailableStates(moveNode);
		evadeNode.addNextAvailableStates(moveNode);
		attackNode.addNextAvailableStates(moveNode);
		*/

		// moveNode.addNextAvailableStates(attackNode);
		// attackNode.addNextAvailableStates(moveNode);

		moveNode.addNextAvailableStates(attackNode);
		attackNode.addNextAvailableStates(moveNode);

		setDefaultState(moveNode);
	}

	private void OnEnable()
	{
		anim.Play("DeterminationEntry");
		GlobalVariableManager.Stance = StancesScriptController.Stance.determination;
	}

	private void Update()
	{
		// if (Input.GetKeyDown(KeyCode.LeftBracket))
		// 	masterController.switchState.Invoke("flow");
	}
}
