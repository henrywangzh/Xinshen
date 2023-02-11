using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBackcast : MonoBehaviour
{
    [SerializeField] CameraController cameraController;
    [SerializeField] int collisionCount;

    private void OnTriggerEnter(Collider other)
    {
        collisionCount++;
        cameraController.SetBackBlocked(true);
    }

    private void OnTriggerExit(Collider other)
    {
        collisionCount--;
        if (collisionCount < 1)
        {
            cameraController.SetBackBlocked(false);
        }
    }
}
