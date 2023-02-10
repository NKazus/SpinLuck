using System;
using UnityEngine ;

[Serializable]
public class WheelPiece : ICloneable
{
    [SerializeField] private Sprite icon;
    public Sprite Icon { get { return icon; } set { icon = value; } }

    [Tooltip("Reward amount")][SerializeField] private int amount;
    public int Amount { get { return amount; } set { amount = value; } }

    [Tooltip("Probability in %")]
    [Range(0f, 100f)][SerializeField] private float chance = 100f;
    public float Chance { get { return chance; } set { chance = value; } }

    public int Index { get; set; }
    public double Weight { get; set; } = 0f;

    public object Clone()
    {
        WheelPiece newPiece = (WheelPiece) MemberwiseClone();
        return newPiece;
    }
}
