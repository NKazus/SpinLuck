using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class DailyBonus : MonoBehaviour
{
    [SerializeField] private Toggle[] days;
    [SerializeField] private Button bonusButton;
    [SerializeField] private int cooldown = 24;
    [SerializeField] private int extraSpinsBonus = 5;
    
    private Image buttonImage;
    private TextMeshProUGUI buttonText;

    private DailyBonusInfo bonus;
    private bool isEnabled = false;
    private CommonMultiplyerInfo mult;

    [Inject] private readonly GlobalResourceManager resourceManager;
    [Inject] private readonly GlobalEventManager eventManager;

    private void Awake()
    {
        buttonImage = bonusButton.transform.GetChild(0).GetComponent<Image>();
        buttonText = bonusButton.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        buttonImage.enabled = buttonText.enabled = false;      
        bonus = resourceManager.GetDailyBonusInfo();
        SetToggles();
        CheckDate();
        if (isEnabled)        
        {
            mult = resourceManager.GetRandomMultiplyer(0);
            buttonImage.sprite = mult.baseInfo.Icon;
            buttonImage.enabled = buttonText.enabled = isEnabled;
            eventManager.UpdateMultiplyer(mult);

            int currentDayIndex = (int)DateTime.Now.DayOfWeek - 1;
            if(currentDayIndex < bonus.days.Count && currentDayIndex >= 0)
            {
                bonus.days[currentDayIndex] = true;
                bonus.lastBonus = DateTime.Now;
                SetToggles();
            }            

            bonusButton.onClick.AddListener(GetBonus);
        }
        
    }

    private void GetBonus()
    {
        if (CheckComplete())
        {
            ActivateExtraSpins();
            for (int i = 0; i < bonus.days.Count; i++)
            {
                bonus.days[i] = false;
            }
            ResetToggles();
        }
        else
        {
            eventManager.UpdateMultiplyer(mult);
        }

        resourceManager.SetDailyBonusInfo(bonus);
        bonusButton.onClick.RemoveListener(GetBonus);
    }

    private void ResetToggles()
    {
        for (int i = 0; i < days.Length; i++)
        {
            days[i].isOn = false;
        }
    }

    private void SetToggles()
    {
        for (int i = 0; i < days.Length; i++)
        {
            days[i].isOn = bonus.days[i];
        }
    }

    private void CheckDate()
    {
        TimeSpan span = DateTime.Now.Subtract(bonus.lastBonus);
        int hours = (int) span.TotalHours;
        isEnabled = (hours < cooldown) ? false : true;
    }

    private bool CheckComplete()
    {
        for(int i = 0; i < bonus.days.Count; i++)
        {
            if (!bonus.days[i])
            {
                return false;
            }
        }
        return true;
    }

    private void ActivateExtraSpins()
    {
        mult.spinInfo.Spins += extraSpinsBonus;
        resourceManager.SetMultiplyer(mult);
        eventManager.UpdateMultiplyer(mult);
    }

}
