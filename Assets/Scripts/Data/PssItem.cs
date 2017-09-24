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
    public int Wpn_Ent;

    public PssItem(int pss_id, int pc_id, string gi_type,  int item_id, int amount, int equip_stat, int wpn_ent)
    {
        PssItem_ID = pss_id;
        PC_ID = pc_id;
        Item_Type = gi_type;
        Item_ID = item_id;
        Amount = amount;
        Equip_Stat = equip_stat;
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




