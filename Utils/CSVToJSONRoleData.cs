using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;

public class CSVToJSONRoleData : MonoBehaviour
{
    public TextAsset roleDataCSV;

    private List<RoleData> roleDatas = new List<RoleData>();
    private List<RoleBonus> roleBonusDatas = new List<RoleBonus>();

    [ContextMenu("CSV TO JSON")]
    private void Convert()
    {
        StartCoroutine(ConvertCoroutine());
    }

    private IEnumerator ConvertCoroutine()
    {
        yield return StartCoroutine(ReadRoleDataCSV());

        roleDatas.Insert(0, null);

        linkData();

        string json = JsonConvert.SerializeObject(roleDatas, Formatting.Indented);
        string outputPath = Path.Combine(Application.dataPath, "Resources/Data/role.json");
        File.WriteAllText(outputPath, json);
        Debug.Log("Role JSON saved to: " + outputPath);
    }

    private IEnumerator ReadRoleDataCSV()
    {
        bool headerRoleData = false; bool headerRoleBonus = false;
        RoleData roleData;
        RoleBonus roleBonus;

        string[] lines = roleDataCSV.text.Split(new[] { "\r\n" }, System.StringSplitOptions.RemoveEmptyEntries);

        for (int i = 0; i < lines.Length; i++)
        {
            /*
            string[] fieldsOriginal = lines[i].Split(',');


            // 转为 List 并去除空或空白项
            List<string> fields = new List<string>(fieldsOriginal);
            */
            List<string> fields = ParseCSVLine(lines[i]);

            // 倒序遍历删除空或空白字段
            for (int j = fields.Count - 1; j >= 0; j--)
            {
                if (string.IsNullOrWhiteSpace(fields[j]))
                {
                    fields.RemoveAt(j);
                }
                else
                {
                    break;
                }
            }
            DebugFieldsContent(fields);

            Debug.Log("fields.Count: " + fields.Count);
            if (fields.Count == 0) continue;

            if (fields.Count == 10)
            {
                if (headerRoleData == false)
                {
                    headerRoleData = true;
                    continue;
                }
                Debug.Log(string.Join(", ",fields));
                roleData = new RoleData
                {
                    id = int.Parse(fields[0]),
                    name = fields[1],
                    avatar = fields[2],
                    description = fields[3],
                    slot = int.Parse(fields[4]),
                    record = int.Parse(fields[5]),
                    unlock = int.Parse(fields[6]),
                    unlockCondition = fields[7],
                    initWeaponID = int.Parse(fields[8]),
                    rolePrefab_location = fields[9]
                };

                roleDatas.Add(roleData);
                
            }

            else if (fields.Count == 5)
            {
                if (headerRoleBonus == false)
                {
                    headerRoleBonus = true;
                    continue;
                }
                Debug.Log(string.Join(", ", fields));
                roleBonus = new RoleBonus
                {
                    idRole = int.Parse(fields[0]),
                    id = int.Parse(fields[1]),
                    name = fields[2],
                    idBonus = int.Parse(fields[3]),
                    value = float.Parse(fields[4])
                };

                roleBonusDatas.Add(roleBonus);
            }
        }

        yield return null;
    }


    private void linkData()
    {

        for (int i = 1; i < roleDatas.Count; i++)
        {
            roleDatas[i].roleBonusDatas = new List<RoleBonus>();

        }

        foreach (RoleBonus roleBonus in roleBonusDatas)
        {
            Debug.Log(roleBonus.idRole);
            Debug.Log(roleDatas.Count);
            roleDatas[roleBonus.idRole].roleBonusDatas.Add(roleBonus);
        }
    }

    private void DebugFieldsContent(List<string> fields)
    {
        Debug.Log("---- Debugging Fields Content ----");
        for (int i = 0; i < fields.Count; i++)
        {
            Debug.Log($"Index {i}: \"{fields[i]}\"");
        }
        Debug.Log("---- End of Fields ----");
    }

    private List<string> ParseCSVLine(string line)
    {
        List<string> result = new List<string>();
        bool insideQuotes = false;
        string currentField = "";

        for (int i = 0; i < line.Length; i++)
        {
            char c = line[i];

            if (c == '"')
            {
                // 处理连续双引号（CSV中""代表一个"）
                if (insideQuotes && i + 1 < line.Length && line[i + 1] == '"')
                {
                    currentField += '"';
                    i++; // 跳过下一个引号
                }
                else
                {
                    insideQuotes = !insideQuotes;
                }
            }
            else if (c == ',' && !insideQuotes)
            {
                result.Add(currentField);
                currentField = "";
            }
            else
            {
                currentField += c;
            }
        }

        result.Add(currentField); // 添加最后一个字段
        return result;
    }

}
