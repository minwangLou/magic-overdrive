using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CloneUtils
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
}
