using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;

public class AttributeManager : MonoBehaviour
{
    public static AttributeManager instance;

    public TextAsset baseAttributeTextAsset;

    public List<Attribute> baseAtributeDatas; //Lista Index empieza en 1
    public List<BonusData> bonusDatas; //Lista Index empieza en 1
    public List<RoleBonus> roleBonusDatas; //Lista Index empieza en 1

    private void Awake()
    {
        instance = this;

    }

    private void Start()
    {
        baseAtributeDatas = JsonConvert.DeserializeObject<List<Attribute>>(baseAttributeTextAsset.text);
        baseAtributeDatas.Insert(0, null);

        bonusDatas = SaveManager.instance.bonusDatas;


    }

    //Se realiza en em momento de empezar la partida
    //Según el RoleData proporcionado, obtener el roleBonus del role seleccionado para partido
    //Configurar la lista de bonusDatas para que contenga todo los datos necesarios

    public void SetUpRoleData(RoleData roleData)
    {
        roleBonusDatas = roleData.roleBonusDatas;
        AssignRoleAttributeBonus();
        TotalBonusValueCalculation();
        
    }

    



    //Calcular el valor porcentaje o valor directo que va a anadir al atributo del role
    //se realiza en el caso de:
    //1. visualizar las característica de cada role
    //2. cuando inicializa el juego, para asignar el atributo inicial al role (debe asignar a la clase de attributeManager roleBonusDatas correspondiente)
    //3. despúes de realizar level up sin elegir opción de skip ni objeto tipo weapon
    public void TotalBonusValueCalculation()
    {
        foreach (BonusData bonusData in bonusDatas)
        {
            if (bonusData != null)
            {
                if (roleBonusDatas != null) //durante un partido del juego
                {
                    bonusData.totalValue = bonusData.roleBonusValue +
                                          (bonusData.outGameCurrentLevel * bonusData.valuePerLevelOutGame) +
                                          (bonusData.inGameCurrentLevel * bonusData.valuePerLevelInGame);
                }
                else //antes de un partido del juego
                {
                    bonusData.totalValue = (bonusData.outGameCurrentLevel * bonusData.valuePerLevelOutGame) +
                                            bonusData.roleBonusValue;
                }
            }
        }
    }


    //Ejecutar antes que TotalBonusValueCalculation() para visualizar los datos de role
    //Ejecutar cuando elige el role que quería jugar para visualizar característica de cada role
    //Ejecutar cuando inicializa el partido, mediante role selected, para tenerlo guradado en List<BonusDatas> bonusDatas
    //Ejecutar una vez
    public void AssignRoleAttributeBonus()
    {

        foreach (RoleBonus roleBonus in roleBonusDatas)
        {
            /*
            Debug.Log(roleBonus.name);
            Debug.Log(roleBonus.idBonus);
            Debug.Log(bonusDatas[1] == null);
            */

            bonusDatas[roleBonus.idBonus].roleBonusValue += roleBonus.value;
        }


    }

    //Debe ejecutar primero TotalValueCalculation(), para calcular totalValue obtenido en todo los bonus
    //Attribute para role and game
    //Se realiza cuando empieza la partida
    //Asigna valor de role atributo para el role,
    public List<Attribute> TotalAttributeCalculation ()
    {
        List<Attribute> totalAttribute = CloneUtils.DeepCopyAttributeList(baseAtributeDatas);
        /*
        for (int i = 1; i < totalAttribute.Count; i++)
        {
            totalAttribute[i].Info();
        }
        */

        for (int i = 1; i< bonusDatas.Count; i++)
        {
            totalAttribute[bonusDatas[i].idAttribute].value = BonusTypeCalculation(bonusDatas[i], null);
        }

        return totalAttribute;
    }


    //Debe ejecutar primero TotalValueCalculation(), para calcular totalValue obtenido en todo los bonus
    //Calcula atributo para cada weapon independiente según el estado que tiene.
    //Devolver la lista de Attribute de nivel actualzado, debe reemplazarlo de originar
    //Se realiza en el caso de selecionar en level up upgrate el objeto de tipo weapon
    public List<Attribute> TotalAttributeCalculation(List <Attribute> weaponAttribute)
    {
        List<Attribute> totalAttribute = CloneUtils.DeepCopyAttributeList(baseAtributeDatas);
        foreach (BonusData bonus in bonusDatas)
        {
            if (bonus != null)
            {
                //totalAttribute[bonusData.idAttribute]
                totalAttribute[bonus.idAttribute].value = 
                    BonusTypeCalculation(bonus, weaponAttribute[bonus.idAttribute]);
            }
        }

        return totalAttribute;

    }

    public void InfoTest()
    {
        Debug.Log(baseAtributeDatas.Count);

        for (int i = 1; i < bonusDatas.Count; i++)
        {
            bonusDatas[i].Info();
        }


        for (int i = 1; i < baseAtributeDatas.Count; i++)
        {
            baseAtributeDatas[i].Info();
        }
    }


    private float BonusTypeCalculation(BonusData bonus, Attribute weaponAttribute)
    {
        float result = 0;
        if (bonus.bonusType == BonusType.DirectBonus) //bonus tipo directo
        {
            //calcular attribute solo para role y gameAttribute
            result = bonus.totalValue + baseAtributeDatas[bonus.idAttribute].value;

            if (weaponAttribute != null)//añadir attributo de weapon para asignarle atributo nuevo
            {
                result += weaponAttribute.value;
            }

        }
        else //bonus tipo porcentaje
        {
            result = baseAtributeDatas[bonus.idAttribute].value;

            if (weaponAttribute != null)//añadir attributo de weapon para asignarle atributo nuevo
            {
                result += weaponAttribute.value;
            }

            if (bonus.id == 5)//cooldown
            {
                result = result * (1 - bonus.totalValue);
            }
            else
            {
                result = result * (1 + bonus.totalValue);
            }

        }

        return result;
    }

    public void SelectBonusToUpgrate (int idBonus)
    {
        if (bonusDatas[idBonus].inGameCurrentLevel == 0)
        {
            ObjectUIManager.instance.AddBonusIcon(bonusDatas[idBonus]);
            PoolObjectManager.instance.currentNumberBonus++;
        }

        bonusDatas[idBonus].inGameCurrentLevel++;

        TotalBonusValueCalculation();

        //Actualizar atributos de Role
        List<Attribute> attributeToRole = TotalAttributeCalculation();
        PlayerController.instance.UpdateRoleAttribute(attributeToRole);

        //Actualizar atributos de todas las armas
        WeaponManager.instance.UpdateAllWeaponAttributes();

    }





}
