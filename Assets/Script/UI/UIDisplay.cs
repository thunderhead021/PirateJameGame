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
                if (PlayerManager.instance.ItemList.Count > 0) 
                {
                    for (int i = 0; i < PlayerManager.instance.ItemList.Count;) 
                    {
                        var itemsList = GetSublist(PlayerManager.instance.ItemList, i, 4);
                        CreateDisplayGrid(itemsList.Cast<BaseObject>().ToList());
                        i += 4;
                    }
                }
                break;
            case GridType.Attack:
                if (PlayerManager.instance.AttackList.Count > 0)
                {
                    for (int i = 0; i < PlayerManager.instance.AttackList.Count;)
                    {
                        var attacksList = GetSublist(PlayerManager.instance.AttackList, i, 4);
                        CreateDisplayGrid(attacksList.Cast<BaseObject>().ToList());
                        i += 4;
                    }
                }
                break;
            case GridType.Spell:
                if (PlayerManager.instance.SpellList.Count > 0)
                {
                    for (int i = 0; i < PlayerManager.instance.SpellList.Count;)
                    {
                        var spellsList = GetSublist(PlayerManager.instance.SpellList, i, 4);
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

        newDisplayGridGO.GetComponent<DisplayGrid>().SetInfo(displayObjects);
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
