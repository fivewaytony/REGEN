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

    public Text EquItemInfoNameText, EquItemInfoDescText;
    public GameObject EquItemInfoBackPanel;    //장착 아이템 상세보기 panel
    public GameObject EnchantBG; //장착 아이템 강화하기 panel
    private int EnchantItemID;  //강화 아이템 ID 전역

    public static InventoryController invenInstance;

    private void Awake()
    {
        PlayerStatLoad();
        PlayerPssItemLoadALL(); //전체 소유 아이템 로드
        PlayerEquItemLoad();    //장착아이템
    }

    void Start () {
        invenInstance = this;
        Instance = this;
        // Retrieve the name of this scene.
        Scene currentScene = SceneManager.GetActiveScene();
        SceneName = currentScene.name;
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
                    if (item.Item_Type == "OWeapon" && passitems[i].Equip_Stat == 1)
                    {
                        Weapon.transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Inventory/" + item.Item_ImgName);
                        Weapon.transform.GetChild(1).GetComponent<Text>().text = "+" + passitems[i].Item_Ent;
                        AtcNum = AtcNum + passitems[i].Item_Ent + item.Wpn_Attack; //공격력
                        Weapon.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(() => ShowEquItemInfoPanel(item.Item_ID));
                    }
                    if (item.Item_Type == "TWeapon" && passitems[i].Equip_Stat == 1)
                    {
                        Weapon.transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Inventory/" + item.Item_ImgName);
                        Weapon.transform.GetChild(1).GetComponent<Text>().text = "+" + passitems[i].Item_Ent;
                        AtcNum = AtcNum + passitems[i].Item_Ent + item.Wpn_Attack; //공격력
                        Weapon.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(() => ShowEquItemInfoPanel(item.Item_ID));
                    }
                    if (item.Item_Type == "Helmet" && passitems[i].Equip_Stat == 1)
                    {
                        Helmet.transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Inventory/" + item.Item_ImgName);
                        Helmet.transform.GetChild(1).GetComponent<Text>().text = "+" + passitems[i].Item_Ent;
                        PrtNum = PrtNum + passitems[i].Item_Ent + item.Prt_Degree;
                        Helmet.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(() => ShowEquItemInfoPanel(item.Item_ID));
                    }
                    if (item.Item_Type == "Armor" && passitems[i].Equip_Stat == 1)
                    {
                        Armor.transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Inventory/" + item.Item_ImgName);
                        Armor.transform.GetChild(1).GetComponent<Text>().text = "+" + passitems[i].Item_Ent;
                        PrtNum = PrtNum + passitems[i].Item_Ent + item.Prt_Degree;
                        Armor.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(() => ShowEquItemInfoPanel(item.Item_ID));
                    }
                    if (item.Item_Type == "Boots" && passitems[i].Equip_Stat == 1)
                    {
                        Boots.transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Inventory/" + item.Item_ImgName);
                        Boots.transform.GetChild(1).GetComponent<Text>().text = "+" + passitems[i].Item_Ent;
                        PrtNum = PrtNum + passitems[i].Item_Ent + item.Prt_Degree;
                        Boots.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(() => ShowEquItemInfoPanel(item.Item_ID));
                    }
                    if (item.Item_Type == "Gauntlet" && passitems[i].Equip_Stat == 1)
                    {
                        Gauntlet.transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Inventory/" + item.Item_ImgName);
                        Gauntlet.transform.GetChild(1).GetComponent<Text>().text = "+" + passitems[i].Item_Ent;
                        PrtNum = PrtNum + passitems[i].Item_Ent + item.Prt_Degree;
                        Gauntlet.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(() => ShowEquItemInfoPanel(item.Item_ID));
                    }
                    if (item.Item_Type == "Necklace" && passitems[i].Equip_Stat == 1)
                    {
                        Necklace.transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Inventory/" + item.Item_ImgName);
                        Necklace.transform.GetChild(1).GetComponent<Text>().text = "+" + passitems[i].Item_Ent;
                        AvoNum = AvoNum + passitems[i].Item_Ent + item.Ace_Degree;
                        Necklace.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(() => ShowEquItemInfoPanel(item.Item_ID));
                    }
                    if (item.Item_Type == "Earring" && passitems[i].Equip_Stat == 1)
                    {
                        Earring.transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Inventory/" + item.Item_ImgName);
                        Earring.transform.GetChild(1).GetComponent<Text>().text = "+" + passitems[i].Item_Ent;
                        AvoNum = AvoNum + passitems[i].Item_Ent + item.Ace_Degree;
                        Earring.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(() => ShowEquItemInfoPanel(item.Item_ID));
                    }
                    if (item.Item_Type == "Ring" && passitems[i].Equip_Stat == 1)
                    {
                        Ring.transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Inventory/" + item.Item_ImgName);
                        Ring.transform.GetChild(1).GetComponent<Text>().text = "+" + passitems[i].Item_Ent;
                        AvoNum = AvoNum + passitems[i].Item_Ent + item.Ace_Degree;
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
        PlayerInfoTextBG.transform.GetChild(0).GetComponent<Text>().text = "<color=#0000ff>" + StrNum.ToString() + "</color>";
        PlayerInfoTextBG.transform.GetChild(1).GetComponent<Text>().text = "<color=#0000ff>" + ConNum.ToString() + "</color>";
        PlayerInfoTextBG.transform.GetChild(2).GetComponent<Text>().text = "<color=#0000ff>" + DexNum.ToString() + "</color>";
        PlayerInfoTextBG.transform.GetChild(3).GetComponent<Text>().text = "<color=#0000ff>" + AtcNum.ToString() + "</color>";
        PlayerInfoTextBG.transform.GetChild(4).GetComponent<Text>().text = "<color=#0000ff>" + PrtNum.ToString() + "</color>";
        PlayerInfoTextBG.transform.GetChild(5).GetComponent<Text>().text = "<color=#0000ff>" + AvoNum.ToString() + "</color>";
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
            if (pssitem.Item_Group == "Weapon")
            {
                equDesc = "공격 : " + item.Wpn_Attack;
            }
            if (pssitem.Item_Group== "Protect")
            {
                equDesc = "방어 : " + item.Prt_Degree;
            }
            if (pssitem.Item_Group == "Acce")
            {
                equDesc = "회피 : " + item.Ace_Degree;
            }
            equDesc = equDesc + "\n강화 : +" + pssitem.Item_Ent;

            EquItemInfoNameText.text = item.Item_Name;
            string DescStr = string.Empty;
            /** 옵션 표시**/
            int optType = pssitem.Item_OptType;
            string optPoint = pssitem.Item_OptPoint.ToString();
            string strColor1 = string.Empty;
            string strColor2 = strColor2 = "</color>"; ;
            if (Convert.ToInt16(optPoint) < 0)
            {
                strColor1 = "<color=#ff0000>";
            }
            else
            {
                strColor1 = "<color=#0000ff>";
                optPoint = "+" + optPoint;
            }
            switch (optType)
            {
                case 1:
                    DescStr = DescStr + "\n<color=#000000>힘 : </color>" + strColor1 + optPoint + strColor2;
                    break;
                case 2:
                    DescStr = DescStr + "\n<color=#000000>체력 : </color>" + strColor1 + optPoint + strColor2;
                    break;
                default:
                    DescStr = DescStr + "\n<color=#000000>민첩 : </color>" + strColor1 + optPoint + strColor2;
                    break;
            }

            equDesc = equDesc + DescStr;
            EquItemInfoDescText.text = equDesc + "\n판매가격 : " + item.Item_Price.ToString() + "G";

            EnchantItemID = item.Item_ID;//강화 아이템 아이디 전역변수
        }
    }

    //장착 아이템 상세 Panel 닫기
    public void CloseEquItemInfoPanel()
    {
        EquItemInfoBackPanel.gameObject.SetActive(false);
    }

    //장착 아이템 강화 Panel 열기
    public void ShowEnchantBG()
    {
        EquItemInfoBackPanel.gameObject.SetActive(false);
        EnchantBG.gameObject.SetActive(true);
        Debug.Log("EnchantItemID=" + EnchantItemID);
    }

    //장착 아이템 강화 Panel 닫기
    public void CloseEnchantBG()
    {
        EnchantBG.gameObject.SetActive(false);
    }

    #endregion

    //#region [인벤토리 아이템 상세보기 패널]
    //public void invenItemInfoPan(int ItemID, int ItemAmount)
    //{
    //    base.ShowItemInfoPanel(ItemID, ItemAmount);
    //}
    //#endregion

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
