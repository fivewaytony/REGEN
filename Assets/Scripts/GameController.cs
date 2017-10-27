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
    public Text LevelText;
    public Text LevelBarText;
    public Image LevelBarFill;

    //골드&다이어
    public Text GoldText;
    public Text DiaText;

    protected int PC_Level;         //현재 레벨
    protected int PC_MakingLevel; //제조 레벨
    protected int PC_Exp;           //현재 경험치
    protected int PC_UpExp;     //업에 필요한 경험치
    protected int PC_FieldLevel;  //출입가능 필드레벨(현재 적용안함)
    protected int PC_Str;           // 힘
    protected int PC_Con;           //체력
    protected int PC_Dex;           //민첩
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

    /* 밸러스 조정용 수치*/
    protected float wpn_attrate;
    protected float pc_hprate;
    protected float mon_attrate;
    protected float mon_hprate;

    /* 가방 / 상점에서 선택한 아이템 정보 */
    protected int SelectItemID; //선택한 아이템 ID
    protected int SelectItemAmount; //선택한 아이템 소유량
    protected int SelectItemPrice;    //선택한 아이템 단가

    /* 아이템 인벤 */
    private int slotCount = 120;     //슬롯 개수
    public Transform SlotsParentContent; //인벤토리 부모 팬넬
    public List<GameObject> slots = new List<GameObject>(); //각 슬록 List

    public GameObject ItemInfoBackPanel;    //아이템 상세보기 panel
    public GameObject ItemInfoSellPanel;    //아이템 팔기 panel
    public Text ItemInfoNameText, ItemInfoDescText; //아이템 정보
    public Image ItemCountBarFill;  //아이템 개수 설정 bar
    public Text ItemCountTxt;       //아이템개수 Text
    public Slider ItemCountSilder; //아이템 개수 선택 슬라이더
    public string SceneName = string.Empty;
    

#if UNITY_IOS
    string gameId = "1537760";
#elif UNITY_ANDROID
    string gameId = "1537759";
#endif
    private void Awake()
    {
        //   DataController.Instance.PlayerStatLoadResourcesDEV();
        //    DataController.Instance.PssItemLoadResourcesDEV();
        PlayerStatLoad();
    }

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
        Instance = this;     //GameController 접근하기 위해
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
            PC_MakingLevel = pcstat.PC_MakingLevel;
            PC_Exp = pcstat.PC_Exp;
            PC_UpExp = pcstat.PC_UpExp;
            PC_MaxHP = pcstat.PC_MaxHP;
            PC_Str = pcstat.PC_Str;
            PC_Con = pcstat.PC_Con;
            PC_Dex = pcstat.PC_Dex;
            PC_Gold = pcstat.PC_Gold;
            PC_Dia = pcstat.PC_Dia;
        }        
       
        LevelText.text = "Lv. " + PC_Level.ToString();
        LevelBarNum = (PC_Exp * 100) / (float)PC_UpExp;      // 현재 경험치 --> %로 표시
        LevelBarText.text = String.Format("{0}", Math.Round(LevelBarNum, 1)) + "%";
        LevelBarFill.gameObject.GetComponent<Image>().fillAmount = PC_Exp / (float)PC_UpExp; //현재 경험치바
        //string goldAmount = Convert.ToDecimal(PC_Gold);
        string vPC_Gold = string.Empty;
        int vPC_Dia;
        if (Convert.ToDecimal(PC_Gold) > 99999999 )
        {
            vPC_Gold = "99999999";
        }
        else
        {
            vPC_Gold = PC_Gold;
        }
        if (Convert.ToDecimal(PC_Dia) > 99999)
        {
            vPC_Dia = 99999;
        }
        else
        {
            vPC_Dia = PC_Dia;
        }
        GoldText.text = String.Format("{0:n0}", Convert.ToDecimal(vPC_Gold));
        DiaText.text = String.Format("{0:n0}", Convert.ToDecimal(vPC_Dia));
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

    #region [골드정산(Plus , Minus)]
    public void CalGold(int price, int amount, string caldiv)
    {
        decimal appGold = price * amount;
        List<PlayerStat> playerstats = DataController.Instance.GetPlayerStatInfo().StatList;
        foreach (var ps in playerstats)
        {
            if (caldiv =="plus")
            {
                ps.PC_Gold = (Convert.ToDecimal(ps.PC_Gold) + Convert.ToDecimal(appGold)).ToString();
            }
            else
            {
                ps.PC_Gold = (Convert.ToDecimal(ps.PC_Gold) - Convert.ToDecimal(appGold)).ToString();
            }
        }
        PlayerStatList playerstatlist = new PlayerStatList();
        playerstatlist.SetPlayerStatList = playerstats;
        DataController.Instance.UpdateGameDataPlayerStat(playerstatlist);
    }
    #endregion
    
    #region [플레이어 소유(인벤) 아이템 전체 로드]
    protected void PlayerPssItemLoadALL()
    {
        /* Scroll viewport 위치 임시 수정 - 확인필요 
         sv.gameObject.SetActive(false);
         sv.gameObject.SetActive(true);
        */
        Debug.Log("invenslot");
        GameObject sv = GameObject.Find("SlotsScroll View");
        sv.gameObject.SetActive(false);

        List<PssItem> passitems = DataController.Instance.GetPssItemInfo().PssItemList; //소유 아이템
        List<GameItemInfo> itemList = DataController.Instance.GetGameItemInfo().GameItemList;  //전체 게임 아이템

        /* 슬롯 그리기 */
        for (int i = 0; i < slotCount; i++)
        {
            GameObject SlotInfo = Resources.Load("Prefabs/InventorySlot") as GameObject;  //프리팹으로 등록된 정보 불러옴
            GameObject objslot = Instantiate(SlotInfo, SlotsParentContent);   //자식 오브젝트
            slots.Add(objslot);

            RectTransform rt = objslot.GetComponent<RectTransform>(); //SlotInfo
            rt.anchoredPosition = new Vector2(0f, 0f);       // 자식 오브젝트를 위치를 잡고 그린다
        }
        sv.gameObject.SetActive(true);
        ItemInfoMng iteminfo = new ItemInfoMng();  //itemID 전달용 

        for (int i = 0; i < passitems.Count; i++)
        {
            foreach (GameItemInfo item in itemList)
            {
                if (passitems[i].Item_ID == item.Item_ID)
                {
                    Color color = slots[i].transform.GetChild(0).GetComponent<Image>().color;
                    color.a = 1f;
                    slots[i].transform.GetChild(0).GetComponent<Image>().color = color;

                    //Debug.Log("item.Item_ImgName=" + item.Item_ImgName);
                    //Debug.Log("item.ItemID = " + passitems[i].Item_ID);
                    slots[i].transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Inventory/" + item.Item_ImgName);
                    slots[i].transform.GetChild(1).GetComponent<Text>().text = passitems[i].Amount.ToString();

                    slots[i].GetComponent<ItemInfoMng>().ItemID = passitems[i].Item_ID;
                    slots[i].GetComponent<ItemInfoMng>().ItemAmount = passitems[i].Amount;

                    //장착 아이템 이미지 표시
                    if (passitems[i].Equip_Stat == 1)
                    {
                        Color ecolor = slots[i].transform.GetChild(2).GetComponent<Image>().color;
                        ecolor.a = 1f;
                        slots[i].transform.GetChild(2).GetComponent<Image>().color = ecolor;
                        slots[i].transform.GetChild(2).GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Inventory/equline");
                    }
                    break;
                }
            }
        }
                
    }
    #endregion

    #region [아이템 정보 Panel Show / Close]
    public void ShowItemInfoPanel(int ItemID, int ItemAmount)
    {
        Debug.Log("ItemID=" + ItemID);
        Debug.Log("ItemAmount=" + ItemAmount);
        if (ItemID != 0)
        {
            ItemInfoBackPanel.gameObject.SetActive(true);
                    
            PssItem pssitem = DataController.Instance.pssitemDic[ItemID];
            GameItemInfo item = DataController.Instance.gameitemDic[ItemID];                //전체 아이템 정보
            string equDesc = string.Empty;
            if (pssitem.Equip_Stat == 1) //장착아이템 (판매X))
            {
                ItemInfoSellPanel.gameObject.SetActive(false);
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
            }
            else
            {
                //장착하기 링크 만들기

                ItemInfoSellPanel.gameObject.SetActive(true);
            }

            ItemInfoNameText.text = item.Item_Name;
            string DescStr = string.Empty;
            DescStr = equDesc + "\n판매가격 : " + item.Item_Price.ToString() + "골드";

            if (item.Item_Type == "Stuff")
            {
                DescStr = DescStr + "\n아이템 제조의 재료로 필요합니다.";
            }
            ItemInfoDescText.text = DescStr;

            /*개수 Silder*/
            ItemCountSilder.value = 1;
            ItemCountSilder.minValue = 1f;   //1개부터
            ItemCountSilder.maxValue = ItemAmount;  //소유 아이템 개수 까지
            ItemCountTxt.text = ItemCountSilder.value.ToString();

            /* 현재 선택 아이템 정보 */
            SelectItemID = ItemID;
            SelectItemAmount = Convert.ToInt32(ItemCountTxt.text); //선택 개수
            SelectItemPrice = item.Item_Price; //단가(골드)
        }
    }

    //Silder 변경하면 개수 표시
    public void ItemCountSilderChange()
    {
        ItemCountTxt.text = ItemCountSilder.value.ToString();
        SelectItemAmount = Convert.ToInt32(ItemCountTxt.text); //선택 개수
    }

    //아이템 팔기
    public void SellpssItem()
    {
        //소유 아이템에서 빼기
        //Gold 더하기 : 판매할 아이템 단가 구하기

        Debug.Log("SelectItemID=" + SelectItemID);
        Debug.Log("SelectItemAmount=" + SelectItemAmount);
        Debug.Log("SelectItemPrice=" + SelectItemPrice);

        List<PssItem> passitems = DataController.Instance.GetPssItemInfo().PssItemList;
        for (int i = 0; i < passitems.Count; i++)
        {
            if (passitems[i].Item_ID == SelectItemID)
            {
                if (passitems[i].Equip_Stat == 1)//장착아이템은 팔수 없음 (물약은 가능)
                {
                    //장착 아이템은 판매 할 수 없음
                }
                else //판매
                {
                    if (passitems[i].Amount > SelectItemAmount) //판매개수만 빼기
                    {
                        passitems[i].Amount = passitems[i].Amount - SelectItemAmount;
                    }
                    else
                    {
                        passitems.RemoveAt(i);
                    }
                    PssItemInfoList pssiteminfolist = new PssItemInfoList();
                    pssiteminfolist.SetPssItemList = passitems;             //소유 아이템 업데이트
                    DataController.Instance.UpdateGameDataPssItem(pssiteminfolist);

                    //골드 정산 
                    CalGold(SelectItemPrice, SelectItemAmount, "plus");
                }
                break;
            }
        }
        SceneManager.LoadScene(SceneName, LoadSceneMode.Single);
        //   ItemInfoBackPanel.gameObject.SetActive(false); //아이템 정보창 닫기
        //   PlayerPssItemLoadALL();                              // 인벤 다시 로드
    }

    //상세 Panel 닫기
    public void CloseItemInfoPanel()
    {
        ItemInfoBackPanel.gameObject.SetActive(false);
    }
    #endregion

    #region [아이템 그룹정의 & 아이템 리스트 - enum 정의]
    public enum ItemGroup
    {
        Weapon = 1, Protect, Acce, Potion, Etc
    }
    //무기
    public enum WeaponType
    {
        OWeapon = 1, TWeapon
    }
    //방어구
    public enum ProtectType
    {
        Helmet=1, Armor, Gauntlet, Boots
    }
    //장신구
    public enum AcceType
    {
        Earring = 1,Necklace,Ring
    }
    //물약
    public enum PotionType
    {
        Hunting = 1, Mining, Foraging
    }
    //기타
    public enum EtcType
    {
        Herb =1 , Mineral, Gem, Special
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

    //대장간 이동
    public void GoForge()
    {
        SceneManager.LoadScene("Forge", LoadSceneMode.Single);
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
