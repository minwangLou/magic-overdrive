using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExperiencePickUp : MonoBehaviour
{

    private int expValue;

    private bool movingToPlayer;
    public float moveSpeed;

    public float timeBetweenChecks = 0.2f;
    private float checkCounter;

    private PlayerController playerController;

    public Sprite exp_green;
    public Sprite exp_blue;
    public Sprite exp_red;
    public Sprite exp_gold;

    public SpriteRenderer expSprite;

    // Start is called before the first frame update
    void Start()
    {
        playerController = PlayerController.instance;
    }

    // Update is called once per frame
    void Update()
    {

        Vector3 positionPlayer = playerController.gameObject.transform.position;

        if (movingToPlayer == true)
        {
            transform.position = Vector3.MoveTowards(transform.position,positionPlayer, moveSpeed * Time.deltaTime);
        }
        else
        {
            checkCounter -= Time.deltaTime;
            if(checkCounter <= 0)
            {
                checkCounter = timeBetweenChecks;

                if (Vector3.Distance(transform.position, positionPlayer) < playerController.pickUpRange)
                {
                    movingToPlayer = true;
                    moveSpeed += playerController.moveSpeed;
                }

            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            ExperienceLevelController.instance.PlayerGetExp(expValue);
            //Debug.Log(expValue);

            Destroy(gameObject);
        }
    }

    public void setExpValue (int expValue)
    {
        //Debug.Log(expValue);
        this.expValue = expValue;

        ChangeExpColor(expValue);
    }

    private void ChangeExpColor(int expValue)
    {
        if (expValue <= 10)
        {
            expSprite.sprite = exp_green;
        }
        else if (expValue <= 30)
        {
            expSprite.sprite = exp_blue;
        }
        else if (expValue <= 80)
        {
            expSprite.sprite = exp_red;
        }
        else
        {
            expSprite.sprite = exp_gold;
        }
    }


}
