using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimation : MonoBehaviour
{

    private Transform sprite;

    public float speed;

    private float activeSize;

    public float maxSize, minSize;



    // Start is called before the first frame update
    void Start()
    {
        activeSize = maxSize;
        sprite = GetComponentInChildren<Transform>();

        speed = speed * Random.Range(0.75f, 1.25f);
    }

    // Update is called once per frame
    void Update()
    {
        sprite.localScale = Vector3.MoveTowards(sprite.localScale, Vector3.one * activeSize, speed * Time.deltaTime);

        if (sprite.localScale.x == activeSize)
        {
            if (activeSize == maxSize)
            {
                activeSize = minSize;
            }
            else
            {
                activeSize = maxSize;
            }
        }
    }
}
