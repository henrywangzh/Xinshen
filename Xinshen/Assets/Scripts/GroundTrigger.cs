using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundTrigger : MonoBehaviour
{
    public bool IsOnGround()
    {
        return touchCount > 0;
    }
    
    [SerializeField] int touchCount;
    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.layer == 6)
        {
            touchCount++;
        }
    }

    private void OnTriggerExit(Collider col)
    {
        if (col.gameObject.layer == 6)
        {
            touchCount--;
        }
    }
}
