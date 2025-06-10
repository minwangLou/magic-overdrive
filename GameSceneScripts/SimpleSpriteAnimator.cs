using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleSpriteAnimator : MonoBehaviour
{
    public Sprite[] sprites;
    public float frameTime = 0.2f;

    private SpriteRenderer sr;
    private int index = 0;
    private float timer = 0f;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= frameTime)
        {
            timer = 0f;
            index = (index + 1) % sprites.Length;
            sr.sprite = sprites[index];
        }
    }
}
