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


    // Use this for initialization
    private float MonHPBarNum;
    private int Mon_HP;         //최초 Max HP
    private int Mon_CurHP;    //현재 Mon HP


    void Start()
    {
        base.JsonDataLoad(); //DataLoad()

        FieldBGLoad(); //사냥터 배경로드
        MonsterLoad();//몹로드
        WeaponLoad(); //무기로드
       
    }

    private void FieldBGLoad()
    {
        FieldImge.sprite = Resources.Load<Sprite>("Sprites/FieldBackGround/" + DataController.Instance.Field_ImgName + "");
    }

    private void WeaponLoad()
    {
        WeaponImage.sprite = Resources.Load<Sprite>("Sprites/Weapon/" + DataController.Instance.Wep_ImgName + "");
        Wep_Attack = DataController.Instance.Wep_Attack;            //현재 플레이어의 무기 공격력
    }

    //몬스터 로딩
    private void MonsterLoad()
    {
        MonsterImage.sprite = Resources.Load<Sprite>("Sprites/Monster/" + DataController.Instance.Mon_ImgName + "");
        MonsterName.text = DataController.Instance.Mon_Name;
        MonsterHPUpdate(0);
    }
    
    //몬스터 HP Update
    private void MonsterHPUpdate(int hitdamage)
    {
        Mon_HP = DataController.Instance.Mon_HP;   //최초 Max HP
        Mon_CurHP = Mon_HP - hitdamage;                            //      
        if (Mon_CurHP <= 0) //현재 몹의 HP가 0
        {
            MonDestory();
        }
        else                     //HP Bar Update
        {
            MonHPBarNum = (Mon_CurHP * 100) / (float)Mon_HP;    // MonHP --> %로 표시
            MonHPBarText.text = String.Format("{0}", Math.Round(MonHPBarNum, 1)) + "%";
            MonHPBarFill.gameObject.GetComponent<Image>().fillAmount = Mon_HP / (float)Mon_CurHP; //현재 HP
        }

    }

    // Update is called once per frame
    void Update()
    {
        AttackToPlayer(); //몬스터가 공격
    }

    //몬스터 공격 - 무기 클릭
    public void HuntingMon()
    {
        MonsterHPUpdate(Wep_Attack + PC_STR);  //현재 무기의 공격력 + 레벨 힘
        Debug.Log(Wep_Attack);
        Debug.Log(PC_STR);

        MonsterHit.gameObject.SetActive(true);
        StartCoroutine(StartMonsterHit());  //hit 이미지 안보이게
    }

    IEnumerator StartMonsterHit()
    {
        yield return new WaitForSecondsRealtime(0.2f);
        MonsterHit.gameObject.SetActive(false);
    }

    private void MonDestory()
    {
        Destroy(GameObject.Find("Monster"));
        MonsterLoad();
        return;
    }

    public void AttackToPlayer() //몬스터가 공격
    {
        Debug.Log("2초에 한번");
    }


}
