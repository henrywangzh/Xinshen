using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeterminationScriptController : ScriptController
{
	StancesScriptController masterController;
	Animator anim;

	// Define sub-script references
	DeterminationMove move;
	// TODO: FlowEvade evade;
	// TODO: FlowAttack attack;

	private void Awake()
	{
			masterController = GetComponent<StancesScriptController>();
			anim = GetComponent<Animator>();

			// Initialize the state machine
			createStates();

			// Get references to sub-scripts
			move = GetComponent<DeterminationMove>();
			// TODO: evade = GetComponent<FlowEvade>();
			// TODO: attack = GetComponent<FlowAttack>();

			// Setting up move node
			string moveStateName = "move";
			ScriptKeyPair state = new ScriptKeyPair(move, KeyCode.None);
			List<MonoBehaviour> requiredStates = new List<MonoBehaviour>();
			List<MonoBehaviour> canBeInterruptedByTheseStates = new List<MonoBehaviour>();
			Node moveNode = addNode(moveStateName, state, null, requiredStates, canBeInterruptedByTheseStates);

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

			setDefaultState(moveNode);
	}

	// private void OnEnable()
	// {
	// 	anim.Play("Unsheathe");
	// }

	private void Update()
	{
        if (Input.GetKeyDown(KeyCode.LeftBracket))
            masterController.switchState.Invoke("flow");
    }
}
