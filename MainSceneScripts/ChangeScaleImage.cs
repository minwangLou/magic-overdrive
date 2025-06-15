using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeScaleImage : MonoBehaviour
{
    public float minScale = 0.9f;       // 最小缩放
    public float maxScale = 1.1f;       // 最大缩放
    public float speed = 1.0f;          // 呼吸速度

    private Vector3 originalScale;

    void Start()
    {
        originalScale = transform.localScale;
    }

    void Update()
    {
        // 使用 unscaledTime 来忽略 Time.timeScale
        float scale = Mathf.Lerp(minScale, maxScale, (Mathf.Sin(Time.unscaledTime * speed) + 1f) / 2f);
        transform.localScale = originalScale * scale;
    }
}