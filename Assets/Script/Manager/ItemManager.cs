using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public static ItemManager instance;

    [HideInInspector]
    public List<BaseItem> items = new();

    private void Awake()
    {
        instance = this;
    }
}