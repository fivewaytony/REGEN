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
        InventoryController.invenInstance.invenItemInfoPan(ItemID, ItemAmount);
       // GameController.Instance.ShowItemInfoPanel(ItemID, ItemAmount);
    }

    /* 대장간 아이템 상세보기 */
    public void ForgeShowitemInfo()
    {
        ForgeController.forgeInstance.forgeItemInfoPan(ItemID, ItemAmount);
    }

}
