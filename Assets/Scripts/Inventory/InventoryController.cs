using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Linq;


public class InventoryController : GameController
{
    /* 상단 장착 아이템 */
    public Image Weapon, Helmet, Armor, Boots, Gauntlet, Necklace, Earring, Ring;
    public GameObject PlayerInfoTextBG;  //플레이어 스텟 
    private int StrNum, ConNum, DexNum, AtcNum, PrtNum, AvoNum;

    public Text EquItemINfoNameText, EquItemInfoDescText;
    public GameObject EquItemInfoBackPanel;    //장착 아이템 상세보기 panel
        

    private int pssItemID;
    public static InventoryController invenInstance;

    void Start () {
        invenInstance = this;
        Instance = this;
        // Retrieve the name of this scene.
        Scene currentScene = SceneManager.GetActiveScene();
        SceneName = currentScene.name;

        PlayerStatLoad();
        PlayerPssItemLoadALL(); //전체 소유 아이템 로드
        PlayerEquItemLoad();    //장착아이템
    }

    #region [ //상단 장착 아이템 슬롯 & 플레이어 능력치]
    public void PlayerEquItemLoad() {
        List<PssItem> passitems = DataController.Instance.GetPssItemInfo().PssItemList; //소유 아이템
        List<GameItemInfo> itemList = DataController.Instance.GetGameItemInfo().GameItemList;  //전체 게임 아이템
        for (int i = 0; i < passitems.Count; i++)
        {
            foreach (GameItemInfo item in itemList)
            {
                if (passitems[i].Item_ID == item.Item_ID)
                {
                    if (item.Item_Type == "Weapon" && passitems[i].Equip_Stat == 1)
                    {
                        Weapon.transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Inventory/" + item.Item_ImgName);
                        Weapon.transform.GetChild(1).GetComponent<Text>().text = "+" + passitems[i].Wpn_Ent;
                        AtcNum = AtcNum + passitems[i].Wpn_Ent + item.Wpn_Attack; //공격력
                        Weapon.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(() => ShowEquItemInfoPanel(item.Item_ID));
                    }
                    if (item.Item_Type == "Helmet" && passitems[i].Equip_Stat == 1)
                    {
                        Helmet.transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Inventory/" + item.Item_ImgName);
                        Helmet.transform.GetChild(1).GetComponent<Text>().text = "+" + passitems[i].Wpn_Ent;
                        PrtNum = PrtNum + passitems[i].Wpn_Ent + item.Prt_Degree;
                        Helmet.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(() => ShowEquItemInfoPanel(item.Item_ID));
                    }
                    if (item.Item_Type == "Armor" && passitems[i].Equip_Stat == 1)
                    {
                        Armor.transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Inventory/" + item.Item_ImgName);
                        Armor.transform.GetChild(1).GetComponent<Text>().text = "+" + passitems[i].Wpn_Ent;
                        PrtNum = PrtNum + passitems[i].Wpn_Ent + item.Prt_Degree;
                        Armor.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(() => ShowEquItemInfoPanel(item.Item_ID));
                    }
                    if (item.Item_Type == "Boots" && passitems[i].Equip_Stat == 1)
                    {
                        Boots.transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Inventory/" + item.Item_ImgName);
                        Boots.transform.GetChild(1).GetComponent<Text>().text = "+" + passitems[i].Wpn_Ent;
                        PrtNum = PrtNum + passitems[i].Wpn_Ent + item.Prt_Degree;
                        Boots.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(() => ShowEquItemInfoPanel(item.Item_ID));
                    }
                    if (item.Item_Type == "Gauntlet" && passitems[i].Equip_Stat == 1)
                    {
                        Gauntlet.transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Inventory/" + item.Item_ImgName);
                        Gauntlet.transform.GetChild(1).GetComponent<Text>().text = "+" + passitems[i].Wpn_Ent;
                        PrtNum = PrtNum + passitems[i].Wpn_Ent + item.Prt_Degree;
                        Gauntlet.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(() => ShowEquItemInfoPanel(item.Item_ID));
                    }
                    if (item.Item_Type == "Necklace" && passitems[i].Equip_Stat == 1)
                    {
                        Necklace.transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Inventory/" + item.Item_ImgName);
                        Necklace.transform.GetChild(1).GetComponent<Text>().text = "+" + passitems[i].Wpn_Ent;
                        AvoNum = AvoNum + passitems[i].Wpn_Ent + item.Ace_Degree;
                        Necklace.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(() => ShowEquItemInfoPanel(item.Item_ID));
                    }
                    if (item.Item_Type == "Earring" && passitems[i].Equip_Stat == 1)
                    {
                        Earring.transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Inventory/" + item.Item_ImgName);
                        Earring.transform.GetChild(1).GetComponent<Text>().text = "+" + passitems[i].Wpn_Ent;
                        AvoNum = AvoNum + passitems[i].Wpn_Ent + item.Ace_Degree;
                        Earring.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(() => ShowEquItemInfoPanel(item.Item_ID));
                    }
                    if (item.Item_Type == "Ring" && passitems[i].Equip_Stat == 1)
                    {
                        Ring.transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Inventory/" + item.Item_ImgName);
                        Ring.transform.GetChild(1).GetComponent<Text>().text = "+" + passitems[i].Wpn_Ent;
                        AvoNum = AvoNum + passitems[i].Wpn_Ent + item.Ace_Degree;
                        Ring.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(() => ShowEquItemInfoPanel(item.Item_ID));
                    }
                }
            }
        }

        //플레이어 스텟 총합 노출
        StrNum = PC_Str;        //힘
        ConNum = PC_Con;    //체력
        DexNum = PC_Dex;    //민첩

        AtcNum = AtcNum + PC_Str; //공격력
        PrtNum = PrtNum + PC_Con; //방어력
        AvoNum = AvoNum + PC_Dex;    //민첩
        PlayerInfoTextBG.transform.GetChild(0).GetComponent<Text>().text = StrNum.ToString();
        PlayerInfoTextBG.transform.GetChild(1).GetComponent<Text>().text = ConNum.ToString();
        PlayerInfoTextBG.transform.GetChild(2).GetComponent<Text>().text = DexNum.ToString();
        PlayerInfoTextBG.transform.GetChild(3).GetComponent<Text>().text = AtcNum.ToString();
        PlayerInfoTextBG.transform.GetChild(4).GetComponent<Text>().text = PrtNum.ToString();
        PlayerInfoTextBG.transform.GetChild(5).GetComponent<Text>().text = AvoNum.ToString();
    }
    #endregion
    
    #region [장착 아이템 정보 Panel Show / Close]
    public void ShowEquItemInfoPanel(int EquItemID)
    {
        Debug.Log("ItemID=" + EquItemID);

        if (EquItemID != 0)
        {
            EquItemInfoBackPanel.gameObject.SetActive(true);

            PssItem pssitem = DataController.Instance.pssitemDic[EquItemID];
            GameItemInfo item = DataController.Instance.gameitemDic[EquItemID];                //전체 아이템 정보
            string equDesc = string.Empty;
            if (pssitem.Item_Type == "Weapon")
            {
                equDesc = "공격 : " + item.Wpn_Attack;
            }
            if (pssitem.Item_Type == "Helmet" || pssitem.Item_Type == "Armor" || pssitem.Item_Type == "Boots" || pssitem.Item_Type == "Gauntlet")
            {
                equDesc = "방어 : " + item.Prt_Degree;
            }
            if (pssitem.Item_Type == "Necklace" || pssitem.Item_Type == "Earring" || pssitem.Item_Type == "Ring")
            {
                equDesc = "회피 : " + item.Ace_Degree;
            }
            equDesc = equDesc + "\n강화 : +" + pssitem.Wpn_Ent;

            EquItemINfoNameText.text = item.Item_Name;
            string DescStr = string.Empty;
            EquItemInfoDescText.text = equDesc + "\n판매가격 : " + item.Item_Price.ToString() + "골드"+ "\n\n[강화하기]";
           
         //강화는 인벤토리에서 바로 진행
        }
    }

    //장착 아이템 상세 Panel 닫기
    public void CloseEquItemInfoPanel()
    {
        EquItemInfoBackPanel.gameObject.SetActive(false);
    }
    #endregion

    #region [인벤토리 아이템 상세보기 패널]
    public void invenItemInfoPan(int ItemID, int ItemAmount)
    {
        base.ShowItemInfoPanel(ItemID, ItemAmount);
    }
    #endregion
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GoMain();
        }
    }

    #region [플레이어 소유/장착 아이템 전체 로드]
    //protected void PlayerPssItemLoadALL()
    //{
    //     /* Scroll viewport 위치 임시 수정 - 확인필요 
    //      sv.gameObject.SetActive(false);
    //      sv.gameObject.SetActive(true);
    //     */

    //    GameObject sv = GameObject.Find("SlotsScroll View");
    //    sv.gameObject.SetActive(false);

    //    List<PssItem> passitems = DataController.Instance.GetPssItemInfo().PssItemList; //소유 아이템
    //    List<GameItemInfo> itemList = DataController.Instance.GetGameItemInfo().GameItemList;  //전체 게임 아이템

    //    /* 슬롯 그리기 */
    //    for (int i = 0; i < slotCount; i++)
    //    {
    //        GameObject SlotInfo = Resources.Load("Prefabs/InventorySlot") as GameObject;  //프리팹으로 등록된 정보 불러옴
    //        GameObject objslot = Instantiate(SlotInfo, SlotsParentContent);   //자식 오브젝트
    //        slots.Add(objslot);

    //        RectTransform rt = objslot.GetComponent<RectTransform>(); //SlotInfo
    //        rt.anchoredPosition = new Vector2(0f, 0f);       // 자식 오브젝트를 위치를 잡고 그린다
    //    }
    //    sv.gameObject.SetActive(true);
    //    ItemInfoMng iteminfo = new ItemInfoMng();  //itemID 전달용 

    //    for (int i = 0; i < passitems.Count; i++)
    //    {
    //        foreach (GameItemInfo item in itemList)
    //         {
    //            if (passitems[i].Item_ID == item.Item_ID)
    //            {
    //                Color color = slots[i].transform.GetChild(0).GetComponent<Image>().color;
    //                color.a = 1f;
    //                slots[i].transform.GetChild(0).GetComponent<Image>().color = color;

    //                //Debug.Log("item.Item_ImgName=" + item.Item_ImgName);
    //                //Debug.Log("item.ItemID = " + passitems[i].Item_ID);
    //                slots[i].transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Inventory/" + item.Item_ImgName);
    //                slots[i].transform.GetChild(1).GetComponent<Text>().text = passitems[i].Amount.ToString();

    //                slots[i].GetComponent<ItemInfoMng>().ItemID = passitems[i].Item_ID;
    //                slots[i].GetComponent<ItemInfoMng>().ItemAmount = passitems[i].Amount;

    //                //장착 아이템 이미지 표시
    //                if (passitems[i].Equip_Stat == 1)
    //                {
    //                    Color ecolor = slots[i].transform.GetChild(2).GetComponent<Image>().color;
    //                    ecolor.a = 1f;
    //                    slots[i].transform.GetChild(2).GetComponent<Image>().color = ecolor;
    //                    slots[i].transform.GetChild(2).GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Inventory/equline");
    //                }

    //                #region [ //상단 장착 아이템 슬롯 & 플레이어 능력치]

    //                if (item.Item_Type == "Weapon" && passitems[i].Equip_Stat == 1)
    //                {
    //                    Weapon.transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Inventory/" + item.Item_ImgName);
    //                    Weapon.transform.GetChild(1).GetComponent<Text>().text = "+" + passitems[i].Wpn_Ent;
    //                    AtcNum = AtcNum + passitems[i].Wpn_Ent + item.Wpn_Attack; //공격력
    //                    Weapon.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(() => ShowEquItemInfoPanel(item.Item_ID));
    //                }
    //                if (item.Item_Type == "Helmet" && passitems[i].Equip_Stat == 1)
    //                {
    //                    Helmet.transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Inventory/" + item.Item_ImgName);
    //                    Helmet.transform.GetChild(1).GetComponent<Text>().text = "+" + passitems[i].Wpn_Ent;
    //                    PrtNum = PrtNum + passitems[i].Wpn_Ent + item.Prt_Degree;
    //                    Helmet.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(() => ShowEquItemInfoPanel(item.Item_ID));
    //                }
    //               if (item.Item_Type == "Armor" && passitems[i].Equip_Stat == 1)
    //                {
    //                    Armor.transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Inventory/" + item.Item_ImgName);
    //                    Armor.transform.GetChild(1).GetComponent<Text>().text = "+" + passitems[i].Wpn_Ent;
    //                    PrtNum = PrtNum + passitems[i].Wpn_Ent + item.Prt_Degree;
    //                    Armor.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(() => ShowEquItemInfoPanel(item.Item_ID));
    //                }
    //                if (item.Item_Type == "Boots" && passitems[i].Equip_Stat == 1)
    //                {
    //                    Boots.transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Inventory/" + item.Item_ImgName);
    //                    Boots.transform.GetChild(1).GetComponent<Text>().text = "+" + passitems[i].Wpn_Ent;
    //                    PrtNum = PrtNum + passitems[i].Wpn_Ent + item.Prt_Degree;
    //                    Boots.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(() => ShowEquItemInfoPanel(item.Item_ID));
    //                }
    //                if (item.Item_Type == "Gauntlet" && passitems[i].Equip_Stat == 1)
    //                {
    //                    Gauntlet.transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Inventory/" + item.Item_ImgName);
    //                    Gauntlet.transform.GetChild(1).GetComponent<Text>().text = "+" + passitems[i].Wpn_Ent;
    //                    PrtNum = PrtNum + passitems[i].Wpn_Ent + item.Prt_Degree;
    //                    Gauntlet.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(() => ShowEquItemInfoPanel(item.Item_ID));
    //                }
    //                if (item.Item_Type == "Necklace" && passitems[i].Equip_Stat == 1)
    //                {
    //                    Necklace.transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Inventory/" + item.Item_ImgName);
    //                    Necklace.transform.GetChild(1).GetComponent<Text>().text = "+" + passitems[i].Wpn_Ent;
    //                    AvoNum = AvoNum + passitems[i].Wpn_Ent + item.Ace_Degree;
    //                    Necklace.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(() => ShowEquItemInfoPanel(item.Item_ID));
    //                }
    //                if (item.Item_Type == "Earring" && passitems[i].Equip_Stat == 1)
    //                {
    //                    Earring.transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Inventory/" + item.Item_ImgName);
    //                    Earring.transform.GetChild(1).GetComponent<Text>().text = "+" + passitems[i].Wpn_Ent;
    //                    AvoNum = AvoNum + passitems[i].Wpn_Ent + item.Ace_Degree;
    //                    Earring.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(() => ShowEquItemInfoPanel(item.Item_ID));
    //                }
    //                if (item.Item_Type == "Ring" && passitems[i].Equip_Stat == 1)
    //                {
    //                    Ring.transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Inventory/" + item.Item_ImgName);
    //                    Ring.transform.GetChild(1).GetComponent<Text>().text = "+" + passitems[i].Wpn_Ent;
    //                    AvoNum = AvoNum + passitems[i].Wpn_Ent + item.Ace_Degree;
    //                    Ring.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(() => ShowEquItemInfoPanel(item.Item_ID));
    //                }
    //              #endregion
    //               break;
    //            }
    //        }
    //     }

    //    //플레이어 스텟 총합 노출
    //    StrNum = PC_Str;        //힘
    //    ConNum = PC_Con;    //체력
    //    DexNum = PC_Dex;    //민첩

    //    AtcNum = AtcNum + PC_Str; //공격력
    //    PrtNum = PrtNum + PC_Con; //방어력
    //    AvoNum = AvoNum + PC_Dex;    //민첩
    //    PlayerInfoTextBG.transform.GetChild(0).GetComponent<Text>().text = StrNum.ToString();
    //    PlayerInfoTextBG.transform.GetChild(1).GetComponent<Text>().text = ConNum.ToString();
    //    PlayerInfoTextBG.transform.GetChild(2).GetComponent<Text>().text = DexNum.ToString();
    //    PlayerInfoTextBG.transform.GetChild(3).GetComponent<Text>().text = AtcNum.ToString();
    //    PlayerInfoTextBG.transform.GetChild(4).GetComponent<Text>().text = PrtNum.ToString();
    //    PlayerInfoTextBG.transform.GetChild(5).GetComponent<Text>().text = AvoNum.ToString();
    //}
    #endregion


    /*
   아이템 설명 : 이름, 용도, 
   장착 아이템 표시
   같은 무기는 무조건 하나만 들 수 있음 --> 무기, 방어구, 장신구는 무조건 제조를 통해서만 가능하며
   제조시 소유하고 있는 무기, 방어구, 장신구는 또 제조 할 수 없도록 제조 리스트에서 보여주지 않는다
   ---> 강화도를 어떻게 저장 할거인가.

    Item_ID --> 강화도  ?
   
    장착아이템중에서 물약을 팔 수 있게 하자 --> 채집 엥벌이 용도


    //장착하기 링크 만들기 --> 아이템 장착 링크 만들기 
    //강화는 인벤토리에서 바로 진행

    //골드 정산 --> GameController로 빼기(정산할 골드 양과. +, -)만 넘겨서
     //PlayerStatLoad(); //일단 에러남 상단 플레이어 레벨표시가 구현되야됨
     */

}
