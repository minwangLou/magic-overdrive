using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils
{
    public static List<Attribute> DeepCopyAttributeList(List<Attribute> originalList)
    {
        List<Attribute> copiedList = new List<Attribute>();

        foreach (Attribute attr in originalList)
        {
            if (attr == null) continue;
            Attribute newAttr = new Attribute
            {
                idLevel = attr.idLevel,
                id = attr.id,
                name = attr.name,
                value = attr.value
            };

            copiedList.Add(newAttr);
        }

        copiedList.Insert(0, null);

        return copiedList;
    }

    public static string FormatTime(float seconds)
    {
        int min = Mathf.FloorToInt(seconds / 60);
        int sec = Mathf.FloorToInt(seconds % 60);
        return $"{min:00}:{sec:00}";
    }
}
