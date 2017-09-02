using System.Collections;
using System.Collections.Generic;

public class DataEntity
{
    public DataEntity() { }
}

public class PlayerStateEnt
{
    public int PC_ID { get; set; }
    public int PC_CurLevel { get; set; }
    public int PC_CurExp { get; set; }
    public string PC_CurGold { get; set; }
}

public class CharacterEnt
{
    public int Character_Level { get; set; }
    public int Character_Str { get; set; }
    public int Character_Con { get; set; }
    public int Character_HP { get; set; }
    public int Character_Exp { get; set; }
}

