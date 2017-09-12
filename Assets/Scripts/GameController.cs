using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Advertisements;

public class GameController : MonoBehaviour {

    //Main Scene
    public Text GoldText;
    public Text LevelText;
    public Text LevelBarText;
    public Image LeveBarFill;

    protected int PC_Exp;           //현재 경험치
    protected int PC_UpExp;     //업에 필요한 경험치
    protected int PC_WpnID;     //무기아이디
    protected int PC_FieldLevel;  //출입가능 필드레벨(현재 적용안함)
    protected int PC_Str;           // 힘
    protected int PC_Con;           //체력
    protected int PC_MaxHP;        //MaxHP

    protected int PssItem_ID;
    protected int GameItem_ID;
    protected string GameItem_Type;
    protected int Amount;
    protected int pssHP_Count = 0; //로딩 시 소유 물약 개수
    protected int CurpassHP_Count = 90;  //사용 or 구매 or 제조 후 업뎃이트 할 물약개수

    private float LevelBarNum;

    public GameObject FieldChoiceBack;
    public Transform FieldChoiceContent; //필드선택 팝업
    public int ChoiceFieldID;  //선택한 사냥터 ID(Level)
    
    public static GameController Instance; //GameController 접근하기 위해
    void Start ()
    {
        //DataController.Instance.CreateGameData();
        //DataController.Instance.CreatePssItem();  
        //DataController.Instance.PssItemLoadResourcesDEV();
        Instance = this;     //GameController 접근하기 위해
        PlayerStatLoad();   //플레이어 상태 로드 - _gameData에서 로드
        PlayerPssItemLoad();  // 플레이어 소유 아이템 로딩 _pssItem에서 로드
    }

    //랜덤숫자 생성 공통
    public int CommonRnd(int min, int max)
    {
        System.Random r = new System.Random();
        int retVal = r.Next(min, max);
        return retVal;
    }

    //플레이어 상태 로드
    protected void PlayerStatLoad()
    {
        LevelText.text = "Lv. " + DataController.Instance.gameData.PC_Level.ToString();
        PC_Exp = DataController.Instance.gameData.PC_Exp;                  //현재 경험치
        PC_UpExp = DataController.Instance.gameData.PC_UpExp;
        PC_WpnID = DataController.Instance.gameData.PC_WpnID;
        PC_MaxHP = DataController.Instance.gameData.PC_MaxHP;
        PC_Str = DataController.Instance.gameData.PC_Str;

        LevelBarNum = (PC_Exp * 100) / (float)PC_UpExp;      // 현재 경험치 --> %로 표시
        LevelBarText.text = String.Format("{0}", Math.Round(LevelBarNum, 1)) + "%";
        LeveBarFill.gameObject.GetComponent<Image>().fillAmount = PC_Exp / (float)PC_UpExp; //현재 경험치바
        
        string goldamount = DataController.Instance.gameData.PC_Gold;
        GoldText.text = String.Format("{0:n0}", Convert.ToDecimal(goldamount));
        LevelText.text = "Lv. " + DataController.Instance.gameData.PC_Level.ToString();

        //PC_FieldLevel = DataController.Instance.gameData.PC_FieldLevel;  //사냥필드레벨 -->출입제한없음
    }

    //플레이어 소유 아이템 로드
    protected void PlayerPssItemLoad()
    {
        List<PssItem> passitems = DataController.Instance.GetPssItemInfo().PssItemList;
        for (int i = 0; i < passitems.Count; i++)
        {
            if (passitems[i].GameItem_Type == "Hpotion")    //물약이면
            {
                pssHP_Count = passitems[i].Amount;          //소유물약개수
            }
            Debug.Log(passitems[i].GameItem_Type);
        }
      
        /*
        passitems.Add(new PssItem(3, 1, 9, "Stuff", 1, 0));
        foreach (var item in passitems)
        {
            Debug.Log("Add="+item.GameItem_Type);
        }
        PssItemInfoList pssiteminfolist = new PssItemInfoList();
        pssiteminfolist.SetPssItemList = passitems;
        DataController.Instance.SaveGameDataPssItem(pssiteminfolist);
        */
}

    //사냥터 선택 팝업
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
                if (text.tag=="FieldName")                                       //지정된 Tag로 정의 
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

    //
    // Update is called once per frame
    void Update () {
        
    }

    #region [JsonDataLoad()]
    //DataController 정의
    protected void JsonDataLoad()
    {

        //DataController.Instance.LoadFunc("PlayerStat.json", "playerstat");
        //DataController.Instance.LoadFunc("Character.json", "character");
        //DataController.Instance.LoadFunc("Weapon.json", "weapon");
        //DataController.Instance.LoadFunc("Field.json", "field");
        //DataController.Instance.LoadFunc("Monster.json", "monster");
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
    
    //광고 보여주기
    void ShowRewardedVideo()
    {
        var options = new ShowOptions();
        options.resultCallback = HandleShowResult;

        Advertisement.Show("rewardedVideo", options);
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
            Debug.LogError("Video failed to show");
        }
    }

}
