using System.Collections;
using System.Collections.Generic;
using System;

[Serializable]
public class StuffInfo
{
    public int Stuff_ID = 1;
    public int GameItem_ID = 11;
    public string Stuff_Name;
}

[Serializable]
public class StuffInfoList
{
    public List<StuffInfo> StuffList;
}
