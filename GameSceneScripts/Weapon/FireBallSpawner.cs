using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBallSpawner : Weapon
{
    public GameObject fireBallPrefab;

    private List<GameObject> fireBallList = new List<GameObject>();
    private List<float> fireBallAngles = new List<float>(); // 每个火球的角度

    private Vector3 weaponMaxSize = Vector3.zero;
    private Vector3 targetSize = Vector3.zero;

    public float ChangeSpeed;
    private float currentLifeTime;

    private float currentRespownTime;
    private bool respawn = false;


    void Update()
    {
        if (fireBallList.Count != 0)
        {
            RotateFireballs();
            ChangeWeaponSize();

            if (!respawn)
            {
                DespawnWeapon();
            }
            else
            {
                RespawnWeapon();
            }
        }
    }

    private void SpawnFireBall()
    {
        for (int i = 0; i < amount; i++)
        {
            float angle = (360f / amount) * i;

            GameObject fireball = Instantiate(fireBallPrefab, transform);
            fireBallList.Add(fireball);
            fireBallAngles.Add(angle); // 保存初始角度

            fireball.GetComponent<FireBallDamager>().weaponDamage = might;

            weaponMaxSize = fireball.transform.localScale;
            fireball.transform.localScale = Vector3.zero;
            targetSize = weaponMaxSize;
        }
    }

    private void ChangeWeaponSize()
    {
        for (int i = 0; i < fireBallList.Count; i++)
        {
            fireBallList[i].transform.localScale = Vector3.MoveTowards(
                fireBallList[i].transform.localScale,
                targetSize,
                ChangeSpeed * Time.deltaTime
            );
        }
    }

    private void DespawnWeapon()
    {
        currentLifeTime += Time.deltaTime;
        if (currentLifeTime >= duration)
        {
            targetSize = Vector3.zero;

            if (fireBallList[0].transform.localScale.x == 0f)
            {
                for (int i = 0; i < fireBallList.Count; i++)
                {
                    fireBallList[i].SetActive(false);
                }

                currentLifeTime = 0f;
                respawn = true;
            }
        }
    }

    private void RespawnWeapon()
    {
        currentRespownTime += Time.deltaTime;
        if (currentRespownTime >= coolDown)
        {
            for (int i = 0; i < fireBallList.Count; i++)
            {
                fireBallList[i].SetActive(true);
            }

            targetSize = weaponMaxSize;
            respawn = false;
            currentRespownTime = 0f;
        }
    }

    // ⭕ 核心修改部分：火球绕中心旋转并调整朝向
    private void RotateFireballs()
    {
        for (int i = 0; i < fireBallList.Count; i++)
        {
            // 更新角度
            fireBallAngles[i] += speed * Time.deltaTime;
            if (fireBallAngles[i] > 360f) fireBallAngles[i] -= 360f;

            float rad = fireBallAngles[i] * Mathf.Deg2Rad;
            Vector3 offset = new Vector3(Mathf.Cos(rad), Mathf.Sin(rad), 0f) * area;

            fireBallList[i].transform.localPosition = offset;

            // 让火球“面朝外”（也可以设置为 inward: -offset）
            fireBallList[i].transform.up = -offset;

        }
    }

    public override void UpdateWeaponStats()
    {
        if (amount > 0)
        {
            for (int i = 0; i < fireBallList.Count; i++)
            {
                Destroy(fireBallList[i]);
            }

            fireBallList.Clear();
            fireBallAngles.Clear();
        }

        SpawnFireBall();

        // 添加一个随机初始角度偏移，避免每次都一样
        float randomOffset = Random.Range(0f, 360f);
        for (int i = 0; i < fireBallAngles.Count; i++)
        {
            fireBallAngles[i] += randomOffset;
        }
    }
}

