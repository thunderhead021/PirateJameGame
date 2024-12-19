using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public enum TargetType 
{
    None,
    Single,
    Triple,
    Ememies,
    Self,
    All,
    Random
}

public class Targeting : MonoBehaviour
{

    public List<GameObject> enemies;
    public BaseObject curObject;
#nullable enable
    public GameObject? curTarget = null;
#nullable disable
    
    private List<GameObject> targets = new();

    public void ChangeTarget() 
    {
        ResetTarget();
        int curPos = GetCurTargetPos();
        switch (curObject.targetType) 
        {
            case TargetType.Single:
                if (curPos >= 0) 
                {
                    curTarget.GetComponent<CheckTarget>().targetingUI.SetActive(true);
                    targets.Add(curTarget);
                    BattleSceneManager.instance.canTarget = true;
                }
                break;
            case TargetType.Triple:
                if (curPos >= 0) 
                {
                    curTarget.GetComponent<CheckTarget>().targetingUI.SetActive(true);
                    targets.Add(curTarget);
                    if (curPos - 1 >= 0) 
                    {
                        enemies[curPos - 1].GetComponent<CheckTarget>().targetingUI.SetActive(true);
                        targets.Add(enemies[curPos - 1]);
                    }
                    if (curPos + 1 <= enemies.Count - 1)
                    {
                        enemies[curPos + 1].GetComponent<CheckTarget>().targetingUI.SetActive(true);
                        targets.Add(enemies[curPos + 1]);
                    }
                    BattleSceneManager.instance.canTarget = true;
                }
                break;
            case TargetType.Ememies:
                foreach (var target in enemies) 
                {
                    target.GetComponent<CheckTarget>().targetingUI.SetActive(true);
                    targets.Add(target);
                }
                BattleSceneManager.instance.canTarget = true;
                break;
        }
    }

    public int GetCurTargetPos() 
    {
        if (curTarget == null)
            return -1;
        else
        {
            for (int i = 0; i < enemies.Count; i++)
            {
                if( enemies[i] == curTarget) return i;
            }
        }
        return -1;
    }

    public void ResetTarget() 
    {
        foreach (var target in enemies) 
        {
            target.GetComponent<CheckTarget>().targetingUI.SetActive(false);
        }
        BattleSceneManager.instance.canTarget = false;
        targets.Clear();
    }

    public void ObjectDoThing() 
    {
        foreach (var target in targets) 
        {
            curObject.DoThing(target);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (BattleSceneManager.instance.canTarget) 
        {
            if (Input.GetMouseButtonDown(1)) 
            {
                BattleSceneManager.instance.Cancle();
            }
        }
    }
}
