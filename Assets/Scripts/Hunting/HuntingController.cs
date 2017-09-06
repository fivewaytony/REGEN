using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class HuntingController : GameController
{
    //Huting Scene
    public Image FieldImge;
    public Image WeaponImage;
    public Image MonsterImage;
    public Text MonsterName;
    public Image MonsterHit;
    public Text MonHPBarText;
    public Image MonHPBarFill;
    public Image PlayerHit;
    public Image WeaponPanel;
    public Text PlayerHPBarText;
    public Image PlayerHPBarFill;

    // Use this for initialization
    private float MonHPBarNum;
    private int Mon_HP;         //최초 Max HP
    private int Mon_CurHP;    //현재 Mon HP
    private bool isMonOnLoad = true;
    private int TotalMonCount = 10; //임시 리젠되는 몹개수 - Field json 에서 설정

    private float PlayerHPBarNum;
    private int Player_HP;
    private int Player_CurHP;


    void Start()
    {
        PlayerStatLoad();
        FieldBGLoad(); //사냥터 배경로드
        //MonsterLoad();//몹로드
        //PlayerLoad(); //플레이어
        //StartCoroutine(AttackToPlayer()); //몬스터가 공격
    }

    ////필드 로딩
    private void FieldBGLoad()
    {
        List<FieldInfo> fieldchoices = DataController.Instance.GetFieldInfo().FieldList;
        int bgNum = Instance.CommonRnd(1, 4);
        Debug.Log(bgNum);
        foreach (FieldInfo fielditem in fieldchoices)
        {
           if (Instance.ChoiceFieldID == fielditem.Field_Level ) //선택 사냥터
           {
             FieldImge.sprite = Resources.Load<Sprite>("Sprites/FieldBackGround/" + fielditem.Field_ImgName + bgNum.ToString());
           }
        }
    }



    ////플레이어 로딩
    //private void PlayerLoad()
    //{
    //    WeaponImage.sprite = Resources.Load<Sprite>("Sprites/Weapon/" + DataController.Instance.Wep_ImgName + "");
    //    Wep_Attack = DataController.Instance.Wep_Attack;            //플레이어의 현재 무기 공격력
    //    PC_STR = DataController.Instance.PC_STR;                        //플레이어의 현재 힘
    //    PlayerHPUpdate(0);                                                   // 플레이어 HPUpdate       
    //}

    ////몬스터 로딩
    //private void MonsterLoad()
    //{
    //    if (TotalMonCount > 0)
    //    {
    //        DataController.Instance.LoadFunc("Monster.json", "monster"); //몹을 10개 들고 오기 ListData 담아논다로 수정 예정
    //        MonsterImage.sprite = Resources.Load<Sprite>("Sprites/Monster/" + DataController.Instance.Mon_ImgName + "");
    //        MonsterName.text = DataController.Instance.Mon_Name;
    //        MonsterHPUpdate(0);
    //        TotalMonCount = TotalMonCount - 1;
    //   }
    //    else //사냥 1판 클리어
    //    {
    //        isMonOnLoad = false;
    //    }

    //}

    ////몬스터 HP Update
    //private void MonsterHPUpdate(int hitdamage)
    //{
    //    Mon_HP = DataController.Instance.Mon_HP;   //최초 Max HP
    //    if (hitdamage == 0 )                                //최초
    //    {
    //        Mon_CurHP = Mon_HP;
    //    }
    //    else
    //    {
    //        Mon_CurHP = Mon_CurHP - hitdamage;
    //    }

    //    if (Mon_CurHP <= 0) //현재 몹의 HP가 0
    //    {
    //       // MonDestory();
    //        MonsterLoad();
    //    }
    //    else                     //HP Bar Update
    //    {
    //        MonHPBarNum = (Mon_CurHP * 100) / (float)Mon_HP;    // MonHP --> %로 표시
    //        MonHPBarText.text = String.Format("{0}", Math.Round(MonHPBarNum, 1)) + "%";
    //        MonHPBarFill.gameObject.GetComponent<Image>().fillAmount = Mon_CurHP / (float)Mon_HP; //현재 HP
    //    }

    //}

    //// Update is called once per frame
    //void Update()
    //{

    //}

    //IEnumerator AttackToPlayer() //몬스터가 공격
    //{
    //    yield return new WaitForSecondsRealtime(2f);  //2초
    //    if (isMonOnLoad == true)
    //    {
    //        StartCoroutine("AttackToPlayer");

    //        float timeStart = Time.time;
    //        float timePassed = Time.time - timeStart;
    //        float rate = timePassed / 0.5f;
    //        while(timePassed <= 0.1f)
    //        {
    //            yield return new WaitForFixedUpdate();
    //            timePassed = Time.time - timeStart;
    //            rate = timePassed / 0.1f;
    //            Color color = PlayerHit.color;
    //            color.a = 1f - rate;
    //            PlayerHit.color = color;
    //        }
    //        Color color2 = PlayerHit.color;
    //        color2.a = 0f;
    //        PlayerHit.color = color2;

    //        // 플레이어 PlayerHPBarUpdate
    //        //몬스터의 공격력 불러오기 메타데이터에서
    //        int hitdamage = 20;
    //        PlayerHPUpdate(hitdamage);
    //    }
    //}

    //public void PlayerHPUpdate(int hitdamage)
    //{
    //    Player_HP = DataController.Instance.Character_HP;   //최초 Max HP
    //    if (hitdamage == 0)                                //최초
    //    {
    //        Player_CurHP = Player_HP;
    //    }
    //    else
    //    {
    //        Player_CurHP = Player_CurHP - hitdamage;
    //    }
    //    if (Player_CurHP <= 0) //플레이어 죽음
    //    {

    //        Player_CurHP = 0;
    //        Debug.Log("플레이어 다이");
    //    }
    //    //P Bar Update

    //    PlayerHPBarNum = (Player_CurHP * 100) / (float)Player_HP;    // MonHP --> %로 표시
    //    PlayerHPBarText.text = String.Format("{0}", Math.Round(PlayerHPBarNum, 1)) + "%";
    //    PlayerHPBarFill.gameObject.GetComponent<Image>().fillAmount = Player_CurHP / (float)Player_HP; //현재 HP

    //}

    ////몬스터 공격 - 무기 클릭
    //public void HuntingMon()
    //{
    //    MonsterHPUpdate(Wep_Attack + PC_STR);  //현재 무기의 공격력 + 레벨 힘
    //    MonsterHit.gameObject.SetActive(true);
    //    Color wcolor = WeaponPanel.color;
    //    wcolor.a = 0.5f;
    //    WeaponPanel.color = wcolor;

    //    StartCoroutine(StartMonsterHit());  //hit 이미지 안보이게
    //}

    //IEnumerator StartMonsterHit()
    //{
    //    yield return new WaitForSecondsRealtime(0.2f);

    //    Color wcolor = WeaponPanel.color;
    //    wcolor.a = 0f;
    //    WeaponPanel.color = wcolor;

    //    MonsterHit.gameObject.SetActive(false);
    //}

    //private void MonDestory()
    //{
    //    //Destroy(GameObject.Find("Monster"));
    //    //isMonOnLoad = false;
    //}




}
