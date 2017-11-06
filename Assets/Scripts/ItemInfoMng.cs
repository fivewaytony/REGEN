using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInfoMng : MonoBehaviour
{
    public int ItemID;
    public int ItemAmount;

    /* 인벤토리 아이템 상세보기*/
    public void ShowItemInfo()
    {
        Debug.Log("ShowItemInfo");
        Debug.Log("ItemID=" + ItemID);
        Debug.Log("ItemAmount=" + ItemAmount);
        GameController.Instance.ShowItemInfoPanel(ItemID, ItemAmount);
    }
    
}
