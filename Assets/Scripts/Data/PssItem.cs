using System.Collections;
using System.Collections.Generic;
using System;

[Serializable]
public class PssItem
{
    public int PssItem_ID = 1;
    public int PC_ID = 1;
    public int GameItem_ID = 1;
    public string GameItem_Type;
    public int Amount;
    public int Equip_Stat;

    public PssItem(int pss_id, int pc_id, int gi_id, string gi_type, int amount, int equip_stat)
    {
        PssItem_ID = pss_id;
        PC_ID = pc_id;
        GameItem_ID = gi_id;
        GameItem_Type = gi_type;
        Amount = amount;
        Equip_Stat = equip_stat;
    }

    public static implicit operator List<object>(PssItem v)
    {
        throw new NotImplementedException();
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




