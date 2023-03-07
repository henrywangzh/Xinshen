using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHP : MonoBehaviour
{
    [SerializeField] int m_HP, m_maxHP;
    [SerializeField] GameObject bloodFX;
    [SerializeField] Transform m_torsoTrfm;
    [SerializeField] CapsuleCollider hitboxCollider;

    static int HP, maxHP, invulnerability;

    static PlayerHP self;

    public static Transform torsoTrfm;

    private void Awake()
    {
        m_HP = m_maxHP;
        maxHP = m_maxHP;
        HP = m_maxHP;

        self = GetComponent<PlayerHP>();
        torsoTrfm = m_torsoTrfm;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (invulnerability > 0)
        {
            if (invulnerability == 1)
            {
                hitboxCollider.enabled = true;
            }
            invulnerability--;
        }
    }

    public static void SetInvulnerable(int duration)
    {
        if (invulnerability < duration)
        {
            invulnerability = duration;
            self.hitboxCollider.enabled = false;
        }
    }

    public static void TakeDamage(int damage, bool playBloodFX = true, bool doDamageNumbers = true)
    {
        HP -= damage;
        self.m_HP = HP;

        if (playBloodFX) { Instantiate(self.bloodFX, torsoTrfm.position, torsoTrfm.rotation); }
        if (doDamageNumbers) { GameManager.InstantiateDamageNumber(torsoTrfm.position, damage); }

        CameraController.SetTrauma(damage);
        
        if (HP <= 0)
        {
            Debug.Log("bit the dust");
        }
    }
}
