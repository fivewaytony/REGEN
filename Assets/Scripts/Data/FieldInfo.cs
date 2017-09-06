using System.Collections;
using System.Collections.Generic;
using System;

[Serializable]
public class FieldInfo
{
    public int Field_ID = 1;
    public int Field_Level = 1;
    public string Field_Name;
    public string Field_ImgName;
    public string GetItem;
}

[Serializable]
public class FieldInfoList
{
    public List<FieldInfo> FieldList;
}
