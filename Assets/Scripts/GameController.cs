using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Advertisements;
using UnityEngine.Purchasing;

public class GameController : MonoBehaviour {

    //Main Scene
    public Text GoldText;
    public Text LevelText;
    public Text LevelBarText;
    public Image LeveBarFill;

    protected int PC_Level;         //현재 레벨
    protected int PC_Exp;           //현재 경험치
    protected int PC_UpExp;     //업에 필요한 경험치
    protected int PC_FieldLevel;  //출입가능 필드레벨(현재 적용안함)
    protected int PC_Str;           // 힘
    protected int PC_Con;           //체력
    protected float PC_MaxHP;        //MaxHP
    protected string PC_Gold;       //골드
    protected int PC_Dia;      //다이아

    protected int PssItem_ID;
    protected string GameItem_Type;
    protected int Amount;

    protected int pssHP_Count; //소유 물약 개수

    private float LevelBarNum;

    public GameObject FieldChoiceBack;
    public Transform FieldChoiceContent; //필드선택 사냥터 - 부모
    public int ChoiceFieldID;  //선택한 사냥터 ID(Level)

    protected float wpn_attrate;
    protected float pc_hprate;
    protected float mon_attrate;
    protected float mon_hprate;

#if UNITY_IOS
    string gameId = "1537760";
#elif UNITY_ANDROID
    string gameId = "1537759";
#endif
    public static GameController Instance; //GameController 접근하기 위해
    void Start ()
    {
        if (Advertisement.isSupported)
        {
            Advertisement.Initialize(gameId, true);
            Debug.Log("Ad init");
        }
        else
        {
            Debug.LogError("Ad is not supported");
        }
       // DataController.Instance.PlayerStatLoadResourcesDEV();
       // DataController.Instance.PssItemLoadResourcesDEV();
        Instance = this;     //GameController 접근하기 위해
        PlayerStatLoad();   //플레이어 상태 로드
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            DialogDataConfirm confirm = new DialogDataConfirm("게임 종료", "게임을 종료하시겠습니까?",
                delegate (bool yn) {
                    if (yn)
                    {
                        Application.Quit();
                        // Debug.Log("Confirm OK");
                    }
                    else
                    {
                       // Debug.Log("Confirm Cancel");
                    }

                });

            DialogManager.Instance.Push(confirm);
        }
     }

    #region [밸런싱 수치]
    protected void BalanceInfoLoad()
    {
        List<Balance> balanceinfos = DataController.Instance.GetBalanceInfo().BalanceList;
        foreach (var bi in balanceinfos)
        {
            wpn_attrate = bi.Wpn_AttackSecRate;
            pc_hprate = bi.PC_HPRate;
            mon_attrate = bi.Mon_AttackSecRate;
            mon_hprate = bi.Mon_HPRate;
        }
    }
    #endregion

    #region [랜덤숫자 생성 -  공통]
    public int CommonRnd(int min, int max)
    {
        System.Random r = new System.Random();
        int retVal = r.Next(min, max);
        return retVal;
    }
    #endregion

    #region [플레이어 상태 로드]
    protected void PlayerStatLoad()
    {
        //string goldAmount = 
        List<PlayerStat> playerstats = DataController.Instance.GetPlayerStatInfo().StatList;
        foreach (var pcstat in playerstats)
        {
            PC_Level = pcstat.PC_Level;
            PC_Exp = pcstat.PC_Exp;
            PC_UpExp = pcstat.PC_UpExp;
            PC_MaxHP = pcstat.PC_MaxHP;
            PC_Str = pcstat.PC_Str;
            PC_Con = pcstat.PC_Con;
            PC_Gold = pcstat.PC_Gold;
        }
        
        LevelText.text = "Lv. " + PC_Level.ToString();
        LevelBarNum = (PC_Exp * 100) / (float)PC_UpExp;      // 현재 경험치 --> %로 표시
        LevelBarText.text = String.Format("{0}", Math.Round(LevelBarNum, 1)) + "%";
        LeveBarFill.gameObject.GetComponent<Image>().fillAmount = PC_Exp / (float)PC_UpExp; //현재 경험치바
        //string goldAmount = Convert.ToDecimal(PC_Gold);
        GoldText.text = String.Format("{0:n0}", Convert.ToDecimal(PC_Gold));
        //DiaText.text = ""; --> 추가예정

        //PC_FieldLevel =  //사냥필드레벨 -->출입제한없음
    }
    #endregion
    
    #region [사냥터 선택 팝업]
    public void FieldChoicePop()
    {
        FieldChoiceBack.gameObject.SetActive(true);
        List<FieldInfo> fieldchoices = DataController.Instance.GetFieldInfo().FieldList;
        int i = 0;

        foreach (FieldInfo fielditem in fieldchoices)
        {
            GameObject FieldInfo = Resources.Load("Prefabs/FieldInfo") as GameObject;  //프리팹으로 등록된 정보 불러옴
            GameObject obj = Instantiate(FieldInfo, FieldChoiceContent);   //자식 오브젝트

            RectTransform rt = obj.GetComponent<RectTransform>(); //FieldInfo
            rt.anchoredPosition = new Vector2(0f, -40f + (i * -80f));       // 자식 오브젝트를 위치를 잡고 그린다

            Text[] texts = obj.GetComponentsInChildren<Text>();         //자식 오브젝트중 Text 동적으로 변경
            foreach (Text text in texts)
            {
                if (text.tag == "FieldName")                                       //지정된 Tag로 정의 
                {
                    int FieldLevel = fielditem.Field_Level;
                    string FieldName = fielditem.Field_Name;

                    // if (PC_FieldLevel >= FieldLevel)  //플레이어의 입장 가능 사냥터 레벨제한 없음
                    String strFieldName = String.Format("Lv. {0}  {1}", FieldLevel, FieldName);
                    text.text = strFieldName;
                }
                if (text.tag == "DropItem")
                {
                    string ItemName = fielditem.GetItem;
                    String strItemName = String.Format("{0}", ItemName);
                    text.text = strItemName;
                }
                Button[] btns = obj.GetComponentsInChildren<Button>();
                foreach (Button btn in btns)
                {
                    btn.onClick.AddListener(() => GoHunting(fielditem.Field_Level));
                }
            }
            i++;
        }

    }
    #endregion
    
    #region [가방 가득 여부 / 가방 가득참 얼럿]
    protected bool GetisSlotEmpty()
    {
        //가방 슬롯 개수와 소유 아이템 개수(소유아이템 뿌릴때 개수를 저녁 변수에 담기) 비교 남은 슬롯 개수 구하기

        return false;
    }

    protected void InvenFullAlert()
    {
        DialogDataAlert alert = new DialogDataAlert("[알림]", "가방이 가득 찼습니다!", delegate () {
            GoInventory();
        });
        DialogManager.Instance.Push(alert);
    }
    #endregion


    //사냥 이동
    public void GoHunting(int cfd)
    {
       ChoiceFieldID = cfd;  //선택한 필드 아이디 할당
       SceneManager.LoadScene("Hunting", LoadSceneMode.Single);
    }

    //상점 이동
    public void GoShop()
    {
        Debug.Log("광고보여주기");
        ShowRewardedVideo();
    }
    
    //가방 이동
    public void GoInventory()
    {
        SceneManager.LoadScene("Inventory", LoadSceneMode.Single);
    }

    //Main 이동
    public void GoMain()
    {
        SceneManager.LoadScene("Main", LoadSceneMode.Single);
    }

#region [광고 보여주기]
    void ShowRewardedVideo()
    {
        if (Advertisement.IsReady("rewardedVideo"))
        {

            var options = new ShowOptions();
            options.resultCallback = HandleShowResult;

            Advertisement.Show("rewardedVideo", options);
        }
        else
        {
            Debug.LogError("Ready Fail");
        }
    }

    void HandleShowResult(ShowResult result)
    {
        if (result == ShowResult.Finished)
        {
            Debug.Log("Video completed - Offer a reward to the player");

        }
        else if (result == ShowResult.Skipped)
        {
            Debug.LogWarning("Video was skipped - Do NOT reward the player");

        }
        else if (result == ShowResult.Failed)
        {
            Debug.LogError("Video failed to show" + result.ToString());
        }
    }
#endregion

#region [인앱결제]
    public void PurchaseComplete(Product p)
    {
        Debug.Log(p.metadata.localizedTitle + " purchase success!");
    }
#endregion


}
