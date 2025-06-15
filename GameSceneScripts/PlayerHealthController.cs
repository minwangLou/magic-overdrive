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

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            OnPlayerDeath();
        }

        ChangeValueHealthSlide();



    }

    private void ChangeValueHealthSlide()
    {
        healthSlider.value = Mathf.Clamp(currentHealth, 0, maxHealth);
    }

    
    private void OnPlayerDeath()
    {
        // 你可以在这里添加玩家死亡时的额外处理逻辑
        gameObject.SetActive(false);  // 这里可以是死亡后触发的动作
        //播放结束角色死亡动画，start coruntine
        SwitchPanelInGame.instance.ShowGameOverPanel();
    }


    //Cada segundo recuperar las cantidades de vida según el valor que tiene recovery
    private void RecoverHealthPerSeconds()
    {
        if (currentHealth < maxHealth && recovery > 0)
        {

            recoverTimer += Time.deltaTime;

            if (recoverTimer >= 1f)
            {
                recoverTimer -= 1f;

                currentHealth += recovery;

                if (currentHealth > maxHealth)
                    currentHealth = maxHealth;

                ChangeValueHealthSlide();
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
}

