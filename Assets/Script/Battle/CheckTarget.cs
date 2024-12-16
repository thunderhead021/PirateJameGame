using UnityEngine;

public class CheckTarget : MonoBehaviour
{
    public GameObject targetingUI;
    private void OnMouseEnter()
    {
        if (!BattleSceneManager.instance.canTarget)
            return;
        else if (BattleSceneManager.instance.targeting.targetType == TargetType.None)
            return;
        else if (BattleSceneManager.instance.targeting.targetType == TargetType.Self)
            return;
        else if (BattleSceneManager.instance.targeting.targetType == TargetType.All)
            return;
        else if (BattleSceneManager.instance.targeting.targetType == TargetType.Random)
            return;

        GetComponentInParent<Targeting>().curTarget = gameObject;
        GetComponentInParent<Targeting>().ChangeTarget();
    }

    private void OnMouseExit()
    {
        //Debug.Log("exit " + this.gameObject.name);
    }
}
