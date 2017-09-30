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
   // public Transform InventorySlot; //각 슬롯
    private int slotAmount = 104;     //슬록 개수
    public List<GameObject> slots = new List<GameObject>(); //각 슬록 List

    public GameObject ItemInfoBackPanel;    //아이템 상세보기 panel
    public Text ItemInfoNameText, ItemInfoDescText; //아이템 정보
    public Image ItemCountBarFill;  //아이템 개수 설정 bar
    public Text ItemCountTxt;       //아이템개수 Text
    public Slider ItemCountSilder; //아이템 개수 선택 슬라이더
  
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
                    slots[i].GetComponent<ItemInfoMng>().ItemAmount = passitems[i].Amount;
                    break;
                }
                
            }
         }

    }

    #endregion

    #region [아이템 정보 Panel Show / Close]
    public void ShowItemInfoPanel(int ItemID, int ItemAmount)
    {
        ItemInfoBackPanel.gameObject.SetActive(true);
        GameItemInfo item = DataController.Instance.gameitemDic[ItemID];
        ItemInfoNameText.text = item.Item_Name;
        ItemInfoDescText.text = "판매가격 : " + item.Item_Price.ToString() + "골드";
        if (item.Item_Type == "Stuff")
        {
            ItemInfoDescText.text = ItemInfoDescText.text + "\n\n아이템 제조의 재료로 필요합니다.";
        }
      
        /*개수 Silder*/
        ItemCountSilder.minValue = 1f;   //1개부터
        ItemCountSilder.maxValue = ItemAmount;  //소유 아이템 개수 까지
        ItemCountTxt.text = ItemCountSilder.value.ToString();

        /* 현재 선택 아이템 정보 */
        SelectItemID = ItemID;
        SelectItemAmount = Convert.ToInt32(ItemCountTxt.text); //선택 개수
        SelectItemPrice = item.Item_Price; //단가(골드)
    }

    //Silder 변경하면 개수 표시
    public void ItemCountSilderChange()  
    {
        ItemCountTxt.text = ItemCountSilder.value.ToString();
        SelectItemAmount = Convert.ToInt32(ItemCountTxt.text); //선택 개수
    }
    
    //아이템 팔기
    public void SellpssItem()
    {
        //소유 아이템에서 빼기
        //Gold 더하기 : 판매할 아이템 단가 구하기
        
        Debug.Log("SelectItemID=" + SelectItemID);
        Debug.Log("SelectItemAmount=" + SelectItemAmount);
        Debug.Log("SelectItemPrice=" + SelectItemPrice);

        List<PssItem> passitems = DataController.Instance.GetPssItemInfo().PssItemList;
        //장착 아이템은 팔수 없음 
        for (int i = 0; i < passitems.Count; i++)
        {
            if (passitems[i].Item_ID == SelectItemID)
            {
                if (passitems[i].Equip_Stat == 1)//장착아이템은 팔수 없음
                {
                    //alert(장착 아이템은 판매 할 수 없음)
                }
                else //판매
                {
                    if (passitems[i].Amount > SelectItemAmount) //판매개수만 빼기
                    {
                        passitems[i].Amount = passitems[i].Amount - SelectItemAmount;
                    }
                    else                                                     
                    {
                          passitems.RemoveAt(i); //제거
                    }

                    PssItemInfoList pssiteminfolist = new PssItemInfoList();
                    pssiteminfolist.SetPssItemList = passitems;             //소유 아이템 업데이트
                    DataController.Instance.UpdateGameDataPssItem(pssiteminfolist);

                    //골드 정산 --> GameController로 빼기(정산할 골드 양과. +, -)만 넘겨서
                    decimal addGold = SelectItemPrice * SelectItemAmount;
                    List<PlayerStat> playerstats = DataController.Instance.GetPlayerStatInfo().StatList;
                    foreach (var ps in playerstats)
                    {
                        ps.PC_Gold = (Convert.ToDecimal(ps.PC_Gold) + Convert.ToDecimal(addGold)).ToString();
                    }
                    PlayerStatList playerstatlist = new PlayerStatList();
                    playerstatlist.SetPlayerStatList = playerstats;
                    DataController.Instance.UpdateGameDataPlayerStat(playerstatlist);
                    //PlayerStatLoad(); //일단 에러남 상단 플레이어 레벨표시가 구현되야됨
                }
                break;
            }
        
        }
    }

    //상세 Panel 닫기
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


     //골드 정산 --> GameController로 빼기(정산할 골드 양과. +, -)만 넘겨서
      //PlayerStatLoad(); //일단 에러남 상단 플레이어 레벨표시가 구현되야됨
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
