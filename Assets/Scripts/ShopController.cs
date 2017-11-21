using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Linq;

public class ShopController : GameController
{
    public static ShopController ishopInstance;

    private void Awake()
    {
         PlayerStatLoad();          //Player 상태 로드
         PlayerPssItemLoadALL(); //전체 소유 아이템 로드
    }

    // Use this for initialization
    void Start()
    {
        ishopInstance = this;
        Instance = this;
        ShowCaseItemView();
        // Retrieve the name of this scene.
        Scene currentScene = SceneManager.GetActiveScene();
        SceneName = currentScene.name;
        MakingLevel.text = "[제조 LV. " + PC_MakingLevel.ToString() + "]";  //제조 레벨
    }

    #region[구매상품 선택]
    public GameObject BuyConfirmBGPanel;    //구매상품정보보기BG panel
    public Image BuyItem;                           //구매할 상품 이미지
    public Text BuyItemName, BuyItemDesc;                      //구매할 상품명, 설명
    public Text GoldCnt;   //필요,소유 골드
    public GameObject CheckBuyPan;      //구매가능 체크 Panel

    public Button btnwpnrecipe; //일반무기제조법구매
    public Button btnprtrecipe; //일반방어구제조법구매
    public Button btnaccerecipe; //일반장신구제조법구매
    public Button btnmagicpowder; //마법가루구매
    public Button btnstuffbox; //고급재료상자구매

    public Button btnGoBuy; //구매하기버튼

    void ShowCaseItemView() { 
            Button wpnrecipe = btnwpnrecipe.GetComponent<Button>();
            wpnrecipe.onClick.AddListener(() => ShowBuyConfirm(1000)); //일반무기제조법

            Button prtrecipe = btnprtrecipe.GetComponent<Button>();
            prtrecipe.onClick.AddListener(() => ShowBuyConfirm(1001)); //일반방어구제조법

            Button accerecipe = btnaccerecipe.GetComponent<Button>();
            accerecipe.onClick.AddListener(() => ShowBuyConfirm(1002)); //일반장신구제조법

            Button magicpowder = btnmagicpowder.GetComponent<Button>();
            magicpowder.onClick.AddListener(() => ShowBuyConfirm(1150)); //마법가루

            Button stuffbox = btnstuffbox.GetComponent<Button>();
            stuffbox.onClick.AddListener(() => ShowBuyConfirm(1101)); //고급재료상자
    }

    void ShowBuyConfirm(int buyitemid)  //구매확인 Panel show
    {
        Debug.Log("buyitemid=" + buyitemid);
        BuyConfirmBGPanel.gameObject.SetActive(true);  //구매하기 BG Show

        GameItemInfo gameitem = DataController.Instance.gameitemDic[buyitemid];                // 아이템 정보

        BuyItem.transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Inventory/" + gameitem.Item_ImgName);
        BuyItemName.transform.GetComponent<Text>().text = gameitem.Item_Name.ToString();
        BuyItemDesc.transform.GetComponent<Text>().text = gameitem.Item_Desc.ToString();

        int buyprice = 0;
        if (buyitemid == 1150) //마법가루는 10개씩 구매
        {
            buyprice = gameitem.Item_BuyPrice * 100000; //단가 * 10
        }
        else
        {
            buyprice = gameitem.Item_BuyPrice;
        }

        string formatNeedGold = String.Format("{0:n0}", Convert.ToDecimal(buyprice));
        GoldCnt.transform.GetComponent<Text>().text = formatPC_Gold + " / " + formatNeedGold;

        //필요골드
        long pssGold = Convert.ToInt64(PC_Gold);  //소유 골드

        Button gobuy = btnGoBuy.GetComponent<Button>();
        gobuy.onClick.RemoveAllListeners(); //구매처리버튼 Add 된 Listener 제거
        if (pssGold >= buyprice)
        {
            CheckBuyPan.gameObject.SetActive(false);  //구매하기 활성화
            gobuy.onClick.AddListener(() => ProcBuyItem(buyitemid, buyprice)); //구매처리하기
        }
        else
        {
            CheckBuyPan.gameObject.SetActive(true);//구매하기 비활성화
        }
    }

    //구매하기 처리
    void ProcBuyItem(int buyitemid,int buyprice)
    {
        Debug.Log("ProcBuyItem buyitemid=" + buyitemid);
        List <PssItem> passitems = DataController.Instance.GetPssItemInfo().PssItemList; //소유아이템

        //골드 정산 
        CalGold(buyprice, 1, "minus");

        bool isHaveItem = false;
        int addCount = 0;
        if (buyitemid == 1150) //마법가루는 10개씩 구매
        {
            addCount = 10; //10개씩 판매
        }
        else
        {
            addCount = 1;
        }

        for (int i = 0; i < passitems.Count; i++)
        {
            if (buyitemid == passitems[i].Item_ID)  //가지고 있으면 수량만 +
            {
                passitems[i].Amount = passitems[i].Amount + addCount;
                isHaveItem = true;
                break;
            }
        }
        if (isHaveItem == false)                //소유하지 않은 아이템은 생성
        {
            GameItemInfo gameitem = DataController.Instance.gameitemDic[buyitemid];                // 아이템 정보
            //(소유아이템 ID, player_id,아아템타입,아이템아이디,개수,장착여부,강화도,옵션타입(힘:1, 체력:2, 민첩3), 옵션 포인트)
            passitems.Add(new PssItem(passitems.Count + 1, 1, gameitem.Item_Group, gameitem.Item_Type, buyitemid, addCount, 0, 0, 0, 0));
        }

        PssItemInfoList pssiteminfolist = new PssItemInfoList();
        pssiteminfolist.SetPssItemList = passitems;             //소유 아이템 업데이트
        DataController.Instance.UpdateGameDataPssItem(pssiteminfolist); //소유 아이템 파일 다시 쓰기
        DataController.Instance.GetPssItemInfoReload();  // 소유 아이템 파일 다시 읽기

        BuyConfirmBGPanel.gameObject.SetActive(false);
        SceneManager.LoadScene(SceneName, LoadSceneMode.Single);  //현재씬 다시로드(가방, 대장간, 상점)
    }

    //구매상품정보보기BG panel 닫기
    public void CloseBuyConfirm()
    {
        BuyConfirmBGPanel.gameObject.SetActive(false);
    }

    #endregion

    // Update is called once per frame
    void Update () {
      
    }
    
}
