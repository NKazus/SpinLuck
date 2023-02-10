using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public struct MultiplyerSpins
{
    public int Spins;
    public int SpinsMax;
}
public struct CommonMultiplyerInfo
{
    public MultiplyerInfo baseInfo;
    public MultiplyerSpins spinInfo;
}
public struct DailyBonusInfo
{
    public DateTime lastBonus;
    public List<bool> days;
}
public class GlobalResourceManager : MonoBehaviour
{
    private List<WheelPiece> resourcesData = new List<WheelPiece>();
    private TypeInfo[] allTypes;
    private MultiplyerInfo[] allMultiplyers;
    private CommonMultiplyerInfo multiplyer;
    private int spinsMax = 10;

    private int daysDefault = 5;
    private DailyBonusInfo bonus;
    private DateTime bonusGameDate;

    private System.Random rand = new System.Random();

    [Inject] private readonly ISaveManager saveManager;

    #region MONO
    private void OnEnable()
    {
        allMultiplyers = Resources.LoadAll<MultiplyerInfo>("ScriptableObjects/Multiplyer");
        allTypes = Resources.LoadAll<TypeInfo>("ScriptableObjects/Types");
        multiplyer.baseInfo = allMultiplyers[0];
        InitializeData();
        LoadResources();
    }

    private void OnDisable()
    {
        SaveResources();
    }
    #endregion

    #region INITIALIZATION
    private void InitializeData()
    {
        for(int i = 0; i < allTypes.Length; i++)
        {            
            WheelPiece piece = new WheelPiece();
            piece.Icon = allTypes[i].Icon;
            piece.Index = allTypes[i].TypeIndex;
            resourcesData.Add(piece);
        }
    }

    private void InititalizeMultiplyer(int value, int spins)
    {
        for (int i = 0; i < allMultiplyers.Length; i++)
        {
            if(value == allMultiplyers[i].MultiplyerValue)
            {
                multiplyer.baseInfo = (MultiplyerInfo) allMultiplyers[i].Clone();
                multiplyer.spinInfo.Spins = spins;
                break;
            }
        }
        multiplyer.spinInfo.SpinsMax = spinsMax;
    }
    #endregion

    #region SAVE_CONVERSION
    private void SaveResources()
    { 
        SaveData saveData = new SaveData();
        for (int i = 0; i < resourcesData.Count; i++)
        {
            ResourceInfo resource = new ResourceInfo();
            resource.Amount = resourcesData[i].Amount;
            resource.Index = resourcesData[i].Index;
            saveData.Resources.Add(resource);
        }
        saveData.LastCompleted = bonus.lastBonus.ToUniversalTime().Ticks;
        saveData.CreditedDays = bonus.days;
        saveData.BonusGameLastPlayed = bonusGameDate.ToUniversalTime().Ticks;
        saveData.Multiplyer = multiplyer.baseInfo.MultiplyerValue;
        saveData.MultiplyedSpins = multiplyer.spinInfo.Spins;

        saveManager.Save(saveData);
    }

    private void LoadResources()
    {
        SaveData saveData = saveManager.Load();
        if (saveData == null)
        {
            for (int i = 0; i < resourcesData.Count; i++)
            {
                resourcesData[i].Amount = 0;
            }
            bonus.days = new List<bool>();
            for (int i = 0; i < daysDefault; i++)
            {
                bonus.days.Add(false);
            }
            bonus.lastBonus = DateTime.Now.AddDays(-1);
            bonusGameDate = DateTime.Now.AddDays(-1);
            InititalizeMultiplyer(1, 0);
        }
        else
        {
            for (int i = 0; i < resourcesData.Count; i++)
            {
                if (resourcesData[i].Index == saveData.Resources[i].Index)
                {
                    resourcesData[i].Amount = saveData.Resources[i].Amount;
                }
            }
            bonus.days = saveData.CreditedDays;
            bonus.lastBonus = new DateTime(saveData.LastCompleted).ToLocalTime();
            bonusGameDate = new DateTime(saveData.BonusGameLastPlayed).ToLocalTime();
            InititalizeMultiplyer(saveData.Multiplyer, saveData.MultiplyedSpins);
        }
        
    }
    #endregion

    #region TRANSFER
    public List<WheelPiece> GetResources()
    {
        List<WheelPiece> requestedResources = new List<WheelPiece>();
        for(int i = 0; i < resourcesData.Count; i++)
        {
            requestedResources.Add((WheelPiece) resourcesData[i].Clone());
        }
        return requestedResources;
    }

    public CommonMultiplyerInfo GetCurrentMultiplyer()
    {
        return multiplyer;
    }

    public CommonMultiplyerInfo GetRandomMultiplyer(int extraSpins)
    {
        multiplyer.baseInfo = (MultiplyerInfo)allMultiplyers[rand.Next(allMultiplyers.Length)].Clone();
        multiplyer.spinInfo.SpinsMax = spinsMax;
        multiplyer.spinInfo.Spins = spinsMax + extraSpins;
        return multiplyer; 
    }

    public void SetMultiplyer(CommonMultiplyerInfo mult)
    {
        multiplyer = mult;
    }

    public DateTime GetBonusGameTime()
    {
        return bonusGameDate;
    }

    public void SetBonusGameTime(DateTime date)
    {
        bonusGameDate = date;
    }

    public DailyBonusInfo GetDailyBonusInfo()
    {
        return bonus;
    }

    public void SetDailyBonusInfo(DailyBonusInfo updatedDays)
    {
        bonus = updatedDays;
    }

    public int MultiplyAmount(WheelPiece piece)
    {
        int winValue;
        if(multiplyer.spinInfo.Spins > 0)
        {
            multiplyer.spinInfo.Spins--;
            winValue =  piece.Amount * multiplyer.baseInfo.MultiplyerValue;
        }
        else
        {
            winValue = piece.Amount;
        }
        for (int i = 0; i < resourcesData.Count; i++) 
        {
            if(piece.Index == i)
            {
                resourcesData[i].Amount += winValue;
            }
        }
        return winValue;
    }
    #endregion

}
