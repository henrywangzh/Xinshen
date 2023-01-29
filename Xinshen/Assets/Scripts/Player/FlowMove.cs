using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowMove : MonoBehaviour
{
    FlowScriptController controller;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<FlowScriptController>();               
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            controller.switchState.Invoke("evade");
        }
        if (Input.GetMouseButtonDown(0))
        {
            controller.switchState.Invoke("attack");
        }
    }
}
