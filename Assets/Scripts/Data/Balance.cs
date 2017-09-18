using System.Collections;
using System.Collections.Generic;
using System;

[Serializable]
public class Balance
{
    public float Wpn_AttackSecRate = 1f;
    public float PC_HPRate = 1f;
    public float Mon_AttackSecRate = 1f;
    public float Mon_HPRate = 1f;
}

[Serializable]
public class BalanceInfoList
{
    public List<Balance> BalanceList;
}