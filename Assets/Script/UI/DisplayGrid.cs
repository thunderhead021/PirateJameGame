using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayGrid : MonoBehaviour
{
    public List<DisplaySlot> allSlots = new();

    public void SetInfo(List<DisplayInfo> displayInfos) 
    {
        if (displayInfos.Count <= 4 && displayInfos.Count > 0) 
        {
            for (int i = 0; i < displayInfos.Count; i++) 
            {
                allSlots[i].gameObject.SetActive(true);
                allSlots[i].SetSlot(displayInfos[i], this);
            }
        }
    }
}
