using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Campfire : Interactable
{
    [SerializeField] ParticleSystem sparks, flames;
    int channelTimer;
    bool isHealing;
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isInteractable)
        {
            if (Input.GetKey(KeyCode.E))
            {
                if (channelTimer == 200)
                {
                    isHealing = true;
                    flames.Play();
                    RespawnManager.SetRespawn();
                }
                else if (channelTimer % 50 == 0)
                {
                    sparks.Play();
                }

                channelTimer++;
            }
            else
            {
                channelTimer = 0;
            }

            if (isHealing)
            {
                PlayerHP.Heal(1);
            }
        } else if (isHealing)
        {
            isHealing = false;
            flames.Stop();
        }
    }

protected override void Interact()
    {

    }
}
