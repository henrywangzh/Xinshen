using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] bool allowScrollWheelZooming;
    [SerializeField] float targetDistance, targetHeight;
    [SerializeField] int collisionMask;
    
    [SerializeField] Transform yawTrfm, pitchTrfm, mountTrfm, cameraTrfm;
    [SerializeField] float xMouse, yMouse, xSensitivity, ySensitivity;
    [SerializeField] Transform playerTrfm;

    [SerializeField] float screenShakeStrength;
    [SerializeField] int trauma;
    [SerializeField] Canvas canvas;

    Vector3 pitchVect3, yawVect3, lockedTargetVect3; //cached vector3's to avoid declaring 'new'

    public static Transform s_cameraTrfm;
    public static CameraController self;

    private void Awake()
    {
        canvas.transform.parent = null;
        s_cameraTrfm = cameraTrfm;
        self = GetComponent<CameraController>();
        GlobalVariableManager.MainCamera = transform;
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        cameraTrfm.localPosition = cameraTrfm.forward * targetDistance;
    }

    // Update is called once per frame
    void Update()
    {
        yawTrfm.position = GetFocusPosition();
        HandleRotation();

        if (Input.GetMouseButtonDown(2))
        {
            if (GlobalVariableManager.LockedTarget)
            {
                GlobalVariableManager.SetLockedTarget(null);
            }
            else
            {
                GlobalVariableManager.SetLockedTarget(GetTarget());
            }
        }

        if (allowScrollWheelZooming)
        {
            targetDistance += Input.mouseScrollDelta.y;
            if (targetDistance > -3) { targetDistance = -3; }
        }
    }

    private void FixedUpdate()
    {
        AdjustDistance();
        ProcessTrauma();
        FocusLockedTarget();
    }

    [SerializeField] Transform lockTarget;
    Transform GetTarget()
    {
        return lockTarget;
    }

    void FocusLockedTarget()
    {
        if (GlobalVariableManager.LockedTarget)
        {
            lockedTargetVect3 = GlobalVariableManager.LockedTarget.position - mountTrfm.position;
            lockedTargetVect3.y = 0;

            if (lockedTargetVect3.sqrMagnitude > .5f)
            {
                mountTrfm.forward += (lockedTargetVect3 - mountTrfm.forward) * .1f;
            }
        }
        else
        {
            mountTrfm.localEulerAngles = Vector3.zero;
        }
    }

    public static void FacePlayer()
    {

    }

    void HandleRotation()
    {
        if (GlobalVariableManager.LockedTarget) { return; }

        yMouse = Input.GetAxis("Mouse Y");      //right now camera is flipped, and rotates indefintley, so fix that and set to max 90 degrees
        xMouse = Input.GetAxis("Mouse X");

        pitchVect3.x = -yMouse * ySensitivity;
        yawVect3.y = xMouse * xSensitivity;
        pitchTrfm.Rotate(pitchVect3);
        yawTrfm.Rotate(yawVect3);
    }

    RaycastHit hit;
    [SerializeField] float move;
    void AdjustDistance()
    {
        Debug.DrawRay(GetFocusPosition() - cameraTrfm.forward, -cameraTrfm.forward, Color.red);

        if (Physics.Raycast(GetFocusPosition() - cameraTrfm.forward, -cameraTrfm.forward, out hit, -targetDistance))
        {
            move = (-hit.distance - cameraTrfm.localPosition.z);
            cameraTrfm.localPosition += new Vector3(0,0,move * .1f);
        }
        else
        {
            move = (targetDistance - cameraTrfm.localPosition.z);
            cameraTrfm.localPosition += new Vector3(0, 0, move * .1f);
        }
    }

    Vector3 GetFocusPosition()
    {
        return playerTrfm.position + Vector3.up * targetHeight;
    }

    public static void AddTrauma(int pTrauma, int max = int.MaxValue)
    {
        pTrauma = Mathf.RoundToInt(pTrauma * self.traumaSharpness);
        self.trauma += pTrauma;
        if (self.trauma > max) { self.trauma = max; }
    }

    public static void SetTrauma(int pTrauma)
    {
        pTrauma = Mathf.RoundToInt(pTrauma * self.traumaSharpness);
        if (self.trauma < pTrauma) { self.trauma = pTrauma; }
    }

    [SerializeField] float recalibrationRate;
    [SerializeField] Vector3 rotationVector;
    [SerializeField] Vector3 rotation;
    [SerializeField] float traumaSharpness, traumaShakeCap;
    void ProcessTrauma()
    {
        if (trauma > 0)
        {
            float processedTrauma;
            if (trauma > traumaShakeCap)
            {
                processedTrauma = Mathf.Pow(traumaShakeCap / traumaSharpness, 2) * screenShakeStrength;
            }
            else
            {
                processedTrauma = Mathf.Pow(trauma / traumaSharpness, 2) * screenShakeStrength;
            }
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
}
