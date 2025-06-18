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

        // ��ʼ������
        initialScale = transform.localScale = Vector3.one * 1.5f; // ��ʼ�Ŵ�һ���

        // ��ʼ����ɫ
        initialColor = damageText.color;
        damageText.color = new Color(initialColor.r, initialColor.g, initialColor.b, 1f);
    }

    private void UpdateDamageNumber()
    {
        if (numberSpawnCounter > 0)
        {
            numberSpawnCounter -= Time.deltaTime;

            float t = 1 - (numberSpawnCounter / numberSpawnTime); // [0, 1] ֮��

            // ����Ч��
            Color fadeColor = damageText.color;
            fadeColor.a = Mathf.Lerp(1f, 0f, t); // ͸���ȴ�1���䵽0
            damageText.color = fadeColor;

            // ���Żص�
            float scale = Mathf.Lerp(1.5f, 1f, t); // ��1.5����1
            transform.localScale = Vector3.one * scale;

            // �ƶ�
            transform.position += Vector3.up * moveSpeed * Time.deltaTime;

            if (numberSpawnCounter <= 0)
            {
                DamageNumberController.instance.RecicleNumberToPool(this);
            }
        }
    }

}
