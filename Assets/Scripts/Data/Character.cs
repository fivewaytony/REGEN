using System.Collections;
using System.Collections.Generic;
using System;

[Serializable]
public class CharInfo
{
    public int Char_Level = 1;
    public int Char_Str = 3;
    public int Char_Con = 2;
    public int Char_Dex = 1;
    public float Char_HP = 100f;
    public int Char_Exp = 100;
}

[Serializable]
public class CharInfoList
{
    public List<CharInfo> CharList;
}
