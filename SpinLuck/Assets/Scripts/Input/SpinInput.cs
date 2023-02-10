using UnityEngine;
using Zenject;

public class SpinInput : MonoBehaviour
{
    [SerializeField] private PickerWheel wheel;

    private bool isInputEnabled = false;

    [Inject] private readonly InputValidator validator;
    [Inject] private readonly GlobalUpdateManager updateManager;
    [Inject] private readonly GlobalEventManager eventManager;

    private void OnEnable()
    {
        updateManager.GlobalUpdateEvent += LocalUpdate;
        isInputEnabled = true;
        eventManager.StartSpinEvent += SwitchInput;
        eventManager.StopSpinNotifyEvent += SwitchInput;
    }

    private void OnDisable()
    {
        eventManager.StartSpinEvent -= SwitchInput;
        eventManager.StopSpinNotifyEvent -= SwitchInput;
        updateManager.GlobalUpdateEvent -= LocalUpdate;
        isInputEnabled = false;
    }

    private void SwitchInput()
    {
        isInputEnabled = !isInputEnabled;
        if (isInputEnabled)
        {
            updateManager.GlobalUpdateEvent += LocalUpdate;
        }
        else
        {
            updateManager.GlobalUpdateEvent -= LocalUpdate;
        }
    }

#if UNITY_EDITOR
    private void LocalUpdate()
    {
        if (Input.GetMouseButtonDown(0) && validator.ValidateGameplayInput(Input.mousePosition))
        {
            wheel.Spin();
        }
    }
#endif

#if UNITY_ANDROID && !UNITY_EDITOR
    private void LocalUpdate()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began && validator.ValidateGameplayInput(Input.GetTouch(0).position))
        {
            wheel.Spin();
        }
    }
#endif
}
