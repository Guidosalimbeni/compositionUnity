using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UpdateCountUIonSelectionItem : MonoBehaviour
{
    private TextMeshProUGUI ItemCount;
    private SelectorOfItemInCompositionStart selectorOfItemInCompositionStart;

    private int IDofCurrentItem;
    private int count;

    private void Start()
    {
        selectorOfItemInCompositionStart = FindObjectOfType<SelectorOfItemInCompositionStart>();
        ItemCount = GetComponent<TextMeshProUGUI>();

        selectorOfItemInCompositionStart.OnSelectionADDEDOfItemID += Handle_OnSelectionADDEDOfItemID;
        selectorOfItemInCompositionStart.OnSelectionREMOVEDOfItemID += handle_OnSelectionREMOVEDOfItemID;

        IDofCurrentItem = GetComponent<CurrentIDitemTag>().ID;

    }
    

    private void Handle_OnSelectionADDEDOfItemID(int ID)
    {
        if (ID == IDofCurrentItem)
        {
            count++;
            ItemCount.text = count.ToString();

        }
    }

    private void handle_OnSelectionREMOVEDOfItemID(int ID)
    {
        if (ID == IDofCurrentItem && count > 0)
        {
            count--;
            ItemCount.text = count.ToString();
        }
    }

    private void OnDisable()
    {
        selectorOfItemInCompositionStart.OnSelectionADDEDOfItemID -= Handle_OnSelectionADDEDOfItemID;
        selectorOfItemInCompositionStart.OnSelectionREMOVEDOfItemID -= handle_OnSelectionREMOVEDOfItemID;
    }
}
