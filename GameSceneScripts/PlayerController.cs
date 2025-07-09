using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    public RoleData roleData;


    private bool roleIniciate = false;

    public Transform _weaponList;

    public Transform _playerPrefab;
    private GameObject playerPrefabInstantiate;
    [HideInInspector]public Animator playerAnimator;

    private SpriteRenderer prefabSprite;
    public float prefabAlpha;
    public float damageAnimationTimer;
    private float currentDamageAnimationTimer;

    //Movimiento del Role
    [HideInInspector] public float moveSpeed;
    private Vector3 movement;

    [HideInInspector]public float pickUpRange;

    private PlayerHealthController healthController;

    public float showDeadAnimationTimer;
    private bool playerDead = false;


    private void Awake()
    {
        instance = this;
    }

    void Start()
    {

        healthController = PlayerHealthController.instance;

        GameManager.instance.RegisterGameManager();

    }

    //测试
    public void IniciateRoleData()
    {
        roleData = GameManager.instance.roleSelected;

        //添加role prefab后取消comentar，根据选择到的role生成对应的prefab
        GameObject rolePrefab = Resources.Load<GameObject>(roleData.rolePrefab_location);
        playerPrefabInstantiate = Instantiate(rolePrefab, _playerPrefab);

        playerAnimator = playerPrefabInstantiate.GetComponent<Animator>();
        prefabSprite = playerPrefabInstantiate.GetComponent<SpriteRenderer>();


        List<Attribute> totalAttribute = AttributeManager.instance.TotalAttributeCalculation();
        UpdateRoleAttribute(totalAttribute);

        WeaponManager.instance._weaponList = _weaponList;
        WeaponManager.instance.InstantiateWeapon(roleData.initWeaponID);
        PoolObjectManager.instance.currentNumberWeapon++;

        roleIniciate = true;
    }


    public void UpdateRoleAttribute(List<Attribute> totalAttribute)
    {
        healthController.armor = totalAttribute[2].value;
        healthController.UpdateMaxHealth(totalAttribute[3].value);
        healthController.recovery = totalAttribute[4].value;

        moveSpeed = totalAttribute[10].value;
        pickUpRange = totalAttribute[11].value;

        ExperienceLevelController.instance.expIncrease = totalAttribute[12].value;
        CoinController.instance.coinIncrease = totalAttribute[13].value;

        if (UIController.instance.skipCountUpdate == false)
        {

            UIController.instance.skipCount = (int)totalAttribute[14].value;
            UIController.instance.skipCountUpdate = true;

            UIController.instance.CheckSkipCount();
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if (roleIniciate && playerDead == false) 
        {
            playerMove();
            TakeDamageAnimation();
        }
        
    }

    public float GetpickUpRange()
    {
        return pickUpRange;
    }

    private void playerMove()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        movement.Normalize();
        transform.position += movement * moveSpeed * Time.deltaTime;

        PlayerMovementAnimation(movement);


    }

    private void PlayerMovementAnimation(Vector2 movement)
    {
        bool isRunning = movement != Vector2.zero;
        playerAnimator.SetBool("isRun", isRunning);

        if (movement.x > 0)
            playerPrefabInstantiate.transform.localScale = new Vector3(1, 1, 1); 
        else if (movement.x < 0)
            playerPrefabInstantiate.transform.localScale = new Vector3(-1, 1, 1);
    }


    public IEnumerator RoleDeath()
    {
        playerAnimator.SetTrigger("isDeath");
        playerDead = true;
        GetComponent<Collider2D>().enabled = false;

        yield return new WaitForSeconds(showDeadAnimationTimer);

        gameObject.SetActive(false);
        SwitchPanelInGame.instance.ShowGameOverPanel();
    }

    private void TakeDamageAnimation()
    {
        if (currentDamageAnimationTimer > 0f)
        {
            currentDamageAnimationTimer -= Time.deltaTime;

            if (!Mathf.Approximately(prefabSprite.color.a, prefabAlpha))
            {
                Color color = prefabSprite.color;
                color.a = Mathf.Clamp01(prefabAlpha);
                prefabSprite.color = color;
            }
        }
        else
        {
            if (!Mathf.Approximately(prefabSprite.color.a, 1f))
            {
                Color color = prefabSprite.color;
                color.a = 1f;
                prefabSprite.color = color;
            }

            currentDamageAnimationTimer = 0f;
        }
    }

    public void playerTakeDamage()
    {
        currentDamageAnimationTimer = damageAnimationTimer;
    }


}
