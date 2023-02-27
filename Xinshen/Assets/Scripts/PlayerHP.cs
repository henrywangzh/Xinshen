using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHP : MonoBehaviour
{
    [SerializeField] int m_HP, m_maxHP;
    [SerializeField] GameObject bloodFX;
    [SerializeField] Transform torsoTrfm;
    [SerializeField] CapsuleCollider hitboxCollider;

    static int HP, maxHP, invulnerability;

    static PlayerHP self;

    static Transform trfm;

    private void Awake()
    {
        m_HP = m_maxHP;
        maxHP = m_maxHP;
        HP = m_maxHP;

        self = GetComponent<PlayerHP>();
        trfm = transform;
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

    public static void TakeDamage(int damage, bool playBloodFX = true)
    {
        HP -= damage;
        self.m_HP = HP;

        if (playBloodFX)
        {
            Instantiate(self.bloodFX, self.torsoTrfm.position, trfm.rotation);
        }
        
        if (HP <= 0)
        {
            Debug.Log("bit the dust");
        }
    }
}
