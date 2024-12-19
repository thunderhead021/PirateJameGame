using UnityEngine;
using UnityEngine.UI;

public class TurnIndicator : MonoBehaviour
{
    public Image Icon;
    public TurnHelper Entity;
    public void Setup(TurnHelper entity) 
    {
        Entity = entity;
        Icon.sprite = entity.baseEntity.TurnIcon;
    }
}
