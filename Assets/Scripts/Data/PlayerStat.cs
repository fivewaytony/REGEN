using System.Collections;
using System.Collections.Generic;
using System;

[Serializable]
public class PlayerStat
{
    public int PC_ID=1;
    public string PC_Type;
    public string PC_Name;
    public int PC_Level;
    public int PC_MakingLevel;
    public int PC_Str;
    public int PC_Con;
    public int PC_Dex;
    public int PC_Exp;
    public int PC_UpExp;
    public float PC_MaxHP;
    public string PC_Gold;
    public int PC_Dia;
    public int PC_FieldLevel;
 }

[Serializable]
public class PlayerStatList
{
    public List<PlayerStat> StatList;

    public List<PlayerStat> SetPlayerStatList
    {
        set
        {
            StatList = value;
        }
    }
}
