using System.Collections;
using System.Collections.Generic;
using System;

[Serializable]
public class GameItemInfo
{
    public int Item_ID = 1;
    public string Item_Type;
    public string Item_Name;
    public string Item_ImgName;
    public int Wpn_Attack;
    public int Wpn_AttackSec;
    public string Item_Stuff;
    public string Item_Enchant;
    public int HP_Rate;
    public int Item_Price;
}

[Serializable]
public class GameItemInfoList
{
    public List<GameItemInfo> GameItemList;
}


