using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInfoMng : MonoBehaviour {

    public int ItemID;
    public int ItemAmount;

    public void ShowItemInfo()
    {
        InventoryController.invenInstance.ShowItemInfoPanel(ItemID, ItemAmount);
     }

}
