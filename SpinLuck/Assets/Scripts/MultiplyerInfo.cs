using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Multiplyer")]
public class MultiplyerInfo : ScriptableObject, ICloneable
{
    [SerializeField] private int multiplyerValue;
    public int MultiplyerValue => multiplyerValue;

    [SerializeField] private Sprite icon;
    public Sprite Icon => icon;

    public object Clone()
    {
        MultiplyerInfo newMult = (MultiplyerInfo)MemberwiseClone();
        return newMult; ;
    }
}
