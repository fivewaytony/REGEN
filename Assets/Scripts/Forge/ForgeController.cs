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
           tempButton.onClick.AddListener(() => ShowMakingPan(item.Item_ID, item.Item_ImgName, item.Item_Name));
        }
     
    }
    #endregion

    #region [아이템 제작 Panel - 아이템 정보]
    private void ShowMakingPan(int itemid, string itemimg, string itemname)
    {
        // 아이템 제작 제료
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
