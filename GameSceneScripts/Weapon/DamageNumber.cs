using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamageNumber : MonoBehaviour
{
    public TMP_Text damageText;

    public float numberSpawnTime;
    private float numberSpawnCounter;

    private float moveSpeed = 0.5f;


    private Vector3 initialScale;
    private Color initialColor;


    // Update is called once per frame
    void Update()
    {
        UpdateDamageNumber();
    }


    public void SetUp(int damageDisplay)
    {
        numberSpawnCounter = numberSpawnTime;
        damageText.text = damageDisplay.ToString();

        if (damageDisplay <= 30)
        {
            damageText.color = Color.white;
        }
        else
        {
            damageText.color = Color.red;
        }

        // 初始化缩放
        initialScale = transform.localScale = Vector3.one * 1.5f; // 初始放大一点点

        // 初始化颜色
        initialColor = damageText.color;
        damageText.color = new Color(initialColor.r, initialColor.g, initialColor.b, 1f);
    }

    private void UpdateDamageNumber()
    {
        if (numberSpawnCounter > 0)
        {
            numberSpawnCounter -= Time.deltaTime;

            float t = 1 - (numberSpawnCounter / numberSpawnTime); // [0, 1] 之间

            // 渐隐效果
            Color fadeColor = damageText.color;
            fadeColor.a = Mathf.Lerp(1f, 0f, t); // 透明度从1渐变到0
            damageText.color = fadeColor;

            // 缩放回弹
            float scale = Mathf.Lerp(1.5f, 1f, t); // 从1.5缩回1
            transform.localScale = Vector3.one * scale;

            // 移动
            transform.position += Vector3.up * moveSpeed * Time.deltaTime;

            if (numberSpawnCounter <= 0)
            {
                DamageNumberController.instance.RecicleNumberToPool(this);
            }
        }
    }

}
