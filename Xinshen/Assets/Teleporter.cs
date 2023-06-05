using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Teleporter : Interactable
{
    public string Scene;
    [SerializeField] Vector3 teleportDestination;
    [SerializeField] float activationRange;
    [SerializeField] ParticleSystem ptclSys;
    [SerializeField] Image blackSquare;
    int holdTimer, tpThreshold = 275, riseTimer;
    Transform target, trfm;
    // Start is called before the first frame update
    void Start()
    {
        target = PredictionManager.playerTrfm;
        trfm = transform;
        trfm.position -= Vector3.up * .0333f * riseTimer;
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

            if (holdTimer < 400)
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

            if (holdTimer == tpThreshold)
            {
                InvokeRepeating("FadeToBlack", 0, .05f);
                holdTimer++;
            }
        }
        else if (holdTimer < tpThreshold)
        {
            if (ptclSys.isPlaying)
            {
                ptclSys.Stop();
            }

            holdTimer = 0;
        }

        if (riseTimer > 0)
        {
            transform.position += Vector3.up * .0333f;
            riseTimer--;
        }
    }

    override protected void Interact()
    {
        
    }

    void FadeToBlack()
    {
        if (blackSquare.color.a < 1.3f)
        {
            blackSquare.color = new Vector4(blackSquare.color.r, blackSquare.color.g, blackSquare.color.b, blackSquare.color.a + 0.03f);
        }
        else
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

    public void Init(string nextScene, int pRiseTimer = 0)
    {
        Scene = nextScene;
        riseTimer = pRiseTimer;
    }
}
