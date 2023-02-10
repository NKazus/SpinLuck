using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResourceHolder : MonoBehaviour
{
    public Transform Transform { get; private set; }
    public Image Image { get; private set; }
    public TextMeshProUGUI Text { get; private set; }

    private bool isHidden;

    private void Awake()
    {
        Transform = GetComponent<Transform>();
        Image = Transform.GetChild(0).GetComponent<Image>();
        Text = Transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        isHidden = !Image.enabled;
    }

    public void SetResourceHolder(WheelPiece piece)
    {
        Image.sprite = piece.Icon;
        Text.text = piece.Amount.ToString();
    }

    public void SetResourceHolder(CommonMultiplyerInfo multiplyerInfo)
    {
        if ((multiplyerInfo.spinInfo.Spins == 0 && !isHidden) || (multiplyerInfo.spinInfo.Spins > 0 && isHidden))
        {
            Hide(!isHidden);
        }
        else if(!isHidden)
        {            
            Image.sprite = multiplyerInfo.baseInfo.Icon;
            Text.text = multiplyerInfo.spinInfo.Spins.ToString() + "/" + multiplyerInfo.spinInfo.SpinsMax.ToString();
        }        
    }

    public void Hide(bool shouldHide)
    {
        Image.enabled = Text.enabled = !shouldHide;
        isHidden = shouldHide;
    }
}
