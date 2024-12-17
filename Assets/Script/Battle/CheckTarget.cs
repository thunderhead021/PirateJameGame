using Unity.VisualScripting;
using UnityEngine;

public class CheckTarget : MonoBehaviour
{
    public GameObject targetingUI;
    private void OnMouseEnter()
    {
        if (!BattleSceneManager.instance.canTarget)
            return;
        else if (BattleSceneManager.instance.targeting.curObject.targetType == TargetType.None)
            return;
        else if (BattleSceneManager.instance.targeting.curObject.targetType == TargetType.Self)
            return;
        else if (BattleSceneManager.instance.targeting.curObject.targetType == TargetType.All)
            return;
        else if (BattleSceneManager.instance.targeting.curObject.targetType == TargetType.Random)
            return;

        GetComponentInParent<Targeting>().curTarget = gameObject;
        GetComponentInParent<Targeting>().ChangeTarget();
    }

    private void OnMouseDown()
    {
        if (BattleSceneManager.instance.canTarget)
        {
            BattleSceneManager.instance.targeting.ObjectDoThing();
            BattleSceneManager.instance.Cancle();
            BattleSceneManager.instance.turnUI.NextTurn();
        }
    }
}
