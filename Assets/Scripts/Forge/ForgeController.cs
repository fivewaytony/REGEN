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

 //   bool isbtnWeapon = false , isbtnProtect = false, isbtnAcce = false, isbtnPotion = false, isbtnEtc = false;
 //   private string curTypeStr; //현재 선택된 아이템 타입
 
    // Use this for initialization
    private void Awake()
    {
        PlayerStatLoad();
        PlayerPssItemLoadALL(); //전체 소유 아이템 로드
    }

    void Start () {
        forgeInstance = this;
        Instance = this;

        // Retrieve the name of this scene.
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
                string[] arrEctType = { "약초", "광물", "보석", "기타" };
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
        List<GameItemInfo> itemList = DataController.Instance.GetGameItemInfo().GameItemList;  //전체 게임 아이템

        //아이템 List 선택 버튼 노출 초기화
        int childCnt = ListParent.childCount;
        for (int i = 0; i < childCnt; i++)
        {
            Destroy(ListParent.GetChild(i).gameObject);  //아이템 종류 선택 버튼 노출 초기화
        }
        ResetMakingPan();//제작 선택 아이템 정보 초기화

        // 대상만 리스트로 추출
        var typelist = from stype in itemList
                       where stype.Item_Type == typeName
                       select stype;

        foreach (var item in typelist)
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
    public Text Stuff1Cnt, Stuff2Cnt, Stuff3Cnt, GoldCnt; //필요 소유 개수
    
    private void ShowMakingPan(int itemid)
    {
        GameItemInfo Makeitem = DataController.Instance.gameitemDic[itemid];                //전체 아이템 정보
        MakeItemImg.transform.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Inventory/" + Makeitem.Item_ImgName);

        string Item_Leve = Makeitem.Item_Level;
        string Item_Name = Makeitem.Item_Name;
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
                break;
            case "Protect":
                Item_DescStr = "방어력 : " + Makeitem.Prt_Degree + "\n옵션 : 체력 ?";
                break;
            case "Acce":
                Item_DescStr = "회피력 : " + Makeitem.Ace_Degree + "\n옵션 : 민첩 ?";
                break;
            case "Potion": //물약의 설명 추가
                //switch (1)
                //{
                //    default:
                //        break;
                //}
                break;
            case "Stuff":
                break;
           case "Enhance":
                break;
            case "EtcProtect":
                break;
            case "Recipe":
                break;
            default:
                break;

        }
        MakeItemDesc.transform.GetComponent<Text>().text = Item_DescStr; //아이템 설명

        int Stuff1_ID = Makeitem.Stuff1_ID;
        int Stuff1_Count = Makeitem.Stuff1_Count;
        int Stuff2_ID = Makeitem.Stuff2_ID;
        int Stuff2_Count = Makeitem.Stuff2_Count;
        int Stuff3_ID = Makeitem.Stuff3_ID;
        int Stuff3_Count = Makeitem.Stuff3_Count;
        
        Debug.Log("Stuff1_ID=" + Stuff1_ID);
        Debug.Log("Stuff1_Count=" + Stuff1_Count);
        Debug.Log("Stuff2_ID=" + Stuff2_ID);
        Debug.Log("Stuff2_Count=" + Stuff2_Count);
        Debug.Log("Stuff3_ID=" + Stuff3_ID);
        Debug.Log("Stuff3_Count=" + Stuff3_Count);
        GameItemInfo Stuff1_itemInfo = DataController.Instance.gameitemDic[Stuff1_ID];
        GameItemInfo Stuff2_itemInfo = DataController.Instance.gameitemDic[Stuff2_ID];
        GameItemInfo Stuff3_itemInfo = DataController.Instance.gameitemDic[Stuff3_ID];
        //  string Stuff1_ImgName =
        Stuff1Img.transform.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Inventory/" + Stuff1_itemInfo.Item_ImgName);
        Stuff2Img.transform.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Inventory/" + Stuff2_itemInfo.Item_ImgName);
        Stuff3Img.transform.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Inventory/" + Stuff3_itemInfo.Item_ImgName);

        Stuff1Name.transform.GetComponent<Text>().text = Stuff1_itemInfo.Item_Name.ToString();
        Stuff2Name.transform.GetComponent<Text>().text = Stuff2_itemInfo.Item_Name.ToString();
        Stuff3Name.transform.GetComponent<Text>().text = Stuff3_itemInfo.Item_Name.ToString();

        int Makeitemstuff1Cnt = Makeitem.Stuff1_Count;
        int Makeitemstuff2Cnt = Makeitem.Stuff2_Count;
        int Makeitemstuff3Cnt = Makeitem.Stuff3_Count;

        PssItem pssStuff1 = DataController.Instance.pssitemDic[Stuff1_ID];
        PssItem pssStuff2 = DataController.Instance.pssitemDic[Stuff2_ID];
        PssItem pssStuff3 = DataController.Instance.pssitemDic[Stuff3_ID];
        Debug.Log("pssStuff1=" + pssStuff1.Amount);
        Debug.Log("pssStuff2=" + pssStuff2.Amount);
        Debug.Log("pssStuff3=" + pssStuff3.Amount);

        //   Stuff1Cnt.transform.GetComponent<Text>().text = Makeitem.Stuff1_Count.ToString();
        ////    Stuff2Cnt.transform.GetComponent<Text>().text = Makeitem.Stuff2_Count.ToString();
        Stuff3Cnt.transform.GetComponent<Text>().text = Makeitem.Stuff3_Count.ToString();



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
