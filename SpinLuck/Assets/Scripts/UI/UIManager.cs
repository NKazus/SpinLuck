using System;
using UnityEngine;
using UnityEngine.UI;

public enum UIState
{
    Menu = 0,
    Gameplay = 1,
    BonusGame = 2,
    DailyBonus = 3,
    Settings = 4,
    Rules = 5
}
public class UIManager : MonoBehaviour
{
    public event Action<UIState> ChangeStateEvent;

    [SerializeField] private Button[] toMenu;
    [SerializeField] private Button toSettings;
    [SerializeField] private Button toRules;
    [SerializeField] private Button toDailyBonus;
    [SerializeField] private Button toBonusGame;
    [SerializeField] private Button toGameplay;

    private UIState currentState;

    #region MONO
    private void Awake()
    {
       currentState = UIState.Menu;
    }

    private void OnEnable()
    {
        for(int i = 0; i < toMenu.Length; i++)
        {
            toMenu[i].onClick.AddListener(() => { TriggerChange(UIState.Menu); });
        }
        toSettings.onClick.AddListener(() => { TriggerChange(UIState.Settings); });
        toRules.onClick.AddListener(() => { TriggerChange(UIState.Rules); });
        toDailyBonus.onClick.AddListener(() => { TriggerChange(UIState.DailyBonus); });
        toBonusGame.onClick.AddListener(() => { TriggerChange(UIState.BonusGame); });
        toGameplay.onClick.AddListener(() => { TriggerChange(UIState.Gameplay); });
    }

    private void Start()
    {
        TriggerChange(currentState);
    }

    private void OnDisable()
    {
        for (int i = 0; i < toMenu.Length; i++)
        {
            toMenu[i].onClick.RemoveAllListeners();
        }
        toSettings.onClick.RemoveAllListeners();
        toRules.onClick.RemoveAllListeners();
        toDailyBonus.onClick.RemoveAllListeners();
        toBonusGame.onClick.RemoveAllListeners();
        toGameplay.onClick.RemoveAllListeners();
    }
    #endregion


    private void TriggerChange(UIState state)
    {
        currentState = state;
        ChangeStateEvent?.Invoke(state);
    }

}
