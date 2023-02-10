using System;

public class GlobalEventManager
{
    public event Action StartSpinEvent;
    public event Action<WheelPiece> StopSpinEvent;
    public event Action StopSpinNotifyEvent;
    public event Action<CommonMultiplyerInfo> MultiplyerUpdateEvent;

    public void StartSpinning()
    {
        StartSpinEvent?.Invoke();
    }

    public void StopSpinning(WheelPiece piece)
    {
        StopSpinEvent?.Invoke(piece);
        StopSpinNotifyEvent?.Invoke();
    }

    public void UpdateMultiplyer(CommonMultiplyerInfo multiplyerInfo)
    {
        MultiplyerUpdateEvent?.Invoke(multiplyerInfo);
    }
}
