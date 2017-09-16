using System.Collections;
using System.Collections.Generic;
using System;

[Serializable]
public class GameItemGroup
{
    public int GameItem_ID = 1;
    public string GameItem_Type;
    public string GameItem_Name;
}

[Serializable]
public class GameItemGroupInfoList
{
    public List<GameItemGroup> GameItemGroupList;
}
