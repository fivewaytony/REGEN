using System.Collections;
using System.Collections.Generic;
using System;

[Serializable]
public class CharInfo
{
    public int Char_Level = 1;
    public int Char_Str = 1;
    public int Char_Con = 1;
    public int Char_HP = 100;
    public int Char_Exp = 100;
}

[Serializable]
public class CharInfoList
{
    public List<CharInfo> CharList;
}
