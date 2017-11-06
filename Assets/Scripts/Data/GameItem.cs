using System.Collections;
using System.Collections.Generic;
using System;

[Serializable]
public class GameItemInfo
{
    public int Item_ID = 1;
    public string Item_Group; 
    public string Item_Type;
    public string Item_Grade;
    public int Item_Level;
    public string Item_Name;
    public string Item_Desc;
    public string Item_ImgName;
    public int Wpn_Attack;
    public int Wpn_AttackSec;
    public int Prt_Degree;
    public int Ace_Degree;
    public int Making_Stat;
    public int Stuff1_ID;
    public int Stuff1_Count;
    public int Stuff2_ID;
    public int Stuff2_Count;
    public int Stuff3_ID;
    public int Stuff3_Count;
    public int Making_Price;
    public int EntGem_ID;
    public int EntGem_Count;
    public int Enchant_Price;
    public int Potion_Time;  //물약 지속시간 초
    public int Box_ItemID; //재료상자안 아이템 ID
    public int Item_Price;  //아이템 판매가격
    public int Item_BuyPrice;  //아이템 구매가격
}

[Serializable]
public class GameItemInfoList
{
    public List<GameItemInfo> GameItemList;
}


