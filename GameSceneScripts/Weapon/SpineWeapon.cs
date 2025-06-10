using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpineWeapon : Weapon
{
    public GameObject fireBallPrefab;
    //public int amount;
    //public float area;
    //public float speed;

    private List<GameObject> fireBallList = new List<GameObject>();

    private Vector3 weaponMaxSize = Vector3.zero;
    private Vector3 targetSize = Vector3.zero;

    public float ChangeSpeed;
    //public float duration;
    private float currentLifeTime;

    //public float coolDownl;
    private float currentRespownTime;
    private bool respawn = false;

    //private float might;



    

    /*

    // Start is called before the first frame update
    void Start()
    {
        SetStats();

        SpawnFireBall();
        

        
        //Iniciar el objeto con el random angulo
        float RandomAngleIniciate = Random.Range(0f, 360f);
        transform.rotation = Quaternion.Euler(0f, 0f, transform.rotation.eulerAngles.z + RandomAngleIniciate);


        //test
        //UIController.instance.levelUpButtons[0].UpdateButtonDisplay(this);
        
    }
    */


    void Update()
    {
        
        if (fireBallList.Count!=0)
        {
            RotateFireballs();
            ChangeWeaponSize();

            if (respawn == false)
            {

                DespawnWeapon();
            }
            else
            {
                RespawnWeapon();

            }

            //Actualizar el estado de arma al subir de nivel
            /*
            if (statsUpdated == true)
            {
                statsUpdated = false;

                UpdateWeaponStats();
            }
            */
        }



    }

    private void SpawnFireBall()
    {


        for (int i=0; i<amount; i++)
        {
            float angle = (360f / amount) * i;

            Vector3 fireballPosition = GetFireballPosition(angle);

            GameObject fireball = Instantiate(fireBallPrefab, transform);
            fireball.transform.localPosition = fireballPosition;

            fireball.GetComponent<FireBallDamager>().weaponDamage = might;
            
            fireBallList.Add(fireball);

            weaponMaxSize = fireball.transform.localScale;
            
            fireball.transform.localScale = Vector3.zero;

            targetSize = weaponMaxSize;

        }

        
    }

    //Cambiar tamaña de herramienta cuando sale y cuando desaparece
    private void ChangeWeaponSize()
    {
        for (int i = 0; i< fireBallList.Count; i++)
        {
            fireBallList[i].transform.localScale = Vector3.MoveTowards(fireBallList[i].transform.localScale, targetSize, ChangeSpeed * Time.deltaTime);
        }

    }

    //Cuando pasa un tiempo determinado, se desaparece la herramienta
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
    
    //Cuando la herramienta ha sido desaparecido un determiando tiempo, restablecerla.
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
            currentRespownTime = 0;
        }

    }
    


    // 计算火球的半径生成位置
    private Vector3 GetFireballPosition(float angle)
    {
        float radians = angle * Mathf.Deg2Rad;
       
        return  new Vector3(Mathf.Cos(radians), Mathf.Sin(radians), 0) * area;
    }


    //Girar la órbita, no fireboll
    private void RotateFireballs()
    {

        transform.rotation = Quaternion.Euler
            (0f, 0f, transform.rotation.eulerAngles.z + (speed * Time.deltaTime));
    }

    /*
    public void SetStats()
    {
        rotationSpeed = stats[weaponLevel].speed;

        weaponDamage = stats[weaponLevel].damage;

        orbitRadius = stats[weaponLevel].range;

        fireBallCount = stats[weaponLevel].amount;

        WeaponLifeTime = stats[weaponLevel].duration;

    }
    */

    public override void UpdateWeaponStats()
    {
        if (amount > 0)
        {
            for (int i = 0; i < fireBallList.Count; i++)
            {
                Destroy(fireBallList[i].gameObject);

            }
            fireBallList.Clear();
            
        }

        SpawnFireBall();

        //Iniciar el objeto con el random angulo
        float RandomAngleIniciate = Random.Range(0f, 360f);
        transform.rotation = Quaternion.Euler(0f, 0f, transform.rotation.eulerAngles.z + RandomAngleIniciate);
    }



}
