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
        if (copiedList[0] != null)
        {
            copiedList.Insert(0, null);
        }

        return copiedList;
    }

    public static string FormatTime(float seconds)
    {
        int min = Mathf.FloorToInt(seconds / 60);
        int sec = Mathf.FloorToInt(seconds % 60);
        return $"{min:00}:{sec:00}";
    }

    public static string ValueBonusToString (float value, BonusType bonusType)
    {
        if (bonusType == BonusType.DirectBonus)
        {
            return value.ToString("+0.#;-0.#");
        }
        else
        {
            return (value * 100f).ToString("+0.#;-0.#") + "%";
        }
    }

    public static IEnumerator DestroyAfterDuration(Animator anim, float duration, GameObject gameObject)
    {
        yield return new WaitForSeconds(duration);

        anim.SetTrigger("Despawn");
        yield return new WaitForEndOfFrame(); // 等一帧让 Animator 切入新状态
        AnimatorStateInfo state = anim.GetCurrentAnimatorStateInfo(0);
        float animLength = state.length;

        // 等待动画播完
        yield return new WaitForSeconds(animLength);

        Object.Destroy(gameObject);
    }
}
