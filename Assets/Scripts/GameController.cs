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

    protected int PC_Exp;   //현재 경험치
    protected int PC_UpExp;     //업에 필요한 경험치
                    
    private float LevelBarNum;
    
    public static GameController Instance; //GameController 접근하기 위해

    // Use this for initialization
    void Start ()
    {
        Instance = this;  //GameController 접근하기 위해
        PlayerStatLoad();//플레이어 상태 로드
        //DataController.Instance.GetStatList();
       // InitTab(); //임시
    }

    //플레이어 상태 로드
    void PlayerStatLoad()
    {
        //DataController.Instance.PlayerStatLoad();
        List<PlayerStat> statList = DataController.Instance.PlayerStatLoad().StatList;

        foreach (PlayerStat item in statList)
        {
            Debug.Log(item.PC_Gold);
            //플레이의 상태
            //GoldText.text = DataController.Instance.PC_Gold.ToString();
            //LevelText.text = "Lv. " + DataController.Instance.PC_Level.ToString();
            //PC_Exp = DataController.Instance.PC_Exp;                  //현재 경험치
            //PC_UpExp = DataController.Instance.PC_UpExp;
            //LevelBarNum = (PC_Exp * 100) / (float)PC_UpExp;      // 현재 경험치 --> %로 표시
            //LevelBarText.text = String.Format("{0}", Math.Round(LevelBarNum, 1)) + "%";
            //LeveBarFill.gameObject.GetComponent<Image>().fillAmount = PC_Exp / (float)PC_UpExp; //현재 경험치바

            //PC_WeaponID = DataController.Instance.PC_WeaponID;          //현재 들고 있는 무기 번호
            //PC_STR = DataController.Instance.PC_STR;              //플레이어의 레벨 힘
            //Debug.Log("PC_STR=" + PC_STR);

        }
    }


    // Update is called once per frame
    void Update () {
        
    }

    #region [JsonDataLoad()]
    //DataController 정의
    protected void JsonDataLoad()
    {

      //  DataController.Instance.LoadFunc("PlayerStat.json", "playerstat");


        //DataController.Instance.LoadFunc("Character.json", "character");
        //DataController.Instance.LoadFunc("Weapon.json", "weapon");
        //DataController.Instance.LoadFunc("Field.json", "field");
        //DataController.Instance.LoadFunc("Monster.json", "monster");
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
        //StatData startData = DataController.Instance.GetStatList();
        //foreach (Stat stat in startData.StatList)
        //{

        //}
    }

    //
}
