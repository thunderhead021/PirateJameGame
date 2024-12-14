using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DisplaySlot : MonoBehaviour
{
    public TextMeshProUGUI text;
    public DisplayGrid displayGrid;

    public void SetSlot(DisplayInfo displayInfo, DisplayGrid displayGrid) 
    {
        text.text = displayInfo.name;
        this.displayGrid = displayGrid;
    }
}
