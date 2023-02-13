using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class BonusGame : MonoBehaviour
{
    [SerializeField] private Slot[] slots;
    [SerializeField] private int maxClickCount = 3;
    [SerializeField] private int maxAwardCount = 1;
    [SerializeField] private int cooldown = 24;

    private int clickCount;
    private int awardCount;
    private bool isEnabled = false;

    private System.Random rand = new System.Random();

    [Inject] private readonly GlobalResourceManager resourceManager;
    [Inject] private readonly GlobalEventManager eventManager;

    private void Awake()
    {
        for(int i = 0; i < slots.Length; i++)
        {
            slots[i].Initialize();
        }
    }

    private void OnEnable()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].Refresh();
        }
        CheckClicks();
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].SetInteractable(isEnabled);
        }
        if (isEnabled)
        {
            for (int i = 0; i < slots.Length; i++)
            {
                slots[i].SetListener(Click);
            }
        }               
    }

    private void OnDisable()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].ResetListener();
        }
    }

    private void Click(Button button)
    {
        if (!isEnabled)
        {
            return;
        }
        if (clickCount <= 0)
        {
            isEnabled = false;
            for (int i = 0; i < slots.Length; i++)
            {
                slots[i].SetInteractable(isEnabled);
            }
            Complete();
            return;            
        }
        if (awardCount > 0)
        {
            if (RandomizeReward())
            {
                CommonMultiplyerInfo mult = resourceManager.GetRandomMultiplyer(0);
                button.transform.parent.GetChild(1).GetComponent<Image>().sprite = mult.baseInfo.Icon;
                eventManager.UpdateMultiplyer(mult);
                awardCount--;
                Complete();
            }
        }
        clickCount--;
        
    }

    private bool RandomizeReward()
    {
        return rand.Next(2) == 1;
    }

    private void Complete()
    {
        resourceManager.SetBonusGameTime(DateTime.Now);
    }

    private void CheckClicks()
    {
        TimeSpan span = DateTime.Now.Subtract(resourceManager.GetBonusGameTime());
        int hours = (int) span.TotalHours;
        if(hours < cooldown)
        {
            clickCount = 0;
            awardCount = 0;
            isEnabled = false;
        }
        else
        {
            clickCount = maxClickCount - 1;
            awardCount = maxAwardCount;
            isEnabled = true;
        }
    }
}
