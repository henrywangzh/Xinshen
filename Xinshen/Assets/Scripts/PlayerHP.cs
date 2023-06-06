using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PlayerHP : MonoBehaviour
{
    [SerializeField] int m_HP, m_maxHP, m_frenzyHPThresholdPercent;
    [SerializeField] GameObject bloodFX;
    [SerializeField] Transform m_torsoTrfm;
    [SerializeField] CapsuleCollider hitboxCollider;
    [SerializeField] private Image frenzySplatter = null, deathScreen = null;
    [SerializeField] Transform DebugSpawnPoint;
    [SerializeField] int FrenzyAtkBoost = 5;

    [SerializeField] bool actuallyDie;

    static int HP, maxHP, frenzyHP, invulnerability;

    static PlayerHP self;

    public static Transform torsoTrfm;
    public static UnityEvent PlayerHit = new UnityEvent();

    private void Awake()
    {
        m_HP = m_maxHP;
        maxHP = m_maxHP;
        HP = m_maxHP;
        frenzyHP = (int)(m_maxHP * (m_frenzyHPThresholdPercent / 100f));
        damageMultiplier = 1;
        self = GetComponent<PlayerHP>();
        torsoTrfm = m_torsoTrfm;
        
        // Debug stuff for now
        GlobalVariableManager.PlayerSpawn = DebugSpawnPoint;
        GlobalVariableManager.FrenzySplatter = frenzySplatter;
        GlobalVariableManager.DeathScreen = deathScreen;
        GlobalVariableManager.FrenzyDamageBonus = FrenzyAtkBoost;

        if (GlobalVariableManager.PlayerSpawn == null) {
            GlobalVariableManager.PlayerSpawn = transform;
        }
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

    float damageMultiplier;
    public static void SetDamageReduction(float value) //0: full damage;  0.3: 30% damage reduction;  1.0: no damage
    {
        self.damageMultiplier = 1 - value;
    }

    public static void TakeDamage(int damage, bool playBloodFX = true, bool doDamageNumbers = true)
    {
        if (HP <= 0) { return; }
        damage = Mathf.RoundToInt(damage * self.damageMultiplier);
        HP -= damage;
        self.m_HP = HP;
        // Debug.Log("HP: " + HP);
        PlayerHit.Invoke();

        if (playBloodFX) { Instantiate(self.bloodFX, torsoTrfm.position, torsoTrfm.rotation); }
        if (doDamageNumbers) { GameManager.InstantiateDamageNumber(torsoTrfm.position, damage, GameManager.RED); }

        CameraController.SetTrauma(damage);
        
        if (HP <= 0)
        {
            Die();
        }
        else if (HP <= frenzyHP)
        {
            Debug.Log("frenzy time");
            setFrenzyMode(true);
        }
        if (HP > frenzyHP || HP <= 0)
        {
            setFrenzyMode(false);
        }
    }

    public static void Heal(int heal=-1)
    {
        if (heal == -1) { heal = maxHP; }
        else { HP += heal; }
        if (HP > maxHP) { HP = maxHP; }
        self.m_HP = HP;
        if (HP > frenzyHP)
        {
            setFrenzyMode(false);
        }
    }
    private static void setFrenzyMode(bool frenzy)
    {
        // TODO: complete boosts
        if (frenzy != GlobalVariableManager.FrenzyMode)
        {
            FrenzyBoost(GlobalVariableManager.FrenzyMode);
        }
        GlobalVariableManager.FrenzyMode = frenzy;
        if (frenzy){
            Color splatterAlpha = GlobalVariableManager.FrenzySplatter.color;
            splatterAlpha.a = 0.4f;
            GlobalVariableManager.FrenzySplatter.color = splatterAlpha;
        }
        else{
            Color splatterAlpha = GlobalVariableManager.FrenzySplatter.color;
            splatterAlpha.a = 0f;
            GlobalVariableManager.FrenzySplatter.color = splatterAlpha;
        }

        // TODO: add frenzy mode stat boost
    }

    private static void Die(){
        RespawnManager.Respawn();
        Debug.Log("bit the dust");
        if (self.actuallyDie || true) { self.gameObject.SetActive(false); }
        self.StartCoroutine(self.FadeIn(GlobalVariableManager.DeathScreen));
        self.StartCoroutine(self.Respawn());
    }

    private static void FrenzyBoost(bool revert = false){
        if (revert){
            GlobalVariableManager.Damage -= GlobalVariableManager.FrenzyDamageBonus;
        }
        else{
            GlobalVariableManager.Damage += GlobalVariableManager.FrenzyDamageBonus;
        }
    }

    IEnumerator Respawn(){
        yield return new WaitForSeconds(3f);
        self.transform.position = GlobalVariableManager.PlayerSpawn.position;
        HP = maxHP;   
        GlobalVariableManager.DeathScreen.color = new Color(1, 1, 1, 0); 
        yield return null;    
    }

    // WIP: Fade in the input UI element

    IEnumerator FadeIn( Image image )
    {
        Color c = image.color;
        for (float alpha = c.a; alpha <= 1; alpha += 0.1f)
        {
            c.a = alpha;
            image.color = c;
            yield return null;
        }
        c.a = 1;
        image.color = c;
        yield return null;
    }

}
