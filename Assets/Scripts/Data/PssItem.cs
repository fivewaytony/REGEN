using System.Collections;
using System.Collections.Generic;
using System;
    
[Serializable]
public class PssItem
{
    public int PssItem_ID = 1;
    public int PC_ID = 1;
    public string Item_Type;
    public int Item_ID = 1;
    public int Amount;
    public int Equip_Stat;
    public int Item_Ent;
    public int Item_OptType;
    public int Item_OptPoint;

    public PssItem(int pss_id, int pc_id, string gi_type,  int item_id, int amount, int equip_stat, int item_ent, int item_opttype, int item_optpoint)
    {
        PssItem_ID = pss_id;
        PC_ID = pc_id;
        Item_Type = gi_type;
        Item_ID = item_id;
        Amount = amount;
        Equip_Stat = equip_stat;
        Item_Ent = item_ent;
        Item_OptType = item_opttype;
        Item_OptPoint = item_optpoint;
    }
}

[Serializable]
public class PssItemInfoList
{
    public List<PssItem> PssItemList;

    public List<PssItem> SetPssItemList
    {
        set
        {
            PssItemList = value;
        }
    }

}




