using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthController : MonoBehaviour
{

    public static PlayerHealthController instance;

    public float currentHealth, maxHealth, recovery, armor;

    public Slider healthSlider;

    private void Awake()
    {
        instance = this;
    }


    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;

        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;

        StartCoroutine(HealthRegenRoutine());
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
    private IEnumerator HealthRegenRoutine()
    {
        float timer = 0f;

        while (true)
        {
            timer += Time.deltaTime;

            if (timer >= 1f)
            {
                timer -= 1f;

                if (currentHealth < maxHealth && recovery > 0)
                {
                    currentHealth += recovery;
                    if (currentHealth > maxHealth)
                        currentHealth = maxHealth;

                    ChangeValueHealthSlide();
                }
            }

            yield return null;
        }
    }
}
