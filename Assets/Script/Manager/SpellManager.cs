using System.Collections.Generic;
using UnityEngine;

public class SpellManager : MonoBehaviour
{
    public static SpellManager instance;

    [HideInInspector]
    public List<BaseSpell> spells = new();

    private void Awake()
    {
        instance = this;
    }
}
