using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemies/New Enemy")]
public class BaseEnemy : ScriptableObject
{
    [SerializeField]
    public List<BaseAttack> AttackList;
    [SerializeField]
    public List<BaseSpell> SpellList;
    [SerializeField]
    public List<BaseItem> ItemList;

    public GameObject Model;
    public string EntityName;
    public BaseEntity entityData;
}

[System.Serializable]
public class BaseEntity
{
    public float HP;
    public float Speed;
    public Sprite TurnIcon;
}