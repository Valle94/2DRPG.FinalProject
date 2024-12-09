using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerHealth : Singleton<PlayerHealth>
{
    public bool IsDead { get; private set; }

    [SerializeField] int maxHealth = 3;
    [SerializeField] float knockBackThrustAmount = 10f;
    [SerializeField] float damageRecoveryTime = 1f;

    Slider healthSlider;
    int currentHealth;
    bool canTakeDamage = true;
    Knockback knockback;
    Flash flash;
    const string HEALTH_SLIDER_TEXT = "Health Slider";
    const string TOWN_TEXT = "Town";
    readonly int DEATH_HASH = Animator.StringToHash("Death");

    protected override void Awake()
    {
        base.Awake();

        knockback = GetComponent<Knockback>();
        flash =  GetComponent<Flash>();
    }

    void Start()
    {
        IsDead = false;
        currentHealth = maxHealth;
        
        UpdateHealthSlider();
    }

    // The collision method is similar to ontriggerenter2d, except
    // it checks every frame for a collision, not just the moment of contact
    void OnCollisionStay2D(Collision2D other)
    {
        // Get enemy ai component (if it exists)
        EnemyAI enemy = other.gameObject.GetComponent<EnemyAI>();

        // If it does exist and we haven't take damage recently
        if (enemy)
        {   
            // Take Damage
            TakeDamage(1, other.transform);
        }
    }

    // This public method is used by our health pickups to heal the player
    public void HealPlayer()
    {
        if (currentHealth < maxHealth)
        {
            currentHealth += 1;
        }
        UpdateHealthSlider();
    }
    
    // This method applies the damage to our current health and sets
    // canTakeDamage to false to create 'invincibility frames' 
    public void TakeDamage(int damageAmount, Transform hitTransform)
    {
        if (!canTakeDamage) 
        {
            return;
        }
        
        // Shake the screen
        ScreenShakeManager.Instance.ShakeScreen();
        // Get knocked back
        knockback.GetKnockedBack(hitTransform, knockBackThrustAmount);
        // Flash the player
        StartCoroutine(flash.FlashRoutine());
        canTakeDamage = false;
        currentHealth -= damageAmount;
        StartCoroutine(DamageRecoveryRoutine());
        // Update the health slider UI
        UpdateHealthSlider();
        // Check if we're dead
        CheckIfPlayerDeath();
    }

    // We die when current HP <= 0
    void CheckIfPlayerDeath()
    {
        if (currentHealth <= 0 && !IsDead)
        {
            IsDead = true;
            // Destroy the Active Weapon so we can't shoot while dying
            Destroy(ActiveWeapon.Instance.gameObject);
            currentHealth = 0;
            // Start Death animation
            GetComponent<Animator>().SetTrigger(DEATH_HASH);
            // Start scenemanager coroutine
            StartCoroutine(DeathLoadSceneRoutine());
        }
    }

    // This coroutine waits a couple of seconds, then
    // destroys the player and loads the scene specified. 
    IEnumerator DeathLoadSceneRoutine()
    {
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
        Stamina.Instance.ReplenishStaminaOnDeath();
        SceneManager.LoadScene(TOWN_TEXT);
    }

    IEnumerator DamageRecoveryRoutine()
    {
        yield return new WaitForSeconds(damageRecoveryTime);
        canTakeDamage = true;
    }

    // This method updates the value of our health slider
    void UpdateHealthSlider()
    {
        // Null reference check
        if (healthSlider == null)
        {
            healthSlider = GameObject.Find(HEALTH_SLIDER_TEXT).GetComponent<Slider>();
        }

        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;
    }
}
