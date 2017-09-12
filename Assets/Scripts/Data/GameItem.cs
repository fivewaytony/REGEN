using System.Collections;
using System.Collections.Generic;
using System;

[Serializable]
public class GameItem
{
    public int GameItem_ID = 1;
    public string GameItem_Type;
    public string GameItem_Name;
}

[Serializable]
public class GameItemInfoList
{
    public List<GameItem> GameItemList;
}
