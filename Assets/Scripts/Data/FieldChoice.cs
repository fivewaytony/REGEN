using System.Collections;
using System.Collections.Generic;
using System;

[Serializable]
public class FieldChoice
{
    public int FieldLevel = 1;
    public string FieldName;
    public string GetItem;
}

[Serializable]
public class FieldChoiceList
{
    public List<FieldChoice> FieldList;
}

