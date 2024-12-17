using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turns : MonoBehaviour
{
    public List<TurnIndicator> turnIndicators;

    public void Setup(List<TurnHelper> entities) 
    {
        int entitiesIndex = 0;
        for (int i = 0; i < turnIndicators.Count; i++) 
        {
            turnIndicators[i].Setup(entities[entitiesIndex]);
            if (entitiesIndex == entities.Count - 1)
                entitiesIndex = 0;
            else
                entitiesIndex++;
        }
        SetTurn(turnIndicators[0]);
    }

    public void NextTurn() 
    {
        if (turnIndicators.Count > 0)
        {
            Transform lastTurn = transform.GetChild(0);
            lastTurn.SetAsLastSibling();

            Transform thisTurn = transform.GetChild(0);
            SetTurn(thisTurn.GetComponent<TurnIndicator>());
            //do something here
        }
    }

    private void SetTurn(TurnIndicator turnIndicator) 
    {
        if (turnIndicator.Entity.gameObject.CompareTag("Enemy"))
        {
            BattleSceneManager.instance.SetEnemyTurn(true);
            turnIndicator.Entity.gameObject.GetComponent<CreateModel>().entity.AttackList[0].DoThing(PlayerManager.instance.gameObject);
            NextTurn();
        }
        else if (turnIndicator.Entity.gameObject.CompareTag("Player")) 
        {
            BattleSceneManager.instance.SetEnemyTurn(false);
        }
    }
}
