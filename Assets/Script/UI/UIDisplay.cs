using DanielLochner.Assets.SimpleScrollSnap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public enum GridType 
{
    None,
    Bag,
    Attack,
    Spell
}

public class UIDisplay : MonoBehaviour
{
    public GridType GridType;
    public GameObject Content;
    public GameObject DisplayGridGO;
    public SimpleScrollSnap SimpleScrollSnap;

    public void SetToBagGridType() => GridType = GridType.Bag;
    public void SetToAttackGridType() => GridType = GridType.Attack;
    public void SetToSpellGridType() => GridType = GridType.Spell;

    private void OnEnable()
    {
        Debug.Log("ENTER");
        while (Content.transform.childCount > 0)
        {
            SimpleScrollSnap.RemoveFromBack();
        }

        switch (GridType) 
        {
            case GridType.Bag:
                if (ItemManager.instance.items.Count > 0) 
                {
                    for (int i = 0; i < ItemManager.instance.items.Count;) 
                    {
                        var itemsList = GetSublist(ItemManager.instance.items, i, 4);
                        CreateDisplayGrid(itemsList.Cast<BaseObject>().ToList());
                        i += 4;
                    }
                }
                break;
            case GridType.Attack:
                if (AttackManager.instance.attacks.Count > 0)
                {
                    for (int i = 0; i < AttackManager.instance.attacks.Count;)
                    {
                        var attacksList = GetSublist(AttackManager.instance.attacks, i, 4);
                        CreateDisplayGrid(attacksList.Cast<BaseObject>().ToList());
                        i += 4;
                    }
                }
                break;
            case GridType.Spell:
                if (SpellManager.instance.spells.Count > 0)
                {
                    for (int i = 0; i < SpellManager.instance.spells.Count;)
                    {
                        var spellsList = GetSublist(SpellManager.instance.spells, i, 4);
                        CreateDisplayGrid(spellsList.Cast<BaseObject>().ToList());
                        i += 4;
                    }
                }
                break;
        }
    }

    private void CreateDisplayGrid(List<BaseObject> displayObjects) 
    {
        GameObject newDisplayGridGO = Instantiate(DisplayGridGO);

        List<DisplayInfo> displayInfos = new();
        foreach (BaseObject displayObject in displayObjects)
            displayInfos.Add(displayObject.DisplayInfo);

        newDisplayGridGO.GetComponent<DisplayGrid>().SetInfo(displayInfos);
        SimpleScrollSnap.AddToBack(newDisplayGridGO);
        Destroy(newDisplayGridGO);
    }

    static List<T> GetSublist<T>(List<T> list, int startIndex, int count)
    {
        List<T> sublist = new();

        // Add elements from the original list if available
        for (int i = startIndex; i < startIndex + count; i++)
        {
            if (i < list.Count)
            {
                sublist.Add(list[i]);
            }
            else
            {
                // If there are not enough elements, add the default value of the type
                sublist.Add(default);
            }
        }
        sublist.RemoveAll(element => EqualityComparer<T>.Default.Equals(element, default));

        return sublist;
    }
}
