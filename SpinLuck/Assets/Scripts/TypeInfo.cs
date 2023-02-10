using UnityEngine;

[CreateAssetMenu(fileName = "Type")]
public class TypeInfo : ScriptableObject
{    
    [SerializeField] private int typeIndex;
    public int TypeIndex => typeIndex;

    [SerializeField] private Sprite icon;
    public Sprite Icon => icon;
}
