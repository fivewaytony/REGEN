using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using LitJson;

public class DataController : MonoBehaviour {

    #region[Singleton class start]
        static GameObject _container;
        static GameObject Container
        {
            get
            {
                return _container;
            }
        }

        static DataController _instance;
        public static DataController Instance
        {
            get
            {
                if (!_instance)
                {
                    _container = new GameObject();
                    _container.name = "DataController";
                    _instance = _container.AddComponent(typeof(DataController)) as DataController;
                    DontDestroyOnLoad(_container);
                }
                return _instance;
            }
        }
    // Singleton class end
    #endregion

    //호출 공통
    public void LoadFunc(string jsonfile, string datadiv)
    {
      //  Debug.Log("Json Data 불러옵니다.");
        StartCoroutine(LoadData(jsonfile, datadiv)); //
    }

    //Load 공통
    IEnumerator LoadData(string jsonfile, string datadiv)
    {
        string JsonString = File.ReadAllText(Application.dataPath + "/Resources/" + jsonfile + "");
        JsonData jsondata = JsonMapper.ToObject(JsonString);
        ParsingJsonData(jsondata, datadiv);
        yield return null;
    }
    
    //PlayerStateEnt pse = new PlayerStateEnt();  //Player 현재 상태
   // public List<PlayerState> pslist = new List<PlayerState>();
    public int PC_ID;
    public int PC_CurLevel;
    public int PC_CurExp;
    public int PC_STR;
    public int PC_CON;
    public string PC_CurGold = string.Empty;
    public int PC_WeaponID;

    //CharacterEnt che = new CharacterEnt(); //레벨별 케릭터 수치 
    //public List<Character> chlist = new List<Character>();
    public int Character_Level;
    public int Character_Str;
    public int Character_Con;
    public int Character_HP;
    public int Character_Exp;

    //무기
    public int Wep_ID;
    public string Wep_Name = string.Empty;
    public string Wep_ImgName = string.Empty;
    public int Wep_Level;
    public int Wep_Attack;
    public int Wep_AttackSec;
    public int Wep_Stuff;

    //사냥터
    public int Field_ID;
    public int Field_Level;
    public string Field_Name;
    public string Field_ImgName;

    //몬스터
    public int Mon_ID;
    public int Mon_Level;
    public string Mon_Name;
    public int Mon_HP;
    public string Mon_ImgName;
    public int Mon_AttackSec;
    public int Mon_ReturnExp;
    public int Mon_FieldLevel;
    public string Mon_DropItem;

    private void ParsingJsonData(JsonData jsondata, string datadiv)
    {
        #region[playerState //Player 현재 상태] 
        if (datadiv == "playerState") 
        {
            for (int i = 0; i < jsondata.Count; i++)
            {
                PC_ID = (int)(jsondata[i]["PC_ID"]);
                PC_CurLevel = (int)(jsondata[i]["PC_CurLevel"]);
                PC_STR = (int)(jsondata[i]["PC_STR"]);
                PC_CON = (int)(jsondata[i]["PC_CON"]);
                PC_CurExp = (int)(jsondata[i]["PC_CurExp"]);
                PC_CurGold = string.Format("{0:n0}", (int)(jsondata[i]["PC_CurGold"]));
                PC_WeaponID = (int)(jsondata[i]["PC_WeaponID"]);
                // pslist.Add(new PlayerState(pse));
            }
            //foreach (var item in pslist)
            //{
            //    PC_CurLevel = item.pc_curlevel;
            //    PC_CurExp = item.pc_curexp;
            //}
        }
        #endregion

        #region[Character //케릭터 레벨별 수치]
        if (datadiv == "character")
        {
            for (int i = 0; i < jsondata.Count; i++)
            {
                if (i == PC_CurLevel-1) //Player 현재 레벨 = 케릭 레벨 수치
                {
                    Character_Level = (int)(jsondata[i]["Character_Level"]);     //레벨
                    Character_Str = (int)(jsondata[i]["Character_Str"]);        //힘
                    Character_Con = (int)(jsondata[i]["Character_Con"]);     //체력
                    Character_HP = (int)(jsondata[i]["Character_HP"]);      //생명력
                    Character_Exp = (int)(jsondata[i]["Character_Exp"]);    //업에 필요한 경험치
                    //chlist.Add(new Character(che));

                    return;
                }
            }
            //foreach (var item in chlist)
            //{
            //        Character_Level = item.character_level;
            //        Character_Str = item.character_str;
            //        Character_Con = item.character_con;
            //        Character_HP = item.character_hp;
            //        Character_Exp = item.character_exp;
            //}
        }
        #endregion

        #region[Weapon //무기 레벨별 수치]
        if (datadiv == "weapon")
        {
            //for (int i = 0; i < jsondata.Count; i++)
            //{
            //    if (i == PC_WeaponID - 1) //Player 현재 무기 ID = 무기 ID
            //    {
            Wep_ID = (int)(jsondata[PC_WeaponID - 1]["Wep_ID"]);     //무기 ID
            Wep_Name = (jsondata[PC_WeaponID - 1]["Wep_Name"]).ToString();        //이름
            Wep_ImgName = (jsondata[PC_WeaponID - 1]["Wep_ImgName"]).ToString();        //이미지명
            Wep_Level = (int)(jsondata[PC_WeaponID - 1]["Wep_Level"]);     //레벨
            Wep_Attack = (int)(jsondata[PC_WeaponID - 1]["Wep_Attack"]);      //공격력
            Wep_AttackSec = (int)(jsondata[PC_WeaponID - 1]["Wep_AttackSec"]);    //초당공격력

            //        return;
            //    }
            //    //        }
            //    //foreach (var item in chlist)
            //    //{
            //    //        Character_Level = item.character_level;
            //    //        Character_Str = item.character_str;
            //    //        Character_Con = item.character_con;
            //    //        Character_HP = item.character_hp;
            //    //        Character_Exp = item.character_exp;
            //    //}
            //}
        }
        #endregion

        #region[Field //사냥터]
        if (datadiv == "field")
        {
            // int field_id = Random.Range(Min, Max);  //일단 임시 : 추후에는 동적으로 받아야됨.. Min, Max)
            int field_id = Random.Range(1, 3); 

            Field_ID = (int)(jsondata[field_id-1]["Field_ID"]);
            Field_Level = (int)(jsondata[field_id - 1]["Field_Level"]);
            Field_Name =jsondata[field_id - 1]["Field_Name"].ToString();
            Field_ImgName = jsondata[field_id - 1]["Field_ImgName"].ToString();
              
        }
        #endregion

        #region[Monster //몬스터]
        if (datadiv == "monster")
        {
            // int mon_id = Random.Range(Min, Max);  //일단 임시 : 추후에는 동적으로 받아야됨.. Min, Max)
            int mon_id = Random.Range(1, 3);

            Mon_ID = (int)(jsondata[mon_id - 1]["Mon_ID"]);                         // ID
            Mon_Level = (int)(jsondata[mon_id - 1]["Mon_Level"]);                   // Level
            Mon_Name = jsondata[mon_id - 1]["Mon_Name"].ToString();             // 이름
            Mon_HP = (int)jsondata[mon_id - 1]["Mon_HP"];                         // HP
            Mon_ImgName = jsondata[mon_id - 1]["Mon_ImgName"].ToString();   // 이미지
            Mon_AttackSec = (int)(jsondata[mon_id - 1]["Mon_AttackSec"]);           //  초당 공격력
            Mon_ReturnExp = (int)(jsondata[mon_id - 1]["Mon_ReturnExp"]);           // 경험치
            Mon_FieldLevel = (int)(jsondata[mon_id - 1]["Mon_FieldLevel"]);             // 출현 필드 레벨
            Mon_DropItem =  jsondata[mon_id - 1]["Mon_DropItem"].ToString();       // 드랍 아이템
        }
        #endregion
    }

    //public class PlayerState
    //{
    //    public int pc_id;
    //    public int pc_curlevel;
    //    public int pc_curexp;
    //    public string pc_curgold;

    //    public PlayerState(PlayerStateEnt pse)
    //    {
    //        pc_id = pse.PC_ID;
    //        pc_curlevel = pse.PC_CurLevel;
    //        pc_curexp = pse.PC_CurExp;
    //        pc_curgold = pse.PC_CurGold;
    //    }
    //}

    //    public class Character
    //    {
    //        public int character_level;
    //        public int character_str;
    //        public int character_con;
    //        public int character_hp;
    //        public int character_exp;

    //        public Character(CharacterEnt che)
    //        {
    //            character_level = che.Character_Level;
    //            character_str = che.Character_Str;
    //            character_con = che.Character_Con;
    //            character_hp = che.Character_HP;
    //            character_exp = che.Character_Exp;
    //          }
    //}




    public StatData statData;
    public StatData GetStatList()
    {
        if (statData == null)
        {
            LoadStatData();
        }
        return statData;
    }

    /// <summary>
    /// 데이터 로드
    /// </summary>
    public void LoadStatData()
    {
        TextAsset statJson = Resources.Load("MetaData/Stat") as TextAsset;
        Debug.Log(statJson.name);
        Debug.Log(statJson.ToString());
        statData = JsonUtility.FromJson<StatData>(statJson.text);

        foreach (Stat stat in statData.StatList)
        {
            Debug.Log(stat.Name);
        }

    }

    public string gameDataProjectFilePath = "/game.json";

    public GameData _gameData;  //GameController 에서 호출 DataController.Instance.gameData.PC_CurExp
    public GameData gameData    //메모리 로딩 후 로드
    {
        get
        {
            if (_gameData == null)
            {
                LoadGameData();
            }
            return _gameData;
        }
    }

    //저장할 게임내의 데이터를 로드
    public void LoadGameData()
    {
        string filePath = Application.persistentDataPath + gameDataProjectFilePath;
    }

    public void SaveGameData()
    {
        string filePath = Application.persistentDataPath + gameDataProjectFilePath;
    }
}



