using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinPickUp : MonoBehaviour
{
    public int coinAmount;

    private bool movingToPlayer;
    public float moveSpeed;

    public float timeBetweenChecks = 0.2f;
    private float checkCounter;

    private PlayerController playerController;

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
            transform.position = Vector3.MoveTowards(transform.position, positionPlayer, moveSpeed * Time.deltaTime);
        }
        else
        {
            checkCounter -= Time.deltaTime;
            if (checkCounter <= 0)
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
        if (collision.tag == "Player")
        {
            CoinController.instance.AddCoins(coinAmount);

            Destroy(gameObject);
        }
    }

}
