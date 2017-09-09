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
   
    private float MonHPBarNum;
    private int Mon_HP;         //최초 Max HP
    private int Mon_CurHP;    //현재 Mon HP
    private int Mon_AttackDmg; // 1회 공격력
    private bool isMonOnLoad = true;
    
    private float PlayerHPBarNum;
    private int Player_CurHP;
    private int Wpn_Attack;


    void Start()
    {
        PlayerStatLoad(); //플레이어 게임컨트롤로딩
        FieldBGLoad(); //사냥터 배경로드
        MonsterLoad();//몹로드
        WeaponLoad(); 
        StartCoroutine(AttackToPlayer()); //몬스터가 공격
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
        List<MonsterInfo> monsterinfos = DataController.Instance.GetMonsterInfo().MonsterList;
        List<MonsterInfo> courMonster = new List<MonsterInfo>();
        int MonNum = MonNumerLoad(Instance.CommonRnd(0, 100));
        isMonOnLoad = true;
        for (int i = 0; i < monsterinfos.Count; i++)
        {
            if (Instance.ChoiceFieldID == monsterinfos[i].Mon_FieldLevel) //선택 사냥터
            {

                courMonster = monsterinfos.GetRange(MonNum - 1, 1);  //인덱스 기준 랜덤하게 선택된 1개몹 정보 
                foreach (MonsterInfo item in courMonster)
                {
                    MonsterName.text = item.Mon_Name;
                    MonsterImage.sprite = Resources.Load<Sprite>("Sprites/Monster/" + item.Mon_ImgName);
                    Mon_HP = item.Mon_HP; //최초 몹피
                    Mon_AttackDmg = item.Mon_AttackDmg;
                    MonsterHPUpdate(0);
                }
            }
        }
    }
    #endregion
    
    #region [랜덤하게 몹리젠(가중치 적용)]
    //랜덤하게 몹리젠(가중치 적용)
    private int MonNumerLoad(int num)
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
            // MonDestory();
            isMonOnLoad = false;
            MonsterLoad();
        }
        else                     //HP Bar Update
        {
            MonHPBarNum = (Mon_CurHP * 100) / (float)Mon_HP;    // MonHP --> %로 표시
            MonHPBarText.text = String.Format("{0}", Math.Round(MonHPBarNum, 1)) + "%";
            MonHPBarFill.gameObject.GetComponent<Image>().fillAmount = Mon_CurHP / (float)Mon_HP; //현재 HP
        }

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
            Debug.Log("플레이어 다이");
        }

        PlayerHPBarNum = (Player_CurHP * 100) / (float)PC_MaxHP;    // HP --> %로 표시
        PlayerHPBarText.text = String.Format("{0}", Math.Round(PlayerHPBarNum, 1)) + "%";
        PlayerHPBarFill.gameObject.GetComponent<Image>().fillAmount = Player_CurHP / (float)PC_MaxHP; //현재 HP
    }
    #endregion

    #region [몬스터가 공격-코루틴]
    IEnumerator AttackToPlayer() //몬스터가 공격
    {
        yield return new WaitForSecondsRealtime(3f);  //3초
        if (isMonOnLoad == true && Player_CurHP > 0 )
        {
            StartCoroutine("AttackToPlayer");
            float timeStart = Time.time;
            float timePassed = Time.time - timeStart;
            float rate = timePassed / 0.5f;
            while (timePassed <= 0.2f)
            {
                yield return new WaitForFixedUpdate();
                timePassed = Time.time - timeStart;
                rate = timePassed / 0.1f;
                PlayerHit.gameObject.SetActive(true);
             }
            PlayerHit.gameObject.SetActive(false);

            // 플레이어 PlayerHPBarUpdate
            int hitdamage = Mon_AttackDmg;
            PlayerHPUpdate(hitdamage);
        }
    }
    #endregion
    
    //몬스터 공격 - 무기 클릭
    public void HuntingMon()
    {
        MonsterHPUpdate(Wpn_Attack + PC_Str);  //현재 무기의 공격력 + 레벨 힘
        MonsterHit.gameObject.SetActive(true);

        Color wcolor = WeaponPanel.color;          //배경색 조정
        wcolor.a = 0.5f;
        WeaponPanel.color = wcolor;

        StartCoroutine(StartMonsterHit());  //hit 이미지  & 무기배경색 안보이게
    }

    IEnumerator StartMonsterHit()
    {
        yield return new WaitForSecondsRealtime(0.2f);

        Color wcolor = WeaponPanel.color;
        wcolor.a = 0f;
        WeaponPanel.color = wcolor;

        MonsterHit.gameObject.SetActive(false);
    }

    // 물약먹기
    // 몬스터 리젠 시간텀 주기
    // 드랍아이템 표시하기
    // 아이템 습득 후 저장
    // 몬스터 사냥 후 플레이어 스텟 업데이트
    // 특템아이템 표시 
    // 특템 아이템 사용 표시



    private void MonDestory()
    {
        //Destroy(GameObject.Find("Monster"));
        //isMonOnLoad = false;
    }



    //// Update is called once per frame
    //void Update()
    //{

    //}



}
