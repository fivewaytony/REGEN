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
    public GameObject btnWeapon;
    public RectTransform TypePanel;
    private GameObject newbutton;

    // Use this for initialization
    private void Awake()
    {
        PlayerStatLoad();
        PlayerPssItemLoadALL(); //전체 소유 아이템 로드

    }

    void Start () {
        forgeInstance = this;
        ClickGroupBtn(); //아이템 그룹 선택
    }
      
    // Update is called once per frame
    void Update () {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            base.GoMain();
        }
    }

    public void ClickGroupBtn()
    {
        btnWeapon.GetComponent<Button>().onClick.AddListener(() => ShowTypePan(1));  //무기선택
        //방어구
        //장신구
        //물약
        //기타
    }

    public void ShowTypePan(int btype)
    {
        switch (btype)
        {
            case 1:
                Debug.Log("무기");
                for (int i = 0; i < 1; i++)
                {
                    //GameObject OnehandWpn = (GameObject)Instantiate(new (Button)newbutton);
                    //OnehandWpn.transform.SetParent(TypePanel, false);
                    //OnehandWpn.transform.localScale = new Vector3(1, 1, 1);

                    //Button tempButton = OnehandWpn.GetComponent<Button>();
                    //int tempInt = i;

                    // tempButton.onClick.AddListener(() => ButtonClicked(tempInt));

                    //var button = Instantiate(RoomButton, Vector3.zero, Quaternion.identity) as Button;
                    //var rectTransform = button.GetComponent<RectTransform>();
                    //rectTransform.SetParent(Canvas.transform);
                    //rectTransform.offsetMin = Vector2.zero;
                    //rectTransform.offsetMax = Vector2.zero;
                    //    button.onClick.AddListener(SpawnPlayer);
                    //  Vector2 btnPos = new Vector2(0, 0);
                    //    var button = CreateButton(newbutton, TypePanel, btnPos, btnPos);
                    Vector3 position = new Vector3(0f, 0f, 0f);
                    Vector2 size = new Vector2(80f, 30f);
                    UnityAction method = new UnityAction(ShowListPan);
                    CreateButton(TypePanel, position, size, method);
                }
                 break;
            case 2:
                Debug.Log("방어구=" );
                break;
            default:
                break;
        }
    }

    public void CreateButton(Transform panel, Vector3 position, Vector2 size, UnityEngine.Events.UnityAction method)
    {
        GameObject button = new GameObject();
        button.transform.SetParent(panel, false);
      //  button.transform.parent = panel;
       button.AddComponent<RectTransform>();
        //button.AddComponent<Button>();
     //   button.transform.position = position;
        button.GetComponent<RectTransform>().sizeDelta = size;
       // button.GetComponent<Button>().onClick.AddListener(method);
    }


    public void ShowListPan()
    {

    }
    //public static Button CreateButton(Button newButton, RectTransform typepanel, Vector2 cornerTopRight, Vector2 cornerBottomLeft)
    //{
    //    var button = UnityEngine.Object.Instantiate(newButton, Vector2.zero, Quaternion.identity) as Button;
    //    var rectTransform = button.GetComponent<RectTransform>();
    //    rectTransform.SetParent(typepanel.transform);
    //    rectTransform.anchorMax = cornerTopRight;
    //    rectTransform.anchorMin = cornerBottomLeft;
    //    rectTransform.offsetMax = Vector2.zero;
    //    rectTransform.offsetMin = Vector2.zero;
    //    return button;
    //}


    #region [인벤토리 아이템 상세보기 패널 - 오류]
    public void forgeItemInfoPan(int ItemID, int ItemAmount)
    {
        base.ShowItemInfoPanel(ItemID, ItemAmount);
    }
    #endregion

    //대장간에서 인벤상세보기 오류 
}
