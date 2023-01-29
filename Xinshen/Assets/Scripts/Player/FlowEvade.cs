using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowEvade : MonoBehaviour
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
        if (Input.GetKeyDown(KeyCode.Space))
        {
            controller.switchState.Invoke("move");
        }
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            controller.switchState.Invoke("jump");
        }
    }
}
