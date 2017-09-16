using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Linq;


public class HuntingController : GameController
{
    //Huting Scene
    public Image FieldImage;
    public Text FieldName;
    public Camera MainCamera;
    public Image WeaponImage;

    public GameObject MonsterBG;
    public Image MonsterImage;
    public Text MonsterName;
    public Image MonsterHit;
    public Text MonHPBarText;
    public Image MonHPBarFill;
    public Image PlayerHit;
    public Image WeaponPanel;
    public Text PlayerHPBarText;
    public Image PlayerHPBarFill;
    public Text GetItemText;

    public Text CurHP_Count;

    public AudioClip SFXClick;

    private float MonHPBarNum;
    private int Mon_HP;         //최초 Max HP
    private int Mon_CurHP;    //현재 Mon HP
    private int Mon_AttackDmg; // 1회 공격력
    private bool isMonOnLoad = false;
    private string Mon_DropItem;
    private int PC_GetItem;
    private int Mon_DropGold;
    private int Mon_ReturnExp;
    private int Mon_GroupLevel;
    
    private float PlayerHPBarNum;
    private int Player_CurHP;
    private int Wpn_Attack;
    private string GetItemTextView;

    void Start()
    {
        /* DataPath 에서 기본 정보 로딩 매신마다 로딩*/
        PlayerStatLoad();   
        PlayerPssItemLoad();
        CurHP_Count.text = pssHP_Count.ToString() + "개"; //물약개수 로딩

        /* 기본 정보 로딩 매신마다 로딩*/
        FieldBGLoad(); //사냥터 배경로드
       
        WeaponLoad(); //무기
        MonsterLoad();//몹로드
        
        StartCoroutine(UpdateGameData()); //게임 데이터(PC 상태, 소유 아이템)
    }

    #region [필드 로딩]
    private void FieldBGLoad()
    {
        List<FieldInfo> fieldinfos = DataController.Instance.GetFieldInfo().FieldList;
        int bgNum = Instance.CommonRnd(1, 4);
        foreach (FieldInfo fielditem in fieldinfos)
        {
            if (Instance.ChoiceFieldID == fielditem.Field_Level) //선택 사냥터
            {
                FieldImage.sprite = Resources.Load<Sprite>("Sprites/FieldBackGround/" + fielditem.Field_ImgName + bgNum.ToString());
                FieldName.text = fielditem.Field_Name;
            }
        }
    }
    #endregion

    #region [몬스터 로딩]
    private void MonsterLoad()
    {
        MonsterBG.gameObject.SetActive(true);
        List<MonsterInfo> monsterinfos = DataController.Instance.GetMonsterInfo().MonsterList;
        List<MonsterInfo> courMonster = new List<MonsterInfo>();
        int MonNum = MonNumberLoad(Instance.CommonRnd(0, 100));
        isMonOnLoad = true;
        for (int i = 0; i < monsterinfos.Count; i++)
        {
            if (Instance.ChoiceFieldID == monsterinfos[i].Field_ID) //선택 사냥터
            {
                courMonster = monsterinfos.GetRange(MonNum - 1, 1);  //인덱스 기준 랜덤하게 선택된 1개몹 정보 
                foreach (MonsterInfo item in courMonster)
                {
                    MonsterName.text = item.Mon_Name;
                    MonsterImage.sprite = Resources.Load<Sprite>("Sprites/Monster/" + item.Mon_ImgName);
                    Mon_GroupLevel = item.Mon_GroupLevel;
                    Mon_HP = item.Mon_HP; //최초 몹피
                    Mon_AttackDmg = item.Mon_AttackDmg;
                    Mon_DropItem = item.Mon_DropItem;
                    Mon_DropGold = item.Mon_DropGold;
                    Mon_ReturnExp = item.Mon_ReturnExp;
                    MonsterHPUpdate(0);
                }
            }
        }
        StopCoroutine("AttackToPlayer");
        StartCoroutine("AttackToPlayer");
    }
    #endregion

    #region [몬스터가 공격-코루틴]
    IEnumerator AttackToPlayer() //몬스터가 공격
    {
        while (true)
        {
            yield return new WaitForSecondsRealtime(3f);  //3초

            if (isMonOnLoad == true && Player_CurHP > 0)
            {
                float timeStart = Time.time;
                float timePassed = Time.time - timeStart;
                while (timePassed <= 0.2f)
                {
                    yield return new WaitForFixedUpdate();  //다음 프레임까지 대기(1초 60프레임)
                    timePassed = Time.time - timeStart;
                    PlayerHit.gameObject.SetActive(true);
                }
                PlayerHit.gameObject.SetActive(false);

                // 플레이어 PlayerHPBarUpdate
                int hitdamage = Mon_AttackDmg;
                PlayerHPUpdate(hitdamage);
            }
        }
       
    }
    #endregion

    #region [랜덤하게 몹리젠(가중치 적용)]
    //랜덤하게 몹리젠(가중치 적용 숫자 리턴)
    private int MonNumberLoad(int num)
    {
        int retVal = 0;
        if (num >= 0 && num < 35)
        {
            retVal = 1;
        }
        else if (num >= 35 && num < 70)
        {
            retVal = 2;
        }
        else if (num >= 70 && num < 85)
        {
            retVal = 3;
        }
        else if (num >= 85 && num < 95)
        {
            retVal = 4;
        }
        else
        {
            retVal = 5;
        }
        return retVal;
    }
    #endregion

    #region [몬스터 HP Update]
    private void MonsterHPUpdate(int hitdamage)
    {
        if (hitdamage == 0)                                //최초
        {
            Mon_CurHP = Mon_HP;
        }
        else
        {
            Mon_CurHP = Mon_CurHP - hitdamage;
        }

        if (Mon_CurHP <= 0) //현재 몹의 HP가 0
        {
            isMonOnLoad = false;
            MonsterBG.gameObject.SetActive(false);
            StartCoroutine(StartMonsterResult());
        }
        else                     //HP Bar Update
        {
            MonHPBarNum = (Mon_CurHP * 100) / (float)Mon_HP;    // MonHP --> %로 표시
            MonHPBarText.text = String.Format("{0}", Math.Round(MonHPBarNum, 1)) + "%";
            MonHPBarFill.gameObject.GetComponent<Image>().fillAmount = Mon_CurHP / (float)Mon_HP; //현재 HP
        }
        
    }
    #endregion

    #region [몬스터 로딩 지연 코루틴]
    IEnumerator StartMonsterResult()
    {
        //경험치/골드 Update - 플레이어 상태
        UpdatePlayerExpGold();
        //플레이어 획득 아이템 Update - 소유아이템
        UpdateGetItem();

        GetItemText.gameObject.SetActive(true);
        GetItemText.text = GetItemTextView;

        yield return new WaitForSecondsRealtime(1.5f);

        GetItemText.text = "";
        GetItemText.gameObject.SetActive(false);

        MonsterLoad();//몹로드
        
    }
    #endregion

    #region [경험치/골드 Update]
    private void UpdatePlayerExpGold()
    {
        PC_Exp = PC_Exp + Mon_ReturnExp;  //경험치 +
        if (PC_Exp > 99)  //레벨업
        {
            PC_Level = PC_Level + 1;
            int Over_Exp = PC_Exp - 100;
            PC_Exp = Over_Exp;  //레벨업 이후 남은 Exp +

            // 케리터 레벨 Data로 플레이어 스텟 Update
            List<CharInfo> charinfos = DataController.Instance.GetCharInfo().CharList; //
            foreach (var cf in charinfos)
            {
                if (cf.Char_Level == PC_Level)
                {
                    PC_Str = cf.Char_Str;
                    PC_Con = cf.Char_Con;
                    PC_MaxHP = cf.Char_HP;
                    PC_UpExp = cf.Char_Exp;
                }
            }
        }
        //골드 +
        PC_Gold = (Convert.ToDecimal(PC_Gold)  + Convert.ToDecimal(Mon_DropGold)).ToString();

        //사용자 정보 Update(파일)
        List<PlayerStat> playerstats = DataController.Instance.GetPlayerStatInfo().StatList;
        foreach (var ps in playerstats)
        {
            ps.PC_Level = PC_Level;
            ps.PC_Exp = PC_Exp;
            ps.PC_Gold = PC_Gold;
            ps.PC_Str = PC_Str;
            ps.PC_Con = PC_Con;
            ps.PC_MaxHP = PC_MaxHP;
            ps.PC_UpExp = PC_UpExp;
        }
         PlayerStatList playerstatlist = new PlayerStatList();
         playerstatlist.SetPlayerStatList = playerstats;            
         DataController.Instance.UpdateGameDataPlayerStat(playerstatlist);
         PlayerStatLoad();

        //Debug.Log("현재 PC_Str=" + PC_Str);
        //Debug.Log("현재 PC_Con=" + PC_Con);
        //Debug.Log("현재 PC_MaxHP=" + PC_MaxHP);
        //Debug.Log("현재 PC_MaxHP=" + PC_MaxHP);
    }
    #endregion

    #region [플레이어 획득 아이템 표시 /  Update - 소유아이템]
    private void UpdateGetItem()
    {
       // Debug.Log("드랍아이템 : " + Mon_DropItem);

        List<PssItem> passitems = DataController.Instance.GetPssItemInfo().PssItemList;
        string[] arrDropItemID = Mon_DropItem.Split('/');
        bool isHaveType = false;
        GetItemTextView = "";
        for (int i = 0; i < arrDropItemID.Length; i++)
        {
           // Debug.Log("드랍 아이템 아이디 : " + arrDropItemID[i]); // 1 / 2 /
            for (int j = 0; j < passitems.Count; j++)
            {
                if (passitems[j].GameItem_Type == "Stuff" && arrDropItemID[i] == passitems[j].Item_ID.ToString()) //재료
                {
                    isHaveType = true;
                    passitems[j].Amount = passitems[j].Amount + 1;
                }
            }
            if (isHaveType == false)
            {
                passitems.Add(new PssItem(passitems.Count + 1, 1, "Stuff", Convert.ToInt32(arrDropItemID[i]), 1, 0));
            }
            isHaveType = false;

            GetItemTextView = GetItemTextView + DataController.Instance.GetStuffName(Convert.ToInt32(arrDropItemID[i]))+ " 1개\n";
        }
        GetItemTextView = GetItemTextView + Mon_DropGold +  "골드\n 를 획득했습니다.";

        PssItemInfoList pssiteminfolist = new PssItemInfoList();
        pssiteminfolist.SetPssItemList = passitems;             //소유 아이템 업데이트
        DataController.Instance.UpdateGameDataPssItem(pssiteminfolist);
        
        PlayerPssItemLoad();
    }
    #endregion
    
    

    #region [무기 로딩]
    private void WeaponLoad()
    {
        List<WeaponInfo> weaponinfos = DataController.Instance.GetWeaponInfo().WeaponList;
        List<WeaponInfo> courWeapon = new List<WeaponInfo>();
        for (int i = 0; i < weaponinfos.Count; i++)
        {
            if (PC_WpnID == weaponinfos[i].Wpn_ID)  //들고있는 무기정보
            {
                courWeapon = weaponinfos.GetRange(PC_WpnID - 1, 1);  //인덱스 기준 랜덤하게 선택된 1개 무기 정보 
                foreach (WeaponInfo item in courWeapon)
                {
                    WeaponImage.sprite = Resources.Load<Sprite>("Sprites/Weapon/" + item.Wpn_ImgName);
                    Wpn_Attack = item.Wpn_AttackSec;  //1회 공격력
                }
            }
        }

        PlayerHPUpdate(0);   // 플레이어 HPUpdate   
    }
    #endregion

    #region [플레이어 HPBar Update]
    public void PlayerHPUpdate(int hitdamage)
    {
        if (hitdamage == 0)                         //처음 로딩
        {
            Player_CurHP = PC_MaxHP;
        }
        else
        {
            Player_CurHP = Player_CurHP - hitdamage;
        }

        if (Player_CurHP <= 0) //플레이어 죽음
        {
            Player_CurHP = 0;
            isMonOnLoad = false;
            StopCoroutine("AttackToPlayer");
            PC_DieAlert();
        }

        PlayerHPBarNum = (Player_CurHP * 100) / (float)PC_MaxHP;    // HP --> %로 표시
        PlayerHPBarText.text = String.Format("{0}", Math.Round(PlayerHPBarNum, 1)) + "%";
        PlayerHPBarFill.gameObject.GetComponent<Image>().fillAmount = Player_CurHP / (float)PC_MaxHP; //현재 HP
    }
    #endregion

    #region [플레이어 사망 Alert - Main이동]
    private  void PC_DieAlert()
    {
        DialogDataAlert alert = new DialogDataAlert("", "사망하였습니다!", delegate () {
          //  Debug.Log("OK Pressed!");

        });
        DialogManager.Instance.Push(alert);
    }
    #endregion
    
    #region [몬스터 공격 - 무기 클릭]
    public void HuntingMon()
    {
        if (isMonOnLoad == true)
        {
            MonsterHPUpdate(Wpn_Attack + PC_Str);  //현재 무기의 공격력 + 레벨 힘
            MonsterHit.gameObject.SetActive(true);

            Color wcolor = WeaponPanel.color;          //배경색 조정
            wcolor.a = 0.5f;
            WeaponPanel.color = wcolor;
            MainCamera.gameObject.GetComponent<AudioSource>().PlayOneShot(SFXClick);
            StartCoroutine(StartMonsterHit());  //hit 이미지  & 무기배경색 안보이게
        }

    }
    #endregion

    #region [공격 시 무기 배경색 변경]
    IEnumerator StartMonsterHit()
    {
        yield return new WaitForSecondsRealtime(0.2f);

        Color wcolor = WeaponPanel.color;
        wcolor.a = 0f;
        WeaponPanel.color = wcolor;

        MonsterHit.gameObject.SetActive(false);
    }
    #endregion

    #region [HP 물약클릭]
    public void ClickPlayerHPBtn()
    {
        if (pssHP_Count > 0 ) //
        {
            pssHP_Count--;
            PlayerHPUpdate(0);  //만피 채우기
        }
        CurHP_Count.text = pssHP_Count.ToString()+"개";
    }
    #endregion

    #region [GameDataUpdate-코루틴]
    IEnumerator UpdateGameData() 
    {
       yield return new WaitForSecondsRealtime(3f);  //3초

        List<PssItem> passitems = DataController.Instance.GetPssItemInfo().PssItemList;
        for (int i = 0; i < passitems.Count; i++)
        {
            if (passitems[i].GameItem_Type == "Hpotion")    //물약이면
            {
                passitems[i].Amount = pssHP_Count;            //사용물약 저장하기
            }
        }
        PssItemInfoList pssiteminfolist = new PssItemInfoList();
        pssiteminfolist.SetPssItemList = passitems;             //소유 아이템 업데이트
        DataController.Instance.UpdateGameDataPssItem(pssiteminfolist); 
        PlayerPssItemLoad();

        StartCoroutine("UpdateGameData");
    }
    #endregion

    // 물약먹기 --> 소유 아이템 Update (O)
    // 몬스터 사냥 후 경험치 Update -->-> 플레이어 획득 아이템 업데이트 --> 획득 아이템 표시 (확률로 드랍)---> 몹 리젠
    // 레벨업했으면 케릭터 기본값 Update 후 리로드
    // 플레이어 사망 시 메인 이동
    // 몬스터 공격 / 플레이어 공격력 랜덤으로 비중 주기
    // 몬스터 리젠 시간텀 주기


    // 특템아이템 표시 
    // 특템 아이템 사용 표시
    // 각 hit 이미지 바꾸기(여러개 노출 랜덤하게) --> 이미지 변경하고 노출 위치 랜덤



    private void MonDestory()
    {
        //Destroy(GameObject.Find("Monster"));
        //isMonOnLoad = false;
    }



    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GoMain();
        }
    }



}
