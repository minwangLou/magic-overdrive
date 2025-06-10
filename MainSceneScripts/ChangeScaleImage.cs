using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeScaleImage : MonoBehaviour
{
    // Start is called before the first frame update
    public float minScale = 0.9f;       // ��С����
    public float maxScale = 1.1f;       // �������
    public float speed = 1.0f;          // �����ٶ�

    private Vector3 originalScale;

    void Start()
    {
        originalScale = transform.localScale;
    }

    void Update()
    {
        float scale = Mathf.Lerp(minScale, maxScale, (Mathf.Sin(Time.time * speed) + 1f) / 2f);
        transform.localScale = originalScale * scale;
    }
}
