using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public List<BaseAttack> AttackList;
    public List<BaseSpell> SpellList;
    public List<BaseItem> ItemList;
    [SerializeField]
    public BaseEntity playerData;

    public static PlayerManager instance;

    private void Awake()
    {
        instance = this;
    }
}
