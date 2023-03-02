using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ScriptController : MonoBehaviour
{
    public UnityEvent<string> switchState; // given a string, change the state machine to that node referenced by a string.
    List<Node> states;
    Node defaultState;
    Node currentState;
    Dictionary<string, Node> stringToNodeMap;
    // Start is called before the first frame update

    public void createStates()
    {
        states = new List<Node>();
        stringToNodeMap = new Dictionary<string, Node>();
    }

    void Start()
    {
        if (defaultState == null)
        {
            Debug.LogError("No default state set");
        }
        foreach (Node n in states)
        {
            n.disableScript();
        }
        if (this.isActiveAndEnabled)
            defaultState.enableScript();

        switchState = new UnityEvent<string>();
        switchState.AddListener(switchStateTriggered);
    }

    void switchStateTriggered(string s)
    {
        Node nextState;
        stringToNodeMap.TryGetValue(s, out nextState);
        if (!currentState.getNextAvailableStates().Contains(nextState))
        {
            Debug.LogError($"state \"{s}\" not in allowed states");
            return;
        }
        currentState.disableScript();
        nextState.enableScript();
        currentState = nextState;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Debug.Log(currentState.getStateName());
        foreach (var node in currentState.getNextAvailableStates())
        {
           if(Input.GetKey(node.getKeyCode()))
            {
                node.enableScript();
                currentState.disableScript();
                currentState = node;
            }
        }
    }

    private void OnEnable()
    {
        defaultState.enableScript();
    }

    private void OnDisable()
    {
        currentState.disableScript();
    }

    public Node addNode(string name, ScriptKeyPair state, List<Node> nextAvailableStates, List<MonoBehaviour> requiredStates, List<MonoBehaviour> canBeInterruptedByTheseStates)
    {
        Node newNode = new Node(state, nextAvailableStates, requiredStates, canBeInterruptedByTheseStates);
        states.Add(newNode);

        stringToNodeMap.Add(name, newNode);
        return newNode;
    }

    public void setDefaultState(Node n) 
    {
        defaultState = n;
        currentState = n;
        // n.enableScript();
    }
}

public class Node
{
    ScriptKeyPair state;
    //bool locked = false;
    List<Node> nextAvailableStates;
    List<MonoBehaviour> requiredStates;
    List<MonoBehaviour> canBeInterruptedByTheseStates;
    public string getStateName()
    {
        return state.getScriptName();
    }
    public KeyCode getKeyCode()
    {
        return state.getKeyCode();
    }
    public void enableScript()
    {
        state.getScript().enabled = true;
    }
    public void disableScript()
    {
        state.getScript().enabled = false;
    }
    public void addNextAvailableStates(Node n)
    {
        nextAvailableStates.Add(n);
    }
    public List<Node> getNextAvailableStates()
    {
        return nextAvailableStates;
    }
    public Node(ScriptKeyPair state, List<Node> nextAvailableStates, List<MonoBehaviour> requiredStates, List<MonoBehaviour> canBeInterruptedByTheseStates)
    {

        this.state = state;
        if (nextAvailableStates == null)
        {
            this.nextAvailableStates = new List<Node>();
        }
        else
        {
            this.nextAvailableStates = nextAvailableStates;
        }
        this.requiredStates = requiredStates;
        this.canBeInterruptedByTheseStates = canBeInterruptedByTheseStates;
    }
}

public class ScriptKeyPair
{
    MonoBehaviour script;
    KeyCode key;

    public string getScriptName()
    {
        return script.ToString();
    }
    public ScriptKeyPair(MonoBehaviour state, KeyCode key)
    {
        this.script = state;
        this.key = key;
    }

    public MonoBehaviour getScript()
    {
        return script;
    }

    public KeyCode getKeyCode()
    {
        return key;
    }
}

public class ActivationCondition<Trigger>
{
    Trigger trigger;

    public ActivationCondition(Trigger trigger)
    {
        this.trigger = trigger;
    }
    public Trigger getTrigger()
    {
        return trigger;
    }
}
