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

    #region [저장된 플레이어 스텟 Load]
    public string playerStatProjectFilePath = "/pcstat.json";  //저장된 플레이어 스텟  테이터 파일

    public PlayerStatList playerStatData;   //플레이어 데이터 
    public PlayerStatList GetPlayerStatInfo()
    {
        if (playerStatData == null)
        {
           string filePath = Application.persistentDataPath + playerStatProjectFilePath;
            if (File.Exists(filePath))          //폰에 저장된 pcstat json 로드
            {
                playerStatData = PlayerStatLoadDataPath();
                Debug.Log("PlayerStatLoadDataPath");
            }
            else                            // Resources PassItem Json 로드 생성후 
            {
                playerStatData = PlayerStatResources();
                Debug.Log("PlayerStatResources");

                if (playerStatData != null)
                    CreateGameData("pcstatData"); // DataPath에 파일생성
            }
        }
        return playerStatData;
    }
    #endregion
    
    #region [플레이어 스텟 -- DataPath에서 읽기 ] 
    public PlayerStatList PlayerStatLoadDataPath()   //플레이어 스텟 저장된 데이터 불러오기
    {
        PlayerStatList DataPathPlayerStat;

        string filePath = Application.persistentDataPath + playerStatProjectFilePath;
        string dataAsJson = File.ReadAllText(filePath);
        DataPathPlayerStat = JsonUtility.FromJson<PlayerStatList>(dataAsJson);

        return DataPathPlayerStat;
    }
    #endregion

    #region [플레이어 스텟 - 최초 한번 Resources MetaData에서 읽기 ] 
    public PlayerStatList PlayerStatResources()
    {
        PlayerStatList ResourcesPlayerStat;

        TextAsset PlayerStat = Resources.Load("MetaData/PlayerStat") as TextAsset;
        ResourcesPlayerStat = JsonUtility.FromJson<PlayerStatList>(PlayerStat.text);

        return ResourcesPlayerStat;
    }
    #endregion
    
    #region [플레이어 스텟 -- DEV 용도 Resources MetaData에서 읽어 쓰기 ] 
    public void PlayerStatLoadResourcesDEV()
    {
        TextAsset PlayerStat = Resources.Load("MetaData/PlayerStat") as TextAsset;
        playerStatData = JsonUtility.FromJson<PlayerStatList>(PlayerStat.text);
        Debug.Log("PlayerStatLoadResourcesDEV");
        CreateGameData("pcstatData"); // DataPath에 파일생성
    }
    #endregion

    #region [플레이어 전용 Update]
    /// <summary>
    /// 플레이어 전용 Update]
    /// </summary>
    /// <param name="playerstatlist"></param>
    public void UpdateGameDataPlayerStat(PlayerStatList playerstatlist)
    {
        string dataAsJson = string.Empty;
        string filePath = string.Empty;

        dataAsJson = JsonUtility.ToJson(playerstatlist);
        filePath = Application.persistentDataPath + playerStatProjectFilePath;

        File.WriteAllText(filePath, dataAsJson);
    }
    #endregion

    #region[저장된 소유아이템 Load]
    public string pssItemProjectFilePath = "/pssitem.json";  //저장된 소유아이템 테이터 파일

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
        PssItemInfoList DataPathPssItem;

        string filePath = Application.persistentDataPath + pssItemProjectFilePath;
        string dataAsJson = File.ReadAllText(filePath);
        Debug.Log("DataPath 소유아이템 : " + dataAsJson);
        DataPathPssItem = JsonUtility.FromJson<PssItemInfoList>(dataAsJson);

        return DataPathPssItem;
    }
    #endregion
    
    #region [플레이어 소유 아이템 -- 최초 한번 Resources MetaData에서 읽기 ] 
    public PssItemInfoList PssItemLoadResources()
    {
        PssItemInfoList ResourcesPssItemData;

        TextAsset PssItem = Resources.Load("MetaData/PssItem") as TextAsset;
        Debug.Log("Resources 소유아이템 : " + PssItem);
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

    #region [플레이어 스텟, 소유 아이템 생성(파일)]
    public void CreateGameData(string GDdiv)
    {
        string dataAsJson = string.Empty;
        string filePath = string.Empty;
        if (GDdiv == "pcstatData")   //플레이어 스텟
        {
            dataAsJson = JsonUtility.ToJson(playerStatData);
            Debug.Log("dataAsJsonPcs=" + dataAsJson);
            filePath = Application.persistentDataPath + playerStatProjectFilePath;
        }
        else                               //플레이어 소유 아이템
        {
            dataAsJson = JsonUtility.ToJson(pssiteminfolist);
            Debug.Log("dataAsJsonPss=" + dataAsJson);
            filePath = Application.persistentDataPath + pssItemProjectFilePath;
        }
        File.WriteAllText(filePath, dataAsJson);
    }
    #endregion

    #region [소유 아이템 전용 Update]
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
    public GameItemGroupInfoList gameitemlist;
    public GameItemGroupInfoList GetGameIteminfo()
    {
        if (gameitemlist == null)
        {
            TextAsset GameItemJson = Resources.Load("MetaData/GameItemGroup") as TextAsset;
            gameitemlist = JsonUtility.FromJson<GameItemGroupInfoList>(GameItemJson.text);
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

    #region [게임 아이템 정보 - 무기/방어구/재료/물약]
    public GameItemInfoList gameiteminfolist;
    public GameItemInfoList GetGameItemInfo()
    {
        if (gameiteminfolist == null)
        {
            TextAsset ItemDataJson = Resources.Load("MetaData/GameItem") as TextAsset;
            Debug.Log("ItemDataJson=" + ItemDataJson);
            gameiteminfolist = JsonUtility.FromJson<GameItemInfoList>(ItemDataJson.text);
            gameitemDic = new Dictionary<int, GameItemInfo>();
            foreach (GameItemInfo item in gameiteminfolist.GameItemList)
            {
                gameitemDic.Add(item.Item_ID, item);
            }
    
          }
        return gameiteminfolist;
    }
    public Dictionary<int, GameItemInfo> gameitemDic;
    #endregion


    #region[재료 아이템 정보]
    //public StuffInfoList stuffinfolist;
    //public StuffInfoList GetStuffInfo()
    //{
    //    if (stuffinfolist == null)
    //    {
    //        TextAsset stuffDataJson = Resources.Load("MetaData/Stuff") as TextAsset;
    //        stuffinfolist = JsonUtility.FromJson<StuffInfoList>(stuffDataJson.text);

    //        stuffDic = new Dictionary<int, StuffInfo>();
    //        foreach (StuffInfo stuff in stuffinfolist.StuffList)
    //        {
    //            stuffDic.Add(stuff.Stuff_ID, stuff);
    //        }
    //        Debug.Log("stuffDic.count = " + stuffDic.Count);
    //    }
    //    return stuffinfolist;
    //}
    //public Dictionary<int, StuffInfo> stuffDic;

    #endregion


    #region[무기 정보]
    //public WeaponInfoList weaponinfolist;
    //public WeaponInfoList GetWeaponInfo()
    //{
    //    if (weaponinfolist == null)
    //    {
    //        TextAsset weaponDataJson = Resources.Load("MetaData/Weapon") as TextAsset;
    //        weaponinfolist = JsonUtility.FromJson<WeaponInfoList>(weaponDataJson.text);
    //    }

    //    return weaponinfolist;
    //}
    #endregion


    #region [레벨별 케릭터 정보]
    public CharInfoList charinfolist;
    public CharInfoList GetCharInfo()
    {
        if (charinfolist == null)
        {
            TextAsset charDataJson = Resources.Load("MetaData/Character") as TextAsset;
            charinfolist = JsonUtility.FromJson<CharInfoList>(charDataJson.text);
        }

        return charinfolist;
    }
    #endregion

   
    #region [밸러싱 수치 정보]
    public BalanceInfoList balancelist;
    public BalanceInfoList GetBalanceInfo()
    {
        if (balancelist == null)
        {
            TextAsset balanceDataJson = Resources.Load("MetaData/BalanceRate") as TextAsset;
             balancelist = JsonUtility.FromJson<BalanceInfoList>(balanceDataJson.text);
        }
        return balancelist;
    }
    #endregion

    #region [ Get 재료아이템 이름]
    public string GetStuffName(int st_id)
    {
        string retVal = string.Empty;
        if (gameiteminfolist == null)
        {
            gameiteminfolist = GetGameItemInfo();
        }
        foreach (var item in gameiteminfolist.GameItemList)
        {
            if (item.Item_ID == st_id)
            {
                retVal = item.Item_Name;
                break;
            }
        }
        return retVal;
    }
    #endregion
    
    #region [FireBase로 메세지 보내기]
    /*void Start()
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
    }*/
    #endregion
    
}




