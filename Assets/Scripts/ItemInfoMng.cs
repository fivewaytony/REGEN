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
        GameController.Instance.ShowItemInfoPanel(ItemID, ItemAmount);
    }
}
