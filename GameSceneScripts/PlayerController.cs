using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    public RoleData roleData;

    //public Transform _playerPrefab;

    private bool roleIniciate = false;

    public Transform _weaponList;

    private void Awake()
    {
        instance = this;
    }

    //public List<Weapon> unassignedWeapons, assignedWeapons;

    public float moveSpeed;
    private Vector3 movement;

    public float pickUpRange;

    public PlayerHealthController healthController;
    //public int maxWeapons = 3;


    
    //public List<Weapon> fullyLevelledWeapons = new List<Weapon>();



    void Start()
    {
        /*
        //random.range nunca se conge el valor más alto, por tanto no es necesario añadir un -1 al valor obtenido de Count
        if (assignedWeapons.Count == 0)
        {
            AddWeapon(Random.Range(0, unassignedWeapons.Count));
        }

        ActivateWeaponStartGame();
        */

        healthController = PlayerHealthController.instance;

        GameManager.instance.RegisterGameManager();

    }

    //测试
    public void IniciateRoleData()
    {
        roleData = GameManager.instance.roleSelected;

        //添加role prefab后取消comentar，根据选择到的role生成对应的prefab
        /*
        GameObject rolePrefab = Resources.Load<GameObject>(roleData.rolePrefab_location);
        Instantiate(rolePrefab, _playerPrefab);
        */
        List<Attribute> totalAttribute = AttributeManager.instance.TotalAttributeCalculation();
        UpdateRoleAttribute(totalAttribute);

        WeaponManager.instance._weaponList = _weaponList;
        WeaponManager.instance.InstantiateWeapon(roleData.initWeaponID);

        roleIniciate = true;
    }


    public void UpdateRoleAttribute(List<Attribute> totalAttribute)
    {
        healthController.armor = totalAttribute[2].value;
        healthController.maxHealth = totalAttribute[3].value;
        healthController.recovery = totalAttribute[4].value;

        moveSpeed = totalAttribute[10].value;
        pickUpRange = totalAttribute[11].value;

        ExperienceLevelController.instance.expIncrease = totalAttribute[12].value;
        CoinController.instance.coinIncrease = totalAttribute[13].value;


        //añadir los atributos de growth, greed y magnet.
        //añadir la actualización de estos atributos.
    }

    // Update is called once per frame
    void Update()
    {
        if (roleIniciate) 
        {
            playerMove();
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
    }


    /*


    private void ActivateWeaponStartGame()
    {
        for (int i = 0; i < unassignedWeapons.Count; i++)
        {
            unassignedWeapons[i].gameObject.SetActive(false);
        }

        assignedWeapons[0].gameObject.SetActive(true);
    }




    public void AddWeapon (int weaponNumber)
    {
        if (weaponNumber < unassignedWeapons.Count)
        {
            assignedWeapons.Add(unassignedWeapons[weaponNumber]);


            unassignedWeapons.RemoveAt(weaponNumber);
        }
    }

    public void AddWeapon (Weapon weaponToAdd)
    {

        assignedWeapons.Add(weaponToAdd);
        unassignedWeapons.Remove(weaponToAdd);

        weaponToAdd.gameObject.SetActive(true);
    }
    */
}
