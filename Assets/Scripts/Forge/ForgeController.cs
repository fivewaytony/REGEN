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
    public RectTransform TypeParent;
    public GameObject prefabTypeBtn;

    bool isbtnWeapon = false , isbtnbtnProtect = false, isbtnAcce = false, isbtnPotion = false, isbtnEtc = false;
 
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
     }

    #region [아이템 그룹 버튼 클릭]
    public void ClickGroupBtn()
    {
        btnWeapon.GetComponent<Button>().onClick.AddListener(() => ShowTypePan(1));     //무기선택
        btnProtect.GetComponent<Button>().onClick.AddListener(() => ShowTypePan(2));  ///방어구
        btnAcce.GetComponent<Button>().onClick.AddListener(() => ShowTypePan(3));  //장신구
        btnPotion.GetComponent<Button>().onClick.AddListener(() => ShowTypePan(4));  //물약
        btnEtc.GetComponent<Button>().onClick.AddListener(() => ShowTypePan(5));     //기타

    }
    #endregion

    #region [아이템 종류 선택 버튼 노출]
    public void ShowTypePan(int typeNum)
    {
        int childCnt = TypeParent.childCount;
        for (int i = 0; i < childCnt; i++)
        {
            Destroy(TypeParent.GetChild(i).gameObject);  //아이템 종류 선택 버튼 노출 초기화
        }

        switch (typeNum)
        {
            case 1:
                Debug.Log("무기");
                if (!isbtnWeapon)
                {
                    isbtnWeapon = true;
                    isbtnbtnProtect = false;
                    isbtnAcce = false;
                    isbtnPotion = false;
                    isbtnEtc = false;
                    //
                    for (int i = 0; i < 1; i++)
                    {
                        GameObject goButton = (GameObject)Instantiate(prefabTypeBtn);
                        goButton.transform.SetParent(TypeParent, false);
                        goButton.transform.localScale = new Vector3(1, 1, 1);
                        goButton.transform.GetChild(0).GetComponent<Text>().text = "한손무기";

                        Button tempButton = goButton.GetComponent<Button>();
                        tempButton.onClick.AddListener(() => ShowListPan(1, i));
                    }
                }
                break;
            case 2:
                Debug.Log("방어구");
                if (!isbtnbtnProtect)
                {
                    isbtnWeapon = false;
                    isbtnbtnProtect = true;
                    isbtnAcce = false;
                    isbtnPotion = false;
                    isbtnEtc = false;
                    for (int i = 0; i < 4; i++)
                    {
                        GameObject goButton = (GameObject)Instantiate(prefabTypeBtn);
                        goButton.transform.SetParent(TypeParent, false);
                        goButton.transform.localScale = new Vector3(1, 1, 1);
                        goButton.transform.GetChild(0).GetComponent<Text>().text = "방어구";

                        Button tempButton = goButton.GetComponent<Button>();
                        tempButton.onClick.AddListener(() => ShowListPan(2, i));
                    }
                }
                break;
            case 3:
                Debug.Log("장신구");
                if (!isbtnAcce)
                {
                    isbtnWeapon = false;
                    isbtnbtnProtect = false;
                    isbtnAcce = true;
                    isbtnPotion = false;
                    isbtnEtc = false;
                    for (int i = 0; i < 3; i++)
                    {
                        GameObject goButton = (GameObject)Instantiate(prefabTypeBtn);
                        goButton.transform.SetParent(TypeParent, false);
                        goButton.transform.localScale = new Vector3(1, 1, 1);
                        goButton.transform.GetChild(0).GetComponent<Text>().text = "장신구";

                        Button tempButton = goButton.GetComponent<Button>();
                        tempButton.onClick.AddListener(() => ShowListPan(3, i));
                    }
                }
                break;
            case 4:
                Debug.Log("물약");
                if (!isbtnPotion)
                {
                    isbtnWeapon = false;
                    isbtnbtnProtect = false;
                    isbtnAcce = false;
                    isbtnPotion = true;
                    isbtnEtc = false;
                    for (int i = 0; i < 3; i++)
                    {
                        GameObject goButton = (GameObject)Instantiate(prefabTypeBtn);
                        goButton.transform.SetParent(TypeParent, false);
                        goButton.transform.localScale = new Vector3(1, 1, 1);
                        goButton.transform.GetChild(0).GetComponent<Text>().text = "사냥";

                        Button tempButton = goButton.GetComponent<Button>();
                        tempButton.onClick.AddListener(() => ShowListPan(4, i));
                    }
                }
                break;
            case 5:
                Debug.Log("기타재료");
                if (!isbtnEtc)
                {
                    isbtnWeapon = false;
                    isbtnbtnProtect = false;
                    isbtnAcce = false;
                    isbtnPotion = false;
                    isbtnEtc = true;
                    string[] EctType =new string[4] { "약초", "광물", "보석", "기타" };
                    for (int i = 0; i < EctType.Length; i++)
                    {
                        GameObject goButton = (GameObject)Instantiate(prefabTypeBtn);
                        goButton.transform.SetParent(TypeParent, false);
                        goButton.transform.localScale = new Vector3(1, 1, 1);
                        goButton.transform.GetChild(0).GetComponent<Text>().text = EctType[i];

                        Button tempButton = goButton.GetComponent<Button>();
                        tempButton.onClick.AddListener(() => ShowListPan(5, i));
                    }
                }
                break;
            default:
                break;
        }
    }
    #endregion
    
    public void ShowListPan(int gNum, int tNum)
    {
        Debug.Log("tNum=" + tNum);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            base.GoMain();
        }
    }
}
