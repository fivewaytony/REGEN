using System.Collections;
using System.Collections.Generic;
using System;

[Serializable]
public class MonsterInfo
{
    public int Mon_ID = 1;
    public int Field_ID =1;
    public int Mon_GroupLevel;
    public string Mon_Name;
    public string Mon_ImgName;
    public int Mon_HP;
    public int Mon_AttackDmg;
    public int Mon_ReturnExp;
    public string Mon_DropItem;
    public int Mon_DropGold;
}

[Serializable]
public class MonsterInfoList
{
    public List<MonsterInfo> MonsterList;
}
