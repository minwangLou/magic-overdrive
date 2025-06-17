using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TitleFlashEffect : MonoBehaviour
{
    public TextMeshProUGUI text;
    public Color[] colors;
    public float cycleDuration = 2f; // ÿ����ɫ�γ�������

    private int currentIndex = 0;
    private float timer = 0f;

    void Update()
    {
        if (colors.Length < 2) return;

        timer += Time.deltaTime;
        float t = timer / cycleDuration;

        // ƽ����ֵ�������� t ��� ease-in-out ������
        float smoothT = Mathf.SmoothStep(0f, 1f, t);

        // ��ǰ����һ����ɫ
        Color from = colors[currentIndex];
        Color to = colors[(currentIndex + 1) % colors.Length];

        text.color = Color.Lerp(from, to, smoothT);

        // ���һ�ֺ��л���ɫ
        if (timer >= cycleDuration)
        {
            timer = 0f;
            currentIndex = (currentIndex + 1) % colors.Length;
        }
    }
}

