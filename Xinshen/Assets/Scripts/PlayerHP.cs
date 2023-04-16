using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHP : MonoBehaviour
{
    [SerializeField] int m_HP, m_maxHP, m_frenzyHPThresholdPercent;
    [SerializeField] GameObject bloodFX;
    [SerializeField] Transform m_torsoTrfm;
    [SerializeField] CapsuleCollider hitboxCollider;
    [SerializeField] private Image frenzySplatter = null;

    static int HP, maxHP, frenzyHP, invulnerability;

    static PlayerHP self;

    public static Transform torsoTrfm;

    private void Awake()
    {
        m_HP = m_maxHP;
        maxHP = m_maxHP;
        HP = m_maxHP;
        frenzyHP = (int)(m_maxHP * (m_frenzyHPThresholdPercent / 100f));
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
        Debug.Log("HP: " + HP);

        if (playBloodFX) { Instantiate(self.bloodFX, torsoTrfm.position, torsoTrfm.rotation); }
        if (doDamageNumbers) { GameManager.InstantiateDamageNumber(torsoTrfm.position, damage, GameManager.RED); }

        CameraController.SetTrauma(damage);
        
        if (HP <= 0)
        {
            Debug.Log("bit the dust");
        }
        else if (HP <= frenzyHP)
        {
            Debug.Log("frenzy time");
            Color splatterAlpha = self.frenzySplatter.color;
            splatterAlpha.a = 0.6f;
            self.frenzySplatter.color = splatterAlpha;
            GlobalVariableManager.FrenzyMode = true;
        }
        if (HP > frenzyHP || HP <= 0)
        {
            Color splatterAlpha = self.frenzySplatter.color;
            splatterAlpha.a = 0f;
            self.frenzySplatter.color = splatterAlpha;
            GlobalVariableManager.FrenzyMode = false;
        }
    }

    public static void Heal(int heal)
    {
        HP += heal;
        if (HP > maxHP) { HP = maxHP; }
        self.m_HP = HP;
    }
}
