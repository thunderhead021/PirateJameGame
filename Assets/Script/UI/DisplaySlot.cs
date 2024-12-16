using TMPro;
using UnityEngine;

public class DisplaySlot : MonoBehaviour
{
    public TextMeshProUGUI text;
    public DisplayGrid displayGrid;
    public static DisplayInfo DisplayInfo = new();

    public void SetSlot(DisplayInfo displayInfo, DisplayGrid displayGrid) 
    {
        DisplayInfo = new(displayInfo.name, displayInfo.targetType); 
        text.text = displayInfo.name;
        this.displayGrid = displayGrid;
    }

    public void CallToBattleSceneManager() 
    {
        BattleSceneManager.instance.targeting.targetType = DisplayInfo.targetType;
        BattleSceneManager.instance.canTarget = true;
        BattleSceneManager.instance.Selected();
    }
}
