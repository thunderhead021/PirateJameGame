using System.Collections.Generic;
using UnityEngine;

public class AttackManager : MonoBehaviour
{
    public static AttackManager instance;

    [HideInInspector]
    public List<BaseAttack> attacks = new();

    private void Awake()
    {
        instance = this;
        BaseAttack attack = new Slash();
        attack.AddInfo();
        attacks.Add(attack);
    }
}
