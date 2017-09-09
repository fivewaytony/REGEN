using System.Collections;
using System.Collections.Generic;
using System;

[Serializable]
public class WeaponInfo
{
    public int Wpn_ID = 1;
    public string Wpn_Name ;
    public string Wpn_ImgName;
    public int Wpn_Level;
    public int Wpn_Attack;
    public int Wpn_AttackSec;
    public string Wpn_Stuff;
}

[Serializable]
public class WeaponInfoList
{
    public List<WeaponInfo> WeaponList;
}
