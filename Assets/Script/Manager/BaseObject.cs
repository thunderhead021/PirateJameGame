using UnityEngine;

public class BaseObject : ScriptableObject
{
    public string infoName;
    public TargetType targetType;

    public virtual void DoThing(GameObject target)
    {

    }
}
