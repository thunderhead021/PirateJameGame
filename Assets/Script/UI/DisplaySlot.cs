using TMPro;
using UnityEngine;

public class DisplaySlot : MonoBehaviour
{
    public TextMeshProUGUI text;
    public DisplayGrid displayGrid;
    public BaseObject DisplayInfo;

    public void SetSlot(BaseObject displayInfo, DisplayGrid displayGrid) 
    {
        DisplayInfo = displayInfo; 
        text.text = displayInfo.infoName;
        this.displayGrid = displayGrid;
    }

    public void CallToBattleSceneManager() 
    {
        BattleSceneManager.instance.targeting.curObject = DisplayInfo;
        BattleSceneManager.instance.canTarget = true;
        BattleSceneManager.instance.Selected();
    }
}
