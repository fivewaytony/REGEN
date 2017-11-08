using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.Events;

public class ForgeController : GameController
{
    public static ForgeController forgeInstance;
    public GameObject btnWeapon, btnProtect, btnAcce, btnPotion, btnEtc;
    public RectTransform TypeParent, ListParent;
    public GameObject prefabTypeBtn, prefabListBtn;
    
    
    private string MakeItemGroupName;//생성할 아이템 GroupName 전역변수(Weapon,  Protect, Acce...)
    private string MakeItemTypeName;//생성할 아이템 TypeName 전역변수(OWeapon,  TWeapon,  Helmet,,Armor, Stuff...)

    // Use this for initialization
    private void Awake()
    {
        PlayerStatLoad();
        PlayerPssItemLoadALL(); //전체 소유 아이템 로드
    }

    void Start () {
        forgeInstance = this;
        Instance = this;

        // 현재씬의 이름
        Scene currentScene = SceneManager.GetActiveScene();
        SceneName = currentScene.name;
        ClickGroupBtn(); //아이템 그룹 선택
        MakingLevel.text = "[제조 LV. " + PC_MakingLevel.ToString() + "]";  //제조 레벨
    }
        
    #region [아이템 그룹 버튼 클릭]
    public void ClickGroupBtn()
    {
        btnWeapon.GetComponent<Button>().onClick.AddListener(() => ShowTypePan(ItemGroup.Weapon));     //무기선택
        btnProtect.GetComponent<Button>().onClick.AddListener(() => ShowTypePan(ItemGroup.Protect));        //방어구
        btnAcce.GetComponent<Button>().onClick.AddListener(() => ShowTypePan(ItemGroup.Acce));              //장신구
        btnPotion.GetComponent<Button>().onClick.AddListener(() => ShowTypePan(ItemGroup.Potion));          //물약
        btnEtc.GetComponent<Button>().onClick.AddListener(() => ShowTypePan(ItemGroup.Etc));                    //기타
    }
    #endregion

    #region [아이템 Type 선택 버튼 노출]
    public void ShowTypePan(ItemGroup groupName)
    {
        int childCnt = TypeParent.childCount;
        for (int i = 0; i < childCnt; i++)
        {
            Destroy(TypeParent.GetChild(i).gameObject);  //아이템 종류 선택 버튼 노출 초기화
        }
        ResetMakingPan();//제작 선택 아이템 정보 초기화

        //아이템 리스트 Panel 노출 초기화
        int ListchildCnt = ListParent.childCount;
        for (int i = 0; i < ListchildCnt; i++)
        {
            Destroy(ListParent.GetChild(i).gameObject);
        }

        switch (groupName)
        {
            case ItemGroup.Weapon:
                string[] arrWpnType = { "한손무기","양손무기" };
                for (int i = 0; i < arrWpnType.Length; i++)
                {
                    GameObject goButton = (GameObject)Instantiate(prefabTypeBtn);
                    goButton.transform.SetParent(TypeParent, false);
                    goButton.transform.localScale = new Vector3(1, 1, 1);
                    goButton.transform.GetChild(0).GetComponent<Text>().text = arrWpnType[i];

                    Button tempButton = goButton.GetComponent<Button>();
                    string stringValue = Enum.GetName(typeof(WeaponType), i + 1);
                    tempButton.onClick.AddListener(() => ShowListPan(ItemGroup.Weapon, stringValue));
                }
                break;
            case ItemGroup.Protect:
                string[] arrPrtType = { "투구", "갑옷", "장갑", "부츠" };
                for (int i = 0; i < arrPrtType.Length; i++)
                {
                    GameObject goButton = (GameObject)Instantiate(prefabTypeBtn);
                    goButton.transform.SetParent(TypeParent, false);
                    goButton.transform.localScale = new Vector3(1, 1, 1);
                    goButton.transform.GetChild(0).GetComponent<Text>().text = arrPrtType[i];

                    Button tempButton = goButton.GetComponent<Button>();
                    string stringValue = Enum.GetName(typeof(ProtectType), i+1);
                    tempButton.onClick.AddListener(() => ShowListPan(ItemGroup.Protect, stringValue));
                }
                break;
            case ItemGroup.Acce:
                string[] arrAceType = { "귀걸이", "목걸이", "반지" };
                for (int i = 0; i < arrAceType.Length; i++)
                {
                    GameObject goButton = (GameObject)Instantiate(prefabTypeBtn);
                    goButton.transform.SetParent(TypeParent, false);
                    goButton.transform.localScale = new Vector3(1, 1, 1);
                    goButton.transform.GetChild(0).GetComponent<Text>().text = arrAceType[i];

                    Button tempButton = goButton.GetComponent<Button>();
                    string stringValue = Enum.GetName(typeof(AcceType), i + 1);
                    tempButton.onClick.AddListener(() => ShowListPan(ItemGroup.Acce, stringValue));
                }
                break;
            case ItemGroup.Potion:
                string[] arrPonType = { "사냥", "채광", "채집" };
                for (int i = 0; i < arrPonType.Length; i++)
                {
                    GameObject goButton = (GameObject)Instantiate(prefabTypeBtn);
                    goButton.transform.SetParent(TypeParent, false);
                    goButton.transform.localScale = new Vector3(1, 1, 1);
                    goButton.transform.GetChild(0).GetComponent<Text>().text = arrPonType[i];

                    Button tempButton = goButton.GetComponent<Button>();
                    string stringValue = Enum.GetName(typeof(PotionType), i + 1);
                    tempButton.onClick.AddListener(() => ShowListPan(ItemGroup.Potion, stringValue));
                }
                break;
            case ItemGroup.Etc:
                string[] arrEctType = { "사냥", "채광", "기타" };
                for (int i = 0; i < arrEctType.Length; i++)
                {
                    GameObject goButton = (GameObject)Instantiate(prefabTypeBtn);
                    goButton.transform.SetParent(TypeParent, false);
                    goButton.transform.localScale = new Vector3(1, 1, 1);
                    goButton.transform.GetChild(0).GetComponent<Text>().text = arrEctType[i];

                    Button tempButton = goButton.GetComponent<Button>();
                    string stringValue = Enum.GetName(typeof(EtcType), i + 1);
                    tempButton.onClick.AddListener(() => ShowListPan(ItemGroup.Etc, stringValue));
                }
                break;
            default:
                break;
        }
    }
    #endregion

    #region [제작할 아이템 리스트 Panel Show]
    public void ShowListPan(ItemGroup gName, string typeName)
    {
        Debug.Log("gName=" + gName);
        Debug.Log("typeName=" + typeName);
        MakeItemGroupName = gName.ToString();
        MakeItemTypeName = typeName;
         //아이템 List 선택 버튼 노출 초기화
        int childCnt = ListParent.childCount;
        for (int i = 0; i < childCnt; i++)
        {
            Destroy(ListParent.GetChild(i).gameObject);  //아이템 종류 선택 버튼 노출 초기화
        }
        ResetMakingPan();//제작 선택 아이템 정보 초기화

        List<GameItemInfo> itemList = DataController.Instance.GetGameItemInfo().GameItemList;  //전체 게임 아이템
        if (gName.ToString() == "Weapon" || gName.ToString() == "Protect" || gName.ToString() == "Acce") //무기 or 방어구 or 장신구
        {
            List<PssItem> passitems = DataController.Instance.GetPssItemInfo().PssItemList;    //소유아이템 중 강화 가능 아이템은 제조제외
            var exceptitemid = from exceptList in passitems
                               where exceptList.Item_Type == "OWeapon" || exceptList.Item_Type == "TWeapon"
                                     || exceptList.Item_Type == "Helmet" || exceptList.Item_Type == "Armor" || exceptList.Item_Type == "Gauntlet" || exceptList.Item_Type == "Boots"
                                     || exceptList.Item_Type == "Earring" || exceptList.Item_Type == "Necklace" || exceptList.Item_Type == "Ring"
                               select exceptList;
                
           // 대상만 리스트로 추출
            var typelist = from stype in itemList
                           where stype.Item_Type == typeName
                                    && !(from exceptid in exceptitemid
                                         select exceptid.Item_ID).Contains(stype.Item_ID)
                           select stype;
            Showtypelist(typelist);
         }
        else if (gName.ToString() == "Potion" && typeName.ToString() == "Hunting")  //물약 - 사냥
        {
            var typelist = from stype in itemList
                           where stype.Making_Stat > 0 && stype.Item_Type == "HPotion"
                           select stype;
            Showtypelist(typelist);
        }
        else if (gName.ToString() == "Potion" && typeName.ToString() == "Mining")  //물약 - 채광
        {
            var typelist = from stype in itemList
                           where stype.Making_Stat > 0 && stype.Item_Type == "MPotion"
                           select stype;
            Showtypelist(typelist);
        }
        else if (gName.ToString() == "Potion" && typeName.ToString() == "Foraging")  //물약 - 채집
        {
            var typelist = from stype in itemList
                           where stype.Item_Type == "FPotion"
                           select stype;
            Showtypelist(typelist);
        }
        else if(gName.ToString() == "Etc" && typeName.ToString() == "Hunting")  //기타 - 사냥
        {
            var typelist = from stype in itemList
                           where stype.Item_Type == "Stuff" && stype.Making_Stat > 0 && stype.Item_ID >= 100 && stype.Item_ID < 200
                           select stype;
            Showtypelist(typelist);
        }
        else if (gName.ToString() == "Etc" && typeName.ToString() == "Mining")  //기타 - 채광
        {
            var typelist = from stype in itemList
                           where stype.Item_Type == "Stuff" && stype.Making_Stat > 0 && stype.Item_ID >= 200 && stype.Item_ID <300
                           select stype;
            Showtypelist(typelist);
        }
        else if (gName.ToString() == "Etc" && typeName.ToString() == "Special")  //기타 - 기타(강화석)
        {
            var typelist = from stype in itemList
                           where stype.Making_Stat > 0 && stype.Item_ID >= 1050 && stype.Item_ID < 1100
                           select stype;
            Showtypelist(typelist);
        }
    }
    #endregion

    #region [제조할 아이템 리스트 생성 공통 Method]
    public void Showtypelist(IEnumerable<GameItemInfo> queries)
    {
        foreach (var item in queries)
        {
            GameObject goButton = (GameObject)Instantiate(prefabListBtn);
            goButton.transform.SetParent(ListParent, false);
            goButton.transform.localScale = new Vector3(1, 1, 1);
            goButton.transform.GetChild(0).GetComponent<Text>().text = item.Item_Name;

            Button tempButton = goButton.GetComponent<Button>();
            tempButton.onClick.AddListener(() => ShowMakingPan(item.Item_ID));
        }
    }
    #endregion
    
    #region [아이템 제작 Panel - 아이템 정보]
    public Image MakeItemImg, Stuff1Img, Stuff2Img, Stuff3Img;
    public Text MakeItemName, MakeItemDesc;  //아이템 이름, 설명
    public Text Stuff1Name, Stuff2Name, Stuff3Name;
    public Text Stuff1Cnt, Stuff2Cnt, Stuff3Cnt, GoldCnt;   //필요 소유 개수
    public GameObject GoCheckPan;   // [제조 버튼 Open Check Pan]

    public GameObject MakeSliderPan; //제조 개수 슬라이드바 팬러
    public Text MakeCnt;       //제조할 아이템개수 Text
    public Slider MakeCountSlider; //아이템 개수 선택 슬라이더

    int tempMaxMakeCount = 1; //최대 제조 가능 개수

    /* 제조버튼 클릭시 이용될 제조 전역 변수 */
    int MaxMakeitemCount = 1;  // 제조 가능 아이템 최대 개수
    int MakeitemCount = 1; //재조 아이템 개수
    int Makeitemid;             //제조 할 아이템 id
    long MakeNeedGold;    //제조에 필요한 골드
    int MakeStuff1_ID=0, MakeStuff2_ID=0, MakeStuff3_ID=0;   //재료 1,2,3 id
    int MakeStuff1_Cnt=0, MakeStuff2_Cnt=0, MakeStuff3_Cnt=0; // 재료 1,2,3 필요 개수
    int pssStuff1Cnt = 0, pssStuff2Cnt = 0, pssStuff3Cnt = 0;  // 소유 1,2,3 개수
    int Making_Stat = 1;  //제조 재료 가지수 체크 변수
    int Item_Level = 0;   //제조 포인트 부여 시 이용
    /* 제조버튼 클릭시 이용될 제조 전역 변수 */

    private void ShowMakingPan(int itemid)
    {
        Makeitemid = itemid;  //제조할 아이템 ID
        Debug.Log("Makeitemid=" + Makeitemid);
        GameItemInfo Makeitem = DataController.Instance.gameitemDic[itemid]; //전체 아이템 정보
        MakeItemImg.transform.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Inventory/" + Makeitem.Item_ImgName);

        string Item_Grade = Makeitem.Item_Grade;
        Item_Level = Makeitem.Item_Level;  
        string Item_Name = Makeitem.Item_Name;
        Making_Stat = Makeitem.Making_Stat;  // 1:재료1개, 2 : 재료2개, 3 : 재료3개
        switch (Item_Grade)
        {
            case "D":
                Item_Name = "<color=white>" + Item_Name + "</color>";
                break;
            case "C":
                Item_Name = "<color=yellow>" + Item_Name + "</color>";
                break;
            case "B":
                Item_Name = "<color=blue>" + Item_Name + "</color>";
                break;
            case "A":
                Item_Name = "<color=red>" + Item_Name + "</color>";
                break;
            case "S":
                Item_Name = "<color=purple>" + Item_Name + "</color>";
                break;
            default:
                Item_Name = "<color=white>" + Item_Name + "</color>";
                break;
        }
        int Item_Price = Makeitem.Item_Price;
        string Item_Group = Makeitem.Item_Group;
        string Item_DescStr = string.Empty;
        MakeItemName.transform.GetComponent<Text>().text = Item_Name.ToString();
        switch (Item_Group)
        {
            case "Weapon": //무기
                Item_DescStr = "공격력 : " + Makeitem.Wpn_Attack +"\n옵션 : 힘 ?,체력 ?";
                MakeSliderPan.gameObject.SetActive(false); 
                break;
            case "Protect":
                Item_DescStr = "방어력 : " + Makeitem.Prt_Degree + "\n옵션 : 힘 ?,체력 ?";
                MakeSliderPan.gameObject.SetActive(false);
                break;
            case "Acce":
                Item_DescStr = "회피력 : " + Makeitem.Ace_Degree + "\n옵션 : 힘 ?,체력 ?";
                MakeSliderPan.gameObject.SetActive(false);
                break;
            case "Potion": //물약의 설명 추가
                           //switch (1)
                           //{
                           //    default:
                           //        break;
                           //}
                MakeSliderPan.gameObject.SetActive(true);
                break;
            case "Stuff":
                MakeSliderPan.gameObject.SetActive(true);
                break;
           case "Enhance":  //강화석
                MakeSliderPan.gameObject.SetActive(true);
                break;
            case "EtcProtect":
                break;
            case "Recipe":
                break;
            default:
                break;

        }
        MakeItemDesc.transform.GetComponent<Text>().text = Item_DescStr; //아이템 설명

         #region [필요 / 소유 아이템, 골드 셋팅]
        MakeStuff1_ID = Makeitem.Stuff1_ID;         //1번 재료의 ID
        MakeStuff1_Cnt = Makeitem.Stuff1_Count;  //1번 재료의 필요 개수
        MakeStuff2_ID = Makeitem.Stuff2_ID;         //2번 재료의 ID
        MakeStuff2_Cnt = Makeitem.Stuff2_Count;     //2번 재료의 필요 개수
        MakeStuff3_ID = Makeitem.Stuff3_ID;          //3번 재료의 ID
        MakeStuff3_Cnt = Makeitem.Stuff3_Count;     //3번 재료의 필요 개수
        MakeNeedGold = Makeitem.Making_Price;     //필요 골드 

        /*필요아이템 개수 따른 체크*/
        GameItemInfo Stuff1_itemInfo = DataController.Instance.gameitemDic[MakeStuff1_ID];
        Stuff1Img.transform.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Inventory/" + Stuff1_itemInfo.Item_ImgName);
        Stuff1Name.transform.GetComponent<Text>().text = Stuff1_itemInfo.Item_Name.ToString();

        if (Making_Stat > 1)  //재료2개 필요
        {
            GameItemInfo Stuff2_itemInfo = DataController.Instance.gameitemDic[MakeStuff2_ID];
            Stuff2Img.transform.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Inventory/" + Stuff2_itemInfo.Item_ImgName);
            Stuff2Name.transform.GetComponent<Text>().text = Stuff2_itemInfo.Item_Name.ToString();
        }
        if (Making_Stat > 2)    //재료3개 필요
        {
            GameItemInfo Stuff3_itemInfo = DataController.Instance.gameitemDic[MakeStuff3_ID];
            Stuff3Img.transform.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Inventory/" + Stuff3_itemInfo.Item_ImgName);
            Stuff3Name.transform.GetComponent<Text>().text = Stuff3_itemInfo.Item_Name.ToString();
        }
                
        List<PssItem> passitems = DataController.Instance.GetPssItemInfo().PssItemList;
        for (int i = 0; i < passitems.Count; i++)
        {
            if (passitems[i].Item_ID == MakeStuff1_ID)//재료1 있으면   
            {      
                pssStuff1Cnt = passitems[i].Amount;    //1번 재료의 소유 개수
            }
            if (passitems[i].Item_ID == MakeStuff2_ID)//재료2 있으면   
            {
                pssStuff2Cnt = passitems[i].Amount;    //2번 재료의 소유 개수
            }
            if (passitems[i].Item_ID == MakeStuff3_ID)//재료3 있으면   
            {
                pssStuff3Cnt = passitems[i].Amount;    //3번 재료의 소유 개수
            }
        }

        bool chkmstat1 = true;  //1번재료 조건
        /* 최대 제조개수 1번재료*/
        double MaxCntStuff1 = pssStuff1Cnt / MakeStuff1_Cnt;
        int MaxMakeCount1 = Convert.ToInt32(MaxCntStuff1);
        tempMaxMakeCount = MaxMakeCount1;

        Stuff1Cnt.transform.GetComponent<Text>().text = pssStuff1Cnt.ToString() + " / " + MakeStuff1_Cnt.ToString();
        if (pssStuff1Cnt >= MakeStuff1_Cnt)
        {
            chkmstat1 = true;
        }
        else
        {
            chkmstat1 = false;
        }

        bool chkmstat2 = true;  //2번재료 조건
        if (Making_Stat > 1) //재료2개 필요
        {
        /* 최대 제조개수 2번재료*/
            double MaxCntStuff2 = pssStuff2Cnt / MakeStuff2_Cnt;
            int MaxMakeCount2 = Convert.ToInt32(MaxCntStuff2);
            if (tempMaxMakeCount > MaxMakeCount2)
            {
                tempMaxMakeCount = MaxMakeCount2;
            }

            Stuff2Cnt.transform.GetComponent<Text>().text = pssStuff2Cnt.ToString() + " / " + MakeStuff2_Cnt.ToString();
            if (pssStuff2Cnt >= MakeStuff2_Cnt)
            {
                chkmstat2 = true;
            }
            else
            {
                chkmstat2 = false;
            }
        }
        bool chkmstat3 = true;  //3번재료 조건
        if (Making_Stat > 2) //재료3개 필요
        {
             /* 최대 제조개수 3번 재료*/
            double MaxCntStuff3 = pssStuff3Cnt / MakeStuff3_Cnt;
            int MaxMakeCount3 = Convert.ToInt32(MaxCntStuff3);
            if (tempMaxMakeCount > MaxMakeCount3)
            {
                tempMaxMakeCount = MaxMakeCount3;
            }
            Stuff3Cnt.transform.GetComponent<Text>().text = pssStuff3Cnt.ToString() + " / " + MakeStuff3_Cnt.ToString();
            if (pssStuff3Cnt >= MakeStuff3_Cnt)
            {
                chkmstat3 = true;
            }
            else
            {
                chkmstat3 = false;
            }
        }
       
        bool chkgold = true;
        long pssGold = Convert.ToInt64(PC_Gold);  //소유 골드
        /*최대 제조개수 골드 Check */
        double MaxCntGold = pssGold / MakeNeedGold;
        int MaxMakeCountGold = Convert.ToInt32(MaxCntGold);
        if (tempMaxMakeCount > MaxMakeCountGold)
        {
            tempMaxMakeCount = MaxMakeCountGold;
        }
        if (pssGold >= MakeNeedGold)
        {
            chkgold = true;
        }
        else
        {
            chkgold = false;
        }
        MaxMakeitemCount = tempMaxMakeCount; //최대 제조가능 개수 정의
        if (MaxMakeitemCount == 0)
        {
            MaxMakeitemCount = 1;
        }
        string formatNeedGold = String.Format("{0:n0}", Convert.ToDecimal(MakeNeedGold));
        GoldCnt.transform.GetComponent<Text>().text = formatPC_Gold + " / " + formatNeedGold;
        
        #endregion

        // 몇개까지 만들 수 있는지를 계산
        /*개수 Silder*/
        MakeCountSlider.value = 1;
        MakeCountSlider.minValue = 1;   //1개부터
        MakeCountSlider.maxValue = MaxMakeitemCount;  //제조가능 개수
        MakeCnt.text = MakeCountSlider.value.ToString();
        MakeitemCount = Convert.ToInt16(MakeCnt.text);

        Debug.Log("chkmstat1=" + chkmstat1);
        Debug.Log("chkmstat2=" + chkmstat2);
        Debug.Log("chkmstat3=" + chkmstat3);
        Debug.Log("chkgold=" + chkgold);
        if (chkmstat1 && chkmstat2 && chkmstat3 && chkgold)
        {
            GoCheckPan.gameObject.SetActive(false);
        }
        else
        {
            GoCheckPan.gameObject.SetActive(true);
        }
    }
    #endregion

    #region [ 제조 아이템 개수 Silder 변경하면 개수 표시]
    public void MakeItemCountSilderChange()
    {
        MakeCnt.text = MakeCountSlider.value.ToString();
        MakeitemCount = Convert.ToInt16(MakeCnt.text);
        Stuff1Cnt.transform.GetComponent<Text>().text = pssStuff1Cnt.ToString() + " / " + (MakeStuff1_Cnt * MakeitemCount).ToString();
        if (Making_Stat > 1)
        {
            Stuff2Cnt.transform.GetComponent<Text>().text = pssStuff2Cnt.ToString() + " / " + (MakeStuff1_Cnt * MakeitemCount).ToString();
        }
        if (Making_Stat > 2)
        {
            Stuff3Cnt.transform.GetComponent<Text>().text = pssStuff3Cnt.ToString() + " / " + (MakeStuff1_Cnt * MakeitemCount).ToString();
        }
        string formatNeedGold = String.Format("{0:n0}", Convert.ToDecimal(MakeNeedGold * Convert.ToInt16(MakeCnt.text)));
        GoldCnt.transform.GetComponent<Text>().text = formatPC_Gold + " / " + formatNeedGold;
    }
    #endregion
    
    #region [제조버튼 클릭]
    public Button BtnGoMake;
    public void btnGoMake()
    {
        Debug.Log("MakeStuff1_ID=" + MakeStuff1_ID);
        Debug.Log("MakeStuff2_ID=" + MakeStuff2_ID);
        Debug.Log("MakeStuff3_ID=" + MakeStuff3_ID);
        Debug.Log("MakeStuff1_Cnt=" + MakeStuff1_Cnt);
        Debug.Log("MakeStuff2_Cnt=" + MakeStuff2_Cnt);
        Debug.Log("MakeStuff3_Cnt=" + MakeStuff3_Cnt);
        Debug.Log("MakeNeedGold=" + MakeNeedGold);
        Debug.Log("MakeitemCount=" + MakeitemCount);

        //재료 정산
        CalStuff(MakeStuff1_ID, MakeStuff1_Cnt * MakeitemCount);
        CalStuff(MakeStuff2_ID, MakeStuff2_Cnt * MakeitemCount);
        CalStuff(MakeStuff3_ID, MakeStuff3_Cnt * MakeitemCount);
        //골드 정산 
        CalGold(MakeNeedGold, MakeitemCount, "minus");
       
        //Makeitemid -아이템 생성
        List<PssItem> passitems = DataController.Instance.GetPssItemInfo().PssItemList;
        bool isHaveItem = false;
        for (int i = 0; i < passitems.Count; i++)
        {
            if (Makeitemid == passitems[i].Item_ID)  //가지고 있으면 수량만 +
            {
                passitems[i].Amount = passitems[i].Amount + MakeitemCount;
                isHaveItem = true;
                break;
            }
        }
        if (isHaveItem == false)                //소유하지 않은 아이템은 생성
        {
            // 옵션 랜덤생성
            int tempOptType = 0; //힘(1),체력(2),민첩(3)
            int tempOptPoint = 0; //Point (-3 ~ 5)
            if (MakeItemGroupName == "Weapon" || MakeItemGroupName == "Protect"  || MakeItemGroupName == "Acce")
            {
                tempOptType = CommonRnd(1, 4);
                tempOptPoint = CommonRnd(-3, 6);
            }
            //(소유아이템 ID, player_id,아아템타입,아이템아이디,개수,장착여부,강화도,옵션타입(힘:1, 체력:2, 민첩3), 옵션 포인트)
            passitems.Add(new PssItem(passitems.Count + 1, 1, MakeItemGroupName, MakeItemTypeName, Makeitemid, MakeitemCount, 0, 0, tempOptType, tempOptPoint));
        }

        PssItemInfoList pssiteminfolist = new PssItemInfoList();
        pssiteminfolist.SetPssItemList = passitems;             //소유 아이템 업데이트
        DataController.Instance.UpdateGameDataPssItem(pssiteminfolist); //소유 아이템 파일 다시 쓰기
        DataController.Instance.GetPssItemInfoReload();  // 소유 아이템 파일 다시 읽기

        //플레이어 제조레벨 Update
        List<PlayerStat> playerstats = DataController.Instance.GetPlayerStatInfo().StatList;
        foreach (var ps in playerstats)
        {
            ps.PC_MakingLevel = ps.PC_MakingLevel + (Item_Level * 5);
        }
        PlayerStatList playerstatlist = new PlayerStatList();
        playerstatlist.SetPlayerStatList = playerstats;
        DataController.Instance.UpdateGameDataPlayerStat(playerstatlist);
        PlayerStatLoad();

        SceneManager.LoadScene(SceneName, LoadSceneMode.Single);  //현재씬 다시로드(가방, 대장간, 상점)
        /*
         * 체크사항
         //제조포인트 업그레이드
         * 
         */
    }
    //재료정산
    private void CalStuff(int stuffid, int stuffcnt)
    {
        List<PssItem> passitems = DataController.Instance.GetPssItemInfo().PssItemList;
        for (int i = 0; i < passitems.Count; i++)
        {
            if (passitems[i].Item_ID == stuffid)
            {
                if (passitems[i].Amount > stuffcnt) //사용개수만 빼기(소유재료개수 > 사요재료개수)
                {
                    passitems[i].Amount = passitems[i].Amount - stuffcnt;
                }
                else
                {
                    passitems.RemoveAt(i);
                }
                break;
            }
        }
    }
    #endregion

    #region [제작 선택 아이템 정보 초기화]
    public void ResetMakingPan()
    {
        MakeItemImg.transform.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Inventory/BoxBg");
        MakeItemName.transform.GetComponent<Text>().text ="";
        MakeItemDesc.transform.GetComponent<Text>().text = "";

        Stuff1Name.transform.GetComponent<Text>().text = "";
        Stuff2Name.transform.GetComponent<Text>().text = "";
        Stuff3Name.transform.GetComponent<Text>().text = "";
        
        Stuff1Cnt.transform.GetComponent<Text>().text = "";
        Stuff2Cnt.transform.GetComponent<Text>().text = "";
        Stuff3Cnt.transform.GetComponent<Text>().text = "";
        GoldCnt.transform.GetComponent<Text>().text = "";

        Stuff1Img.transform.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Inventory/RectButtonPressed");
        Stuff2Img.transform.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Inventory/RectButtonPressed");
        Stuff3Img.transform.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Inventory/RectButtonPressed");

        GoCheckPan.gameObject.SetActive(true);
        MakeSliderPan.gameObject.SetActive(false);
    }
    #endregion

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            base.GoMain();
        }
    }
}
