using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] float targetDistance, targetHeight;
    [SerializeField] int collisionMask;
    
    [SerializeField] Transform yawTrfm, pitchTrfm, cameraTrfm;
    [SerializeField] float xMouse, yMouse, xSensitivity, ySensitivity;
    [SerializeField] Transform playerTrfm;

    [SerializeField] float screenShakeStrength;
    [SerializeField] int trauma;

    Vector3 pitchVect3, yawVect3; //cached vector3's to avoid declaring 'new'

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        cameraTrfm.localPosition = cameraTrfm.forward * targetDistance;
    }

    // Update is called once per frame
    void Update()
    {
        yawTrfm.position = focusPos();
        HandleRotation();

        if (Input.GetKeyDown(KeyCode.Alpha1)) { AddTrauma(10); }
        if (Input.GetKeyDown(KeyCode.Alpha2)) { AddTrauma(20); }
        if (Input.GetKeyDown(KeyCode.Alpha3)) { AddTrauma(30); }
        if (Input.GetKeyDown(KeyCode.Alpha4)) { AddTrauma(40); }
    }

    private void FixedUpdate()
    {
        AdjustDistance();
        ProcessTrauma();
    }

    void HandleRotation()
    {
        yMouse = Input.GetAxis("Mouse Y");      //right now camera is flipped, and rotates indefintley, so fix that and set to max 90 degrees
        xMouse = Input.GetAxis("Mouse X");

        pitchVect3.x = -yMouse * ySensitivity;
        yawVect3.y = xMouse * xSensitivity;
        pitchTrfm.Rotate(pitchVect3);
        yawTrfm.Rotate(yawVect3);
    }
    [SerializeField] bool camBackBlocked;
    [SerializeField] int adjustMode, inWall;
    const int NEUTRAL = 0, FORWARD = 1, BACKWARDS = 2;
    void AdjustDistance()
    {
        Debug.DrawLine(cameraTrfm.position, focusPos() + -cameraTrfm.forward, Color.red);
        //camFrontBlocked = Physics.Linecast(cameraTrfm.position, focusPos() + -cameraTrfm.forward);
        //camBackBlocked = Physics.Linecast(cameraTrfm.position, cameraTrfm.position + cameraTrfm.forward);

        if (adjustMode == FORWARD || inWall > 0)
        {
            AdjustForward();
        }
        else if (adjustMode == BACKWARDS)
        {
            cameraTrfm.position += cameraTrfm.forward * -.01f;
        }

        if (Physics.Linecast(cameraTrfm.position, focusPos() + -cameraTrfm.forward))
        {
            adjustMode = FORWARD;
        } else if (cameraTrfm.localPosition.z > targetDistance && !camBackBlocked)
        {
            adjustMode = BACKWARDS;
        }
        else
        {
            adjustMode = NEUTRAL;
        }
    }

    void AdjustForward()
    {
        if (cameraTrfm.localPosition.z < -2)
        {
            cameraTrfm.position += cameraTrfm.forward * .02f;
        }
    }

    public void SetBackBlocked(bool state)
    {
        camBackBlocked = state;
    }

    Vector3 focusPos()
    {
        return playerTrfm.position + Vector3.up * targetHeight;
    }

    public void AddTrauma(int pTrauma, int max = int.MaxValue)
    {
        pTrauma *= traumaSharpness;
        trauma += pTrauma;
        if (trauma > max) { trauma = max; }
    }

    public void SetTrauma(int pTrauma)
    {
        pTrauma *= traumaSharpness; 
        if (trauma < pTrauma) { trauma = pTrauma; }
    }

    [SerializeField] float recalibrationRate;
    [SerializeField] Vector3 rotationVector;
    [SerializeField] Vector3 rotation;
    [SerializeField] int traumaSharpness;
    void ProcessTrauma()
    {
        if (trauma > 0)
        {
            float processedTrauma = Mathf.Pow(trauma/traumaSharpness, 2) * screenShakeStrength;
            rotationVector.x = Random.Range(-1f,1f) * processedTrauma;
            rotationVector.y = Random.Range(-1f,1f) * processedTrauma;
            rotationVector.z = Random.Range(-1f,1f) * processedTrauma;

            cameraTrfm.localEulerAngles += rotationVector;

            trauma--;
        }

        if (cameraTrfm.localEulerAngles.x > 180) { rotation.x = (360 - cameraTrfm.localEulerAngles.x) * recalibrationRate; }
        else { rotation.x = cameraTrfm.localEulerAngles.x * -recalibrationRate; }
        if (cameraTrfm.localEulerAngles.y > 180) { rotation.y = (360 - cameraTrfm.localEulerAngles.y) * recalibrationRate; }
        else { rotation.y = cameraTrfm.localEulerAngles.y * -recalibrationRate; }
        if (cameraTrfm.localEulerAngles.z > 180) { rotation.z = (360 - cameraTrfm.localEulerAngles.z) * recalibrationRate; }
        else { rotation.z = cameraTrfm.localEulerAngles.z * -recalibrationRate; }

        cameraTrfm.localEulerAngles += rotation;
    }
    
    private void OnTriggerStay(Collider2D collision)
    {
        //adjustMode = FORWARD;
    }

    private void OnTriggerEnter(Collider other)
    {
        inWall++;
    }

    private void OnTriggerExit(Collider other)
    {
        inWall--;
    }
}
