using System;
using System.Collections.Generic;

[Serializable]
public class SaveData
{
    public List<ResourceInfo> Resources = new List<ResourceInfo>();
    public int Multiplyer;
    public int MultiplyedSpins;
    public Int64 BonusGameLastPlayed;
    public Int64 LastCompleted;
    public List<bool> CreditedDays = new List<bool>();
}
