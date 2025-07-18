﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;

public class CSVToJson : MonoBehaviour
{
 
    public TextAsset weaponDataCSV;

    private List<Attribute> attributes = new List<Attribute>();
    private List<WeaponLevelAttribute> levelAttributes = new List<WeaponLevelAttribute>();
    private WeaponData weaponData = new WeaponData();

    [ContextMenu("CSV TO JSON")]
    private void Convert()
    {
        StartCoroutine(Ejecutar());
    }


    private IEnumerator Ejecutar()
    {
        attributes.Insert(0, null);
        levelAttributes.Insert(0, null);

        yield return (ReadWeaponDataCSV());
        linkData();

        string json = JsonConvert.SerializeObject(weaponData, Formatting.Indented);
        string outputPath = Path.Combine(Application.dataPath + "/Resources/Data/Weapon", weaponData.name + ".json");
        File.WriteAllText(outputPath, json);
        Debug.Log("Weapon JSON saved to: " + outputPath);

        attributes = new List<Attribute>();
        levelAttributes = new List<WeaponLevelAttribute>();
        weaponData = new WeaponData(); 
    }

    private IEnumerator ReadWeaponDataCSV()
    {

        bool headerAttribute = false, headerLevelAttribute = false, headerWeapon = false;
        Attribute attr;
        WeaponLevelAttribute levelAttr;
        WeaponData weapon;  


        string[] lines = weaponDataCSV.text.Split(new[] { "\r\n" }, System.StringSplitOptions.RemoveEmptyEntries);

        for (int i = 0; i < lines.Length; i++)
        {
            
            string[] fieldsOriginal = lines[i].Split(',');

            
            // 转为 List 并去除空或空白项
            List<string> fields = new List<string>(fieldsOriginal);

            // 倒序遍历删除空或空白字段
            for (int j = fields.Count - 1; j >= 0; j--)
            {
                if (string.IsNullOrWhiteSpace(fields[j]))
                {
                    fields.RemoveAt(j);
                }
            }



            if (fields.Count == 0) continue;

            //Read Attribute Data column
            if (fields.Count == 9)
            {
                if (headerWeapon == false) ////Jump Weapon Data header
                {
                    headerWeapon = true;
                    continue;
                }
                weapon = new WeaponData
                {
                    id = int.Parse(fields[0]),
                    name = fields[1],
                    weaponIcon_location = fields[2],
                    rarityWeight = int.Parse(fields[3]),
                    currentLevel = int.Parse(fields[4]),
                    maxLevel = int.Parse(fields[5]),
                    weaponPrefab_location = fields[6],
                    unloke = int.Parse(fields[7]),
                    knockBackForce = float.Parse(fields[8])
                };


                weaponData = weapon;

            }else

            if (fields.Count == 2)
            {
                if (headerLevelAttribute == false) //Jump Weapon Level Attribute header
                {
                    headerLevelAttribute = true; 
                    continue;
                }


                levelAttr = new WeaponLevelAttribute
                {
                    id = int.Parse(fields[0]),
                    upgrateText = fields[1],
                };


                levelAttributes.Add(levelAttr);
            }else


            if (fields.Count == 4)
            {
                if (headerAttribute == false) //Jump Attribute header
                {
                    headerAttribute = true;
                    continue;
                }
                attr = new Attribute
                {
                    
                    idLevel = int.Parse(fields[0]),
                    id = int.Parse(fields[1]),
                    name = fields[2],
                    value = float.Parse(fields[3])
                };

                Debug.Log("idLevel: " + attr.idLevel + 
                            "\nid: " + attr.id + 
                            "\nname: " + attr.name +
                            "\nvalue; " + attr.value);
                attributes.Add(attr);
            }



            
        }
        yield return null;
    }

    private void linkData()
    {
        foreach (WeaponLevelAttribute levelAttribute in levelAttributes)
        {
            if (levelAttribute != null)
            {
                levelAttribute.currentLevelAttribute = new List<Attribute>();
                
                Debug.Log((attributes.Count - 1) / (levelAttributes.Count - 1));
                Debug.Log((attributes.Count - 1));
                Debug.Log((levelAttributes.Count - 1));

                for (int i = 0; i< ((attributes.Count-1)/ (levelAttributes.Count-1)); i++) //number of Attribute (71-1) / (6-1) = 14
                {
                    levelAttribute.currentLevelAttribute.Add(null);
                }
                assignAttributeToLevel(levelAttribute);
                levelAttribute.currentLevelAttribute.Insert(0, null);
            }
        }
        //Debug.Log(levelAttributes.Count);
        weaponData.weaponAttribute = new List<WeaponLevelAttribute>();
        for (int i = 0; i< levelAttributes.Count; i++)
        {
            weaponData.weaponAttribute.Add(null);
        }
        assignLevelToWeapon(weaponData);


    }

    private void assignAttributeToLevel(WeaponLevelAttribute levelAttr)
    {
        int contador = 0;

        foreach (Attribute attr in attributes)
        {
            
            if (attr != null && attr.idLevel == levelAttr.id)
            {
                levelAttr.currentLevelAttribute[contador] = attr;
                contador++;
            }
            
        }

        Debug.Log("contador:" + contador);



    }

    private void assignLevelToWeapon (WeaponData weapon)
    {

        foreach (WeaponLevelAttribute level in levelAttributes)
        {
            if (level != null)
            {
                weapon.weaponAttribute[level.id] = level;
            }
        }
    }

}


