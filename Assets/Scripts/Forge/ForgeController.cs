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
    public Text MakingLevel;

    //생성할 아이템 TypeName 전역변수(OWeapon,    TWeapon,    Helmet,,Armor, Stuff...
    private string MakeItemTypeName;

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
        MakingLevel.text = "[제조 LV. "+PC_MakingLevel.ToString()+"]";  //제조 레벨
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
                string[] arrEctType = { "사냥", "채광", "채집", "기타" };
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
        else if(gName.ToString() == "Etc" && typeName.ToString() == "Hunting")  //기타 - 사냥
        {
            var typelist = from stype in itemList
                           where stype.Item_Type == "Stuff" && stype.Making_Stat > 0 && stype.Item_ID >= 100 && stype.Item_ID < 200
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
    public GameObject MakeSliderPan; //제조 개수 슬라이드바
    /* 제조버튼 클릭시 이용될 제조 전역 변수 */
    int MakeitemCount = 1;  // 제조 아이템 개수
    int Makeitemid;             //제조 할 아이템 id
    long MakeNeedGold;    //제조에 필요한 골드
    int MakeStuff1_ID=0, MakeStuff2_ID=0, MakeStuff3_ID=0;   //재료 1,2,3 id
    int MakeStuff1_Cnt=0, MakeStuff2_Cnt=0, MakeStuff3_Cnt=0; // 재료 1,23 필요 개수
    /* 제조버튼 클릭시 이용될 제조 전역 변수 */
    
    private void ShowMakingPan(int itemid)
    {
        Makeitemid = itemid;  //제조할 아이템 ID
        Debug.Log("Makeitemid=" + Makeitemid);
        GameItemInfo Makeitem = DataController.Instance.gameitemDic[itemid]; //전체 아이템 정보
        MakeItemImg.transform.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Inventory/" + Makeitem.Item_ImgName);

        string Item_Leve = Makeitem.Item_Level;
        string Item_Name = Makeitem.Item_Name;
        int Make_Stat = Makeitem.Making_Stat; //제조에 필요한 재료 개수
        switch (Item_Leve)
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
                Item_DescStr = "방어력 : " + Makeitem.Prt_Degree + "\n옵션 : 체력 ?";
                MakeSliderPan.gameObject.SetActive(false);
                break;
            case "Acce":
                Item_DescStr = "회피력 : " + Makeitem.Ace_Degree + "\n옵션 : 민첩 ?";
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

        GameItemInfo Stuff1_itemInfo = DataController.Instance.gameitemDic[MakeStuff1_ID];
        GameItemInfo Stuff2_itemInfo = DataController.Instance.gameitemDic[MakeStuff2_ID];
        GameItemInfo Stuff3_itemInfo = DataController.Instance.gameitemDic[MakeStuff3_ID];

        Stuff1Img.transform.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Inventory/" + Stuff1_itemInfo.Item_ImgName);
        Stuff2Img.transform.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Inventory/" + Stuff2_itemInfo.Item_ImgName);
        Stuff3Img.transform.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Inventory/" + Stuff3_itemInfo.Item_ImgName);

        Stuff1Name.transform.GetComponent<Text>().text = Stuff1_itemInfo.Item_Name.ToString();
        Stuff2Name.transform.GetComponent<Text>().text = Stuff2_itemInfo.Item_Name.ToString();
        Stuff3Name.transform.GetComponent<Text>().text = Stuff3_itemInfo.Item_Name.ToString();

        PssItem pssStuff1 = DataController.Instance.pssitemDic[MakeStuff1_ID];
        PssItem pssStuff2 = DataController.Instance.pssitemDic[MakeStuff2_ID];
        PssItem pssStuff3 = DataController.Instance.pssitemDic[MakeStuff3_ID];
        int pssStuff1Cnt = pssStuff1.Amount;  //1번 재료의 소유 개수
        int pssStuff2Cnt = pssStuff2.Amount; //2번 재료의 소유 개수
        int pssStuff3Cnt = pssStuff3.Amount; //3번 재료의 소유 개수
        long pssGold = Convert.ToInt64(PC_Gold);  //소유 골드

        Stuff1Cnt.transform.GetComponent<Text>().text = pssStuff1Cnt.ToString() + " / " + MakeStuff1_Cnt.ToString();
        Stuff2Cnt.transform.GetComponent<Text>().text = pssStuff2Cnt.ToString() + " / " + MakeStuff2_Cnt.ToString();
        Stuff3Cnt.transform.GetComponent<Text>().text = pssStuff3Cnt.ToString() + " / " + MakeStuff3_Cnt.ToString();
        GoldCnt.transform.GetComponent<Text>().text = fPC_Gold + " / " + MakeNeedGold.ToString();

        #endregion
        // 몇개까지 만들 수 있는지를 계산
        if (pssStuff1Cnt >= MakeStuff1_Cnt * MakeitemCount &&
            pssStuff2Cnt >= MakeStuff2_Cnt * MakeitemCount &&
            pssStuff3Cnt >= MakeStuff3_Cnt * MakeitemCount &&
            pssGold >= MakeNeedGold * MakeitemCount)
        {
            GoCheckPan.gameObject.SetActive(false);
        }
        else
        {
            GoCheckPan.gameObject.SetActive(true);
        }
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
        //아이템 생성
        //Makeitemid
        List<PssItem> passitems = DataController.Instance.GetPssItemInfo().PssItemList;
        bool isHaveItem = false;
        for (int i = 0; i < passitems.Count; i++)
        {
            if (Makeitemid == passitems[i].Item_ID)
            {
                passitems[i].Amount = passitems[i].Amount + MakeitemCount;
                isHaveItem = true;
                break;
            }
        }
        if (isHaveItem == false)
        {
            //3. 여기서 MakeItemTypeName 에 따른 옵션 랜덤생성해서..넣기 일단. 임시
            int tempOptType = 1; //힘
            int tempOptPoint = 5;
            //(소유아이템 ID, player_id,아아템타입,아이템아이디,개수,장착여부,강화도,옵션타입(힘:S, 체력:C, 민첩), 옵션 포인트)
            passitems.Add(new PssItem(passitems.Count + 1, 1, MakeItemTypeName, Makeitemid, MakeitemCount, 0, 0, tempOptType, tempOptPoint));
        }

        PssItemInfoList pssiteminfolist = new PssItemInfoList();
        pssiteminfolist.SetPssItemList = passitems;             //소유 아이템 업데이트
        DataController.Instance.UpdateGameDataPssItem(pssiteminfolist);

        SceneManager.LoadScene(SceneName, LoadSceneMode.Single);  //현재씬 다시로드(가방, 대장간, 상점)
        /*
         * 체크사항
         * 1. 제조 재료가 1~3개 체크(Making_Stat 값으로) 재조 재료 슬롯 초기화 드 
         * 2. 각 필요재료 개수와 소유재료 개수를 가각 체크해서 최대 몇개까지 만들 수 있는지 체크해 슬라이드 벨류 결정(강화 아이템은 무조건 1개)
         * --> 각각 4개 조건(골드포함) 각각 최소 생성 가능 개수가 전체 생성 가능개수로 결정 후 슬라이드 벨류로 결정
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
                if (passitems[i].Equip_Stat == 1)
                {
                    //장착 아이템은 판매 할 수 없음
                }
                else //판매
                {
                    if (passitems[i].Amount > stuffcnt) //판매개수만 빼기
                    {
                        passitems[i].Amount = passitems[i].Amount - stuffcnt;
                    }
                    else
                    {
                        passitems.RemoveAt(i);
                    }
                    PssItemInfoList pssiteminfolist = new PssItemInfoList();
                    pssiteminfolist.SetPssItemList = passitems;             //소유 아이템 업데이트
                    DataController.Instance.UpdateGameDataPssItem(pssiteminfolist);
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
