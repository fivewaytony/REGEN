using System.Collections;
using System.Collections.Generic;
using System;

[Serializable]
public class FieldInfo
{
    public int Field_ID = 101;
    public string Field_Type = "N";
    public string Field_Grade = "D";
    public int Field_Level = 1;
    public string Field_Name;
    public string Field_ImgName;
    public string GetItem;
    public int Field_LimitLevel = 1;
}

[Serializable]
public class FieldInfoList
{
    public List<FieldInfo> FieldList;
}
