using System.Collections;
using System.Collections.Generic;
using System;

[Serializable]
public class PlayerStat
{
    public int PC_ID=1;
    public int PC_Level;
    public int PC_Str;
    public int PC_Con;
    public int PC_Exp;
    public int PC_UpExp;
    public int PC_Gold;
    public int PC_WpnID;
    public int PC_WpnEct;
    public int PC_FieldLevel;
    public string PC_Type;
    public string PC_Name;
}

[Serializable]
public class PlayerStatList
{
    public List<PlayerStat> StatList;
}
