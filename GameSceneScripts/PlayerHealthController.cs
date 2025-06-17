using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthController : MonoBehaviour
{

    public static PlayerHealthController instance;

    public float currentHealth, maxHealth, recovery, armor;

    private float recoverTimer = 0f;

    public Slider healthSlider;

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        RecoverHealthPerSeconds();
    }



    public void TakeDamage(float damageTaked)
    {

        float finalDamage = Mathf.Max(0, damageTaked - armor);
        currentHealth -= finalDamage;

        PlayerController.instance.playerTakeDamage();


        if (currentHealth <= 0)
        {
            currentHealth = 0;
            //OnPlayerDeath();
            StartCoroutine(PlayerController.instance.RoleDeath());
        }

        ChangeValueHealthSlide();



    }

    private void ChangeValueHealthSlide()
    {
        healthSlider.value = Mathf.Clamp(currentHealth, 0, maxHealth);
    }

   

    //Cada segundo recuperar las cantidades de vida seg¨²n el valor que tiene recovery
    private void RecoverHealthPerSeconds()
    {
        if (currentHealth < maxHealth && recovery > 0)
        {

            recoverTimer += Time.deltaTime;

            if (recoverTimer >= 1f)
            {
                recoverTimer -= 1f;
                /*
                currentHealth += recovery;

                if (currentHealth > maxHealth)
                    currentHealth = maxHealth;

                ChangeValueHealthSlide();
                */
                RecoverRoleHealth(recovery);
            }
        }

    }

    public void UpdateMaxHealth(float updateMaxHealth)
    {
        float maxHealthDiferent = updateMaxHealth - maxHealth;
        currentHealth += maxHealthDiferent;
        maxHealth = updateMaxHealth;

        healthSlider.maxValue = maxHealth;
        ChangeValueHealthSlide();
    }

    public void RecoverRoleHealth(float healthAmount)
    {
        currentHealth += healthAmount;

        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }

        ChangeValueHealthSlide();
    }
}

