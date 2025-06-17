using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TitleFlashEffect : MonoBehaviour
{
    public TextMeshProUGUI text;
    public Color[] colors;
    public float cycleDuration = 2f; // 每个颜色段持续几秒

    private int currentIndex = 0;
    private float timer = 0f;

    void Update()
    {
        if (colors.Length < 2) return;

        timer += Time.deltaTime;
        float t = timer / cycleDuration;

        // 平滑插值函数：让 t 变成 ease-in-out 的曲线
        float smoothT = Mathf.SmoothStep(0f, 1f, t);

        // 当前与下一个颜色
        Color from = colors[currentIndex];
        Color to = colors[(currentIndex + 1) % colors.Length];

        text.color = Color.Lerp(from, to, smoothT);

        // 完成一轮后，切换颜色
        if (timer >= cycleDuration)
        {
            timer = 0f;
            currentIndex = (currentIndex + 1) % colors.Length;
        }
    }
}

