using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Linq;


public class InventoryController : GameController
{
    public Transform SlotsParentContent; //인벤토리 부모 팬넬
    public Transform InventorySlot; //각 슬롯
    private int slotAmount = 42;
    public List<GameObject> slots = new List<GameObject>();
    public GameObject ItemInfoBackPanel;

    public Text ItemInfoNameText, ItemInfoDescText;

    private int pssItemID;
    public static InventoryController invenInstance;
    
    void Start () {
        invenInstance = this;
        PlayerPssItemLoadALL(); //전체 소유 아이템 로드
        
    }
    #region [플레이어 소유 아이템 전체 로드]
    protected void PlayerPssItemLoadALL()
    {
        for (int i = 0; i < slotAmount; i++)
        {
            GameObject SlotInfo = Resources.Load("Prefabs/InventorySlot") as GameObject;  //프리팹으로 등록된 정보 불러옴
            GameObject objslot = Instantiate(SlotInfo, SlotsParentContent);   //자식 오브젝트
            slots.Add(objslot);

            RectTransform rt = objslot.GetComponent<RectTransform>(); //SlotInfo
            rt.anchoredPosition = new Vector2(0f, 0f);       // 자식 오브젝트를 위치를 잡고 그린다
        }

         List<PssItem> passitems = DataController.Instance.GetPssItemInfo().PssItemList; //소유 아이템
         List<GameItemInfo> itemList = DataController.Instance.GetGameItemInfo().GameItemList;

        for (int i = 0; i < passitems.Count; i++)
        {
            foreach (GameItemInfo item in itemList)
            {
                if (passitems[i].Item_ID == item.Item_ID)
                {
                    Color color = slots[i].transform.GetChild(0).GetComponent<Image>().color;
                    color.a = 1f;
                    slots[i].transform.GetChild(0).GetComponent<Image>().color = color;

                   // Debug.Log("item.Item_ImgName=" + item.Item_ImgName);
                    slots[i].transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Inventory/" + item.Item_ImgName);
                    slots[i].transform.GetChild(1).GetComponent<Text>().text = passitems[i].Amount.ToString();

                    slots[i].GetComponent<ItemInfoMng>().ItemID = passitems[i].Item_ID;

                    break;
                }
                
            }
         }

    }

    #endregion

    #region [아이템 정보 Panel Show / Close]
    public void ShowItemInfoPanel(int ItemID)
    {
        ItemInfoBackPanel.gameObject.SetActive(true);
        GameItemInfo item = DataController.Instance.gameitemDic[ItemID];
        ItemInfoNameText.text = item.Item_Name;
        ItemInfoDescText.text = "판매가격 : " + item.Item_Price.ToString() + "골드";
        if (item.Item_Type == "Stuff")
        {
            ItemInfoDescText.text = ItemInfoDescText.text + "\n\n아이템 제조의\n재료로 필요합니다.";
        }
    }

    public void CloseItemInfoPanel()
    {
        ItemInfoBackPanel.gameObject.SetActive(false);
    }
    #endregion


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GoMain();
        }
    }
    /*
    아이템 설명 : 이름, 용도, 
    장착 아이템 표시
    같은 무기는 무조건 하나만 들 수 있음 --> 무기, 방어구, 장신구는 무조건 제조를 통해서만 가능하며
    제조시 장착하고 있는 무기, 방어구, 장신구는 또 제조 할 수 없도록 제조 리스트에서 보여주지 않는다

      */


    #region [삭제예정]
    //List<WeaponInfo> weaponinfos = DataController.Instance.GetWeaponInfo().WeaponList;

    //for (int i = 0; i < passitems.Count; i++)
    //{
    //     //무기로딩
    //    foreach (WeaponInfo weapon in weaponinfos)
    //    {
    //        if (passitems[i].Item_Type == "Weapon" && weapon.Wpn_ID == passitems[i].Item_ID)
    //        {
    //            Color color = slots[i].transform.GetChild(0).GetComponent<Image>().color;
    //            color.a = 1f;
    //            slots[i].transform.GetChild(0).GetComponent<Image>().color = color;
    //            slots[i].transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Inventory/Weapon/" + weapon.Wpn_ImgName);
    //            slots[i].transform.GetChild(1).GetComponent<Text>().text = passitems[i].Amount.ToString();
    //            break;
    //        }
    //    }
    //    //물약로딩
    //    foreach (WeaponInfo weapon in weaponinfos)
    //    {
    //        if (passitems[i].Item_Type == "Hpotion")
    //        {
    //            Color color = slots[i].transform.GetChild(0).GetComponent<Image>().color;
    //            color.a = 1f;
    //            slots[i].transform.GetChild(0).GetComponent<Image>().color = color;
    //            slots[i].transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Inventory/Hp");
    //            slots[i].transform.GetChild(1).GetComponent<Text>().text = passitems[i].Amount.ToString();
    //            break;
    //        }
    //    }
    //    //재료로딩
    //    foreach (StuffInfo stuff in stuffList)
    //    {
    //        if(passitems[i].Item_Type == "Stuff" && stuff.Stuff_ID == passitems[i].Item_ID)
    //        {
    //            Color color = slots[i].transform.GetChild(0).GetComponent<Image>().color;
    //            color.a = 1f;
    //            slots[i].transform.GetChild(0).GetComponent<Image>().color = color;
    //            slots[i].transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Inventory/Stuff/" + stuff.Stuff_ImgName);
    //            slots[i].transform.GetChild(1).GetComponent<Text>().text = passitems[i].Amount.ToString();

    //            slots[i].GetComponent<ItemInfoMng>().ItemID = passitems[i].Item_ID;
    //            //Image[] items= ItemInfoBackPanel.GetComponentsInChildren<Image>();
    //            //foreach (Image item in items)
    //            //{

    //            //   // item. => GoHunting(fielditem.Field_Level));
    //            //}
    //            break;
    //        }
    //    }

    //}
    #endregion
}
