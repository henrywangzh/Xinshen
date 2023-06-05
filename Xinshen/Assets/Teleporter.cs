using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Teleporter : MonoBehaviour
{
    [SerializeField] string Scene;
    [SerializeField] Vector3 teleportDestination;
    [SerializeField] float activationRange;
    [SerializeField] ParticleSystem ptclSys;
    int holdTimer;
    Transform target, trfm;
    // Start is called before the first frame update
    void Start()
    {
        target = PredictionManager.playerTrfm;
        trfm = transform;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Mathf.Abs(target.position.x - trfm.position.x) < activationRange && Mathf.Abs(target.position.y - trfm.position.y) < activationRange
            && Vector3.SqrMagnitude(target.position - trfm.position) < activationRange * activationRange
            && Input.GetKey(KeyCode.E))
        {
            if (!ptclSys.isPlaying)
            {
                ptclSys.Play();
            }

            holdTimer++;

            if (holdTimer < 278)
            {
                if (Mathf.RoundToInt(holdTimer * holdTimer / 500) < 1)
                {
                    ptclSys.emissionRate = 2;
                }
                else
                {
                    ptclSys.emissionRate = Mathf.RoundToInt(holdTimer * holdTimer / 500);
                }
            } else if (ptclSys.isPlaying)
            {
                ptclSys.Stop();
            }

            if (holdTimer > 300)
            {
                if (Scene != "")
                {
                    SceneManager.LoadScene(Scene);
                }
                else
                {
                    PredictionManager.playerTrfm.position = teleportDestination;
                }
            }
        }
        else
        {
            if (ptclSys.isPlaying)
            {
                ptclSys.Stop();
            }

            holdTimer = 0;
        }
    }

    
}
