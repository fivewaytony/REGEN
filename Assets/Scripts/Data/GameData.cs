using System.Collections;
using System.Collections.Generic;
using System;

// 플레이어 상태
[Serializable]
public class GameData  
{
    public int PC_ID=1;
    public int PC_Level;
    public int PC_Str;
    public int PC_Con;
    public int PC_Exp;
    public int PC_UpExp;
    public int PC_MaxHP;
    public string PC_Gold;
    public int PC_WpnID;
    public int PC_WpnEct;
    public int PC_FieldLevel;
    public string PC_Type;
    public string PC_Name;
}

////소유 아이템
//[Serializable]
//public class PssItemData
//{
//    public int PssItem_ID = 1;
//    public int PC_ID = 1;
//    public int GameItem_ID = 1;
//    public string GameItem_Type;
//    public int Amount;
//    public int Equip_Stat;
//}

