using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

    //Main Scene
    public Text GoldText;
    public Text LevelText;
    public Text LevelBarText;
    public Image LeveBarFill;
        
    private int PC_CurExp;
    private int Character_Exp;
    private float LevelBarNum;
    protected int PC_WeaponID;
    protected int Wep_Attack; //무기공격력
    protected int PC_STR;       // 플레이어 힘

    //public static GameController Instance
    //{
    //    get 
    //}
    // Use this for initialization
    void Start ()
    {
        //JsonDataLoad();
        //PlayerStateLoad();//플레이어 상태 로드
        //DataController.Instance.GetStatList();
        InitTab(); //임시
    }

    //플레이어 상태 로드
    void PlayerStateLoad()
    {
        //플레이의 상태
        GoldText.text = DataController.Instance.PC_CurGold.ToString();
        LevelText.text = "Lv. " + DataController.Instance.PC_CurLevel.ToString();
        PC_CurExp = DataController.Instance.PC_CurExp;                  //현재 경험치
        PC_WeaponID = DataController.Instance.PC_WeaponID;          //현재 들고 있는 무기 번호
        PC_STR = DataController.Instance.PC_STR;              //플레이어의 레벨 힘
        Debug.Log("PC_STR=" + PC_STR);


        //Character 레벨별 수치 정의
        Character_Exp = DataController.Instance.Character_Exp;         //필요 Max 경험치
        LevelBarNum = (PC_CurExp * 100) / (float)Character_Exp;      // 현재 경험치 --> %로 표시
        LevelBarText.text = String.Format("{0}", Math.Round(LevelBarNum, 1)) + "%";
        LeveBarFill.gameObject.GetComponent<Image>().fillAmount = PC_CurExp / (float)Character_Exp; //현재 경험치바
    }


    // Update is called once per frame
    void Update () {
        
    }

    #region [JsonDataLoad()]
    //DataController 정의
    protected void JsonDataLoad()
    {
        DataController.Instance.LoadFunc("PlayerState.json", "playerState");
        DataController.Instance.LoadFunc("Character.json", "character");
        DataController.Instance.LoadFunc("Weapon.json", "weapon");
        DataController.Instance.LoadFunc("Field.json", "field");
        DataController.Instance.LoadFunc("Monster.json", "monster");
    }
    #endregion

    //사냥 이동
    public void GoHunting()
    {
        SceneManager.LoadScene("Hunting", LoadSceneMode.Single);
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
    
    public void InitTab()
    {
        StatData startData = DataController.Instance.GetStatList();
        foreach (Stat stat in startData.StatList)
        {

        }
    }

    //
}
