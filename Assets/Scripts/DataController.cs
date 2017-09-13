using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using LitJson;

public class DataController : MonoBehaviour
{

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

    #region [플레이어 데이터 -- 최초 한번]
    public PlayerStatList plyaerStatData;   //플레이어 데이터 -- 최초 한번 --> 저장된 게임 데이터가 없을때
    public PlayerStatList PlayerStatLoadResources()
    {
        if (plyaerStatData == null)
        {
            TextAsset statJson = Resources.Load("MetaData/PlayerStat") as TextAsset;
            plyaerStatData = JsonUtility.FromJson<PlayerStatList>(statJson.text);
        }
        return plyaerStatData;
    }
    #endregion
    
    #region[저장된 gamedata Load/Save]
    public string gameDataProjectFilePath = "/pcstat.json";  //저장된 테이터 파일

    PlayerStat playerStatData;     // 플레이어 스텟 저장된(할) 데이터
    public PlayerStat PlayerStat
    {
        get
        {
            if (playerStatData == null)
            {
                PlayerStatLoadDataPath();
            }
            return playerStatData;
        }
    }
    
    public void PlayerStatLoadDataPath()   //플레이어 스텟 저장된 데이터 불러오기
    {
        string filePath = Application.persistentDataPath + gameDataProjectFilePath;

        if (File.Exists(filePath))
        {
            Debug.Log("PlayerStatLoadDataPath!");
            string dataAsJson = File.ReadAllText(filePath);
            playerStatData = JsonUtility.FromJson<PlayerStat>(dataAsJson);
        }
        else
        {
            CreatePlayerStatData();
        }
    }

    /// <summary>
    ///  최초 플레이어 정보 Json 로드
    /// </summary>
    public void CreatePlayerStatData() {
        Debug.Log("CreatePlayerStatData");
        playerStatData = new PlayerStat();
        List<PlayerStat> statList = DataController.Instance.PlayerStatLoadResources().StatList;
        foreach (PlayerStat item in statList)
        {
            playerStatData.PC_ID = item.PC_ID;
            playerStatData.PC_Level = item.PC_Level;
            playerStatData.PC_Str = item.PC_Str;
            playerStatData.PC_Con = item.PC_Con;
            playerStatData.PC_Exp = item.PC_Exp;
            playerStatData.PC_UpExp = item.PC_UpExp;
            playerStatData.PC_MaxHP = item.PC_MaxHP;
            playerStatData.PC_Gold = item.PC_Gold;
            playerStatData.PC_WpnID = item.PC_WpnID;
            playerStatData.PC_WpnEct = item.PC_WpnEct;
            playerStatData.PC_FieldLevel = item.PC_FieldLevel;
            playerStatData.PC_Type = item.PC_Type;
            playerStatData.PC_Name = item.PC_Name;
        }
        CreateGameData("pcstatData");
    }

    /// <summary>
    ///  플레이어 정보 UpdateGameData
    /// </summary>
    public void UpdateGameDataPlayerStat()
    {
        Debug.Log("UpdateGameDataPlayerStat");
        playerStatData = new PlayerStat();
        //_gameData.PC_ID = item.PC_ID;
        //_gameData.PC_Level = item.PC_Level;
        //_gameData.PC_Str = item.PC_Str;
        //_gameData.PC_Con = item.PC_Con;
        //_gameData.PC_Exp = item.PC_Exp;
        //_gameData.PC_UpExp = item.PC_UpExp;
        //_gameData.PC_MaxHP = item.PC_MaxHP;
        //_gameData.PC_Gold = item.PC_Gold;
        //_gameData.PC_WpnID = item.PC_WpnID;
        //_gameData.PC_WpnEct = item.PC_WpnEct;
        //_gameData.PC_FieldLevel = item.PC_FieldLevel;
        //_gameData.PC_Type = item.PC_Type;
        //_gameData.PC_Name = item.PC_Name;

        //SaveGameData();

    }

    #endregion
    
    #region[저장된 소유아이템 Load/Save]

    public string pssItemProjectFilePath = "/pssitem.json";  //저장된 테이터 파일

    public PssItemInfoList pssiteminfolist;
    public PssItemInfoList GetPssItemInfo()
    {
        if (pssiteminfolist == null)
        {
            string filePath = Application.persistentDataPath + pssItemProjectFilePath;
            if (File.Exists(filePath))          //폰에 저장된 gamedata pssitem json 로드
            {
                pssiteminfolist = PssItemLoadDataPath();
                Debug.Log("PssItemLoadDataPath");
            }
            else                            // Resources PassItem Json 로드 생성후 
            {
                pssiteminfolist = PssItemLoadResources();
                Debug.Log("PssItemLoadResources");

                if (pssiteminfolist != null)
                    CreateGameData("pssItemData"); // DataPath에 파일생성
            }
         }
        return pssiteminfolist;
    }
    #endregion

    #region [플레이어 소유 아이템 -- DataPath에서 읽기 ] 
    public PssItemInfoList PssItemLoadDataPath()
    {
        PssItemInfoList DataPathPssItemData;

        string filePath = Application.persistentDataPath + pssItemProjectFilePath;
        string dataAsJson = File.ReadAllText(filePath);
        DataPathPssItemData = JsonUtility.FromJson<PssItemInfoList>(dataAsJson);

        return DataPathPssItemData;
    }
    #endregion
    
    #region [플레이어 소유 아이템 -- 최초 한번 Resources MetaData에서 읽기 ] 
    public PssItemInfoList PssItemLoadResources()
    {
        PssItemInfoList ResourcesPssItemData;

        TextAsset PssItem = Resources.Load("MetaData/PssItem") as TextAsset;
        ResourcesPssItemData = JsonUtility.FromJson<PssItemInfoList>(PssItem.text);

        return ResourcesPssItemData;
    }
    #endregion

    #region [플레이어 소유 아이템 -- DEV 용도 Resources MetaData에서 읽어 쓰기 ] 
    public void PssItemLoadResourcesDEV()
    {
        TextAsset PssItem = Resources.Load("MetaData/PssItem") as TextAsset;
        pssiteminfolist = JsonUtility.FromJson<PssItemInfoList>(PssItem.text);
        Debug.Log("PssItemLoadResourcesDEV");
        CreateGameData("pssItemData"); // DataPath에 파일생성
    }
    #endregion

    #region [PCStatData, PCPssItemData 생성]
    public void CreateGameData(string GDdiv)
    {
        string dataAsJson = string.Empty;
        string filePath = string.Empty;
        if (GDdiv == "pcstatData")   //플레이어 스텟
        {
            dataAsJson = JsonUtility.ToJson(playerStatData);
            filePath = Application.persistentDataPath + gameDataProjectFilePath;
        }
        else                               //플레이어 소유 아이템
        {
            dataAsJson = JsonUtility.ToJson(pssiteminfolist);
            Debug.Log("dataAsJson=" + dataAsJson);
            filePath = Application.persistentDataPath + pssItemProjectFilePath;
        }
        File.WriteAllText(filePath, dataAsJson);
    }

    /// <summary>
    /// 소유 아이템 전용 Update
    /// </summary>
    /// <param name="pssiteminfolist"></param>
    public void UpdateGameDataPssItem(PssItemInfoList pssiteminfolist)
    {
        string dataAsJson = string.Empty;
        string filePath = string.Empty;
        
        dataAsJson = JsonUtility.ToJson(pssiteminfolist);
        filePath = Application.persistentDataPath + pssItemProjectFilePath;
     
        File.WriteAllText(filePath, dataAsJson);
    }
    #endregion

    #region [게임 아이템 정보]
    public GameItemInfoList gameitemlist;
    public GameItemInfoList GetGameIteminfo()
    {
        if (gameitemlist == null)
        {
            TextAsset GameItemJson = Resources.Load("MetaData/GameItem") as TextAsset;
            gameitemlist = JsonUtility.FromJson<GameItemInfoList>(GameItemJson.text);
        }

        return gameitemlist;
    }
    #endregion

    #region[사냥터 선택 팝업]
    public FieldInfoList fieldinfolist;
    public FieldInfoList GetFieldInfo()
    {
        if (fieldinfolist == null)
        {
            TextAsset fieldDataJson = Resources.Load("MetaData/FieldInfo") as TextAsset;
            fieldinfolist = JsonUtility.FromJson<FieldInfoList>(fieldDataJson.text);
        }

        return fieldinfolist;
    }
    #endregion

    #region[몬스터 정보]
    public MonsterInfoList monsterinfolist;
    public MonsterInfoList GetMonsterInfo()
    {
        if (monsterinfolist == null)
        {
            TextAsset monsterDataJson = Resources.Load("MetaData/Monster") as TextAsset;
            monsterinfolist = JsonUtility.FromJson<MonsterInfoList>(monsterDataJson.text);
        }

        return monsterinfolist;
    }
    #endregion
    
    #region[무기 정보]
    public WeaponInfoList weaponinfolist;
    public WeaponInfoList GetWeaponInfo()
    {
        if (weaponinfolist == null)
        {
            TextAsset weaponDataJson = Resources.Load("MetaData/Weapon") as TextAsset;
            weaponinfolist = JsonUtility.FromJson<WeaponInfoList>(weaponDataJson.text);
        }

        return weaponinfolist;
    }
    #endregion
    
    #region [FireBase로 메세지 보내기]
    void Start()
    {
        Firebase.Messaging.FirebaseMessaging.TokenReceived += OnTokenReceived;
        Firebase.Messaging.FirebaseMessaging.MessageReceived += OnMessageReceived;
    }

    public void OnTokenReceived(object sender, Firebase.Messaging.TokenReceivedEventArgs token)
    {
        UnityEngine.Debug.Log("Received Registration Token: " + token.Token);
    }

    public void OnMessageReceived(object sender, Firebase.Messaging.MessageReceivedEventArgs e)
    {
        UnityEngine.Debug.Log("Received a new message from: " + e.Message.From);
    }
    #endregion
    
}




